using ApiPractica2Ti.Data;
using ApiPractica2Ti.Dtos;
using ApiPractica2Ti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica2Ti.Controllers
{
    [Route("api/Estados")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        public PractiaTiDbContext context;

        public EstadosController(PractiaTiDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> Get(int page = 1, int pageSize = 5, string search = "")
        {
            var query = context.Estados.AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.NombreEstado.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EstadosDto
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado,
                    Descripcion = e.Descripcion,
                })
                .ToListAsync();

            var response = new PaginationDto<EstadosDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = data
            };

            return Ok(response);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetPorId(int id)
        {
            var estado = await context.Estados
                  .Select(p => new Dtos.EstadosDto
                  {
                      IdEstado = p.IdEstado,
                      NombreEstado = p.NombreEstado,
                      Descripcion = p.Descripcion,
                  }).FirstOrDefaultAsync(p => p.IdEstado == id);
            return Ok(estado);

        }

        [HttpPost]
        public async Task<ActionResult> Post(Estado estado)
        {
            context.Add(estado);
            await context.SaveChangesAsync();
            return Ok(estado);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Put(int id, Estado estado)
        {

            if (id != estado.IdEstado)
            {
               return BadRequest();
            }

            context.Update(estado);
            await context.SaveChangesAsync();
            return Ok(estado);
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Estados.Where(e => e.IdEstado == id).ExecuteDeleteAsync();
            if (existe == 0)
            {
                return NotFound();
            }

            return NoContent();

        }

    }
}
