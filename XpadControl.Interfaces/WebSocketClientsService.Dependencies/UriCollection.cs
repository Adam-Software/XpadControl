using System;
using System.Configuration;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies
{
    public class UriCollection
    {
        public UriCollection(string webSocketHost, string webSocketPort, string wheelWebSocketPath, string servosWebSocketPath) 
        {
            if (string.IsNullOrEmpty(webSocketHost) 
                || string.IsNullOrEmpty(webSocketPort) 
                || string.IsNullOrEmpty(wheelWebSocketPath)
                || string.IsNullOrEmpty(servosWebSocketPath))
                throw new ConfigurationErrorsException("Error create uri from configuration");

            string wheelPath = $"ws://{webSocketHost}:{webSocketPort}{wheelWebSocketPath}";
            string servosPath = $"ws://{webSocketHost}:{webSocketPort}{servosWebSocketPath}";

            WheelWebSocketUri = new Uri(wheelPath); 
            ServosWebSocketUri = new Uri(servosPath);
        }

        public UriCollection(string webSocketHost, int webSocketPort, string wheelWebSocketPath, string servosWebSocketPath)
        {
            if (string.IsNullOrEmpty(webSocketHost)
                || webSocketPort == 0
                || string.IsNullOrEmpty(wheelWebSocketPath)
                || string.IsNullOrEmpty(servosWebSocketPath))
                throw new ConfigurationErrorsException("Error create uri from configuration");

            UriBuilder wheelUriBuilder = new UriBuilder() 
            {
                Scheme = "ws://",
                Host  = webSocketHost,
                Port = webSocketPort,
                Path = wheelWebSocketPath,
            };

            UriBuilder servoUriBuilder = new UriBuilder()
            {
                Scheme = "ws://",
                Host = webSocketHost,
                Port = webSocketPort,
                Path = servosWebSocketPath,
            };
            
            WheelWebSocketUri = wheelUriBuilder.Uri;
            ServosWebSocketUri = servoUriBuilder.Uri;
        }

        public Uri WheelWebSocketUri { get; }
        public Uri ServosWebSocketUri { get; }
    }
}
