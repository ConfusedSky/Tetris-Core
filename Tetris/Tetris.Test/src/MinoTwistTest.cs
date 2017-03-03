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

        void TestBase(string[] text, MinoType type, Action TestActions, Point position)
        {
            board = RectangularTetrisBoard.CreateBoardFromText(text, 0);
            mino = Utils.SetupMino(board, text, type);
            Utils.PrintBoard(board);
            Console.WriteLine();
            TestActions();
            Utils.PrintBoard(board);
            Console.WriteLine();
            //position = new Point(position.x, board.Height - position.y - 1);
            Assert.AreEqual(position, 
                new Point(mino.Position.x, board.Height - mino.Position.y - 1 ));
        }

        #region Tests

        #region Tspins
        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), new Point(2, 1));
        }

        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        [Category("T Spins")]
        [Test]
        public void TestTSpinSingleRight()
        {
            string[] text =
            {
                "   ",
                "X L",
                "   "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), new Point(1, 0));
        }

        [Category("T Spins")]
        [Test]
        public void TestTSpinSingleLeft()
        {
            string[] text =
            {
                "   ",
                "R X",
                "   "
            };
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), new Point(1, 0));
        }

        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), new Point(3, 1));
        }

        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), new Point(3, 1));
        }

        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("T Spins")]
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
            TestBase(text, Tetromino.T, () => mino.Rotate(Rotation.Left), new Point(3, 1));
        }

        [Category("T Spins")]
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
            }, new Point(1, 1));
        }
        #endregion

        #region ISpins
        [Category("I Spins")]
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
            }, new Point(1, 2));
        }

        [Category("I Spins")]
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
            TestBase(text, Tetromino.I, () => mino.Rotate(Rotation.Right), new Point(4, 0));
        }

        [Category("I Spins")]
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
            TestBase(text, Tetromino.I, () => mino.Rotate(Rotation.Left), new Point(2, 0));
        }
        #endregion

        #region SZSpins
        [Category("SZ Spins")]
        [Test]
        public void TestZSpinInPlaceRight()
        {
            string[] text =
            {
                "X   ",
                " R X",
                "X  X"
            };
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("SZ Spins")]
        [Test]
        public void TestSSpinInPlaceLeft()
        {
            string[] text =
            {
                "   X",
                "X L ",
                "X  X"
            };
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), new Point(3, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Right), new Point(2, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.S, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("SZ Spins")]
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
            TestBase(text, Tetromino.Z, () => mino.Rotate(Rotation.Left), new Point(3, 1));
        }
        #endregion

        #region JLSpins
        [Category("JL Spins")]
        [Test]
        public void TestLSpinInPlaceRight()
        {
            string[] text =
            {
                "   XX",
                "X R X",
                "X   X"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(2, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinInPlaceLeft()
        {
            string[] text =
            {
                "XX  ",
                "X L X",
                "X   X"
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinTuckTopRight()
        {
            string[] text =
            {
                "   ",
                "XXL",
                "   "
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(1, 0));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinTuckTopLeft()
        {
            string[] text =
            {
                "   ",
                "RXX",
                "   "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(1, 0));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinTuckBottomRight()
        {
            string[] text =
            {
                "XX    ",
                " XL   ",
                "      "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Right), new Point(1, 0));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinTuckBottomLeft()
        {
            string[] text =
            {
                "    XX",
                "   RX ",
                "      "
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Left), new Point(4, 0));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinHighOverhangRight()
        {
            string[] text =
            {
                "    ",
                " RXX",
                "    ",
                "XXX "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Right), new Point(2, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinHighOverhangLeft()
        {
            string[] text =
            {
                "    ",
                "XXL ",
                "    ",
                " XXX"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpiLowOverhangRight()
        {
            string[] text =
            {
                "    ",
                "   X",
                " R  ",
                "  X "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Right), new Point(2, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinLowOverhangLeft()
        {
            string[] text =
            {
                "    ",
                "X   ",
                "  L ",
                " X  "
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinCaveRight()
        {
            string[] text =
            {
                "    ",
                "XR X",
                "X   ",
                "XXX "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Right), new Point(2,1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinCaveLeft()
        {
            string[] text =
            {
                "    ",
                "X LX",
                "   X",
                " XXX"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinTuckDoubleRight()
        {
            string[] text =
            {
                "   ",
                "R X",
                "   ",
                " XX"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinTuckDoubleLeft()
        {
            string[] text =
            {
                "   ",
                "X L",
                "   ",
                "XX "
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinTuckOverhangRight()
        {
            string[] text =
            {
                "     ",
                "  RXX",
                "     ",
                "XX XX"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(3, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinTuckOverhangLeft()
        {
            string[] text =
            {
                "     ",
                "XXL  ",
                "     ",
                "XX XX"
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinJumpTripleRight()
        {
            string[] text =
            {
                "XX  ",
                "X 0 ",
                "X XX",
                "X XX",
                "X  X"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinJumpTripleLeft()
        {
            string[] text =
            {
                "  XX",
                " 0 X",
                "XX X",
                "XX X",
                "X  X"
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestLSpinJumpTripleRightUpperHang()
        {
            string[] text =
            {
                "XX  ",
                "X   ",
                "X 0 ",
                "X XX",
                "X XX",
                "X  X"
            };
            TestBase(text, Tetromino.L, () => mino.Rotate(Rotation.Right), new Point(1, 1));
        }

        [Category("JL Spins")]
        [Test]
        public void TestJSpinJumpTripleLeftUpperHang()
        {
            string[] text =
            {
                "  XX",
                "   X",
                " 0 X",
                "XX X",
                "XX X",
                "X  X"
            };
            TestBase(text, Tetromino.J, () => mino.Rotate(Rotation.Left), new Point(2, 1));
        }

        #endregion

        #endregion
    }
}
