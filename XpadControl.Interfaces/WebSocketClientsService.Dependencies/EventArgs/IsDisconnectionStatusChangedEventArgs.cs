namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies.EventArgs
{
    public class IsDisconnectionStatusChangedEventArgs : System.EventArgs
    {
        public string ClientName { get; set; }
        public bool IsDisconnection { get; set; }
    }
}
