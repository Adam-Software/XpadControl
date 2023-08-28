using System;

namespace XpadControl.Services
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        public int CalculateCustomerAgs(int args)
        {
            return args + 10;
        }
    }
}
