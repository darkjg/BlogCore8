using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid) { 
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(categoria);
        }
        //llamadas a api
        [HttpGet]
        public IActionResult Getall()
        {
            var data = _contenedorTrabajo.Categoria.GetAll();
            foreach (var item in data) {
                int id = Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null));
                string nombre = item.GetType().GetProperty("Nombre").GetValue(item, null).ToString();
                int orden = Convert.ToInt32(item.GetType().GetProperty("Orden").GetValue(item, null));

                Debug.WriteLine($"Id: {id}, Nombre: {nombre}, Orden: {orden}");
            }
            return Json(new { data });
        }
    }
}
