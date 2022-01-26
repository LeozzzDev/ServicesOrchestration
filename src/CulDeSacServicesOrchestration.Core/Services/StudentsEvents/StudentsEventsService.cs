using System.Text;
using System.Text.Json;
using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Models;

namespace CulDeSacServicesOrchestration.Core.Services.StudentsEvents;

public class StudentsEventsService : IStudentsEventsService
{
    private readonly IQueueBroker? queueBroker;

    public StudentsEventsService(IQueueBroker? queueBroker)
    {
        this.queueBroker = queueBroker;
    }

    public void ListenToStudentsEvents(Func<Student, ValueTask> studentEventHandler)
    {
        queueBroker?.ListenToStudentsQueue(async (message, token) =>
        {
            string serializedStudent = Encoding.UTF8.GetString(message.Body);
            Student incomingStudent = JsonSerializer.Deserialize<Student>(serializedStudent);
            
            await studentEventHandler(incomingStudent);
        });
    }
}