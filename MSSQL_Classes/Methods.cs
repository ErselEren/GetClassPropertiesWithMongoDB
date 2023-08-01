namespace mongo1.MSSQL_Classes
{
    public class Methods
    {
        public Methods()
        {

        }

        //public Methods(string name, Classes c, ReturnTypes returnType)
        //{
        //    MethodName = name;
        //    className = c;
        //    returnType = returnType;
        //}

        public Methods(string name, Classes c, string returnType)
        {
            MethodName = name;
            className = c;
            ReturnTypeName = returnType;
        }

        public int MethodId { get; set; }

        public string MethodName { get; set; }

        public int ClassId { get; set; }
        
        public string ReturnTypeName { get; set; }
        
        //public int ReturnTypeId { get; set; }

        //public ReturnTypes returnType { get; set; }
        public Classes className { get; set; }

        public List<Parameters> Parameters { get; set; }

    }
}
