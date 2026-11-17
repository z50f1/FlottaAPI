using Flotta_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flotta_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GepjarmuController : ControllerBase
    {
        FlottaContext _context;

        public GepjarmuController(FlottaContext context)
        {
            _context = context;
        }

        // GETALL DEPENDENCY INJECTION (vizsgan és dolgozatban)
        //[HttpGet("GetAll")]
        //public IActionResult GetAll()
        //{
        //    try
        //    {
        //        using(var context = new FlottaContext())
        //        {
        //            return Ok(context.Gepjarmus.ToList());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Hiba: " + ex.Message);
        //    }
        //}


        //GETALL DEPENDENCY INJECTION TERVEZÉSI (vizsgaremekben)
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Gepjarmus.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba: " + ex.Message);
            }
        }




        [HttpGet("GetByRendszam")]
        public IActionResult GetByRendszam(string rendszam)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(x => x.Rendszam == rendszam);
                if (result == null)
                {
                    return NotFound("Nincs ilyen rendszámú jármű!");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba: " + ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult Post(Gepjarmu gepjarmu)
        {
            try
            {
                _context.Gepjarmus.Add(gepjarmu);
                _context.SaveChanges();
                return Ok("Jármű hozzáadva!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba: " + ex.Message);
            }
        }

        [HttpPut("Put")]
        public IActionResult Put(Gepjarmu gepjarmu)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(x => x.Id == gepjarmu.Id);
                if (result == null)
                {
                    return NotFound("Nincs ilyen ID-jű jármű!");
                }
                _context.ChangeTracker.Clear();
                _context.Gepjarmus.Update(gepjarmu);
                _context.SaveChanges();
                return Ok("Sikeres módosítás!");


            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba:" + ex.Message);
            }
        }

        [HttpDelete("Delete")]

        public IActionResult Delete(int id)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(x => x.Id == id);
                if (result == null)
                {
                    return NotFound("Nincs megfelelő gépjármű");
                }
                _context.ChangeTracker.Clear();
                _context.Gepjarmus.Remove(result);
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba: " + ex.Message);
            }

        }

        [HttpGet("GetByIdSoforok/{id}")]

        public IActionResult GetByIdSoforok(int id)
        {
            try
            {
                var result = _context.Kikuldottjarmus.Where(x => x.Sofor == id).ToList();
                if (result.Count == 0)
                {
                    return NotFound("Nincs ilyen sofőr!");
                }
                var result2 = _context.Kikuldottjarmus.Include(k => k.SoforNavigation).Include(k => k.Gepjarmu).Where(k => k.Gepjarmu.Id == id).Select(k => k.SoforNavigation).ToList();
                return Ok(result2); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba: " + ex.Message);
            }
        }
    }
}
