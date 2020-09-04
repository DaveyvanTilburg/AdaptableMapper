using System.Collections.Generic;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.DataStructure;
using MappingFramework.TDD.Simple;

namespace MappingFramework.TDD.Cases.DataStructureCases
{
    public class DataStructure
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
                            result = new Item();
                            break;
                        case "mix":
                            result = new Mix();
                            break;
                        case "deepmix":
                            result = new DeepMix();
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
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(new DataStructureTargetInstantiatorSource());
                    break;
                case ContextType.InvalidSourceType:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(CreateModelTargetInstantiatorInvalidSource());
                    break;
                case ContextType.ValidSource:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(CreateModelTargetInstantiatorSource());
                    break;
                case ContextType.ValidParent:
                    result = new List<TraversableDataStructure>();
                    break;
            }

            return result;
        }

        private static Item CreateTestItem()
        {
            var result = new Item();

            result.Items = new ChildList<Item>(result)
            {
                new Item
                {
                    Code = "1",
                    Name = "Davey"
                },
                new Item
                {
                    Code = "2",
                    Name = "Joey"
                }
            };

            return result;
        }

        private static DeepMix CreateTestDeepMix()
        {
            var result = new DeepMix();
            var mixes = new ChildList<Mix>(result);
            result.Mixes = mixes;

            var mix = new Mix()
            {
                Code = "1"
            };

            var mix2 = new Mix()
            {
                Code = "2"
            };

            mix.Items = new ChildList<Item>(mix)
            {
                new Item(),
                new Item()
            };

            mixes.Add(mix);
            mixes.Add(mix2);

            return result;
        }

        public static DataStructureTargetInstantiatorSource CreateModelTargetInstantiatorInvalidSource()
        {
            var testType = typeof(NoItem);
            var testModel = new DataStructureTargetInstantiatorSource
            {
                AssemblyFullName = testType.Assembly.FullName,
                TypeFullName = testType.FullName
            };

            return testModel;
        }

        public static DataStructureTargetInstantiatorSource CreateModelTargetInstantiatorSource()
        {
            var testType = typeof(Item);
            var testModel = new DataStructureTargetInstantiatorSource
            {
                AssemblyFullName = testType.Assembly.FullName,
                TypeFullName = testType.FullName
            };

            return testModel;
        }
    }
}