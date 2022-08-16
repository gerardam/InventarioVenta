using IV.AccesoDatos.Data;
using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio
{
    public class CarroComprasRepositorio : Repositorio<CarroCompras>, ICarroComprasRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CarroComprasRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(CarroCompras carroCompras)
        {
            _db.Update(carroCompras);
        }
    }
}
