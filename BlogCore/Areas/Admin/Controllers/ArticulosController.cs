using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public ArticulosController (IContenedorTrabajo contenedorTrabajo)
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
            ArticuloViewModel articuloViewModel = new ArticuloViewModel()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias =_contenedorTrabajo.Categoria.GetListaCategorias()

            };
            return View(articuloViewModel);
        }
        //llamadas a api
        [HttpGet]
        public IActionResult Getall()
        {
            var data = _contenedorTrabajo.Articulo.GetAll(includeProperties:"Categoria");

            return Json(new { data });
        }
    }
}
