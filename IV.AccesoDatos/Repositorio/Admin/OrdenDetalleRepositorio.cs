using IV.AccesoDatos.Data;
using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio
{
    public class OrdenDetalleRepositorio : Repositorio<OrdenDetalle>, IOrdenDetalleRepositorio
    {
        private readonly ApplicationDbContext _db;

        public OrdenDetalleRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(OrdenDetalle ordenDetalle)
        {
            _db.Update(ordenDetalle);
        }
    }
}
