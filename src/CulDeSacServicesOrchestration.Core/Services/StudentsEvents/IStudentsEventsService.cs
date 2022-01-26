using CulDeSacServicesOrchestration.Core.Models;

namespace CulDeSacServicesOrchestration.Core.Services.StudentsEvents;

public interface IStudentsEventsService
{
    void ListenToStudentsEvents(Func<Student, ValueTask> studentEventHandler);
}