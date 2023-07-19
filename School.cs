namespace mongo1
{
    public class School
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public City city { get; set; }
        public String State { get; set; }

        public Student TopStudent { get; set; }
    }
}
