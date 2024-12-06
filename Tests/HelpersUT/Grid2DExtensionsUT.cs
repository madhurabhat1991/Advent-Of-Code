using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Xunit;

namespace Tests.HelpersUT
{
    public class Grid2DExtensionsUT
    {
        [Fact]
        public void TestCreateGrid2D()
        {
            string text = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";
            string[] lines = text.Split('\n');
            var grid = Grid2DExtensions.CreateGrid2D(lines);
            Assert.Equal(10, grid.GetLength(0));
            Assert.Equal(10, grid.GetLength(1));
            Assert.Equal('A', grid[4, 4]);
        }

        [Fact]
        public void TestWordSearch()
        {
            string text = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";
            string searchWord = "XMAS";
            string[] lines = text.Split('\n');
            var grid = Grid2DExtensions.CreateGrid2D(lines);
            long wordCount = Grid2DExtensions.WordSearch(grid, searchWord);
            Assert.Equal(18, wordCount);
        }
    }
}
