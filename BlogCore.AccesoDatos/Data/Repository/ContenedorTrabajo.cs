﻿using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;
        public ContenedorTrabajo(ApplicationDbContext DB)
        {
            _db = DB;
            Categoria= new CategoriaRepository(_db);
            Articulo= new ArticuloRepository(_db);
         
        }

        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }

        public void Dispose()
        {
           _db.Dispose();
        }

        public void Save()
        {
         _db.SaveChanges(); 
        }
    }
}
