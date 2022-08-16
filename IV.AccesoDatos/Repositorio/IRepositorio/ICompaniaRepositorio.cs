using IV.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICompaniaRepositorio : IRepositorio<Compania>
    {
        void Actualizar(Compania compania);
    }
}
