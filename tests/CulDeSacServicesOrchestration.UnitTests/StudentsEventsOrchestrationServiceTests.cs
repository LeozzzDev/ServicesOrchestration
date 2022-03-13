using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CulDeSacServicesOrchestration.Core.Models;
using CulDeSacServicesOrchestration.Core.Orchestrations.StudentsEvents;
using CulDeSacServicesOrchestration.Core.Services.Students;
using CulDeSacServicesOrchestration.Core.Services.StudentsEvents;
using Microsoft.Azure.ServiceBus;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace CulDeSacServicesOrchestration.UnitTests;

public class StudentsEventsOrchestrationServiceTests
{
     private readonly Mock<IStudentsService> studentsServiceMock;
    private readonly Mock<IStudentsEventsService> studentsEventsServiceMock;
    private readonly IStudentsEventsOrchestrationService studentsEventsOrchestrationService;

    public StudentsEventsOrchestrationServiceTests()
    {
        studentsServiceMock = 
            new Mock<IStudentsService>();

        studentsEventsServiceMock =
            new Mock<IStudentsEventsService>();

        studentsEventsOrchestrationService =
            new StudentsEventsOrchestrationService(
                studentsServiceMock.Object, studentsEventsServiceMock.Object);
    }

    [Fact]
    public void ShouldListenToStudentsEventsAndRegisterStudent()
    {
        Student incomingStudent = GetRandomStudent();

        studentsEventsServiceMock.Setup(eventsService =>
            eventsService.ListenToStudentsEvents(It.IsAny<Func<Student, ValueTask>>()))
                .Callback<Func<Student, ValueTask>>(eventFunction =>
                    eventFunction.Invoke(incomingStudent));
        
        studentsEventsOrchestrationService.ListenToStudentsEvents();

        studentsEventsServiceMock.Verify(eventsService =>
            eventsService.ListenToStudentsEvents(It.IsAny<Func<Student, ValueTask>>()), Times.Once());

        studentsServiceMock.Verify(service =>
            service.RegisterStudentAsync(incomingStudent), Times.Once());

        studentsEventsServiceMock.VerifyNoOtherCalls();
        studentsServiceMock.VerifyNoOtherCalls();   
    }

    private Message GetStudentMessage(Student student)
    {
        string serializedStudent = JsonSerializer.Serialize(student);
        return new Message(Encoding.UTF8.GetBytes(serializedStudent));
    }

    private Student GetRandomStudent()
        => new Filler<Student>().Create();
}