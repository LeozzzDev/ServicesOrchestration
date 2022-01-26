using CulDeSacServicesOrchestration.Core.Services.Students;
using CulDeSacServicesOrchestration.Core.Services.StudentsEvents;

namespace CulDeSacServicesOrchestration.Core.Orchestrations.StudentsEvents;
public class StudentsEventsOrchestrationService : IStudentsEventsOrchestrationService
{
    private readonly IStudentsEventsService studentsEventsService;
    private readonly IStudentsService studentsService;

    public StudentsEventsOrchestrationService(
        IStudentsService studentsService,
        IStudentsEventsService studentsEventsService)
    {
        this.studentsEventsService = studentsEventsService;
        this.studentsService = studentsService;
    }
    
    public void ListenToStudentsEvents()
    {
        studentsEventsService.ListenToStudentsEvents(async (student) => 
            await studentsService.RegisterStudentAsync(student));
    }
}
