using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BinCalcTest
{
    class Program
    {  
        static void Main(string[] args)
        {
            Assembly SampleAssembly = Assembly.LoadFrom("BinCalc.dll");
            Type[] types = SampleAssembly.GetTypes();

            Console.WriteLine("----------\nТипы:");
            foreach (Type type in types)
            {
                Console.WriteLine(type.Name + "\t" + type.FullName);
            }

            Console.WriteLine("----------\nПоля:");
            FieldInfo[] ts = types[0].GetFields();
            foreach (FieldInfo t in ts)
                Console.WriteLine(t.Name + "\t" + t.FieldType);

            Console.WriteLine("----------\nМетоды:");
            MethodInfo[] methods = types[0].GetMethods();
            foreach (MethodInfo method in methods)
            {
                string parameters = GetParams(method.GetParameters());
                Console.WriteLine(method.ReturnType.ToString() + " " + method.Name + parameters + "\t from " + method.DeclaringType);
            }

            MethodInfo operation = types[0].GetMethods()[0];
            object obj = Activator.CreateInstance(types[0]);
            string answer = (string)operation.Invoke(obj, new object[] { "0|4" });
            Console.WriteLine("\n23|4 = " + answer);

            Console.ReadKey();
        }

        static string GetParams(ParameterInfo[] parameters)
        {
            string formatParams = "(";

            for (int i = 0; i < parameters.Length; ++i)
            {
                formatParams += parameters[i].ParameterType.ToString() + " " + parameters[i].Name;

                if (i != parameters.Length - 1)
                    formatParams += ", ";
            }

            return formatParams + ")";
        }
    }
}
