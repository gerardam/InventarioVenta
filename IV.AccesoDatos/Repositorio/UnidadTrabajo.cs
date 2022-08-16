using IV.AccesoDatos.Data;
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
        public IProductoRepositorio Producto { get; private set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }
        public ICompaniaRepositorio Compania { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);//Inicializamos
            Categoria = new CategoriaRepositorio(_db);//Inicializamos
            Marca = new MarcaRepositorio(_db);//Inicializamos
            Producto = new ProductoRepositorio(_db);//Inicializamos
            UsuarioAplicacion = new UsuarioAplicacionRepositorio(_db);//Inicializamos
            Compania = new CompaniaRepositorio(_db);//Inicializamos
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
