using Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.HelpersUT
{
    public class PacketParserUT
    {
        [Fact]
        public void TestObjectListToString()
        {
            List<object> objects = new List<object>();
            var result = PacketParser.ObjectListToString(objects);
            Assert.Equal("[]", result);

            objects = new List<object>() { 1 };
            result = PacketParser.ObjectListToString(objects);
            Assert.Equal("[1]", result);

            objects = new List<object>() { 1, 2, 3, 4 };
            result = PacketParser.ObjectListToString(objects);
            Assert.Equal("[1,2,3,4]", result);

            objects = new List<object>() {
                1,
                new List<object>(){2,3,4},
                5,
                new List<object>(){6,7}
            };
            result = PacketParser.ObjectListToString(objects);
            Assert.Equal("[1,[2,3,4],5,[6,7]]", result);

            objects = new List<object>() {
                new List<object>()
                {
                    1,
                    new List<object>(){2,3}
                },
                new List<object>()
                {
                    new List<object>(){4,5},
                    6,
                    new List<object>()
                    {
                        new List<object>{7},
                        new List<object>{8}
                    }
                }
            };
            result = PacketParser.ObjectListToString(objects);
            Assert.Equal("[[1,[2,3]],[[4,5],6,[[7],[8]]]]", result);
        }

        [Fact]
        public void TestCompareElements()
        {
            char start = '[', end = ']';

            Assert.Equal(0, PacketParser.CompareElements(100, 100));
            Assert.Equal(-1, PacketParser.CompareElements(50, 75));
            Assert.Equal(1, PacketParser.CompareElements(360, -50));

            List<object> first = PacketParser.Parse("[1,1,3,1,1]", start, end);
            List<object> second = PacketParser.Parse("[1,1,5,1,1]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[2,3,4]]", start, end);
            second = PacketParser.Parse("[4]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[9]", start, end);
            second = PacketParser.Parse("[[8,7,6]]", start, end);
            Assert.Equal(1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[4,4],4,4]", start, end);
            second = PacketParser.Parse("[[4,4],4,4,4]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[7,7,7,7]", start, end);
            second = PacketParser.Parse("[7,7,7]", start, end);
            Assert.Equal(1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[]", start, end);
            second = PacketParser.Parse("[3]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[[]]]", start, end);
            second = PacketParser.Parse("[[]]", start, end);
            Assert.Equal(1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[1,[2,[3,[4,[5,6,7]]]],8,9]", start, end);
            second = PacketParser.Parse("[1,[2,[3,[4,[5,6,0]]]],8,9]", start, end);
            Assert.Equal(1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[1],[2,3,4]]", start, end);
            second = PacketParser.Parse("[1,[2,[3,[4,[5,6,7]]]],8,9]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[1],[2,3,4]]", start, end);
            second = PacketParser.Parse("[1,[2,[3,[4,[5,6,0]]]],8,9]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[[4,4],4,4,4]", start, end);
            second = PacketParser.Parse("[[6]]", start, end);
            Assert.Equal(-1, PacketParser.CompareElements(first, second));

            first = PacketParser.Parse("[7,7,7]", start, end);
            second = PacketParser.Parse("[[6]]", start, end);
            Assert.Equal(1, PacketParser.CompareElements(first, second));
        }

        [Fact]
        public void TestParseInt()
        {
            Queue<char> queue = PacketParser.StringToQueue("689,85]");
            int result = PacketParser.ParseInt(queue);
            Assert.Equal(689, result);
            Assert.Equal(",85]", string.Join("", queue));

            queue = PacketParser.StringToQueue("85]");
            result = PacketParser.ParseInt(queue);
            Assert.Equal(85, result);
            Assert.Equal("]", string.Join("", queue));
        }

        [Fact]
        public void TestParseList()
        {
            char start = '[', end = ']';

            Queue<char> queue = PacketParser.StringToQueue("[]");
            List<object> result = PacketParser.ParseList(queue, start, end);
            List<object> expected = new List<object>() { };
            Assert.Equal(expected, result);

            queue = queue = PacketParser.StringToQueue("[1]");
            result = PacketParser.ParseList(queue, start, end);
            expected = new List<object>() { 1 };
            Assert.Equal(expected, result);

            queue = queue = PacketParser.StringToQueue("[1, 2]");
            result = PacketParser.ParseList(queue, start, end);
            expected = new List<object>() { 1, 2 };
            Assert.Equal(expected, result);

            queue = queue = PacketParser.StringToQueue("[1,2,3,4]");
            result = PacketParser.ParseList(queue, start, end);
            expected = new List<object>() { 1, 2, 3, 4 };
            Assert.Equal(expected, result);

            queue = queue = PacketParser.StringToQueue("[1,[2,3],4,[5,6]]");
            result = PacketParser.ParseList(queue, start, end);
            expected = new List<object>()
            {
                1,
                new List<object>(){2,3},
                4,
                new List<object>(){5,6}
            };
            Assert.Equal(expected, result);

            queue = queue = PacketParser.StringToQueue("[[1,[2,[3]]]]");
            result = PacketParser.ParseList(queue, start, end);
            expected = new List<object>()
            {
                new List<object>()
                {
                    1,
                    new List<object>()
                    {
                        2,
                        new List<object>()
                        {
                            3
                        }
                    }
                }
            };
            Assert.Equal(expected, result);
        }
    }
}
