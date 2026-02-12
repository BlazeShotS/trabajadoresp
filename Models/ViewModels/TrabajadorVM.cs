using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System;

namespace CRUDTRABAJADORES.Models.ViewModels
{
    public class TrabajadorVM
    {

        public Trabajador oTrabajador { get; set; } //Con esto guardamos un objeto trabajador


        public List<SelectListItem> oListaCargo { get; set; }

        // Archivo cargado desde el formulario
        public IFormFile? FotoFile { get; set; }

        // URL pública para mostrar la imagen cargada
        public string? FotoUrl { get; set; }

    }
}
