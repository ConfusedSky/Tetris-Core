using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Test
{
    static class Utils
    {
        public static void PrintBoard( RectangularTetrisBoard b )
        {
            Point p = new Point();
            for( p.y = 0; p.y < b.Height; p.y++ )
            {
                for( p.x = 0; p.x < b.Width; p.x++ )
                {
                    if( b[p].BackgroundColor == Mino.ShadowColor )
                    {
                        Console.Write("S");
                    }
                    else if( b[p].Color != null )
                    {
                        Console.Write("E");
                    }
                    else if( b[p].BackgroundColor != null )
                    {
                        Console.Write("B");
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
