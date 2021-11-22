using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuevoProyecto.Controllers.V1.ViewModels
{
    public class GatoViewModel
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Raza { get; set; }        
    }

    public class GatoResponseViewModel : GatoViewModel
    {
        public int IdGato { get; set; }
    }
}
