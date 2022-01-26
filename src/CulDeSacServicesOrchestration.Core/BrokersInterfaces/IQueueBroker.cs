using Microsoft.Azure.ServiceBus;

namespace CulDeSacServicesOrchestration.Core.BrokersInterfaces;

public interface IQueueBroker
{
    void ListenToStudentsQueue(Func<Message, CancellationToken, Task> eventHandler);
    
}