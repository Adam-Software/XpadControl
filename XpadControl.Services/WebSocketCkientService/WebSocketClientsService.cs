﻿using System;
using XpadControl.Services.LoggerService;

namespace XpadControl.Services.WebSocketCkientService
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        private readonly ILoggerService mLoggerService;

        public WebSocketClientsService(ILoggerService loggerService) 
        {
            mLoggerService = loggerService;
        }

        public int CalculateCustomerAgs(int args)
        {
            return args + 10;
        }

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(WebSocketCkientService)} called");
        }
    }
}
