using CulDeSacServicesOrchestration.Core.Models;

namespace CulDeSacServicesOrchestration.Core.BrokersInterfaces;

public interface IDatabaseStudentsBroker
{
    ValueTask<Student> InsertStudentAsync(Student student);
    ValueTask<IEnumerable<Student>> SelectStudentsAsync();
}