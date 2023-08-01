using mongo2;
using System;
using System.Collections.Generic;

namespace mongo1
{
    public class ClassEntry : ClassNames
    {
        public List<(String,String)> types { get; set; }

        public Garage MyProperty { get; set; }     

        public ClassEntry()
        {
                types = new List<(String, String)>();
            }
    }
}
