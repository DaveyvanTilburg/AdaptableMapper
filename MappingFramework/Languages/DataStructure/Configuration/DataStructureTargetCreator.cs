using System;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureTargetCreator : TargetCreator, ResolvableByTypeId
    {
        public const string _typeId = "6a36996c-2376-45f3-b556-a0e66da9a891";
        public string TypeId => _typeId;

        public DataStructureTargetCreator() { }

        public object Create(Context context, object source)
        {
            if (!(source is string template))
            {
                context.InvalidInput(source, typeof(string));
                return new NullDataStructure();
            }

            DataStructureTargetCreatorSource dataStructureTargetCreatorSource;
            try
            {
                dataStructureTargetCreatorSource = JsonSerializer.Deserialize<DataStructureTargetCreatorSource>(template);
            }
            catch(Exception exception)
            {
                context.OperationFailed(this, exception);
                return new NullDataStructure();
            }

            object result;
            try
            {
                result = Activator.CreateInstance(dataStructureTargetCreatorSource.AssemblyFullName, dataStructureTargetCreatorSource.TypeFullName).Unwrap();
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