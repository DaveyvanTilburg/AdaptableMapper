using MappingFramework.Converters;
using MappingFramework.Traversals;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Configuration.Json
{
    public class JsonChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "88a88d4e-b79e-471a-9866-04a217e2e890";
        public string TypeId => _typeId;

        public JsonChildCreator() { }

        public object CreateChild(Template template)
        {
            if (!(template.Parent is JArray jArray))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonChildCreator#1; Parent is not of expected type jArray", "error", template.Parent?.GetType().Name);
                return new JObject();
            }

            if (!(template.Child is JToken jTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonChildCreator#2; Template is not of expected type jToken", "error", template.Child?.GetType().Name);
                return new JObject();
            }

            var jTemplateCopy = jTemplate.DeepClone();
            return jTemplateCopy;
        }

        public void AddToParent(Template template, object newChild)
        {
            if (!(template.Parent is JArray jArray))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonChildCreator#3; Parent is not of expected type jArray", "error", template.Parent?.GetType().Name);
                return;
            }

            if (!(newChild is JToken jNewChild))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonChildCreator#4; Template is not of expected type jToken", "error", template.Child?.GetType().Name);
                return;
            }

            jArray.Add(jNewChild);
        }
    }
}