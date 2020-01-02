using System;
using System.Collections.Generic;
using System.Reflection;

namespace AdaptableMapper.Converters
{
    internal class TypeCollection
    {
        private readonly List<Type> _types;

        public TypeCollection(IEnumerable<Type> types)
            => _types = new List<Type>(types);

        public Type GetType(string typeId)
        {
            Type result = null;

            foreach (Type foundType in _types)
            {
                FieldInfo typeIdConstant = foundType.GetField("_typeId");
                object typeIdConstantValue = typeIdConstant.GetRawConstantValue();

                if (typeId.Equals((string)typeIdConstantValue))
                {
                    result = foundType;
                    break;
                }
            }

            if (result == null)
                throw new ArgumentException($"Invalid typeId: {typeId}");

            return result;
        }
    }
}