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
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Apontamento.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApontamentoContext _context;

        public UsuariosController(ApontamentoContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Usuarios/Login
        public IActionResult Login()
        {
            return View();
        }



        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nome, string email, string senha, [Bind("UsuarioID,Nome,Email,Senha")] Usuario usuario)
        {
            if (nome == null)
            {
                ViewData["NomeNull"] = "Nome não pode ser vazio!";
            }
            else if (email == null)
            {
                ViewData["EmailNull"] = "E-mail não pode ser vazio!";
            }
            else if (senha == null)
            {
                ViewData["SenhaNull"] = "Senha não pode ser vazia!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioID,Nome,Email,Senha")] Usuario usuario)
        {
            if (id != usuario.UsuarioID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioID))
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
            return View(usuario);
        }


        // POST: Usuarios/Login/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UsuarioID,Nome,Email,Senha")] Usuario usuario)
        {

            if (usuario.Email == null || usuario.Senha == null)
            {
                ViewData["Mensagem"] = "Preencha login e senha.";
                return View();
            }

            Usuario userDB = _context.Usuario.Where(u => u.Email == usuario.Email).FirstOrDefault();

            if (userDB == null)
            {
                ViewData["Mensagem"] = "Usuario Nulo!";
                return View();
            }

            if (usuario.Senha != userDB.Senha)
            {
                ViewData["Mensagem"] = "Senha incorreta";
                return View();
            }


            HttpContext.Session.SetInt32("SessionUsuario", userDB.UsuarioID);



            return RedirectToAction("Index", "TabelaControles");

        }

        public async Task<IActionResult> Logout()
        {
            //var session = HttpContext.Session.GetInt32("SessionUsuario");
            HttpContext.Session.Remove("SessionUsuario");


                //HttpContext.Session.SetInt32("SessionUsuario", usuario.UsuarioID);

                //await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Usuarios");

        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioID == id);
        }


    }
}
