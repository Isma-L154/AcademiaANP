using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANP_Academy.Controllers
{
    public class FAQsController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;

        public FAQsController(AnpdesarrolloContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.FAQs.ToListAsync());
        }

        public async Task<IActionResult> VistaFAQs()
        {
            var faqs = await _dbContext.FAQs.ToListAsync();
            return View(faqs);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Id,Pregunta,Respuesta")] FAQ faq)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(faq);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _dbContext.FAQs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Pregunta,Respuesta")] FAQ faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(faq);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(faq.Id))
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
            return View(faq);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var faq = await _dbContext.FAQs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }

            _dbContext.FAQs.Remove(faq);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }



        private bool FAQExists(int id)
        {
            return _dbContext.FAQs.Any(e => e.Id == id);
        }
    }
}
