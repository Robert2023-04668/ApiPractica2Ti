namespace ApiPractica2Ti.Dtos
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }

        public int? IdImagen { get; set; }

        public string ImagenPath { get; set; }
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public int? Edad { get; set; }

        public int? IdDepartamento { get; set; }

        public string? NombreDepartamento { get; set; }

        public int? IdPuesto { get; set; }
        public string? NombrePuesto { get; set; }

        public int? Salario { get; set; }

        public DateOnly? FechaDeNacimiento { get; set; }

        public DateOnly? FechaDeContratacion { get; set; }

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public int? IdEstado { get; set; }
        public string? NombreEstado { get; set; }

    }
}
