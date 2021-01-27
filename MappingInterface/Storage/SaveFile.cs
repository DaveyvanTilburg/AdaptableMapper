using System.IO;

namespace MappingFramework.MappingInterface.Storage
{
    public class SaveFile
    {
        private readonly MappingConfiguration _mappingConfiguration;
        private readonly string _path;

        public SaveFile(MappingConfiguration mappingConfiguration, string path)
        {
            _mappingConfiguration = mappingConfiguration;
            _path = path;
        }

        public void Save()
            => File.WriteAllText(_path, _mappingConfiguration.ToString());
    }
}