using System.Collections.Generic;

namespace MappingFramework.Model
{
    public sealed class ModelList<T> : List<T> where T : ModelBase
    {
        private readonly ModelBase _parent;

        public ModelList(ModelBase parent)
        {
            _parent = parent;
        }

        public new void Add(T model)
        {
            model.Parent = _parent;

            base.Add(model);
        }
    }
}