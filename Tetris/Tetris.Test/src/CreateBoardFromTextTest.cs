using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Tetris.Test.src
{
    [TestFixture]
    class CreateBoardFromTextTest
    {
        [Test]
        public void TestCreateSimpleX()
        {
            string[] text =
            {
                "X X",
                " X ",
                "X X"
            };
            RectangularTetrisBoard board = RectangularTetrisBoard.CreateBoardFromText( text, 4 );
            Assert.AreEqual(board.Width, 3, "Width is incorrect");
            Assert.AreEqual(board.Height, 8, "Height is incorrect");
            Utils.PrintBoard(board);
            Point p = new Point();
            for( p.y = 5; p.y < board.Height; p.y++ )
            {
                for( p.x = 0; p.x < board.Width; p.x++ )
                {
                    if ((p.y + p.x) % 2 == 1)
                    {
                        Console.WriteLine(p);
                        Assert.That(board[p].Occupied);
                    }
                }
            }
            Assert.That(!board.Lost, "Board is dead");
        }
    }
}
