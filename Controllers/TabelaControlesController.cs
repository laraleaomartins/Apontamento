using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apontamento.Data;
using Apontamento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Apontamento.Controllers
{
    
    public class TabelaControlesController : Controller
    {
        private readonly ApontamentoContext _context;

        public TabelaControlesController(ApontamentoContext context)
        {
            _context = context;
        }

        // GET: TabelaControles
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetInt32("SessionUsuario");

            if (session == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            Usuario UserDB = _context.Usuario.Where(u => u.UsuarioID == session).FirstOrDefault();

            return View(_context.TabelaControle.Where(c => c.UsuarioID == session).ToList());

        }


        // GET: TabelaControles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaControle = await _context.TabelaControle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaControle == null)
            {
                return NotFound();
            }

            return View(tabelaControle);
        }

        // GET: TabelaControles/Create
        public IActionResult Create()
        {
            var session = HttpContext.Session.GetInt32("SessionUsuario");
            return View(
                new TabelaControle
                {
                    UsuarioID = session.GetValueOrDefault()
                }); ;
        }

        // POST: TabelaControles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TabelaControle tabelaControle)
        {
            if (ModelState.IsValid)
            {

                var usuario = _context.Usuario.FirstOrDefault(u => u.UsuarioID == tabelaControle.UsuarioID);


                tabelaControle.HorasTrabalhadas = HorasTrabalhadas(tabelaControle.HoraFinal, tabelaControle.HoraInicial);
                tabelaControle.Periodo = Periodo(tabelaControle.HoraInicial);
                tabelaControle.DiasDaSemana = Convert.ToString(DiasDaSemana(tabelaControle.Data));
                tabelaControle.UsuarioID = usuario.UsuarioID;
                _context.Add(tabelaControle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tabelaControle);
        }

        // GET: TabelaControles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaControle = await _context.TabelaControle.FindAsync(id);
            if (tabelaControle == null)
            {
                return NotFound();
            }
            return View(tabelaControle);
        }

        // POST: TabelaControles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Consultor,DiasDaSemana,Periodo,Data,HoraInicial,HoraFinal,HorasTrabalhadas,Atividade,Observacao")] TabelaControle tabelaControle)
        {
            if (id != tabelaControle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tabelaControle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabelaControleExists(tabelaControle.Id))
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
            return View(tabelaControle);
        }

        // GET: TabelaControles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaControle = await _context.TabelaControle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaControle == null)
            {
                return NotFound();
            }

            return View(tabelaControle);
        }

        // POST: TabelaControles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tabelaControle = await _context.TabelaControle.FindAsync(id);
            _context.TabelaControle.Remove(tabelaControle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabelaControleExists(int id)
        {
            return _context.TabelaControle.Any(e => e.Id == id);
        }

        public TimeSpan HorasTrabalhadas(DateTime HoraFinal, DateTime HoraInicial)
        {

            TimeSpan totalHoras = HoraFinal.Subtract(HoraInicial);
            return totalHoras;

        }
        public string Periodo(DateTime Horainicial)
        {
            if (Convert.ToInt32(Horainicial.Hour) > 5 && Convert.ToInt32(Horainicial.Hour) < 12)
            {
                return "Manhã";

            }
            else
            {
                return "Tarde";
            }
        }
        public Enum DiasDaSemana(DateTime Data)
        {
            return Data.DayOfWeek;
        }




    }
}


