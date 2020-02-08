using System;
using System.Linq;
using System.Reflection;

namespace MappingFramework.Builder.Interpreters
{
    internal class Create : Interpreter
    {
        public string CommandName => "create";

        public void Receive(Visitor visitor)
        {
            string typeToCreateName = visitor.Command.Next();

            Assembly MappingFrameworkAssembly = GetAssemblyByName("MappingFramework");
            Type[] types = MappingFrameworkAssembly.GetTypes();
            Type typeToCreate = types.FirstOrDefault(t => t.Name.Equals(typeToCreateName, StringComparison.OrdinalIgnoreCase));

            object result = Activator.CreateInstance(typeToCreate);
            visitor.Subject = result;
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}