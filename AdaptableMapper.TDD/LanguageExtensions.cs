using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Process;
using FluentAssertions;

namespace AdaptableMapper.TDD
{
    public static class LanguageExtensions
    {
        public static void ValidateResult(this IReadOnlyCollection<Information> information, IReadOnlyCollection<string> expectedCodes)
        {
            information.Count.Should().Be(expectedCodes.Count, GetBecause(information, expectedCodes));

            foreach (IGrouping<string, string> expectedCodeGrouping in expectedCodes.GroupBy(c => c))
            {
                string[] expectedInformation = expectedCodeGrouping.First().Split('-');
                string type = expectedInformation[0];
                string code = expectedInformation[1];

                information.Count(i => i.Type.StartsWith(type) && i.Message.StartsWith(code)).Should().Be(expectedCodeGrouping.Count(), $"MissingCode: {code}, {GetBecause(information, expectedCodes)}");
            }
        }

        public static List<Information> Observe(this Action action)
        {
            var observer = new TestErrorObserver();
            observer.Register();

            action.Invoke();

            observer.Unregister();
            return observer.GetInformation();
        }

        private static string GetBecause(IReadOnlyCollection<Information> information, IReadOnlyCollection<string> expectedCodes)
        {
            var expectedFormatted = expectedCodes.Select(c => c.Substring(c.IndexOf('-') + 1, c.IndexOf(';') + 1 - (c.IndexOf('-') + 1)));

            IEnumerable<string> raisedCodes = information.Select(i => i.Message.Substring(0, i.Message.IndexOf(';')+1));
            IEnumerable<string> missingCodes = expectedFormatted.Except(raisedCodes);
            IEnumerable<string> extraCodes = raisedCodes.Except(expectedFormatted);

            string raised = string.Concat(raisedCodes);
            string missing = string.Concat(missingCodes);
            string extra = string.Concat(extraCodes);

            return $"Raised:'{raised}', Missing:'{missing}', Extra: '{extra}'";
        }
    }
}