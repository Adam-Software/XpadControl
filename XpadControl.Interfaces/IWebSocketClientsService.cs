using System;

namespace XpadControl.Interfaces.WebSocketCkientService
{
    public interface IWebSocketClientsService : IDisposable
    {
        public int CalculateCustomerAgs(int args);
    }
}
