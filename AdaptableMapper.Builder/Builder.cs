using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Builder.Interpreters;

namespace AdaptableMapper.Builder
{
    public class Builder
    {
        private readonly List<Interpreter> _interpreters;

        public Builder()
        {
            _interpreters = new List<Interpreter>
            {
                new Create(),
                new Cache(),
                new DirectMap(),
                new CreateWithCache(),
                new DirectAddTo(),
                new CreateWithVariables()
            };
        }

        public MappingConfiguration Build(string[] sourceCommands)
        {
            var visitor = new Visitor();
            List<Command> commands = sourceCommands.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => new Command(c)).ToList();

            foreach (Command command in commands)
            {
                visitor.Command = command;
                string commandName = command.Next();
                Interpreter interpreter = _interpreters.FirstOrDefault(i => i.CommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase));

                interpreter?.Receive(visitor);
            }

            return visitor.Result;
        }
    }
}