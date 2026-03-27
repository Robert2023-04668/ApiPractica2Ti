using ApiPractica2Ti.Data;
using ApiPractica2Ti.Dtos;
using ApiPractica2Ti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica2Ti.Controllers
{
    [Route("api/Puestos")]
    [ApiController]
    public class PuestosController : ControllerBase
    {
        public PractiaTiDbContext context;

        public PuestosController(PractiaTiDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Puesto>>> Get(int page = 1, int pageSize = 5, string search = "")
        {
            var query = context.Puestos.AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.NombrePuesto.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new PuestosDto
                {
                    IdPuesto = e.IdPuesto,
                    IdDepartamento = e.IdDepartamento,
                    NombrePuesto = e.NombrePuesto,
                    Descripcion = e.Descripcion,
                })
                .ToListAsync();

            var response = new PaginationDto<PuestosDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = data
            };

            return Ok(response);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<PuestosDto>> GetPorId(int id)
        {
            var puesto = await context.Puestos
                .Include(p => p.IdDepartamentoNavigation)
                .Where(p => p.IdPuesto == id)
                .Select(p => new PuestosDto
                {
                    IdPuesto = p.IdPuesto,
                    NombrePuesto = p.NombrePuesto,
                    Descripcion = p.Descripcion,
                    IdDepartamento = p.IdDepartamento,
                    Departamento = p.IdDepartamentoNavigation.NombreDepartamento
                })
                .FirstOrDefaultAsync();

            if (puesto == null)
                return NotFound();

            return Ok(puesto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PuestosDto puesto)
        {
            var entidad = new Puesto
            {
                NombrePuesto = puesto.NombrePuesto,
                Descripcion= puesto.Descripcion,
                IdDepartamento = puesto.IdDepartamento,
            };

           

            context.Add(entidad);
            await context.SaveChangesAsync();
            return Ok(puesto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var puesto = await context.Puestos.Where(p => p.IdPuesto == id).ExecuteDeleteAsync();

            if (puesto == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Puesto requested)
        {

            if (id != requested.IdPuesto)
            {
               return BadRequest();
            }

            context.Update(requested);
            await context.SaveChangesAsync();

            return Ok(requested);
        }
    }
}
