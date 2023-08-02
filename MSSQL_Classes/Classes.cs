namespace mongo1.MSSQL_Classes
{
    public class Classes
    {
        public Classes()
        {
                
        }

        public Classes(string className)
        {
            Name = className;
        }

        public int ClassId { get; set; }
        
        public string Name { get; set; }

        public List<Methods> Methods { get; set; }
    }
}
