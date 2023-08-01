namespace mongo1
{
    public class Message
    {

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public string Body { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }


    }
}