using IV.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarroComprasRepositorio : IRepositorio<CarroCompras>
    {
        void Actualizar(CarroCompras carroCompras);
    }
}
