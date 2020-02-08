using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MappingFramework.Builder.Interpreters
{
    internal class CreateWithCache : Interpreter
    {
        public string CommandName => "create-with-cache";

        public void Receive(Visitor visitor)
        {
            string typeToCreateName = visitor.Command.Next();

            List<string> parameterNames = new List<string>();
            while (true)
            {
                string newParameterName = visitor.Command.Next();
                if (string.IsNullOrWhiteSpace(newParameterName))
                    break;

                parameterNames.Add(newParameterName);
            }

            List<object> arguments = new List<object>();
            foreach (string parameterName in parameterNames)
                arguments.Add(visitor.DeStash(parameterName));

            Assembly MappingFrameworkAssembly = GetAssemblyByName("MappingFramework");
            Type[] types = MappingFrameworkAssembly.GetTypes();
            Type typeToCreate = types.FirstOrDefault(t => t.Name.Equals(typeToCreateName, StringComparison.OrdinalIgnoreCase));

            object result = Activator.CreateInstance(typeToCreate, arguments.ToArray());
            visitor.Subject = result;
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}