namespace HuffmanTest
{
    public class HuffmanCodeTester
    {
        [Fact]
        public void Test1()
        {
            char c = 'A';
            byte value = (byte) (c%65);

            Assert.True(value == 0);
        }
    }
}