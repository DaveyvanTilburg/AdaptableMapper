using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Builder
{
    public class Builder
    {
        public Builder()
        {
            _resultObjectConverterBuilder = new ResultObjectConverterBuilder();
        }

        private readonly ResultObjectConverterBuilder _resultObjectConverterBuilder;

        public MappingConfiguration Build(string[] sourceCommands)
        {
            var visitor = new Visitor();

            List<Command> commands = sourceCommands.Select(c => new Command(c)).ToList();

            foreach (Command command in commands)
            {
                visitor.Command = command;

                switch (command.Next())
                {
                    case "resultobjectconverterbuilder":
                        _resultObjectConverterBuilder.Receive(visitor);
                        break;
                }
            }

            return visitor.Result;
        }
    }
}