using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Models;
using CulDeSacServicesOrchestration.Core.Services.StudentsEvents;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Azure.ServiceBus;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace CulDeSacServicesOrchestration.UnitTests;

public class StudentsEventsServiceTests
{
    private readonly Mock<IQueueBroker> queueBrokerMock;
    private readonly IStudentsEventsService studentsEventsService;
    private readonly ICompareLogic compareLogic;

    public StudentsEventsServiceTests()
    {
        queueBrokerMock = new Mock<IQueueBroker>();
        studentsEventsService = new StudentsEventsService(queueBrokerMock.Object);
        compareLogic = new CompareLogic();
    }

    [Fact]
    public void ShouldListenToStudentsEvents()
    {
        var studentEventHandlerMock = new Mock<Func<Student, ValueTask>>();
        Student incomingStudent = GetRandomStudent();
        Message incomingStudentQueueMessage = GetStudentMessage(incomingStudent);

        queueBrokerMock.Setup(queueBroker =>
            queueBroker.ListenToStudentsQueue(
                It.IsAny<Func<Message, CancellationToken, Task>>())
                    ).Callback<Func<Message, CancellationToken, Task>>(eventFunction =>
                        eventFunction.Invoke(incomingStudentQueueMessage, It.IsAny<CancellationToken>()));

        studentsEventsService.ListenToStudentsEvents(studentEventHandlerMock.Object);

        studentEventHandlerMock.Verify(handler =>
            handler.Invoke(It.Is(SameStudentAs(incomingStudent))), Times.Once);

        queueBrokerMock.Verify(queueBroker =>
            queueBroker.ListenToStudentsQueue(
                    It.IsAny<Func<Message, CancellationToken, Task>>()), Times.Once());

        queueBrokerMock.VerifyNoOtherCalls();
    }

    private Expression<Func<Student, bool>> SameStudentAs(Student expectedStudent)
    {
        return actualStudent =>
            compareLogic.Compare(expectedStudent, actualStudent).AreEqual;
    }

    private Message GetStudentMessage(Student student)
    {
        string serializedStudent = JsonSerializer.Serialize(student);
        return new Message(Encoding.UTF8.GetBytes(serializedStudent));
    }

    private Student GetRandomStudent()
        => new Filler<Student>().Create();
}