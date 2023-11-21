using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _Config;

        public UsuarioController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuarios>>> GetAllUsuario()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var oUsuario = conexion.Query<Usuarios>("MostrarUsuarios", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oUsuario);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<Usuarios>>> GetUsuariobyID(int IDUsuario)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDUsuario", IDUsuario);
            var oUsuario = conexion.Query<Usuarios>("MostrarUsuarioPorId", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oUsuario);
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> CreateN_Usuario(Usuarios cl)
        {
            try
            {
                using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
                conexion.Open();
                var parametro = new DynamicParameters();
                parametro.Add("@Nombre", cl.Nombre);
                parametro.Add("@CorreoElectronico", cl.CorreoElectronico);
                parametro.Add("@Contraseña", cl.Contraseña);
                
                var oUsuario = conexion.Query<Usuarios>("InsertarUsuario", parametro, commandType: System.Data.CommandType.StoredProcedure);

                // Verificar si la operación fue exitosa (por ejemplo, si ousuario no es nulo)
                if (oUsuario != null)
                {
                    
                    var mensaje = "Usuario creado exitosamente.";
                    return Ok(new { mensaje, resultado = oUsuario });
                }
                else
                {
                   
                    var mensaje = "No se pudo crear el Usuario.";
                    return BadRequest(new { mensaje });
                }
            }
            catch (Exception ex)
            {
                
                var mensaje = "Se produjo un error al crear el usuario: " + ex.Message;
                return StatusCode(500, new { mensaje });
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Usuarios>>> UpdateUsuario(Usuarios cl)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDUsuario", cl.IDUsuario);
            parametro.Add("@Nombre", cl.Nombre);
            parametro.Add("@CorreoElectronico", cl.CorreoElectronico);
            parametro.Add("@Contraseña", cl.Contraseña);
            var oUsuario = conexion.Query<Usuarios>("ActualizarUsuario", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oUsuario);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeleteUsuariobyID(int IDUsuario)
        {
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("Database")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@IDUsuario", IDUsuario);

                    // Ejecutar el procedimiento almacenado para eliminar el Usuario
                    await conexion.ExecuteAsync("EliminarUsuario", parametro, commandType: CommandType.StoredProcedure);

                    // Devolver una respuesta de éxito
                    return Ok("Usuario eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver una respuesta de error
                return StatusCode(500, $"Error al eliminar el usuario: {ex.Message}");
            }
        } 
    }

}
