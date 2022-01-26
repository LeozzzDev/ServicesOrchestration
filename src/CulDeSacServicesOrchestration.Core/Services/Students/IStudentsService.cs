using CulDeSacServicesOrchestration.Core.Models;

namespace CulDeSacServicesOrchestration.Core.Services.Students;

public interface IStudentsService
{
    ValueTask<Student> RegisterStudentAsync(Student student);
    ValueTask<IEnumerable<Student>> GetStudentsAsync();
}