using System;
namespace MappingFramework.MappingInterface.Generics
{
    public class ListItemLink : IObjectLink
    {
        private readonly Action<object> _updateAction;
        private readonly Func<object> _valueAction;
        private readonly string _name;
        private readonly Type _itemType;

        public ListItemLink(Action<object> updateAction, Func<object> valueAction, string name, Type itemType)
        {
            _updateAction = updateAction;
            _valueAction = valueAction;
            _name = name;
            _itemType = itemType;
        }

        public ObjectComponentDisplayType Type()
            => ObjectComponentDisplayType.Item;

        public string Name()
            => _name;

        public Type PropertyType()
            => _itemType;

        public void Update(object value)
            => _updateAction(value);

        public object Value()
            => _valueAction();

        public Action<object> UpdateAction()
            => _updateAction;
    }
}