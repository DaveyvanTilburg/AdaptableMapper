using System.Collections.Generic;
using MappingFramework.Configuration.Model;
using MappingFramework.Model;

namespace MappingFramework.TDD.Cases.ModelCases
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
                    switch (type)
                    {
                        case "item":
                            result = CreateTestItem();
                            break;
                        case "deepmix":
                            result = CreateTestDeepMix();
                            break;
                    }
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
                case ContextType.ValidParent:
                    result = new List<ModelBase>();
                    break;
            }

            return result;
        }

        private static ModelObjects.Simple.Item CreateTestItem()
        {
            var result = new ModelObjects.Simple.Item();

            result.Items = new MappingFramework.Model.ModelList<ModelObjects.Simple.Item>(result)
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
            };

            return result;
        }

        private static ModelObjects.Simple.DeepMix CreateTestDeepMix()
        {
            var result = new ModelObjects.Simple.DeepMix();
            var mixes = new MappingFramework.Model.ModelList<ModelObjects.Simple.Mix>(result);
            result.Mixes = mixes;

            var mix = new ModelObjects.Simple.Mix()
            {
                Code = "1"
            };

            var mix2 = new ModelObjects.Simple.Mix()
            {
                Code = "2"
            };

            mix.Items = new MappingFramework.Model.ModelList<ModelObjects.Simple.Item>(mix)
            {
                new ModelObjects.Simple.Item(),
                new ModelObjects.Simple.Item()
            };

            mixes.Add(mix);
            mixes.Add(mix2);

            return result;
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