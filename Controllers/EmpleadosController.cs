using ApiPractica2Ti.Data;
using ApiPractica2Ti.Dtos;
using ApiPractica2Ti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica2Ti.Controllers
{

    [Route("api/Empleados")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        public PractiaTiDbContext _context;

        public EmpleadosController(PractiaTiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> Get(
            int page = 1,
            int pageSize = 5,
            string search = "",
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var query = _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Puesto)
                .Include(e => e.Imagene)
                .Include(e => e.Estado)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.Nombre.Contains(search) ||
                    e.Apellido.Contains(search));
            }

            if (fechaInicio.HasValue)
            {
                var fechaInicioOnly = DateOnly.FromDateTime(fechaInicio.Value);

                query = query.Where(e =>
                    e.FechaDeContratacion >= fechaInicioOnly);
            }

            if (fechaFin.HasValue)
            {
                var fechaFinOnly = DateOnly.FromDateTime(fechaFin.Value);

                query = query.Where(e =>
                    e.FechaDeContratacion <= fechaFinOnly);
            }

            int total = query.Count();

            query = query
                .OrderBy(e => e.IdEmpleado) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var empleado = await query
                .Select(e => new EmpleadoDto
                {
                    IdEmpleado = e.IdEmpleado,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Direccion = e.Direccion,
                    NombreEstado = e.Estado.NombreEstado,
                    NombrePuesto = e.Puesto.NombrePuesto,
                    NombreDepartamento = e.Departamento.NombreDepartamento,
                    FechaDeContratacion = e.FechaDeContratacion,
                    FechaDeNacimiento = e.FechaDeNacimiento,
                    ImagenPath = e.Imagene.RutaArchivo,
                    Telefono = e.Telefono,
                    Correo = e.Correo,
                    Salario = e.Salario,
                    IdDepartamento = e.IdDepartamento,
                    IdPuesto = e.IdPuesto,
                    IdEstado = e.IdEstado,
                    IdImagen = e.IdImagen
                }).ToListAsync();

            var response = new PaginationDto<EmpleadoDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = empleado
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetPorId(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Estado).Include(e => e.Puesto).Include(e => e.Departamento).Include(e => e.Imagene)
                .Select(e => new EmpleadoDto
                {
                    IdEmpleado = e.IdEmpleado, 
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Direccion = e.Direccion,
                    NombreEstado = e.Estado.NombreEstado,
                    NombrePuesto = e.Puesto.NombrePuesto,
                    NombreDepartamento = e.Departamento.NombreDepartamento,
                    FechaDeContratacion = e.FechaDeContratacion,
                    FechaDeNacimiento = e.FechaDeNacimiento,
                    ImagenPath = e.Imagene.RutaArchivo,
                    IdImagen = e.IdImagen ,
                    Telefono = e.Telefono,
                    Correo = e.Correo,
                    Edad = e.Edad,
                    Salario = e.Salario,
                    
                  
                }).FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null) return NotFound();
            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<Empleado>> Post(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPorId), new { id = empleado.IdEmpleado }, empleado);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var empleado = await _context.Empleados.Where(e => e.IdEmpleado == id).ExecuteDeleteAsync();

            if (empleado == 0)
            {
                return BadRequest();
            }
            return Ok();

        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, Empleado empleado)
        {
            if (empleado.IdEmpleado != id)
            {
                return BadRequest();
            }

            _context.Update(empleado);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
