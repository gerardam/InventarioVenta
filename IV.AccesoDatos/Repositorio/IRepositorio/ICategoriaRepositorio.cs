using IV.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        void Actualizar(Categoria categoria);
    }
}
