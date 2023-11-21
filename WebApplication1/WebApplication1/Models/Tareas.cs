namespace WebApplication1.Models
{
    public class Tareas
    {
        public int IDTarea { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set;}
        public DateTime FechaVencimiento { get; set;}
        public int Prioridad { get; set;}
        public string Estado { get; set;}


    }
}
