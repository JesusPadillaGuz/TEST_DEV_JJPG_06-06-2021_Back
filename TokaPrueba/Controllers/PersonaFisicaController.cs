using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokaPrueba.Context;
using TokaPrueba.Managers;
using TokaPrueba.Models;
using TokaPrueba.ResponseDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TokaPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaFisicaController : ControllerBase
    {
        // GET: api/<PersonaFisicaController>
        [EnableCors("CorsPolicy")]
        [Route("getPersonasFisicas")]
        [HttpGet]
        public ListaPersonasFisicasDto GetPersonas()
        {
            PersonaFisicaManager pfManager = new PersonaFisicaManager();
            return pfManager.GetPersonas();
        }

        // GET api/<PersonaFisicaController>/5
        [EnableCors("CorsPolicy")]
        [Route("getPersonaFisica/{id}")]
        [HttpGet]
        public PersonaFisicaDto Get(int id)
        {
            PersonaFisicaManager pfManager = new PersonaFisicaManager();
            return pfManager.GetPersonaById(id);
        }

        // POST api/<PersonaFisicaController>
        [EnableCors("CorsPolicy")]
        [Route("agregarPersonaFisica")]
        [HttpPost]
        public async Task<RespuestaDto> PostAsync([FromBody] PersonaFisica personaFisica)
        {
            PersonaFisicaManager pfManager = new PersonaFisicaManager();
            return await pfManager.AgregarPersona(personaFisica);
           // return pfManager.AgregarPersonaEF(personaFisica);
        }

        // PUT api/<PersonaFisicaController>/5
        [EnableCors("CorsPolicy")]
        [Route("actualizarPersonaFisica/{id}")]
        [HttpPut]
        public async Task<RespuestaDto> Put(int id, [FromBody] PersonaFisica personaFisica)
        {
            PersonaFisicaManager pfManager = new PersonaFisicaManager();
            return await pfManager.ActualizarPersona(personaFisica);
            // return pfManager.ActualizarPersonaEF(personaFisica);
        }

        // DELETE api/<PersonaFisicaController>/5
        [EnableCors("CorsPolicy")]
        [Route("bajaPersonaFisica/{id}")]
        [HttpDelete]
        public RespuestaDto Delete(int id)
        {
            PersonaFisicaManager pfManager = new PersonaFisicaManager();
            return pfManager.BorrarPersona(id);
            // return pfManager.BorrarPersonaEF(id);
        }
    }
}
