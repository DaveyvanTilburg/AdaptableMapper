namespace AdaptableMapper
{
    public sealed class NullObjectConverter : ResultObjectConverter
    {
        public object Convert(object source)
        {
            return source;
        }
    }
}