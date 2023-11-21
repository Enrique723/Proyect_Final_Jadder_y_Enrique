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
    public class TareaController : ControllerBase
    {
        private IConfiguration _Config;

        public TareaController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tareas>>> GetAllTareas()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var oTarea = conexion.Query<Tareas>("MostrarTareas", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTarea);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<List<Tareas>>> GetTareabyID(int IDTarea)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDTarea", IDTarea);
            var oTarea = conexion.Query<Tareas>("MostrarTareasPorId", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTarea);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateN_Tarea(Tareas cl)
        {
            try
            {
                using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
                conexion.Open();
                var parametro = new DynamicParameters();
                parametro.Add("@Nombre", cl.Nombre);
                parametro.Add("@Descripcion", cl.Descripcion);
                parametro.Add("@FechaVencimiento", cl.FechaVencimiento);
                parametro.Add("@Prioridad", cl.Prioridad);
                parametro.Add("@Estado", cl.Estado);

                var oTarea = conexion.Query<Tareas>("InsertarTareas", parametro, commandType: System.Data.CommandType.StoredProcedure);

                // Verificar si la operación fue exitosa (por ejemplo, si Tarea no es nulo)
                if (oTarea != null)
                {

                    var mensaje = "Tarea creada exitosamente.";
                    return Ok(new { mensaje, resultado = oTarea });
                }
                else
                {

                    var mensaje = "No se pudo crear el Tarea.";
                    return BadRequest(new { mensaje });
                }
            }
            catch (Exception ex)
            {

                var mensaje = "Se produjo un error al crear la Tarea: " + ex.Message;
                return StatusCode(500, new { mensaje });
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Usuarios>>> UpdateTarea(Tareas cl)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("Database"));
            conexion.Open();
            var parametro = new DynamicParameters();
            parametro.Add("@IDTarea", cl.IDTarea);
            parametro.Add("@Nombre", cl.Nombre);
            parametro.Add("@Descripcion", cl.Descripcion);
            parametro.Add("@FechaVencimiento", cl.FechaVencimiento);
            parametro.Add("@Prioridad", cl.Prioridad);
            parametro.Add("@Estado", cl.Estado);

            var oTarea = conexion.Query<Usuarios>("ActualizarTarea", parametro, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(oTarea);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeleteUsuariobyID(int IDTarea)
        {
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("Database")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@IDTarea", IDTarea);

                    // Ejecutar el procedimiento almacenado para eliminar la Tarea
                    await conexion.ExecuteAsync("EliminarTarea", parametro, commandType: CommandType.StoredProcedure);

                    // Devolver una respuesta de éxito
                    return Ok("Tarea eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y devolver una respuesta de error
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }

}
