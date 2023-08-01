namespace mongo1.MSSQL_Classes
{
    public class ParameterTypes
    {
        public ParameterTypes()
        {
        }
        
        public ParameterTypes(string name)
        {
            Name = name;
        }

        public int ParameterTypeId { get; set; }
        
        public string Name { get; set; }

        public List<Parameters> Parameters { get; set; }
    }
}
