using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MappingFramework.MappingInterface.Storage
{
    public class Saves
    {
        public List<LoadFile> LoadFiles()
        {
            CreateSavesFolder();

            List<LoadFile> loadFiles = Directory.GetFiles(SavesFolder()).Select(f => new LoadFile(f)).ToList();
            return loadFiles;
        }
        
        public SaveFile NewSaveFile(MappingConfiguration mappingConfiguration, string name)
        {
            CreateSavesFolder();

            string path = Path.Combine(SavesFolder(), $"{name}.txt");
            return new SaveFile(mappingConfiguration, path);
        }
        
        public LoadFile Load(string fileName)
        {
            CreateSavesFolder();
            
            string path = Path.Combine(SavesFolder(), $"{fileName}.txt");
            return new LoadFile(path);
        }
        
        private string SavesFolder() => Path.Combine(Directory.GetCurrentDirectory(), "Saves");
        
        private void CreateSavesFolder()
        {
            string path = SavesFolder();

            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}