using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// Endpoint => URL
// https://localhost:5001/categories ou http://ocalhost:5001

[Route("categories")]
public class CategoriesController : ControllerBase {

  [HttpGet]
  [Route("")]
  public async Task<ActionResult<List<Category>>> Get()
  {
    return new List<Category>();
  }

  [HttpGet]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> GetById(int id)
  {
    return new Category();
  }

  [HttpPost]
  [Route("")]
  public async Task<ActionResult<Category>> Post([FromBody]Category model)
  {
    if(!ModelState.IsValid)
      return BadRequest(ModelState);
    return Ok(model);
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model)
  {
    if(id != model.Id)
      return NotFound(new { message = "Categoria não encontrada"});

    if(!ModelState.IsValid)
      return BadRequest(ModelState);
  
    return Ok(model);
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>>Delete()
  {
    return Ok();
  }
}