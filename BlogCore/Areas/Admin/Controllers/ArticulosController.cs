using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using BlogCore.Models;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
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
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            return View(articuloViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticuloViewModel articuloViewModel)
        {

            if (ModelState.IsValid)
            {
                string rutaPrincipal =  _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                //validamos si se subió un archivo y si se está creando un artículo nuevo
                if (articuloViewModel.Articulo.Id == 0)
                {
                    if (archivos.Count() > 0)
                    {
                        //nuevo artículo
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imágenes\artículos");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStreams);
                        }
                        articuloViewModel.Articulo.UrlImagen = @"imágenes\artículos\" + nombreArchivo + extension;
                        articuloViewModel.Articulo.FechaDeCreacion = DateTime.Now.ToString();


                        _contenedorTrabajo.Articulo.Add(articuloViewModel.Articulo);
                        _contenedorTrabajo.Save();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("imagen", "Debes seleccionar una imagen");
                    }
                }
            }
            //si todo falla se regresa al Create GET y pasamos el artiVM
            //para no perder la lista de categorias para el dropdown
            articuloViewModel.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloViewModel);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria");
            return Json(new { data });
        }
    }
}
