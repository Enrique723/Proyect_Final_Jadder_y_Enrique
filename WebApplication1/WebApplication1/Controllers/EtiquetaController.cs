using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetaController : ControllerBase
    {
        private IConfiguration _Config;

        public EtiquetaController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Etiquetas>>> GetAllEtiqueta()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var oEtiquetas = conexion.Query<Etiquetas>("MostrarEtiquetas", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oEtiquetas);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<Etiquetas>>> GetEtiquetabyID(int IDEtiqueta)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDEtiqueta", IDEtiqueta);
            var oEtiquetas = conexion.Query<Etiquetas>("MostrarEtiquetaPorId", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oEtiquetas);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateN_Etiquetas(Etiquetas cl)
        {
            try
            {
                using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
                conexion.Open();
                var parametro = new DynamicParameters();
                parametro.Add("@Nombre", cl.Nombre);

                var oEtiqueta = conexion.Query<Etiquetas>("InsertarEtiqueta", parametro, commandType: System.Data.CommandType.StoredProcedure);

                // Verificar si la operación fue exitosa (por ejemplo, si ousuario no es nulo)
                if (oEtiqueta != null)
                {

                    var mensaje = "Etiqueta creado exitosamente.";
                    return Ok(new { mensaje, resultado = oEtiqueta });
                }
                else
                {

                    var mensaje = "No se pudo crear el Etiqueta.";
                    return BadRequest(new { mensaje });
                }
            }
            catch (Exception ex)
            {

                var mensaje = "Se produjo un error al crear la Etiqueta: " + ex.Message;
                return StatusCode(500, new { mensaje });
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Etiquetas>>> UpdateEtiqueta(Etiquetas cl)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDEtiqueta", cl.IDEtiqueta);
            parametro.Add("@Nombre", cl.Nombre);
          
            var oEtiqueta = conexion.Query<Etiquetas>("ActualizarEtiqueta", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oEtiqueta);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeleteEtiquetabyID(int IDEtiqueta)
        {
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("Database")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@IDEtiqueta", IDEtiqueta);

                    // Ejecutar el procedimiento almacenado para eliminar el Usuario
                    await conexion.ExecuteAsync("EliminarEtiqueta", parametro, commandType: CommandType.StoredProcedure);

                    // Devolver una respuesta de éxito
                    return Ok("Etiqueta eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver una respuesta de error
                return StatusCode(500, $"Error al eliminar la Etiqueta: {ex.Message}");
            }
        }
    }
}
