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
                string code = expectedCodeGrouping.First();

                information.Count(i => i.Message.Contains(code)).Should().Be(expectedCodeGrouping.Count(), $"MissingCode: {code}, {GetBecause(information, expectedCodes)}");
                information.Any(i => i.Message.Contains(code)).Should().BeTrue(code, GetBecause(information, expectedCodes));
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
            IEnumerable<string> raisedCodes = information.Select(i => i.Message.Substring(0, i.Message.IndexOf(";")+1));
            IEnumerable<string> missingCodes = expectedCodes.Except(raisedCodes);

            string raised = string.Join("", raisedCodes);
            string missing = string.Join("", missingCodes);

            return $"Raised:'{raised}', Missing:'{missing}'";
        }
    }
}