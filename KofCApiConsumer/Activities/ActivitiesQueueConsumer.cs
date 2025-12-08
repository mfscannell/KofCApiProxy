using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KofCApiConsumer.Activities;

internal interface IActivitiesQueueConsumer : IDisposable
{
    public void Start(
        Func<CreateActivityMessage, Task> handleMessageAsync,
        CancellationToken cancellationToken);
}

internal class ActivitiesQueueConsumer : IActivitiesQueueConsumer
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Start(
        Func<CreateActivityMessage, Task> handleMessageAsync,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
