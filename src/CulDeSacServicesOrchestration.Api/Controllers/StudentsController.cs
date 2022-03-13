using CulDeSacServicesOrchestration.Core.Models;
using CulDeSacServicesOrchestration.Core.Services.Students;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CulDeSacServicesOrchestration.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : RESTFulController
{
    private readonly IStudentsService studentsService;

    public StudentsController(IStudentsService studentsService)
    {
        this.studentsService = studentsService;
    }

    [HttpGet]
    public async ValueTask<ActionResult<Student>> GetStudentsAsync()
    {
        IEnumerable<Student> students = await studentsService.GetStudentsAsync();
        return Ok(students);
    }

    [HttpPost]
    public async ValueTask<ActionResult<Student>> PostStudentAsync(Student student)
    {
        Student returnedStudent = await studentsService.RegisterStudentAsync(student);
        return Created(returnedStudent);
    }
}