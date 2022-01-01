using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Json;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureTargetCreator : TargetCreator, ResolvableByTypeId
    {
        public const string _typeId = "6a36996c-2376-45f3-b556-a0e66da9a891";
        public string TypeId => _typeId;

        public string SerializedCreatorSource { get; set; }

        public DataStructureTargetCreator() { }

        public DataStructureTargetCreator(string serializedCreatorSource)
        {
            SerializedCreatorSource = serializedCreatorSource;
        }

        public object Create(Context context, object source)
        {
            if (string.IsNullOrWhiteSpace(SerializedCreatorSource))
            {
                context.PropertyIsEmpty(this, nameof(SerializedCreatorSource));
                return new NullDataStructure();
            }

            DataStructureTargetCreatorSource dataStructureTargetCreatorSource;
            try
            {
                dataStructureTargetCreatorSource = JsonSerializer.Deserialize<DataStructureTargetCreatorSource>(SerializedCreatorSource);
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

            if (result is not TraversableDataStructure)
            {
                context.InvalidType(result, typeof(TraversableDataStructure));
                return new NullDataStructure();
            }

            return result;
        }

        public string SerializeExample()
        {
            object instance = Create(null, null);
            Populate(instance);

            return JsonSerializer.Serialize(instance);
        }

        private void Populate(object instance)
        {
            foreach (PropertyInfo propertyInfo in IterateAndDiscoverChildLists(instance))
                AddOneChildToChildLists(instance, propertyInfo);
        }

        private void AddOneChildToChildLists(object target, PropertyInfo propertyInfo)
        {
            object child = CreateChild(propertyInfo);
            IList list = propertyInfo.GetValue(target) as IList;
            list.Add(child);

            Populate(child);
        }

        private IEnumerable<PropertyInfo> IterateAndDiscoverChildLists(object target)
        {
            Type instanceType = target.GetType();

            IEnumerable<PropertyInfo> propertyInfos = instanceType.GetProperties().AsEnumerable();
            IEnumerable<PropertyInfo> childListProperties = propertyInfos.Where(p => p.PropertyType.Name.Contains("childlist", StringComparison.OrdinalIgnoreCase));

            foreach (PropertyInfo propertyInfo in childListProperties)
            {
                if (propertyInfo.GetValue(target) is not IList)
                    continue;

                Type childType = propertyInfo.PropertyType.GetGenericArguments()[0];
                if (target.GetType() == childType)
                    continue;

                yield return propertyInfo;
            }
        }

        private object CreateChild(PropertyInfo propertyInfo)
            => propertyInfo.PropertyType.CreateDataStructure(null);
    }
}