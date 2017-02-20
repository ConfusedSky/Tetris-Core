using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Tetris.RectangularTetrisBoard board = new Tetris.RectangularTetrisBoard(10, 23, 1);
            ConsoleInputManager inputManager = new ConsoleInputManager();
            Tetris.RandomItemGenerator<Tetris.MinoType> queue = 
                new Tetris.RandomItemGenerator<Tetris.MinoType>( Tetris.Tetromino.TETROMINO_TYPES, 5, 4, 4 );
            Tetris.Game game = new Tetris.Game(inputManager, board, queue);
            RectangularTetrisBoardView view = new RectangularTetrisBoardView(board);

            Console.CursorVisible = false;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while( true )
            {
                game.Update(((float)watch.ElapsedTicks)/Stopwatch.Frequency);
                view.Update();
                //while (watch.ElapsedTicks < Stopwatch.Frequency / 60) ;
                watch.Restart();
            }
        }
    }
}
