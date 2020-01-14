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
    [HttpGet]
    [Route("users")]
    [Authorize(Roles="manager")]
    public async Task<ActionResult<User>> Get(
      [FromServices]DataContext context
    )
    {
      var users = await context
        .Users
        .AsNoTracking()
        .ToArrayAsync();

      return Ok(users);
    }


    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Post(
        [FromBody]User model,
        [FromServices]DataContext context
      )
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);
      
      try
      {
        // força usuário ser employee
        model.Role = "employee";
        context.Users.Add(model);
        await context.SaveChangesAsync();

        // esconder a senha
        model.Password = "";
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