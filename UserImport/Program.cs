using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthyChef.DAL;

namespace UserImport
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            ImportedCustomer.Import(out count);
           
            //List<string> errors = ImportedCustomer.Import(out count);
            //Console.ReadLine();
            //foreach (string error in errors)
            //{
            //    Console.WriteLine(error + count);
            //}
        }
    }
}
