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
        public void TestParseNextInt()
        {
            Queue<char> queue = PacketParser.StringToQueue("689,85]");
            int result = PacketParser.ParseNextInt(queue);
            Assert.Equal(689, result);

            queue = PacketParser.StringToQueue("85]");
            result = PacketParser.ParseNextInt(queue);
            Assert.Equal(85, result);

            queue = PacketParser.StringToQueue(",1],2],3],4]");
            result = PacketParser.ParseNextInt(queue);
            Assert.Equal(1, result);

            queue = PacketParser.StringToQueue("]]]]],10],2],3],4]");
            result = PacketParser.ParseNextInt(queue);
            Assert.Equal(10, result);

            queue = PacketParser.StringToQueue("]]]]");
            try
            {
                result = PacketParser.ParseNextInt(queue);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("No int found", ex.Message);
            }

            queue = PacketParser.StringToQueue("[]]],[],[[]],[]]");
            try
            {
                result = PacketParser.ParseNextInt(queue);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("No int found", ex.Message);
            }

            queue = PacketParser.StringToQueue("[689,85");
            result = PacketParser.ParseNextInt(queue, false);
            Assert.Equal(85, result);

            queue = PacketParser.StringToQueue("[689,85]");
            result = PacketParser.ParseNextInt(queue, false);
            Assert.Equal(85, result);

            queue = PacketParser.StringToQueue("[689,");
            result = PacketParser.ParseNextInt(queue, false);
            Assert.Equal(689, result);

            queue = PacketParser.StringToQueue("[1,[2,[3,[4,");
            result = PacketParser.ParseNextInt(queue, false);
            Assert.Equal(4, result);

            queue = PacketParser.StringToQueue("[1,[2,[3,[14,[[[[");
            result = PacketParser.ParseNextInt(queue, false);
            Assert.Equal(14, result);

            queue = PacketParser.StringToQueue("[[[[");
            try
            {
                result = PacketParser.ParseNextInt(queue, false);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("No int found", ex.Message);
            }

            queue = PacketParser.StringToQueue("[[],[[],[]]]");
            try
            {
                result = PacketParser.ParseNextInt(queue, false);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Equal("No int found", ex.Message);
            }
        }

        [Fact]
        public void TestAddToNextInt()
        {
            string input = "689,85]";
            int add = 4;
            string result = PacketParser.AddToNextInt(input, add);
            Assert.Equal("693,85]", result);

            input = "85]";
            add = 10;
            result = PacketParser.AddToNextInt(input, add);
            Assert.Equal("95]", result);

            input = ",1],2],3],4]";
            add = 50;
            result = PacketParser.AddToNextInt(input, add);
            Assert.Equal(",51],2],3],4]", result);

            input = "]]]]],10],2],3],4]";
            add = 10;
            result = PacketParser.AddToNextInt(input, add);
            Assert.Equal("]]]]],20],2],3],4]", result);

            input = "]]]]";
            add = 10;
            result = PacketParser.AddToNextInt(input, add);
            Assert.Equal("]]]]", result);

            input = "[]]],[],[[]],[]]";
            add = 10;
            result = PacketParser.AddToNextInt(input, add);
            Assert.Equal("[]]],[],[[]],[]]", result);

            input = "[689,85";
            add = 4;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[689,89", result);

            input = "[689,85]";
            add = 10;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[689,95]", result);

            input = "[689,";
            add = 1;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[690,", result);

            input = "[1,[2,[3,[4,";
            add = 50;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[1,[2,[3,[54,", result);

            input = "[1,[2,[3,[14,[[[[";
            add = 50;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[1,[2,[3,[64,[[[[", result);

            input = "[[[[";
            add = 50;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[[[[", result);

            input = "[[],[[],[]]]";
            add = 50;
            result = PacketParser.AddToNextInt(input, add, false);
            Assert.Equal("[[],[[],[]]]", result);
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

        [Fact]
        public void TestCheckExplode()
        {
            char start = '[', end = ']';
            int explodeLimit = 4;

            int index = 0;

            string input = "[[[[[9,8],1],2],3],4]";
            var explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.True(explode);
            Assert.Equal(4, index);

            input = "[7,[6,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.True(explode);
            Assert.Equal(12, index);

            input = "[[6,[5,[4,[3,2]]]],1]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.True(explode);
            Assert.Equal(10, index);

            input = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.True(explode);
            Assert.Equal(10, index);

            input = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.True(explode);
            Assert.Equal(24, index);

            input = "[[[[1,1],[2,2]],[3,3]],[4,4]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            Assert.False(explode);
            Assert.Equal(0, index);
        }

        [Fact]
        public void TestExplode()
        {
            char start = '[', end = ']';
            int explodeLimit = 4;

            int index = 0;

            string input = "[[[[[9,8],1],2],3],4]";
            var explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            var result = PacketParser.Explode(input, start, end, index);
            Assert.Equal("[[[[0,9],2],3],4]", result);

            input = "[7,[6,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            result = PacketParser.Explode(input, start, end, index);
            Assert.Equal("[7,[6,[5,[7,0]]]]", result);

            input = "[[6,[5,[4,[3,2]]]],1]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            result = PacketParser.Explode(input, start, end, index);
            Assert.Equal("[[6,[5,[7,0]]],3]", result);

            input = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            result = PacketParser.Explode(input, start, end, index);
            Assert.Equal("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", result);

            input = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
            explode = PacketParser.CheckExplode(input, start, end, explodeLimit, out index);
            result = PacketParser.Explode(input, start, end, index);
            Assert.Equal("[[3,[2,[8,0]]],[9,[5,[7,0]]]]", result);
        }

        [Fact]
        public void TestCheckSplit()
        {
            int splitLimit = 10;

            int index = 0;
            int length = 0;

            string input = "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]";
            var split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            Assert.False(split);
            Assert.Equal(0, index);

            input = "[[[[0,7],4],[7,[[8,4],9]]],[1,1]]";
            split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            Assert.False(split);
            Assert.Equal(0, index);

            input = "[[[[0,7],4],[15,[0,13]]],[1,1]]";
            split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            Assert.True(split);
            Assert.Equal(13, index);
            Assert.Equal(2, length);

            input = "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]";
            split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            Assert.True(split);
            Assert.Equal(22, index);
            Assert.Equal(2, length);
        }

        [Fact]
        public void TestSplit()
        {
            char start = '[', end = ']';
            int splitLimit = 10;

            int index = 0;
            int length = 0;

            string input = "[[[[0,7],4],[15,[0,13]]],[1,1]]";
            var split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            var result = PacketParser.Split(input, start, end, index, length);
            Assert.Equal("[[[[0,7],4],[[7,8],[0,13]]],[1,1]]", result);

            input = "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]";
            split = PacketParser.CheckSplit(input, splitLimit, out index, out length);
            result = PacketParser.Split(input, start, end, index, length);
            Assert.Equal("[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]", result);
        }

        [Fact]
        public void TestGetMagnitude()
        {
            char start = '[';
            string input = "[[1,2],[[3,4],5]]";
            var result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(143, result);

            input = "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(1384, result);

            input = "[[[[1,1],[2,2]],[3,3]],[4,4]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(445, result);

            input = "[[[[3,0],[5,3]],[4,4]],[5,5]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(791, result);

            input = "[[[[5,0],[7,4]],[5,5]],[6,6]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(1137, result);

            input = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(3488, result);

            input = "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]";
            result = PacketParser.GetMagnitude(input, start);
            Assert.Equal(4140, result);
        }
    }
}
