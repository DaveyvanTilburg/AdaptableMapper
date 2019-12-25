using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            ConstructorInfo[] constructors = typeToCreate.GetConstructors();

            ConstructorInfo[] validConstructors =
                constructors.Where(c => c.GetParameters().Length == parameterValues.Count).ToArray();

            object result = null;
            foreach (ConstructorInfo constructorInfo in validConstructors)
            {
                List<ParameterInfo> parameterInfos = constructorInfo.GetParameters().ToList();

                List<object> constructorParameters = new List<object>();
                foreach (ParameterInfo parameterInfo in parameterInfos)
                {
                    Type parameterType = parameterInfo.ParameterType;
                    object changedValue = TypeDescriptor.GetConverter(parameterType).ConvertFromString(parameterValues[parameterInfos.IndexOf(parameterInfo)]);

                    constructorParameters.Add(changedValue);
                }

                result = constructorInfo.Invoke(constructorParameters.ToArray());
            }

            visitor.Subject = result;
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}