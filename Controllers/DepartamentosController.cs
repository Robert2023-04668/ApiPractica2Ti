using ApiPractica2Ti.Data;
using ApiPractica2Ti.Dtos;
using ApiPractica2Ti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica2Ti.Controllers
{
    [Route("api/Departamentos")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        public PractiaTiDbContext _context;

        public DepartamentosController(PractiaTiDbContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<ActionResult <IEnumerable<Departamento>>> Get(int page = 1, int pageSize = 5, string search = "")
        {
            var query = _context.Departamentos.AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.NombreDepartamento.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new DepartamentoDto
                {
                    IdDepartamento = e.IdDepartamento,
                    NombreDepartamento = e.NombreDepartamento,
                    Descripcion = e.Descripcion,
                })
                .ToListAsync();

            var response = new PaginationDto<DepartamentoDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = data
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Departamento>> GetPorId(int id)
        {
        
                var departamento = await _context.Departamentos
                    .Select(p => new Dtos.DepartamentoDto
                    {
                       IdDepartamento = p.IdDepartamento,
                       NombreDepartamento = p.NombreDepartamento,
                       Descripcion = p.Descripcion,
                    }).FirstOrDefaultAsync(p => p.IdDepartamento == id);
                return Ok(departamento);
            
        }

        [HttpPost]

        public async Task<ActionResult> Post(Departamento departamento)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return Ok(departamento);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var departamento = await _context.Departamentos.Where(d => d.IdDepartamento == id).ExecuteDeleteAsync();

            if (departamento == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Departamento departamento)
        {
            if (id != departamento.IdDepartamento)
            {
                return BadRequest();
            }

            else
            {
                _context.Update(departamento);
                await _context.SaveChangesAsync();
                return Ok(departamento);
            }
        }

        [HttpPatch("{id:int}")]

        public async Task<ActionResult> Patch(int id, Departamento departamento)
        {
            var existente = _context.Departamentos.Find(id);

            if (id != departamento.IdDepartamento)
            {
                return BadRequest();
            }

            if (departamento.NombreDepartamento != null)
            {
                existente.NombreDepartamento = departamento.NombreDepartamento;
            }

            if (departamento.Descripcion != null)
            {
                existente.Descripcion = departamento.Descripcion;
            }


            return Ok(departamento);
        }
    }
}
