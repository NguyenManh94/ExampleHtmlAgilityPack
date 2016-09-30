using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            //var carSample = new Car<Employee>();
            //var lstEmp = new List<Employee>()
            //{
            //    new Employee {Id="1", Scalary=150},
            //    new Employee {Id="2", Scalary=160},
            //    new Employee {Id="3", Scalary=170}
            //};
            //var carSample2 = new Car<Student>();
            //var lstStudent = new List<Student>()
            //{
            //    new Student{Name="Manh",Phone="0978089594"},
            //    new Student{Name="What",Phone="01678891234"}
            //};
            //carSample.ShowData(lstEmp);
            //carSample2.ShowData(lstStudent);

            var st = new Student();
            st.GetDataInfor();
            var bean = new SrcBean();
            var dcDataPrint = bean.GetDcDataPrintTemp();
            foreach (var data in dcDataPrint)
            {
                Console.WriteLine(data.Values);
            }
            Console.ReadLine();
        }

    }

    class Student
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public override string ToString()
        {
            return string.Format("Name: {0,-15} - Phone: {1,-30}", Name, Phone);
        }

        public void GetDataInfor()
        {

            var lstStudent = new List<Dictionary<string, object>>();
            var lstTemStudent = new List<Student>()
            {
                new Student{Name="Manh",Phone="0978089594"},
                new Student{Name="What",Phone="01678891234"}
            };

            for (int i = 0; i < lstTemStudent.Count; i++)
            {
                var dcStudent = new Dictionary<string, Object>();
                dcStudent.Add(Constankey.key_user_id, lstTemStudent[i].Name);
                dcStudent.Add(Constankey.key_address, lstTemStudent[i].Phone);
                lstStudent.Add(dcStudent);
            }

            var bean = new SrcBean();
            bean.SetDcDataPrintTemp(lstStudent);
        }
    }
}
