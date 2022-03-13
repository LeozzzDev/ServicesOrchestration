using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Models;

namespace CulDeSacServicesOrchestration.Core.Services.Students;

public class StudentsService : IStudentsService
{
    private readonly IDatabaseStudentsBroker databaseStudentsBroker;

    public StudentsService(IDatabaseStudentsBroker databaseStudentsBroker)
    {
        this.databaseStudentsBroker = databaseStudentsBroker;
    }

    public async ValueTask<IEnumerable<Student>> GetStudentsAsync()
        => await databaseStudentsBroker.SelectStudentsAsync();
        
    public async ValueTask<Student> RegisterStudentAsync(Student student)
        => await databaseStudentsBroker.InsertStudentAsync(student);
}