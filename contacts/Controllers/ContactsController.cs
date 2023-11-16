using contacts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contacts.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactDbContext _contacts;

    public ContactsController(ContactDbContext contacts)
    {
        _contacts = contacts;
    }

    //TODO: mb need to return id after created/edited
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_contacts.Contact);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var contact = _contacts.Contact.Find(id);

        if (contact == null) return NotFound();

        return Ok(contact);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Contact model)
    {
        if (_contacts.Contact.Any(contact => contact == model))
            return Conflict(new { Message = "User Already Exist." });

        if (_contacts.Contact.Any(contact => contact.MobilePhone == model.MobilePhone))
            return Conflict(new { Message = "Mobile Phone Already Used." });

        _contacts.Add(model);
        _contacts.SaveChanges();

        return StatusCode(201);
    }

    [HttpPut]
    public IActionResult Put([FromBody] Contact model)
    {
        var contact = _contacts.Contact.Find(model.Id);

        if (contact == null) return NotFound();

        if (contact.MobilePhone != model.MobilePhone)
            if (_contacts.Contact.Any(item => item.MobilePhone == model.MobilePhone))
                return Conflict(new { Message = "Mobile Phone Already Used." });

        _contacts.Entry(contact).State = EntityState.Detached;
        _contacts.Contact.Attach(model);
        _contacts.Entry(model).State = EntityState.Modified;

        _contacts.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var contact = _contacts.Contact.Find(id);

        if (contact == null) return NotFound();

        _contacts.Contact.Remove(contact);
        _contacts.SaveChanges();

        return NoContent();
    }
}