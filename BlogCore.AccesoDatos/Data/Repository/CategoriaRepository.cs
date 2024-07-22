﻿using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public IEnumerable<SelectListItem> GetListaCategorias() {
            return _db.Categoria.Select(c => new SelectListItem()
            {
                Text = c.Nombre,
                Value = c.Id.ToString()

            });
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
