using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using Microsoft.Azure.ServiceBus;

namespace CulDeSacServicesOrchestration.Infrastructure.Queue;

public class QueueStudentsBroker : IQueueBroker
{
    private readonly IQueueClient studentsQueueClient;

    public QueueStudentsBroker(IQueueClient studentsQueueClient)
    {
        this.studentsQueueClient = studentsQueueClient;
    }

    public void ListenToStudentsQueue(Func<Message, CancellationToken, Task> eventHandler)
    {
        MessageHandlerOptions messageHandlerOptions = GetMessageHandlerOptions();

        Func<Message, CancellationToken, Task> messageEventHandler = 
            CompleteStudentsQueueMessageAsync(eventHandler);

        studentsQueueClient.RegisterMessageHandler(messageEventHandler, messageHandlerOptions);
    }

    private Func<Message, CancellationToken, Task> CompleteStudentsQueueMessageAsync(
        Func<Message, CancellationToken, Task> eventHandler)
    {
        return async (message, token) => 
        {
            await eventHandler(message, token);
            await studentsQueueClient.CompleteAsync(message.SystemProperties.LockToken);
        };
    }

    private MessageHandlerOptions GetMessageHandlerOptions()
    {
        return new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            AutoComplete = false,
            MaxConcurrentCalls = 1
        };
    }
    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        ExceptionReceivedContext context = args.ExceptionReceivedContext;
        return Task.CompletedTask;

    }
}