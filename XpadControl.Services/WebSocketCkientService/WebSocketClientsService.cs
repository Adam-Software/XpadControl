using System;

namespace XpadControl.Services.WebSocketCkientService
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        public int CalculateCustomerAgs(int args)
        {
            return args + 10;
        }
    }
}
