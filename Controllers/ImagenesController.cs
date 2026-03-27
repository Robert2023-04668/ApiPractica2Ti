using ApiPractica2Ti.Data;
using ApiPractica2Ti.Dtos;
using ApiPractica2Ti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica2Ti.Controllers
{
    [Route("api/imagenes")]
    [ApiController]
    public class ImagenesController : ControllerBase
    {
        private readonly PractiaTiDbContext _context;

        public ImagenesController(PractiaTiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<PaginationDto<Imagene>>> GetFiles(int page = 1, int pageSize = 5)
        {
            var query = _context.Imagenes.AsQueryable();
            int total = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new PaginationDto<Imagene>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Imagene>> GetFile(int id)
        {
            var file = await _context.Imagenes.FindAsync(id);

            if (file == null)
                return NotFound();

            return file;
        }

        [HttpPost]
        public async Task<ActionResult<Imagene>> Post(Imagene imagen)
        {
            _context.Imagenes.Add(imagen);
            await _context.SaveChangesAsync();

            return Ok(imagen);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> PutFile(int id, Imagene file)
        {
            if (id != file.IdImagen)
                return BadRequest();

            _context.Entry(file).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Imagenes.Any(e => e.IdImagen == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.Imagenes.FindAsync(id);
            if (file == null)
                return NotFound();

            _context.Imagenes.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}


