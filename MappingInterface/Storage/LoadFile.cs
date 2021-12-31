using System.IO;
using MappingFramework.Json;

namespace MappingFramework.MappingInterface.Storage
{
    public class LoadFile
    {
        private readonly string _path;

        public LoadFile(string path)
        {
            _path = path;
        }

        public string Name() => Path.GetFileNameWithoutExtension(_path);

        public void Delete() => File.Delete(_path);
        
        public MappingConfiguration MappingConfiguration()
            => JsonSerializer.Deserialize<MappingConfiguration>(File.ReadAllText(_path));
    }
}