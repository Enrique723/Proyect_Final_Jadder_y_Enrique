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
    public class TareaEtiquetaController : ControllerBase
    {
        private IConfiguration _Config;

        public TareaEtiquetaController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<TareasEtiquetas>>> GetAllTE()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var oTE = conexion.Query<TareasEtiquetas>("MostrarTareasEtiquetas", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTE);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<TareasEtiquetas>>> GetTEbyID(int ID)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@ID", ID);
            var oTE = conexion.Query<TareasEtiquetas>("MostrarTareasEtiquetasPorId", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTE);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateN_TE(TareasEtiquetas cl)
        {
            try
            {
                using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
                conexion.Open();
                var parametro = new DynamicParameters();
                parametro.Add("@IDTarea", cl.IDTarea);
                parametro.Add("@IDEtiqueta", cl.IDEtiqueta);

                var oTE = conexion.Query<TareasEtiquetas>("InsertarTareasEtiquetas", parametro, commandType: System.Data.CommandType.StoredProcedure);

                // Verificar si la operación fue exitosa (por ejemplo, si ousuario no es nulo)
                if (oTE != null)
                {

                    var mensaje = "TareaEtiqueta creada exitosamente.";
                    return Ok(new { mensaje, resultado = oTE });
                }
                else
                {

                    var mensaje = "No se pudo crear la TareaEtiqueta.";
                    return BadRequest(new { mensaje });
                }
            }
            catch (Exception ex)
            {

                var mensaje = "Se produjo un error al crear la TareaEtiqueta: " + ex.Message;
                return StatusCode(500, new { mensaje });
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<TareasEtiquetas>>> UpdateTE(TareasEtiquetas cl)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@ID", cl.ID);
            parametro.Add("@IDTarea", cl.IDTarea);
            parametro.Add("@IDEtiqueta", cl.IDEtiqueta);

            var oTE = conexion.Query<TareasEtiquetas>("ActualizarTareasEtiquetas", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTE);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeleteTEbyID(int ID)
        {
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("Database")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID", ID);

                    // Ejecutar el procedimiento almacenado para eliminar el Usuario
                    await conexion.ExecuteAsync("EliminarTareasEtiquetas", parametro, commandType: CommandType.StoredProcedure);

                    // Devolver una respuesta de éxito
                    return Ok("TareaEtiqueta eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver una respuesta de error
                return StatusCode(500, $"Error al eliminar la TareaEtiqueta: {ex.Message}");
            }
        }
    }
}
