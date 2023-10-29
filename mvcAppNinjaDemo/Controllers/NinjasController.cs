using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using mvcAppNinjaDemo.Data;
using mvcAppNinjaDemo.Models;

namespace mvcAppNinjaDemo.Controllers
{
    public class NinjasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NinjasController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Ninjas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ninjas.Include(n => n.Image).Include(n => n.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ninjas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ninjas == null)
            {
                return NotFound();
            }

            var ninja = await _context.Ninjas
                .Include(n => n.Image)
                .Include(n => n.Team)
                .FirstOrDefaultAsync(m => m.NinjaId == id);
            if (ninja == null)
            {
                return NotFound();
            }

            return View(ninja);
        }

        // GET: Ninjas/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId");
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Ninjas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NinjaId,FirstName,LastName,DateOfBirth,Jutsu,NinjaImagePath,IsAlive,Information,TeamId,ImageId")] Ninja ninja, IFormFile? NinjaImage)
        {

            ninja.IsAlive = true; // Todos os ninjas são criados vivos
            var imageName = string.Empty;
            var imagePath = string.Empty;

            if (NinjaImage != null && NinjaImage.Length > 0)
            {
                imageName = ninja.FullName.Replace(" ", "_") + Path.GetExtension(NinjaImage.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "Uploads", "Ninjas", imageName);

                // Verifique se o arquivo já existe

                //string baseImageName = imageName.Replace(Path.GetExtension(NinjaImage.FileName), ""); // Nome da imagem sem extensão


                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await NinjaImage.CopyToAsync(stream);
                }

                ninja.NinjaImagePath = $"/Uploads/Ninjas/{imageName}"; // Salve o nome da imagem no banco de dados
            }
            else
            {
                ModelState.AddModelError("", "Você deve enviar uma imagem para o ninja.");
            }

            if (ModelState.IsValid)
            {
                // Verifica se já existe um ninja com o mesmo nome e sobrenome
                bool ninjaExists = _context.Ninjas.Any(n => n.FirstName == ninja.FirstName);

                if (ninjaExists)
                {
                    ModelState.AddModelError("", "Já existe um ninja com o mesmo nome e sobrenome.");
                    return View(ninja);
                }

                _context.Add(ninja);
                await _context.SaveChangesAsync();


                if (imagePath != null) // Só crie o objeto Image se imagePath for definido
                {
                    Image image = new Image
                    {
                        ImageName = imageName,
                        ImagePath = imagePath,
                        NinjaId = ninja.NinjaId
                    };

                    _context.Add(image);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", ninja.ImageId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", ninja.TeamId);
            return View(ninja);
        }

        // GET: Ninjas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ninjas == null)
            {
                return NotFound();
            }

            var ninja = await _context.Ninjas.FindAsync(id);
            if (ninja == null)
            {
                return NotFound();
            }

            // Preparando a lista de equipes com menos de 3 ninjas
            var availableTeams = _context.Teams
                .Where(t => t.Ninjas.Count < 3)
                .ToList();

            ViewBag.Teams = new SelectList(availableTeams, "TeamId", "TeamName");



            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", ninja.ImageId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", ninja.TeamId);
            return View(ninja);
        }

        // POST: Ninjas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NinjaId,FirstName,LastName,DateOfBirth,Jutsu,NinjaImagePath,IsAlive,Information,TeamId,ImageId")] Ninja ninja, IFormFile? NinjaImage)
        {
            if (id != ninja.NinjaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (NinjaImage != null)
                {
                    var imageName = ninja.FullName.Replace(" ", "_") + Path.GetExtension(NinjaImage.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Ninjas", NinjaImage.FileName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await NinjaImage.CopyToAsync(stream);
                    }
                    // O caminho relativo para ser salvo no banco de dados
                    ninja.NinjaImagePath = $"/Uploads/Ninjas/{NinjaImage.FileName}";
                }
                else
                {
                    // Se nenhuma imagem for fornecida, mantenha a imagem anterior.
                    var existingNinja = await _context.Ninjas.AsNoTracking().FirstOrDefaultAsync(n => n.NinjaId == id);
                    if (existingNinja != null)
                    {
                        ninja.NinjaImagePath = existingNinja.NinjaImagePath;
                    }
                }

                try
                {
                    _context.Update(ninja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NinjaExists(ninja.NinjaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageId", ninja.ImageId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", ninja.TeamId);
            return View(ninja);
        }

        // GET: Ninjas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ninjas == null)
            {
                return NotFound();
            }

            var ninja = await _context.Ninjas
                .Include(n => n.Image)
                .Include(n => n.Team)
                .FirstOrDefaultAsync(m => m.NinjaId == id);
            if (ninja == null)
            {
                return NotFound();
            }

            return View(ninja);
        }

        // POST: Ninjas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ninjas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ninjas'  is null.");
            }
            var ninja = await _context.Ninjas.FindAsync(id);
            if (ninja != null)
            {
                _context.Ninjas.Remove(ninja);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NinjaExists(int id)
        {
          return (_context.Ninjas?.Any(e => e.NinjaId == id)).GetValueOrDefault();
        }
    }
}
