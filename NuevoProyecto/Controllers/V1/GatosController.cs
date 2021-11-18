using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuevoProyecto.Controllers.V1.ViewModels;

namespace NuevoProyecto.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatosController : ControllerBase
    {
        List<GatoViewModel> gatos = new List<GatoViewModel>
        {
            new GatoViewModel {Nombre= "Luna", Edad = 13, Raza= "Siames"},
            new GatoViewModel {Nombre= "Milo", Edad = 12, Raza= "Persa"},
            new GatoViewModel {Nombre= "Lupe", Edad = 14, Raza= "Siames"}
        };

        [HttpGet]
        public ActionResult<List<GatoViewModel>> Get()
        {
            return Ok(gatos);
        }

        [HttpGet("{nombre}")]
        public ActionResult<GatoViewModel> Get(string nombre)
        {
            //Primero me fijo si el gato existe dentro de mi colección
            if (gatos.Exists(gato => gato.Nombre == nombre))
            {
                //Si lo encuentro, lo busco y lo devuelvo
                var gato = gatos.Find(gato => gato.Nombre == nombre);
                return Ok(gato);
            }
            else
            {
                //Si no lo encuentro mando un BadRequest con mensaje de error
                return BadRequest($"No tienes un gato con el nombre {nombre}");
            }
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<List<GatoViewModel>> Adoptar([FromBody]GatoViewModel unGato)
        {
            //Agrego el gato a la colección y luego regreso la colección
            gatos.Add(unGato);
            return Ok(gatos);
        }

        [HttpPut]
        [Route("[action]/{nombre}")]
        public ActionResult<List<GatoViewModel>> Modificar([FromBody]GatoViewModel unGato, string nombre)
        {
            //Primero me fijo si el gato existe dentro de mi colección
            if (gatos.Exists(gato => gato.Nombre == nombre))
            {
                //Busco al gato dentro de la colección mediante su nombre
                var gato = gatos.Find(gato => gato.Nombre == nombre);
                //Modifico sus propiedades
                gato.Nombre = unGato.Nombre;
                gato.Edad = unGato.Edad;
                gato.Raza = unGato.Raza;
                //Devuelvo a todos los gatos
                return Ok(gatos);
            }
            else
            {
                //Si no lo encuentro mando un BadRequest con mensaje de error
                return BadRequest($"No tienes un gato con el nombre {nombre}");
            }
           
        }

        [HttpDelete]
        [Route("[action]/{nombre}")]
        public ActionResult<List<GatoViewModel>> Regalar(string nombre)
        {
            //Primero busco al gato mediante su nombre
            var gatoARegalar = gatos.Find(gato => gato.Nombre == nombre);
            //Luego lo quito de la lista
            var success = gatos.Remove(gatoARegalar);
            if (success)
            {
                return Ok($"Regalaste a uno de tus gatos. ¡Adios {nombre}!");
            }
            else
            {
                return BadRequest($"No pudiste regalar a {nombre}. ¿El nombre es correcto?");
            }
        }
    }
}