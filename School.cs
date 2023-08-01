namespace mongo1
{
    public class School
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public City city { get; set; }
        public String State { get; set; }

        public Student TopStudent { get; set; }


        public double GetGPAofTopStudent()
        {
            return 3.2;
        }

        public void SchoolFunction(Student student, String name, String Address,City city, int PostalCode, double doubleParameter)
        {

        }

        public Student GetTopStudent()
        {
            return TopStudent;
        }

    }
}
