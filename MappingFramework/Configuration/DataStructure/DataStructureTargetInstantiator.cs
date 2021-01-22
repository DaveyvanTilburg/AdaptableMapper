using System;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Configuration.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureTargetInstantiator : TargetInstantiator, ResolvableByTypeId
    {
        public const string _typeId = "6a36996c-2376-45f3-b556-a0e66da9a891";
        public string TypeId => _typeId;

        public DataStructureTargetInstantiator() { }

        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#25; Source is not of expected type string", "error", source, source?.GetType().Name);
                return new NullDataStructure();
            }

            DataStructureTargetInstantiatorSource dataStructureTargetInstantiatorSource;
            try
            {
                dataStructureTargetInstantiatorSource = JsonSerializer.Deserialize<DataStructureTargetInstantiatorSource>(template);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#26; string does not contain a serialized DataStructureTargetInstantiatorSource", "error", template, exception.GetType().Name, exception.Message);
                return new NullDataStructure();
            }

            object result;
            try
            {
                result = Activator.CreateInstance(dataStructureTargetInstantiatorSource.AssemblyFullName, dataStructureTargetInstantiatorSource.TypeFullName).Unwrap();
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#24; assembly and typename could not be instantiated", "error", dataStructureTargetInstantiatorSource, exception.GetType().Name, exception.Message);
                return new NullDataStructure();
            }

            if (!(result is TraversableDataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#31; instantiated object is not of type TraversableDataStructure", "error", dataStructureTargetInstantiatorSource);
                return new NullDataStructure();
            }

            return result;
        }
    }
}