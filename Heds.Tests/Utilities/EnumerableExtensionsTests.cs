using System.Linq;
using FluentAssertions;
using Heds.Utilities;
using Xunit;

namespace Heds.Tests.Utilities
{
    public class EnumerableExtensionsTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(100, false)]
        public void ContainsExactlyOneItemReturnsExpectedValue(int numItems, bool expected)
        {
            var source = Enumerable.Range(0, numItems).ToArray();
            source.ContainsExactlyOneItem().Should().Be(expected);
        }
    }
}