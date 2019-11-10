using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Process;
using FluentAssertions;

namespace AdaptableMapper.TDD.EdgeCases
{
    public static class LanguageExtensions
    {
        public static void ValidateResult(this IReadOnlyCollection<Information> information, IReadOnlyCollection<string> expectedCodes)
        {
            information.Count.Should().Be(expectedCodes.Count);

            foreach (IGrouping<string, string> expectedCodeGrouping in expectedCodes.GroupBy(c => c))
            {
                string code = expectedCodeGrouping.First();

                information.Count(i => i.Message.Contains(code)).Should().Be(expectedCodeGrouping.Count(), code);
                information.Any(i => i.Message.Contains(code)).Should().BeTrue(code);
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
    }
}