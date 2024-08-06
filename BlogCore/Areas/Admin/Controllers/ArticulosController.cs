using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using BlogCore.Models;
using System.Net;

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
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                //validamos si se subió un archivo y si se está creando un artículo nuevo
                if (articuloViewModel.Articulo.Id == 0)
                {
                    if (archivos.Count() > 0)
                    {
                        //nuevo artículo
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStreams);
                        }
                        articuloViewModel.Articulo.UrlImagen = @"imagenes\articulos\" + nombreArchivo + extension;
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
        public IActionResult Edit(int? id)
        {
            ArticuloViewModel articuloViewModel = new ArticuloViewModel()
            {
                Articulo = new Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            if (id != null)
            {
                articuloViewModel.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(articuloViewModel);
        }

        //articuloViewModel.Articulo medio vacio consultar mañana 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloViewModel articuloViewModel)
        {
            ModelState.Remove("Articulo.Categoria");
            ModelState.Remove("Articulo.UrlImagen");
            ModelState.Remove(nameof(ArticuloViewModel.ListaCategorias));
            string decodificado = WebUtility.HtmlDecode(articuloViewModel.Articulo.Descripcion);
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                Console.WriteLine("ID del Articulo: " + articuloViewModel.Articulo);
                // Depurar el valor del ID
                Console.WriteLine("ID del Articulo: " + articuloViewModel.Articulo.Id);

                // Recuperar el artículo para actualizar
                Console.WriteLine(_contenedorTrabajo.Articulo.GetAll());
                var articuloToUpdate = _contenedorTrabajo.Articulo.Get(articuloViewModel.Articulo.Id);

                // Depurar el artículo recuperado
                if (articuloToUpdate == null)
                {
                    Console.WriteLine("Articulo no encontrado.");
                    return NotFound(); // Agregar un retorno para manejar el caso de no encontrar el artículo
                }
                else
                {
                    Console.WriteLine("Articulo encontrado: " + articuloToUpdate.Nombre);
                }

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloToUpdate.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloViewModel.Articulo.UrlImagen = @"imagenes\articulos\" + nombreArchivo + extension;
                    articuloViewModel.Articulo.FechaDeCreacion = DateTime.UtcNow.ToString();

                    _contenedorTrabajo.Articulo.Update(articuloViewModel.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articuloViewModel.Articulo.UrlImagen = articuloToUpdate.UrlImagen;
                    articuloViewModel.Articulo.FechaDeCreacion = DateTime.UtcNow.ToString();
                    articuloViewModel.Articulo.Descripcion = decodificado;
                }

                _contenedorTrabajo.Articulo.Update(articuloViewModel.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

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
