using System;

using NUnit.Framework;

namespace Tetris.Test
{
    [TestFixture]
    class MinoTwistTest
    {
        RectangularTetrisBoard board;
        Mino mino;

        [OneTimeSetUp]
        public void init()
        {
            board = new RectangularTetrisBoard(4, 4, 0);
            mino = Mino.CreateNewMino(board, Tetromino.T);
        }

        void TestBase(string[] text, MinoType type, Action TestActions, Func<Point> ExpectedPosition)
        {
            board = RectangularTetrisBoard.CreateBoardFromText(text, 0);
            mino = Utils.SetupMino(board, text, type);
            Utils.PrintBoard(board);
            Console.WriteLine();
            TestActions();
            Utils.PrintBoard(board);
            Console.WriteLine();
            Assert.AreEqual(ExpectedPosition(), mino.Position);
        }

        #region Tests
        [Test]
        public void TestTSpinTimeSaverRight()
        {
            string[] text =
            {
             "X_____",
             "XR_X__",
             "X___X_",
             "XX_X__"
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), () => new Point(2, board.Height - 2));
        }

        [Test]
        public void TestTSpinTimeSaverLeft()
        {
            string[] text =
            {
             "X_____",
             "XX_LX_",
             "X___X_",
             "XX_X__"
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), () => new Point(2, board.Height - 2));
        }

        [Test]
        public void TestTSpinSingleRight()
        {
            string[] text =
            {
                "   ",
                "X L",
                "   "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height - 1));
        }

        [Test]
        public void TestTSpinSingleLeft()
        {
            string[] text =
            {
                "   ",
                "R X",
                "   "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), () => new Point(1, board.Height - 1));
        }

        [Test]
        public void TestTSpinTripleRight()
        {
            string[] text =
            {
                "  XX   ",
                "  X 0  ",
                " XX XXX",
                " XX  XX",
                " XX XXX"
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), () => new Point(3, board.Height - 2));
        }

        [Test]
        public void TestTSpinTripleLeft()
        {
            string[] text =
            {
                "   XX  ",
                "  0 X  ",
                "XXX XX ",
                "XX  XX ",
                "XXX XX "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), () => new Point(3, board.Height - 2));
        }

        [Test]
        public void TestTSpinVerticalSingleRight()
        {
            string[] text =
            {
                "XX   ",
                "X    ",
                "X 0  ",
                "X XX "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestTSpinVerticalSingleLeft()
        {
            string[] text =
            {
                "   XX",
                "    X",
                "  0 X",
                " XX X"
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), () => new Point(3, board.Height - 2));
        }

        [Test]
        public void TestTSpinComplicated()
        {
            string[] text =
            {
                "XX  ",
                "  R ",
                "X  X",
                "X  X",
                "X XX"
            };
            TestBase(text, Tetromino.T, () =>
            {
                mino.Rotate(Rotation.Right);
                mino.Move(-1, 0);
                mino.Rotate(Rotation.Left);
            }, () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestISpinCross()
        {
            string[] text =
            {
                "    ",
                "X XX",
                "    ",
                "XLXX",
                "X XX"
            };
            TestBase(text, Tetromino.I, () =>
            {
                mino.Rotate(Rotation.Right);
            }, () => new Point(1, board.Height - 3));
        }
        #endregion
    }
}
