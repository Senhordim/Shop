using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shop.Data;
using Shop.Models;
using System;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("v1/users")]
  public class UsersController : Controller
  {
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<User>> Post(
        [FromBody]User model,
        [FromServices]DataContext context
      )
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);
      
      try
      {
        context.Users.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar o Usuário" });
      }
    }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
          [FromServices] DataContext context,
          [FromBody]User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            // Esconde a senha
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
  }
}