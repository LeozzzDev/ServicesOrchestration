using System.Threading.Tasks;
using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Models;
using CulDeSacServicesOrchestration.Core.Services.Students;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace CulDeSacServicesOrchestration.UnitTests;

public class StudentsServiceTests
{
    private readonly Mock<IDatabaseStudentsBroker>? databaseStudentsBrokerMock;
    private readonly IStudentsService? studentsService;

    public StudentsServiceTests()
    {
        databaseStudentsBrokerMock = new Mock<IDatabaseStudentsBroker>();

        studentsService = new StudentsService(
            databaseStudentsBroker: databaseStudentsBrokerMock.Object);
    }

    [Fact]
    public async Task ShouldAddStudentAsync()
    {
        Student student = GetRandomStudent();
        Student expectedStudent = student.DeepClone();

        databaseStudentsBrokerMock?.Setup(broker =>
            broker.InsertStudentAsync(student))
                .ReturnsAsync(expectedStudent);

        Student returnedStudent =
            await studentsService.RegisterStudentAsync(student);

        returnedStudent.Should().
            BeEquivalentTo(expectedStudent);

        databaseStudentsBrokerMock?.Verify(broker =>
            broker.InsertStudentAsync(student), Times.Once());

        databaseStudentsBrokerMock?.VerifyNoOtherCalls();
    }

    private Student GetRandomStudent()
        => new Filler<Student>().Create();
}