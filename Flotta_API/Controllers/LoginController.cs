using Argon2Sharp;
using Flotta_API.DTOs;
using Flotta_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flotta_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly FlottaContext _context;

    public LoginController(FlottaContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.LoginName)
                             || string.IsNullOrWhiteSpace(loginDTO.Password))
            return BadRequest("Felhasználónév és jelszó szükséges!");

        try
        {
            var user = _context.Users.FirstOrDefault(u => u.LoginName == loginDTO.LoginName);
            if (user == null || string.IsNullOrWhiteSpace(user.Hash))
                return Unauthorized("Hibás felhasználónév vagy jelszó!");

            var (isOk, _) = Argon2PhcFormat.VerifyPhcString(loginDTO.Password, user.Hash);
            if (!isOk)
                return Unauthorized("Hibás felhasználónév vagy jelszó!");

            if (!user.Active || user.Status != "active")
                return StatusCode(403, "A felhasználói fiók nem aktív!");

            // Ez csak sikeres hitelesítési visszajelzés, NEM JWT-token.
            return Ok(new { message = "Sikeres bejelentkezés!" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Hiba történt a bejelentkezés során.");
        }
    }
}
