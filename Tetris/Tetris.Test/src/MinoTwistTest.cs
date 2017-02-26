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
        public void Init()
        {
            board = new RectangularTetrisBoard(10, 22, 1);
            mino = Mino.CreateNewMino(board, Tetromino.I);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            mino.Destroy();
            mino = null;
            board = null;
        }

        [SetUp]
        public void Clear()
        {
            board.Reset();
        }

        [Test]
        public void TestTSpinTimeSaver()
        {
            mino.BlockType = Tetromino.T;
            // X_____
            // XR_X__
            // X___X_
            // XX_X__
            board.PlaceBlocks(
                new Point[]
                {
                    new Point( 0, board.Height-1 ),
                    new Point( 0, board.Height-2 ),
                    new Point( 0, board.Height-3 ),
                    new Point( 0, board.Height-4 ),
                    new Point( 1, board.Height-1 ),
                    new Point( 3, board.Height-1 ),
                    new Point( 4, board.Height-2 ),
                    new Point( 3, board.Height-3 )
                },
                BlockColor.black
                );
            mino.Rotate( Rotation.Right );
            mino.MoveTo( new Point(1,board.Height-3) );
            Utils.PrintBoard(board);
            Console.WriteLine();
            mino.Rotate( Rotation.Right );
            Utils.PrintBoard(board);
            Console.WriteLine();
            Assert.AreEqual(new Point(2, board.Height - 2), mino.Position);
        }
    }
}
