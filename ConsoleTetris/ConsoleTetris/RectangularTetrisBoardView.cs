using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris;

namespace ConsoleTetris
{
    class RectangularTetrisBoardView
    {
        private RectangularTetrisBoard board;
        
        public RectangularTetrisBoardView( RectangularTetrisBoard board )
        {
            this.board = board;
        }

        public void Update()
        {
            Point p = new Point();
            for( p.y = 0; p.y < board.Height; p.y++ )
            {
                Console.SetCursorPosition(0, p.y);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                for( p.x = 0; p.x < board.Width; p.x++ )
                {
                    Console.SetCursorPosition(p.x*2+1, p.y);
                    Block b = board[p];
                    ConsoleColor color = ConsoleColor.White;
                    if (b.DisplayedColor == BlockColor.black) color = ConsoleColor.Black;
                    else if (b.DisplayedColor == BlockColor.cyan) color = ConsoleColor.Cyan;
                    else if (b.DisplayedColor == BlockColor.blue) color = ConsoleColor.Blue;
                    else if (b.DisplayedColor == BlockColor.red) color = ConsoleColor.Red;
                    else if (b.DisplayedColor == BlockColor.magenta) color = ConsoleColor.Magenta;
                    else if (b.DisplayedColor == BlockColor.orange) color = ConsoleColor.DarkYellow;
                    else if (b.DisplayedColor == BlockColor.green) color = ConsoleColor.Green;
                    else if (b.DisplayedColor == BlockColor.yellow) color = ConsoleColor.Yellow;
                    else if (b.DisplayedColor == BlockColor.white) color = ConsoleColor.White;

                    Console.ForegroundColor = color;
                    Console.Write((b.DisplayedColor != null) ? 'O' : ' ');
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("|");
            }
        }
    }
}
