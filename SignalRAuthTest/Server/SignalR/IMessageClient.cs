using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAuthTest.Server.SignalR
{
    public interface IMessageClient
    {
        Task ReceiveMessage(string user, string message);
        // Task ReceiveMessage(string message);
    }
}
