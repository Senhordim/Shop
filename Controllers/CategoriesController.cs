using Microsoft.AspNetCore.Mvc;

// Endpoint => URL
// https://localhost:5001/categories ou http://ocalhost:5001

[Route("categories")]
public class CategoriesController : ControllerBase {
  [Route("")]
  public string MeuMetodo()
  {
    return "Olá mundo!";
  }
}