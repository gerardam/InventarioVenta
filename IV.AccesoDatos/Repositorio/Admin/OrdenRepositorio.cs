using IV.AccesoDatos.Data;
using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio
{
    public class OrdenRepositorio : Repositorio<Orden>, IOrdenRepositorio
    {
        private readonly ApplicationDbContext _db;

        public OrdenRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Orden orden)
        {
            _db.Update(orden);
        }
    }
}
