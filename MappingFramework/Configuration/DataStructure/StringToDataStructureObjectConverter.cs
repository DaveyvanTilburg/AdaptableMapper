using System;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Configuration.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class StringToDataStructureObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "42dec10d-abb3-4c96-8c9a-dcc6798caa5a";
        public string TypeId => _typeId;

        public StringToDataStructureObjectConverter() { }

        public StringToDataStructureObjectConverter(DataStructureTargetInstantiatorSource dataStructureTargetInstantiatorSource)
        {
            DataStructureTargetInstantiatorSource = dataStructureTargetInstantiatorSource;
        }

        public DataStructureTargetInstantiatorSource DataStructureTargetInstantiatorSource { get; set; }

        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#27; source is not of expected type string", "error", source);
                return new NullDataStructure();
            }

            Type sourceType;
            try
            {
                sourceType = Activator.CreateInstance(
                    DataStructureTargetInstantiatorSource.AssemblyFullName,
                    DataStructureTargetInstantiatorSource.TypeFullName
                ).Unwrap().GetType();
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#28; could not instantiate sourceType", "error", DataStructureTargetInstantiatorSource, exception.GetType().Name, exception.Message);
                return new NullDataStructure();
            }

            object result;
            try
            {
                result = JsonSerializer.Deserialize(sourceType, input);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#29; could not deserialize to type", "error", source, exception.GetType().Name, exception.Message);
                return new NullDataStructure();
            }

            if (!(result is TraversableDataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#30; sourceType is not of type TraversableDataStructure", "error", DataStructureTargetInstantiatorSource);
                return new NullDataStructure();
            }

            return result;
        }
    }
}