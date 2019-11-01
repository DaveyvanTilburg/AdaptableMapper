namespace AdaptableMapper.Process
{
    public sealed partial class ProcessObservable
    {
        private static ProcessObservable _instance;
        public static ProcessObservable GetInstance()
        {
            return _instance ?? (_instance = new ProcessObservable());
        }
    }
}