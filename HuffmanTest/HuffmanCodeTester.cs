using HuffmanCoding;

namespace HuffmanTest
{
    public class HuffmanCodeTester
    {
        [Fact]
        public void StringTest()
        {
            HuffmanCode huffman = new HuffmanCode();
            string text = "everythingisfine";
            string code = huffman.Encode(text);
            string original = huffman.Decode(code);
            Assert.False(code.Equals(text));
            Assert.True(original.Equals(text));
            Assert.False(code.Equals(original));
        }

        [Fact]
        public void ByteTest()
        {
            HuffmanCode huffman = new HuffmanCode();
            string text = "It works with lots of things for some reason, I don't even remember allowing this stuff to work though?";
            byte[] code = huffman.ByteEncode(text);
            string original = huffman.ByteDecode(code);
            Assert.False(code.Equals(text));
            Assert.True(original.Equals(text));
            Assert.False(code.Equals(original));
        }
    }
}