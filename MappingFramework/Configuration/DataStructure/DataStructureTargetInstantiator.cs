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

        public object Create(Context context, object source)
        {
            if (!(source is string template))
            {
                context.InvalidInput(source, typeof(string));
                return new NullDataStructure();
            }

            DataStructureTargetInstantiatorSource dataStructureTargetInstantiatorSource;
            try
            {
                dataStructureTargetInstantiatorSource = JsonSerializer.Deserialize<DataStructureTargetInstantiatorSource>(template);
            }
            catch(Exception exception)
            {
                context.OperationFailed(this, exception);
                return new NullDataStructure();
            }

            object result;
            try
            {
                result = Activator.CreateInstance(dataStructureTargetInstantiatorSource.AssemblyFullName, dataStructureTargetInstantiatorSource.TypeFullName).Unwrap();
            }
            catch(Exception exception)
            {
                context.OperationFailed(this, exception);
                return new NullDataStructure();
            }

            if (!(result is TraversableDataStructure))
            {
                context.InvalidType(result, typeof(TraversableDataStructure));
                return new NullDataStructure();
            }

            return result;
        }
    }
}