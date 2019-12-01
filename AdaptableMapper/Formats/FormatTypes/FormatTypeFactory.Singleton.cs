namespace AdaptableMapper.Formats.FormatTypes
{
    internal partial class FormatTypeFactory
    {
        private static FormatTypeFactory _instance;

        public static FormatTypeFactory GetInstance()
            => _instance ?? (_instance = new FormatTypeFactory());
    }
}