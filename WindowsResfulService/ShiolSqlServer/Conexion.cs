using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration; 


namespace AD
{
    public class Conexion
    {
        static public String SqlConnectionString { get; set; }

        public static String connectionString()
        {
            return SqlConnectionString;
        }
     
    }
}
