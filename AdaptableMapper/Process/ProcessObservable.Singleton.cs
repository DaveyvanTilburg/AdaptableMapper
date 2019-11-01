namespace AdaptableMapper.Errors
{
    public sealed partial class ProcessObservable
    {
        private static ProcessObservable _instance;
        public static ProcessObservable GetInstance()
        {
            if (_instance == null)
                _instance = new ProcessObservable();

            return _instance;
        }
    }
}