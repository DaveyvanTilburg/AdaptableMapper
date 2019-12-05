using System.Collections.Generic;
using AdaptableMapper.Configuration.Model;

namespace AdaptableMapper.TDD.Cases.ModelCases
{
    public class Model
    {
        public static object CreateTarget(ContextType contextType, string type)
        {
            object result = null;

            switch (contextType)
            {
                case ContextType.EmptyString:
                    result = string.Empty;
                    break;
                case ContextType.EmptyObject:
                    switch (type)
                    {
                        case "item":
                            result = new ModelObjects.Simple.Item();
                            break;
                        case "mix":
                            result = new ModelObjects.Simple.Mix();
                            break;
                        case "deepmix":
                            result = new ModelObjects.Simple.DeepMix();
                            break;
                    }
                    
                    break;
                case ContextType.TestObject:
                    result = CreateTestItem();
                    break;
                case ContextType.InvalidType:
                    result = 0;
                    break;
                case ContextType.InvalidSource:
                    result = "abcd";
                    break;
                case ContextType.EmptySourceType:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(new ModelTargetInstantiatorSource());
                    break;
                case ContextType.InvalidSourceType:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(CreateModelTargetInstantiatorInvalidSource());
                    break;
                case ContextType.ValidSource:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(CreateModelTargetInstantiatorSource());
                    break;
            }

            return result;
        }

        private static ModelObjects.Simple.Item CreateTestItem()
        {
            return new ModelObjects.Simple.Item
            {
                Items = new List<ModelObjects.Simple.Item>
                {
                    new ModelObjects.Simple.Item
                    {
                        Code = "1",
                        Name = "Davey"
                    },
                    new ModelObjects.Simple.Item
                    {
                        Code = "2",
                        Name = "Joey"
                    }
                }
            };
        }

        public static ModelTargetInstantiatorSource CreateModelTargetInstantiatorInvalidSource()
        {
            var testType = typeof(ModelObjects.Simple.NoItem);
            var testModel = new ModelTargetInstantiatorSource
            {
                AssemblyFullName = testType.Assembly.FullName,
                TypeFullName = testType.FullName
            };

            return testModel;
        }

        public static ModelTargetInstantiatorSource CreateModelTargetInstantiatorSource()
        {
            var testType = typeof(ModelObjects.Simple.Item);
            var testModel = new ModelTargetInstantiatorSource
            {
                AssemblyFullName = testType.Assembly.FullName,
                TypeFullName = testType.FullName
            };

            return testModel;
        }
    }
}