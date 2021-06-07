using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokaPrueba.Context;
using TokaPrueba.Models;
using TokaPrueba.ResponseDto;
using System.Data.SqlClient;
using System.Globalization;

namespace TokaPrueba.Managers
{
    public class PersonaFisicaManager
    {
        private ApplicationDbContext applicationDbContext { get; set; }

        public ListaPersonasFisicasDto GetPersonas()
        {
            ListaPersonasFisicasDto responsePersonas = new ListaPersonasFisicasDto();
            try
            {
                applicationDbContext = new ApplicationDbContext();
                var query = applicationDbContext.Tb_PersonasFisicas.Where(x => x.Activo == true);
                responsePersonas.personasFisicas = query.ToList();
                responsePersonas.success = true;
                responsePersonas.message = "personas recuperadas exitosamente";
            }
            catch (Exception e)
            {
                return new ListaPersonasFisicasDto {
                    success = false,
                    message = "Algo ha salido mal"
                };
            }
            finally
            {
                applicationDbContext.Dispose();
            }
            return responsePersonas;
        }

        public PersonaFisicaDto GetPersonaById(int id)
        {
            PersonaFisicaDto respPersona = new PersonaFisicaDto();
            try
            {
                applicationDbContext = new ApplicationDbContext();
                respPersona.personaFisica = applicationDbContext.Tb_PersonasFisicas.Where(x => x.IdPersonaFisica == id).FirstOrDefault();
                respPersona.success = true;
                respPersona.message = "persona recuperada exitosamente";
            }
            catch (Exception e)
            {
                return new PersonaFisicaDto
                {
                    success = false,
                    message = "Algo ha salido mal"
                };
            }
            finally
            {
                applicationDbContext.Dispose();
            }
            return respPersona;
        }

        public async Task<RespuestaDto> AgregarPersona(PersonaFisica persona)
        {
            applicationDbContext = new ApplicationDbContext();
           
            applicationDbContext = new ApplicationDbContext(); 
            var fecha = persona.FechaNacimiento.ToString("yyyy-MM-dd");
            string StoredProc = "exec sp_AgregarPersonaFisica " + "'" +
                         persona.Nombre + "', '" +
                        persona.ApellidoPaterno + "', '" +
                       persona.ApellidoMaterno + "', '" +
                         persona.RFC + "','" +
                         fecha + "'," +
                        persona.UsuarioAgrega+";";
            try
            {
                await applicationDbContext.Database.ExecuteSqlRawAsync(StoredProc);
                if (!await applicationDbContext.Tb_PersonasFisicas.AnyAsync(x=>x.RFC==persona.RFC))
                {
                    return new RespuestaDto
                    {
                        success = false,
                        message = "Algo ha salido mal al registrar la persona"
                    };
                }
                return new RespuestaDto
                {
                    success = true,
                    message = "Persona registrada correctamente"
                };
            }
            catch(Exception e)
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al registrar la persona"
                };
            }

        }

        public async Task<RespuestaDto> ActualizarPersona(PersonaFisica persona)
        {
            var fecha = persona.FechaNacimiento.ToString("yyyy-MM-dd");
            string StoredProc = "exec sp_ActualizarPersonaFisica " +
                persona.IdPersonaFisica + ",'" +
                         persona.Nombre + "', '" +
                        persona.ApellidoPaterno + "', '" +
                       persona.ApellidoMaterno + "', '" +
                         persona.RFC + "','" +
                         fecha + "'," +
                        persona.UsuarioAgrega + ";";
            applicationDbContext = new ApplicationDbContext();
            try
            {
                await applicationDbContext.Database.ExecuteSqlRawAsync(StoredProc);
                return new RespuestaDto
                {
                    success = true,
                    message = "Persona Actualizada correctamente"
                };
            }
            catch (Exception e)
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al actualizar la persona"
                };
            }
        }

        public RespuestaDto BorrarPersona(int Id)
        {
            applicationDbContext = new ApplicationDbContext();
            SqlConnection conn = (SqlConnection)applicationDbContext.Database.GetDbConnection();
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_EliminarPersonaFisica";
            cmd.Parameters.Add("@IdPersonaFisica", System.Data.SqlDbType.Int).Value = Id;
            var response = cmd.ExecuteNonQuery();
            conn.Close();
            if ( applicationDbContext.Tb_PersonasFisicas.Any(x=>x.IdPersonaFisica==Id&&x.Activo==true))
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al dar de baja la persona"
                };
            }
            return new RespuestaDto
            {
                success = true,
                message = "Persona dada de baja correctamente"
            };
            
        }

        public RespuestaDto AgregarPersonaEF(PersonaFisica personaFisica)
        {
            try
            {
                applicationDbContext = new ApplicationDbContext();
                applicationDbContext.Tb_PersonasFisicas.Add(personaFisica);
                applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al registrar la persona"
                };
            }
            finally
            {
                applicationDbContext.Dispose();
            }
            return new RespuestaDto
            {
                success = true,
                message = "Persona registrada correctamente"
            };
        }

        public RespuestaDto ActualizarPersonaEF(PersonaFisica personaFisica)
        {
            try
            {
                applicationDbContext = new ApplicationDbContext();
                personaFisica.FechaActualizacion = DateTime.Now;
                applicationDbContext.Entry(personaFisica).State = EntityState.Modified;
                /*if (applicationDbContext.Tb_PersonasFisicas.Any(x => x.IdPersonaFisica == personaFisica.IdPersonaFisica))
                {
                    applicationDbContext.Tb_PersonasFisicas.Update(personaFisica);
                }*/
                applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al actualizar la persona"
                };
            }
            finally
            {
                applicationDbContext.Dispose();
            }
            return new RespuestaDto
            {
                success = true,
                message = "Persona actualizada correctamente"
            };
        }

        public RespuestaDto BorrarPersonaEF(int id)
        {
            try
            {
                var persona = applicationDbContext.Tb_PersonasFisicas.Where(x => x.IdPersonaFisica.Equals(id)).FirstOrDefault();
                Action<PersonaFisica> action = x => x.Activo = false;
                action(persona);
                applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaDto
                {
                    success = false,
                    message = "Algo ha salido mal al dar de baja la persona"
                };
            }
            finally
            {
                applicationDbContext.Dispose();
            }
            return new RespuestaDto
            {
                success = true,
                message = "Persona dada de baja correctamente"
            };
        }
    }
}
