using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDTRABAJADORES.Models;
using CRUDTRABAJADORES.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace CRUDTRABAJADORES.Controllers
{
    public class HomeController : Controller
    {

        private readonly TrabajadoresPruebaContext _DBContext;

        public HomeController(TrabajadoresPruebaContext context)
        {
            _DBContext = context;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        //Este metodo es mi get , listar  , trae a todos los trabajadores incluido el cargo
        public IActionResult Index()
        {
            List<Trabajador> lista = _DBContext.Trabajadors.Include(c => c.oCargo).ToList();
            return View(lista); //Cuando NO le pasas nombre a la vista, ASP.NET MVC hace esto, busca una vista que tenga el mismo nombre que el método
        }


        //PARA CREAR Y EDITAR

        //Este metodo es para el detalle del trabajador , es decir para agregar o editar un trabajador , SOLO DEVUELE LA VISTA, si el idTrabajador es 0 es para agregar , si es diferente de 0 es para editar
        [HttpGet]
        public IActionResult Trabajador_Detalle(int idTrabajador)
        {
            TrabajadorVM oTrabajadorVM = new TrabajadorVM() //oTrabajadorVM es una variable del tipo TrabajadorVM que es la clase que creamos en ViewModelS
            {
                oTrabajador = new Trabajador(),
                oListaCargo = _DBContext.Cargos.Select(cargo => new SelectListItem()
                {
                    Text = cargo.Tipo,
                    Value = cargo.IdCargo.ToString()

                }).ToList()
            };

            if (idTrabajador != 0)
            {
                oTrabajadorVM.oTrabajador = _DBContext.Trabajadors.Find(idTrabajador);
            }

            return View(oTrabajadorVM);
        }



        [HttpPost]
        public IActionResult Trabajador_Detalle(TrabajadorVM oTrabajadorVM)
        {
            // Intentar parsear FechaNacimiento porque DateOnly puede no enlazarse automáticamente
            // desde el form. Leemos el valor crudo y lo convertimos a DateOnly si es posible.
            var fechaStr = Request.Form["oTrabajador.FechaNacimiento"].FirstOrDefault();
            if (!string.IsNullOrEmpty(fechaStr))
            {
                if (DateTime.TryParse(fechaStr, out var dt))
                {
                    oTrabajadorVM.oTrabajador.FechaNacimiento = DateOnly.FromDateTime(dt);
                    // Si ModelState tenía un error por FechaNacimiento, lo eliminamos
                    ModelState.Remove("oTrabajador.FechaNacimiento");
                }
            }
            
            if (oTrabajadorVM.FotoFile != null && oTrabajadorVM.FotoFile.Length > 0) // primero ve si hay subida de archivo (si hay).
            {
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

                var fileName = $"trabajador_{Guid.NewGuid()}{Path.GetExtension(oTrabajadorVM.FotoFile.FileName)}"; //este Guid.NewGuid() es oara que no se repitan nombres
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    oTrabajadorVM.FotoFile.CopyTo(stream); //aqui yo guardo el archivo físicamente en el servidor, en la carpeta wwwroot/uploads con un nombre único generado por Guid.
                }

                // Guardo solo la ruta relativa en el modelo (se guardará en la BD)
                oTrabajadorVM.oTrabajador.FotoUrl = "/uploads/" + fileName;
                // Expongo la URL en el VM para mostrarla inmediatamente
                oTrabajadorVM.FotoUrl = oTrabajadorVM.oTrabajador.FotoUrl;
            }
            else
            {
                // Si es edición y no subieron nueva foto, conservar la FotoUrl existente
                if (oTrabajadorVM.oTrabajador.IdTrabajador != 0)
                {
                    var existing = _DBContext.Trabajadors.AsNoTracking().FirstOrDefault(t => t.IdTrabajador == oTrabajadorVM.oTrabajador.IdTrabajador);
                    if (existing != null)
                    {
                        oTrabajadorVM.oTrabajador.FotoUrl = existing.FotoUrl;
                    }
                }
            }


            if (oTrabajadorVM.oTrabajador.IdTrabajador == 0)
            {
                _DBContext.Trabajadors.Add(oTrabajadorVM.oTrabajador);
            }
            else
            {
                _DBContext.Trabajadors.Update(oTrabajadorVM.oTrabajador);
            }

            try
            {
                _DBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Mostrar error sencillo en la vista
                ViewBag.Error = "Error guardando en la base de datos: " + ex.Message;
                oTrabajadorVM.oListaCargo = _DBContext.Cargos.Select(cargo => new SelectListItem()
                {
                    Text = cargo.Tipo,
                    Value = cargo.IdCargo.ToString()
                }).ToList();

                return View(oTrabajadorVM);
            }

            TempData["SuccessMessage"] = "Trabajador guardado correctamente.";
            return RedirectToAction("Index", "Home");
        }



        //PARA ELIMINAR


        [HttpGet]
        public IActionResult Eliminar(int idTrabajador)
        {
            Trabajador oTrabajador = _DBContext.Trabajadors.Include(c => c.oCargo).Where(e => e.IdTrabajador == idTrabajador).FirstOrDefault();

            return View(oTrabajador);

        }


        [HttpPost]
        public IActionResult Eliminar(Trabajador oTrabajador)
        {
            _DBContext.Trabajadors.Remove(oTrabajador);
            _DBContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



    }
}
