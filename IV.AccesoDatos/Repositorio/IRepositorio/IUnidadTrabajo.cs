using System;
using System.Collections.Generic;
using System.Text;

namespace IV.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }

        void Guardar();
    }
}
