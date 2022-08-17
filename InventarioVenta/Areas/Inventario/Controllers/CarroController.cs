﻿using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using IV.Modelos.ViewModels;
using IV.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace InventarioVenta.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public CarroComprasViewModel CarroComprasVM { get; set; }

        public CarroController(IUnidadTrabajo unidadTrabajo, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);

            CarroComprasVM = new CarroComprasViewModel()
            {
                Orden = new Orden(),
                CarroComprasLista = _unidadTrabajo.CarroCompras.ObtenerTodos(u => u.UsuarioAplicacionId ==
                                                                claim.Value, incluirPropiedades: "Producto")
            };

            CarroComprasVM.Orden.TotalOrden = 0;
            CarroComprasVM.Orden.UsuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == claim.Value);

            foreach (var lista in CarroComprasVM.CarroComprasLista)
            {
                lista.Precio = lista.Producto.Precio;
                CarroComprasVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }

            return View(CarroComprasVM);
        }

        public IActionResult mas(int carroId)
        {
            var carroCompras = _unidadTrabajo.CarroCompras.ObtenerPrimero(c => c.Id == carroId, incluirPropiedades: "Producto");
            carroCompras.Cantidad += 1;
            _unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult menos(int carroId)
        {
            var carroCompras = _unidadTrabajo.CarroCompras.ObtenerPrimero(c => c.Id == carroId, incluirPropiedades: "Producto");
            if (carroCompras.Cantidad == 1)
            {
                var numeroProductos = _unidadTrabajo.CarroCompras.ObtenerTodos(u => u.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId).ToList().Count();
                _unidadTrabajo.CarroCompras.Remover(carroCompras);
                _unidadTrabajo.Guardar();
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                _unidadTrabajo.Guardar();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult remover(int carroId)
        {
            var carroCompras = _unidadTrabajo.CarroCompras.ObtenerPrimero(c => c.Id == carroId, incluirPropiedades: "Producto");

            var numeroProductos = _unidadTrabajo.CarroCompras.ObtenerTodos(u => u.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId).ToList().Count();
            _unidadTrabajo.CarroCompras.Remover(carroCompras);
            _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);

            return RedirectToAction(nameof(Index));
        }
    }
}
