using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuevoProyecto.Controllers.V1.ViewModels;

namespace NuevoProyecto.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatosController : ControllerBase
    {
        private readonly GatosContexto contexto;

        public GatosController(GatosContexto contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet]
        public ActionResult<List<GatoViewModel>> Get()
        {
            return Ok(contexto.Gato);
        }

        [HttpGet("{nombre}")]
        public ActionResult<GatoViewModel> Get(string nombre)
        {
            //Primero me fijo si el gato existe dentro de mi colección
            if (contexto.Gato.ToList().Exists(gato => gato.Nombre == nombre))
            {
                //Si lo encuentro, lo busco y lo devuelvo
                var gato = contexto.Gato.ToList().Find(gato => gato.Nombre == nombre);
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
        public ActionResult<List<GatoViewModel>> Adoptar([FromBody] GatoViewModel unGato)
        {
            //Agrego el gato a la colección y luego regreso la colección
            contexto.Gato.Add(new Gato { Nombre = unGato.Nombre, Edad = unGato.Edad, Raza = unGato.Raza});
            contexto.SaveChanges();
            return Ok(contexto.Gato);
        }

        [HttpPut]
        [Route("[action]/{nombre}")]
        public ActionResult<List<GatoViewModel>> Modificar([FromBody]GatoViewModel unGato, string nombre)
        {
            //Primero me fijo si el gato existe dentro de mi colección
            if (contexto.Gato.ToList().Exists(gato => gato.Nombre == nombre))
            {
                //Busco al gato dentro de la colección mediante su nombre
                var gato = contexto.Gato.ToList().Find(gato => gato.Nombre == nombre);
                //Modifico sus propiedades
                gato.Nombre = unGato.Nombre;
                gato.Edad = unGato.Edad;
                gato.Raza = unGato.Raza;

                contexto.SaveChanges();

                //Devuelvo a todos los gatos
                return Ok(contexto.Gato);
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
            var gatoARegalar = contexto.Gato.ToList().Find(gato => gato.Nombre == nombre);
            
            if (gatoARegalar != null)
            {
                //Luego lo quito de la lista
                contexto.Gato.Remove(gatoARegalar);

                contexto.SaveChanges();

                return Ok($"Regalaste a uno de tus gatos. ¡Adios {nombre}!");
            }
            else
            {
                return BadRequest($"No pudiste regalar a {nombre}. ¿El nombre es correcto?");
            }
        }
    }
}