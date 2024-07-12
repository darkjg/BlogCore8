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
    internal class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }
        public void Update(Categoria categoria)
        {
            var objDb = _db.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
            objDb.Nombre=categoria.Nombre;
            objDb.Orden=categoria.Orden;
            _db.SaveChanges();
        }
    }
}
