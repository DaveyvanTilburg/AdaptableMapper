namespace AdaptableMapper
{
    public sealed class NullObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            return source;
        }
    }
}