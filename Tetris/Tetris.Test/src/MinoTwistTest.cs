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

        #region Tspins
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
        #endregion

        #region ISpins
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

        [Test]
        public void TestISpinEndRight()
        {
            string[] text =
            {
                "      ",
                " XRX  ",
                "XX XXX",
                "XX    "
            };
            TestBase(text, Tetromino.I, () => mino.Rotate(Rotation.Right), () => new Point(4, board.Height-1 ));
        }

        [Test]
        public void TestISpinEndLeft()
        {
            string[] text =
            {
                "       ",
                "  X X  ",
                "XXXLXXX",
                "       "
            };
            TestBase(text, Tetromino.I, () => mino.Rotate(Rotation.Left), () => new Point(2, board.Height - 1));
        }
        #endregion

        #region SZSpins
        [Test]
        public void TestZSpinInPlaceRight()
        {
            string[] text =
            {
                "X   ",
                " R X",
                "X  X"
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestSSpinInPlaceLeft()
        {
            string[] text =
            {
                "   X",
                "X L ",
                "X  X"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Left), () => new Point(2, board.Height - 2));
        }

        [Test]
        public void TestSSpinTuckRight()
        {
            string[] text =
            {
                "    ",
                "R XX",
                "X  X",
                "  XX"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height-2));
        }

        [Test]
        public void TestZSpinTuckLeft()
        {
            string[] text =
            {
                "    ",
                "XX L",
                "X  X",
                "XX  "
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), () => new Point(2, board.Height - 2));
        }

        [Test]
        public void TestSSpinJumpRight()
        {
            string[] text =
            {
                "XX   ",
                "X 0  ",
                "X XXX",
                "X  XX",
                "XX XX"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestZSpinJumpLeft()
        {
            string[] text =
            {
                "   XX",
                "  0 X",
                "XXX X",
                "XX  X",
                "XX XX"
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), () => new Point(3, board.Height - 2));
        }

        [Test]
        public void TestZSpinTuckRightNoOverhang()
        {
            string[] text =
            {
                "    ",
                "XR X",
                "X  X",
                "XX  "
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Right), () => new Point(2, board.Height - 2));
        }

        [Test]
        public void TestSSpinTuckLeftNoOverhang()
        {
            string[] text =
            {
                "    ",
                "X LX",
                "X  X",
                "  XX"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Left), () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestSSpinJumpRightNoOverhang()
        {
            string[] text =
            {
                "    X",
                "  0XX",
                "X XXX",
                "X  XX",
                "XX XX"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), () => new Point(1, board.Height - 2));
        }

        [Test]
        public void TestZSpinJumpLeftNoOverhang()
        {
            string[] text =
            {
                "X    ",
                "XX0  ",
                "XXX X",
                "XX  X",
                "XX XX"
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), () => new Point(3, board.Height - 2));
        }
        #endregion

        #endregion
    }
}
