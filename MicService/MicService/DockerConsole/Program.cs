using System;
using System.Linq;
using System.Reflection;

namespace DockerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var interface1 = Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITestOne))).ToArray<Type>();
            var interface2 = Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITestTwo))).ToArray<Type>();
            Console.ReadKey();
        }
    }
}
