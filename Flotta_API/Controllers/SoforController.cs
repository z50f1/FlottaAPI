using Flotta_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flotta_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {
        private FlottaContext _context;
        public SoforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Sofors.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpGet("GetbyId/{id}")]
        public IActionResult GetbyId(int id)
        {
            try
            {
                var result = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (result == null) return NotFound("Nincs ilyen sofőr!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult Post(Sofor sofor)
        {
            try
            {
                _context.Sofors.Add(sofor);
                _context.SaveChanges();
                return Ok("Hozzáadva!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
       
        public IActionResult Put(int id, Sofor sofor)
            {
                try
                {
                    var IdSofor = _context.Sofors.FirstOrDefault(x => x.Id == id);
                    if (IdSofor == null) return NotFound("Nincs ilyen sofőr!");

                    IdSofor.Name = sofor.Name;
                    IdSofor.Address = sofor.Address;
                    IdSofor.Email = sofor.Email;
                    IdSofor.Phone = sofor.Phone;
                    IdSofor.Image = sofor.Image;
    
                    _context.SaveChanges();
                    return Ok("Módosítva!");
                }
                catch (Exception ex)
                {
                    return BadRequest("Error! " + ex.Message);
                }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var IdSofor = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (IdSofor == null) return NotFound("Nincs ilyen sofőr!");
                _context.Sofors.Remove(IdSofor);
                _context.SaveChanges();
                return Ok("Törölve!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpGet("GetAllData")]
        public IActionResult GetAllData()
        {
            try
            {
                var result = _context.Kikuldottjarmus
                    .AsNoTracking()
                    .Select(j => new SoforRendszam
                    {
                        Name = j.SoforNavigation!.Name,
                        Rendszam = j.Gepjarmu!.Rendszam
                    })
                    .AsEnumerable()
                    .GroupBy(j => j.Name)
                    .Select(group => new
                    {
                        Name = group.Key,
                        Rendszamok = group.Select(j => j.Rendszam).Distinct().ToList()
                    })
                    .ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }
    }   
}
