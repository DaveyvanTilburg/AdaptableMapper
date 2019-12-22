using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptableMapper.Builder.Interpreters
{
    internal class CreateWithVariables : Interpreter
    {
        public string CommandName => "create-with-variables";

        public void Receive(Visitor visitor)
        {
            string typeToCreateName = visitor.Command.Next();

            List<string> parameterValues = new List<string>();
            while (true)
            {
                string newParameterName = visitor.Command.Next();
                if (string.IsNullOrWhiteSpace(newParameterName))
                    break;

                parameterValues.Add(newParameterName);
            }

            Assembly adaptableMapperAssembly = GetAssemblyByName("AdaptableMapper");
            Type[] types = adaptableMapperAssembly.GetTypes();
            Type typeToCreate = types.FirstOrDefault(t => t.Name.Equals(typeToCreateName, StringComparison.OrdinalIgnoreCase));

            //typeToCreate.GetConstructors().First().GetParameters();

            object result = Activator.CreateInstance(typeToCreate, parameterValues.ToArray());
            visitor.Subject = result;
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}