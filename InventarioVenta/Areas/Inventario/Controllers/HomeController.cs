﻿using IV.AccesoDatos.Data;
using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using IV.Modelos.ViewModels;
using IV.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventarioVenta.Areas.Inventario.Controllers
{
	[Area("Inventario")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnidadTrabajo _unidadTrabajo;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public CarroComprasViewModel CarroComprasVM { get; set; }

		public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
		{
			_logger = logger;
			_unidadTrabajo = unidadTrabajo;
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Producto> productoLista = _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca");
			var claimIdentidad = (ClaimsIdentity)User.Identity;
			var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
			if(claim != null)
			{
				var numeroProductos = _unidadTrabajo.CarroCompras.ObtenerTodos(c => c.UsuarioAplicacionId ==
																		  claim.Value).ToList().Count();
				HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);
			}

			return View(productoLista);
		}

		public IActionResult Detalle(int id)
		{
			CarroComprasVM = new CarroComprasViewModel();
			CarroComprasVM.Compania = _db.Compania.FirstOrDefault();
			CarroComprasVM.BodegaProducto = _db.BodegaProducto.Include(p => p.Producto).Include(p => p.Producto.Categoria).Include(p => p.Producto.Marca).FirstOrDefault(b => b.ProductoId == id && b.BodegaId == CarroComprasVM.Compania.BodegaVentaId);

			if(CarroComprasVM.BodegaProducto == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				CarroComprasVM.CarroCompras = new CarroCompras()
				{
					Producto = CarroComprasVM.BodegaProducto.Producto,
					ProductoId = CarroComprasVM.BodegaProducto.ProductoId
				};
				return View(CarroComprasVM);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult Detalle(CarroComprasViewModel carroComprasVM)
		{
			var claimIdentidad = (ClaimsIdentity)User.Identity;
			var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
			carroComprasVM.CarroCompras.UsuarioAplicacionId = claim.Value;

			CarroCompras carroDb = _unidadTrabajo.CarroCompras.ObtenerPrimero(
					   u => u.UsuarioAplicacionId == carroComprasVM.CarroCompras.UsuarioAplicacionId
					   && u.ProductoId == carroComprasVM.CarroCompras.ProductoId,
					   incluirPropiedades: "Producto"
				);
			if (carroDb == null)
			{
				// No hay registro para el usuario en el Carro de compras del producto
				_unidadTrabajo.CarroCompras.Agregar(carroComprasVM.CarroCompras);
			}
			else
			{
				carroDb.Cantidad += carroComprasVM.CarroCompras.Cantidad;
				_unidadTrabajo.CarroCompras.Actualizar(carroDb);
			}
			_unidadTrabajo.Guardar();

			// Agregar valor a la sesion
			var numeroProductos = _unidadTrabajo.CarroCompras.ObtenerTodos(c => c.UsuarioAplicacionId ==
																		  carroComprasVM.CarroCompras.UsuarioAplicacionId).ToList().Count();
			HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
