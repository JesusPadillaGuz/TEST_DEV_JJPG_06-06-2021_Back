using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokaPrueba.Models;

namespace TokaPrueba.ResponseDto
{
    public class ListaPersonasFisicasDto
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<PersonaFisica> personasFisicas { get; set; }
    }
}
