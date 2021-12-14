using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatProject.Controllers.V1.ViewModels;
using CatProject.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NuevoProyecto.Controllers.V1.ViewModels;

namespace NuevoProyecto.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatosController : ControllerBase
    {
        private readonly GatosContexto contexto;
        private readonly IConfiguration configuration;

        public GatosController(GatosContexto contexto, IConfiguration configuration)
        {
            this.contexto = contexto;
            this.configuration = configuration;
        }
        private bool ValidarToken(string token)
        {
            try
            {
                var json = JwtBuilder.Create()
                         .WithAlgorithm(new HMACSHA256Algorithm()) 
                         .WithSecret(configuration["Jwt:secret"])
                         .MustVerifySignature()
                         .Decode(token);

                JwtViewModel jwt = JsonConvert.DeserializeObject<JwtViewModel>(json);
                
                if (jwt.Name != configuration["Jwt:name"] && jwt.Sub != configuration["Jwt:sub"]) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public ActionResult<List<GatoResponseViewModel>> Get([FromHeader] string token)
        {
            if (!ValidarToken(token)) return Unauthorized();

            return Ok(contexto.Gato);
        }

        [HttpGet("{nombre}")]
        public ActionResult<GatoResponseViewModel> Get([FromHeader] string token, string nombre)
        {
            if (!ValidarToken(token)) return Unauthorized();

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
        public ActionResult<List<GatoViewModel>> Adoptar([FromHeader] string token, [FromBody] GatoViewModel unGato)
        {
            if (!ValidarToken(token)) return Unauthorized();

            //Agrego el gato a la colección y luego regreso la colección
            contexto.Gato.Add(new Gato { Nombre = unGato.Nombre, Edad = unGato.Edad, Raza = unGato.Raza});
            contexto.SaveChanges();
            return Ok(contexto.Gato);
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<List<GatoViewModel>> AdoptarMuchos([FromHeader] string token, [FromBody] List<GatoViewModel> unosGatos)
        {
            if (!ValidarToken(token)) return Unauthorized();

            var gatos = new List<Gato>();
            //Agrego el gato a la colección y luego regreso la colección
            foreach (var unGato in unosGatos)
            {
                var gato = new Gato { Nombre = unGato.Nombre, Edad = unGato.Edad, Raza = unGato.Raza };
                contexto.Gato.Add(gato);
            }
            
            contexto.SaveChanges();
            return Ok(contexto.Gato);
        }

        [HttpPut]
        [Route("[action]/{idGato}")]
        public ActionResult<List<GatoViewModel>> Modificar([FromHeader] string token, [FromBody] GatoViewModel unGato, int idGato)
        {
            if (!ValidarToken(token)) return Unauthorized();

            //Primero me fijo si el gato existe dentro de mi colección
            if (contexto.Gato.ToList().Exists(gato => gato.IdGato == idGato))
            {
                //Busco al gato dentro de la colección mediante su nombre
                var gato = contexto.Gato.ToList().Find(gato => gato.IdGato == idGato);
                //Modifico sus propiedades
                gato.Nombre = unGato.Nombre;
                gato.Edad = unGato.Edad;
                gato.Raza = unGato.Raza;

                contexto.SaveChanges();

                //Devuelvo a todos los gatos
                return Ok(gato.IdGato);
            }
            else
            {
                //Si no lo encuentro mando un BadRequest con mensaje de error
                return BadRequest($"No tienes un gato con el idGato {idGato}");
            }
           
        }

        [HttpDelete]
        [Route("[action]/{nombre}")]
        public ActionResult<List<GatoViewModel>> Regalar([FromHeader] string token, string nombre)
        {
            if (!ValidarToken(token)) return Unauthorized();

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