using System;

namespace XpadControl.Services.WebSocketCkientService
{
    public interface IWebSocketClientsService : IDisposable
    {
        public int CalculateCustomerAgs(int args);
    }
}
