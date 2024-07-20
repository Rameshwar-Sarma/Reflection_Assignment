
namespace NotificationEvents
{
    public class NotificationEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public DateTime Timestamp { get; private set; }

        public NotificationEventArgs(string message)
        {
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
}
