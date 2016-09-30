using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneric
{
    class Car<T>
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

        public void ShowData(List<T> lstData)
        {
            for (int i = 0; i < lstData.Count; i++)
            {
                Console.WriteLine(lstData[i].ToString());
            }
        }

        public override string ToString()
        {
            return string.Format("Id: {0,-15} -Address: {1,-30} -Name: {2,-30}", Id, Name, Address);
        }
    }

    class Employee
    {
        public string Id { get; set; }
        public float Scalary { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0,-15} -Scalary: {1,-30}", Id, Scalary);
        }
    }
}
