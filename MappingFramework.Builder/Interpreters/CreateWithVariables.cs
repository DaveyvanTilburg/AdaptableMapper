using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MappingFramework.Builder.Interpreters
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

            Assembly MappingFrameworkAssembly = GetAssemblyByName("MappingFramework");
            Type[] types = MappingFrameworkAssembly.GetTypes();
            Type typeToCreate = types.FirstOrDefault(t => t.Name.Equals(typeToCreateName, StringComparison.OrdinalIgnoreCase));

            ConstructorInfo[] constructors = typeToCreate.GetConstructors();

            ConstructorInfo[] validConstructors =
                constructors.Where(c => c.GetParameters().Length == parameterValues.Count).ToArray();

            object result = null;
            foreach (ConstructorInfo constructorInfo in validConstructors)
            {
                bool convertSuccessful = TryConvertParametersForConstructor(constructorInfo, parameterValues, out object[] constructorParameters);

                if (!convertSuccessful)
                    continue;

                result = constructorInfo.Invoke(constructorParameters.ToArray());
            }

            if (result == null)
                throw new Exception($"No constructor found for type {typeToCreateName} with parameters {string.Join(",", parameterValues)}");

            visitor.Subject = result;
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        private bool TryConvertParametersForConstructor(ConstructorInfo constructorInfo, List<string> parameterValues, out object[] result)
        {
            List<ParameterInfo> parameterInfos = constructorInfo.GetParameters().ToList();

            List<object> constructorParameters = new List<object>();
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                string parameterValue = parameterValues[parameterInfos.IndexOf(parameterInfo)];
                bool convertSuccessful = TryConvertToParameterType(parameterInfo, parameterValue, out object convertedParameter);

                if (!convertSuccessful)
                {
                    result = null;
                    return false;
                }
                
                constructorParameters.Add(convertedParameter);
            }

            result = constructorParameters.ToArray();
            return true;
        }
        private bool TryConvertToParameterType(ParameterInfo targetType, string source, out object result)
        {
            try
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(targetType.ParameterType);
                result = typeConverter.ConvertFromString(source);
            }
            catch
            {
                result = null;
                return false;
            }

            return true;
        }

    }
}