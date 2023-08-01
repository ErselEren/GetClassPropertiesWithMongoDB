namespace mongo1.MSSQL_Classes
{
    public class Parameters
    {

        public Parameters()
        {
          
        }

        public Parameters(ParameterTypes paramType, Methods method, string paramName)
        {
            methodName = method;
            parameterType = paramType;
            ParamName = paramName;
        }



        public int ParamId { get; set; }
        
        public string ParamName { get; set; }
        
        public int MethodId { get; set; }

        public int ParameterTypeId { get; set; }

        public Methods methodName { get; set; }

        public ParameterTypes parameterType { get; set; }

    }
}
