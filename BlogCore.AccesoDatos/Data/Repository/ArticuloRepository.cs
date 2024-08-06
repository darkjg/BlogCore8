using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }
        public void Update(Articulo articulo)
        {
            var objDb = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            objDb.Nombre= articulo.Nombre;
            objDb.Descripcion= articulo.Descripcion;
            objDb.UrlImagen= articulo.UrlImagen;
            objDb.CategoriaId= articulo.CategoriaId;
            _db.SaveChanges();
        }
      
    }

}
