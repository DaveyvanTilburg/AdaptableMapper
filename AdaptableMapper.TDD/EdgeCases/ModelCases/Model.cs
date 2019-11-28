using System.Collections.Generic;

namespace AdaptableMapper.TDD.EdgeCases.ModelCases
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
                        Code = "1"
                    },
                    new ModelObjects.Simple.Item
                    {
                        Code = "2"
                    }
                }
            };
        }
    }
}