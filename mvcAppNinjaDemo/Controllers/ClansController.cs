using System;
using System.Collections.Generic;
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
    public class ClansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ClansController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Clans
        public async Task<IActionResult> Index()
        {
              return _context.Clans != null ? 
                          View(await _context.Clans.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Clans'  is null.");
        }

        // GET: Clans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clans == null)
            {
                return NotFound();
            }

            var clan = await _context.Clans
                .FirstOrDefaultAsync(m => m.ClanId == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // GET: Clans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClanId,ClanName,ClanImagePath,Leader,ClanInfo")] Clan clan, IFormFile? ClanImage)
        {
            var imageName = string.Empty;
            var imagePath = string.Empty;

            if (ClanImage != null && ClanImage.Length > 0)
            {
                imageName = clan.ClanName.Replace(" ", "_") + Path.GetExtension(ClanImage.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "Uploads", "Clans", imageName);

                // Verifique se o arquivo já existe

                //string baseImageName = imageName.Replace(Path.GetExtension(NinjaImage.FileName), ""); // Nome da imagem sem extensão


                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await ClanImage.CopyToAsync(stream);
                }

                clan.ClanImagePath = $"/Uploads/Clans/{imageName}"; // Salve o nome da imagem no banco de dados
            }

            if (ModelState.IsValid)
            {
                bool clanExists = _context.Clans.Any(n => n.ClanName == clan.ClanName);

                if (clanExists)
                {
                    ModelState.AddModelError("", "Já existe um ninja com o mesmo nome e sobrenome.");
                    return View(clan);
                }

                _context.Add(clan);
                await _context.SaveChangesAsync();

                if (imagePath != null) // Só crie o objeto Image se imagePath for definido
                {
                    Image image = new Image
                    {
                        ImageName = imageName,
                        ImagePath = imagePath,
                        NinjaId = clan.ClanId
                    };

                    _context.Add(image);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(clan);
        }

        // GET: Clans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clans == null)
            {
                return NotFound();
            }

            var clan = await _context.Clans.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }
            return View(clan);
        }

        // POST: Clans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClanId,ClanName,ClanImagePath,Leader,ClanInfo")] Clan clan)
        {
            if (id != clan.ClanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanExists(clan.ClanId))
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
            return View(clan);
        }

        // GET: Clans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clans == null)
            {
                return NotFound();
            }

            var clan = await _context.Clans
                .FirstOrDefaultAsync(m => m.ClanId == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // POST: Clans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clans'  is null.");
            }
            var clan = await _context.Clans.FindAsync(id);
            if (clan != null)
            {
                _context.Clans.Remove(clan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanExists(int id)
        {
          return (_context.Clans?.Any(e => e.ClanId == id)).GetValueOrDefault();
        }
    }
}
