﻿using IV.AccesoDatos.Data;
using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepositorio Bodega { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public IMarcaRepositorio Marca { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);//Inicializamos
            Categoria = new CategoriaRepositorio(_db);//Inicializamos
            Marca = new MarcaRepositorio(_db);//Inicializamos
        }

        public void Guardar()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
