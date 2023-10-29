using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcAppNinjaDemo.Data;
using mvcAppNinjaDemo.Models;

namespace mvcAppNinjaDemo.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {/*
               _context.Teams != null ? 
                          View(await _context.Teams.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
            */

            if (_context.Teams == null) // Verifica se a tabela existe
                return Problem("Entity set 'EFCoreRelationshipContext.Teams' is null.");

            ICollection<Team> teams = _context.Teams.ToList(); // faz uma colção da classe Categorias - Pega todas as Categories e manda pra um formato de lista

            ICollection<OverviewTeamCount> teamsOverviews = //Cria uma lista do viewModel

                teams.Select(t => //Pra cada category, dentro de categories, cria-se um novo CategoryOverViewModel

                new OverviewTeamCount //cria-se um novo CategoryOverViewModel
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    NinjaCount = (t.Ninjas != null) ? t.Ninjas.Count() : 0,
                    HasMission = (t.Mission != null) ? true : false,
                    RankMission = (t.Mission != null) ? t.Mission.Rank : "No Mission"                  

                }).ToList();

            return View(teamsOverviews);

            //return _context.Categories != null ? 
            //            View(await _context.Categories.ToListAsync()) :
            //            Problem("Entity set 'EFCoreRelationshipContext.Categories'  is null.");

        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Ninjas)
                .Include(t => t.Mission)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [Authorize]
        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,TeamName")] Team team)
        {
            if (ModelState.IsValid)
            {

                bool teamExists = _context.Teams.Any(t => t.TeamName == team.TeamName);

                if (teamExists)
                {
                    ModelState.AddModelError("TeamName", "Já existe um time com esse nome.");

                }
                else
                {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }

            }
            return View(team);
        }
        [Authorize]
        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,TeamName")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
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
            return View(team);
        }
        [Authorize]
        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
    }
}
