using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // Для використання асинхронних методів
using System.Linq;  // Для LINQ методів

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Отримання поточного студента за OpenID
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentStudent()
    {
        var user = User.Identity.Name;  // Ваш OpenID або інший спосіб ідентифікації
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.OpenId == user);

        if (student != null && student.OpenId != null) 
        {
            return NotFound();
        }

        return Ok(student);
    }

    // Реєстрація для курсової роботи
    [HttpPost("{studentId}/coursework")]
    public async Task<IActionResult> RegisterForCoursework(int studentId, [FromBody] Coursework coursework)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student != null && student.OpenId != null) 
        {
            return NotFound();
        }

        coursework.StudentId = studentId;
        _context.Courseworks.Add(coursework);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(RegisterForCoursework), new { id = coursework.Id }, coursework);
    }
}