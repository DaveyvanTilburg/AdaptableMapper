namespace AdaptableMapper.TDD.ATDD
{
    public class MappingConfigurationBuilder
    {
        private MappingConfiguration _result;
        public MappingConfigurationBuilder()
        {
            StartNew();
        }

        public void StartNew()
        {
            _result = new MappingConfiguration(null, null, null);
        }

        public MappingConfiguration GetResult()
        {
            MappingConfiguration tempResult = _result;
            StartNew();

            return tempResult;
        }
    }
}