using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDTRABAJADORES.Models;
using CRUDTRABAJADORES.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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


        //Este metodo es para el detalle del trabajador , es decir para agregar o editar un trabajador , si el idTrabajador es 0 es para agregar , si es diferente de 0 es para editar
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
            if (oTrabajadorVM.oTrabajador.IdTrabajador == 0)
            {
                _DBContext.Trabajadors.Add(oTrabajadorVM.oTrabajador); //Trabajadors es la propiedad que representa la tabla Trabajador en el contexto de la base de datos, y Add es el método que se utiliza para agregar un nuevo registro a esa tabla. oTrabajadorVM.oTrabajador es el objeto que contiene los datos del trabajador que se va a agregar a la base de datos.

            }
            else
            {
                _DBContext.Trabajadors.Update(oTrabajadorVM.oTrabajador);
            }

            _DBContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }








    }
}
