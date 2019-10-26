using XPathSerialization.XPathConfigurations;

namespace XPathSerialization.Traversals.AdaptableTraversals
{
    public class AdaptableGet : GetTraversal
    {
        public string Path { get; set; }

        public string GetValue(object source)
        {
            if(!(source is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(Path);

            Adaptable pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            return value;
        }
    }
}