using NotificationEvents;
public class Program
{
    static void Main(string[] args)
    {

        FacebookPublisher publisher = new FacebookPublisher();

        publisher.NotificationEvent += MessegeNotification;
        publisher.NotificationEvent += MailNotification;

        Console.Write("Do you want send notification for mobile Number?");
        Console.Write("Press Y for Yes and Any other Key for No:");
        string isMobile = Console.ReadLine();

        Console.Write("Do you want send notification for Mail? Y or N:");
        Console.Write("Press Y for Yes and Any other Key for No:");
        string isForMail = Console.ReadLine();

        if (isMobile != "Y" && isMobile != "y")
        {
            publisher.NotificationEvent -= MessegeNotification;
        }
        if (isForMail != "Y" && isForMail != "y")
        {
            publisher.NotificationEvent -= MailNotification;
        }

        // Try to publish another notification
        publisher.Publish("You have been recieved 1 Lakh rupees Coupon!!!.");

        Console.ReadLine();
    }

    // Event handler method
    private static void MessegeNotification(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"Messege: Notification received: {e.Message} at {e.Timestamp}");
    }
    private static void MailNotification(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"Mail: Notification received: {e.Message} at {e.Timestamp}");
    }
}
