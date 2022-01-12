using System.Linq;
using Xunit;

namespace Reverse.Tests
{
    public class ReverserTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 })]
        public void ReverserReversesTheArray(int[] input, int[] output)
        {
            Reverser.Reverse(input, input.Length);

            Assert.True(input.SequenceEqual(output));
        }

        [Fact]
        public void ReverserReversesTheArrayAtTheSpecifiedIndex()
        {
            var input = new int[] { 1, 2, 3, 4 };
            var output = new int[] { 2, 1, 3, 4 };

            Reverser.Reverse(input, 2);

            Assert.True(input.SequenceEqual(output));
        }

        [Fact]
        public void ReversingAtIndexOneDoesNotChangeTheArray()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };

            Reverser.Reverse(input, 1);

            Assert.True(input.SequenceEqual(output));
        }
    }
}
