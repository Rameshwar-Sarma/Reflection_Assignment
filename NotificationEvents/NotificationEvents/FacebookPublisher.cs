
namespace NotificationEvents
{
    public class FacebookPublisher
    {
        // Declare the event using EventHandler<T>
        public event EventHandler<NotificationEventArgs> NotificationEvent;

        // Lock object for thread safety
        private readonly object _lock = new object();

        public void Publish(string message)
        {
            NotificationEventArgs args = new NotificationEventArgs(message);
            OnNotification(args);
        }

        protected virtual void OnNotification(NotificationEventArgs e)
        {
            EventHandler<NotificationEventArgs> handler;
            lock (_lock)
            {
                handler = NotificationEvent;
            }

            // Ensure there are subscribers
            if (handler != null)
            {
                foreach (EventHandler<NotificationEventArgs> subscriber in handler.GetInvocationList())
                {
                    try
                    {
                        subscriber(this, e);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error notifying subscriber: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No subscribers to notify.");
            }
        }
    }
}
