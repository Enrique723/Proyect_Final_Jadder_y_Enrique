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
    public class AsignacionController : ControllerBase
    {
        private IConfiguration _Config;

        public AsignacionController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Asignaciones>>> GetAllAsignaciones()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var oAsignacion = conexion.Query<Asignaciones>("MostrarAsignaciones", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oAsignacion);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<Asignaciones>>> GetEtiquetabyID(int ID)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@ID", ID);
            var oAsignacion = conexion.Query<Asignaciones>("MostrarAsignacionesPorId", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oAsignacion);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateN_Asignacion(Asignaciones cl)
        {
            try
            {
                using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
                conexion.Open();
                var parametro = new DynamicParameters();
                parametro.Add("@IDTarea", cl.IDTarea);
                parametro.Add("@IDUsuario", cl.IDUsuario);

                var oAsignacion = conexion.Query<Asignaciones>("InsertarAsignacion", parametro, commandType: System.Data.CommandType.StoredProcedure);

                // Verificar si la operación fue exitosa (por ejemplo, si ousuario no es nulo)
                if (oAsignacion != null)
                {

                    var mensaje = "Asignacion creada exitosamente.";
                    return Ok(new { mensaje, resultado = oAsignacion });
                }
                else
                {

                    var mensaje = "No se pudo crear la asignacion.";
                    return BadRequest(new { mensaje });
                }
            }
            catch (Exception ex)
            {

                var mensaje = "Se produjo un error al crear la asignacion: " + ex.Message;
                return StatusCode(500, new { mensaje });
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Asignaciones>>> UpdateAsignaciones(Asignaciones cl)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@ID", cl.ID);
            parametro.Add("@IDTarea", cl.IDTarea);
            parametro.Add("@IDUsuario", cl.IDUsuario);

            var oAsignacion = conexion.Query<Asignaciones>("Actualizarasignaciones", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oAsignacion);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeleteAsignacionesbyID(int ID)
        {
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("Database")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID", ID);

                    // Ejecutar el procedimiento almacenado para eliminar el Usuario
                    await conexion.ExecuteAsync("EliminarAsignaciones", parametro, commandType: CommandType.StoredProcedure);

                    // Devolver una respuesta de éxito
                    return Ok("Asignacion eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver una respuesta de error
                return StatusCode(500, $"Error al eliminar la asignacion: {ex.Message}");
            }
        }
    }
}
