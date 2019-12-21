using AdaptableMapper.Configuration;
using AdaptableMapper.Configuration.Xml;

namespace AdaptableMapper.Builder
{
    internal class ResultObjectConverterBuilder
    {
        public void Receive(Visitor visitor)
        {
            switch (visitor.Command.Next())
            {
                case "create":
                    Create(visitor);
                    break;
                case "finish":
                    Finish(visitor);
                    break;
            }
        }

        private void Create(Visitor visitor)
        {
            switch (visitor.Command.Next())
            {
                case "xelementtostringobjectconverter":
                    visitor.Subject = new XElementToStringObjectConverter();
                    break;
            }
        }

        private void Finish(Visitor visitor)
        {
            visitor.Result.ResultObjectConverter = visitor.Subject as ResultObjectConverter;
        }
    }
}