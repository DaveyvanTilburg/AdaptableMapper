namespace AdaptableMapper.Errors
{
    public sealed partial class ErrorObservable
    {
        private static ErrorObservable _instance;
        public static ErrorObservable GetInstance()
        {
            if (_instance == null)
                _instance = new ErrorObservable();

            return _instance;
        }
    }
}