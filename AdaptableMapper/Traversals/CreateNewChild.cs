namespace AdaptableMapper.Traversals
{
    public abstract class CreateNewChild
    {
        private object _template;

        public object CreateChildOn(object target)
        {
            if (_template == null)
                _template = GetTemplate(target);

            object newChild = DuplicateTemplate(_template);
            return newChild;
        }

        protected abstract object GetTemplate(object target);

        protected abstract object DuplicateTemplate(object template);
    }
}