using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUDTRABAJADORES.Models.ViewModels
{
    public class TrabajadorVM
    {

        public Trabajador oTrabajador { get; set; } //Con esto guardamos un objeto trabajador


        public List<SelectListItem> oListaCargo { get; set; }

    }
}
