using System.Collections.Generic;

namespace MappingFramework.Languages.DataStructure
{
    public sealed class ChildList<T> : List<T> where T : TraversableDataStructure
    {
        private readonly TraversableDataStructure _parent;

        public ChildList(TraversableDataStructure parent)
        {
            _parent = parent;
        }

        public new void Add(T dataStructure)
        {
            dataStructure.Parent = _parent;

            base.Add(dataStructure);
        }
    }
}