using System.Collections.Generic;

namespace MappingFramework.Builder
{
    public class Command
    {
        private readonly Queue<string> _commandParts;

        public Command(string command)
            => _commandParts = new Queue<string>(command.Split(' '));

        public string Next()
        {
            if (_commandParts.Count == 0)
                return string.Empty;

            return _commandParts.Dequeue();
        }
    }
}