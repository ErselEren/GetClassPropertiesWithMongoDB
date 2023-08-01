namespace mongo1
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message> SentMessages { get; set; }
        public List<Message> ReceivedMessages { get; set; }



    }
}
