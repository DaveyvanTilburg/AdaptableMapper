using MappingFramework.Model;

namespace ModelObjects.Hardwares
{
    public class Root : ModelBase
    {
        public Root()
        {
            Motherboards = new ModelList<Motherboard>(this);
        }

        public ModelList<Motherboard> Motherboards { get; set; }
    }
}