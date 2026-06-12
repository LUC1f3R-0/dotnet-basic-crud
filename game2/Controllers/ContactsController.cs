using Microsoft.AspNetCore.Mvc;

namespace TestCrudApplication.Api.Controllers;

[Route("Contacts")]
public class ContactsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok("this is working");
    }
}