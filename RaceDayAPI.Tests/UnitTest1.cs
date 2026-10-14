
using Xunit;

namespace RaceDayAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SampleTest_ShouldPass()
        {
            // Arrange
            int expected = 10;

            // Act
            int actual = 5 + 5;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
