using System.Collections.Generic;

namespace AdaptableMapper.Builder
{
    public class Command
    {
        private readonly Queue<string> _commandParts;

        public Command(string command)
        {
            _commandParts = new Queue<string>(command.Split(' '));
        }

        public string Next()
            => _commandParts.Dequeue().ToLower();
    }
}