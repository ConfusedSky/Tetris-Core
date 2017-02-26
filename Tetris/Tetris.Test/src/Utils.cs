using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Test
{
    static class Utils
    {
        public static Mino SetupMino( RectangularTetrisBoard board, string[] text, MinoType type )
        {
            Mino m = Mino.CreateNewMino(board, type);
            char[] posChars = { '0', 'L', 'R', 'U' };
            Point p = new Point();
            for( p.y = 0; p.y < text.Length; p.y++ )
            {
                for( p.x = 0; p.x < text[0].Length; p.x++ )
                {
                    if (posChars.Contains(text[p.y][p.x]))
                    {
                        switch (text[p.y][p.x])
                        {
                            case '0':
                                m.Rotate(Rotation.None);
                                break;
                            case 'L':
                                m.Rotate(Rotation.Left);
                                break;
                            case 'R':
                                m.Rotate(Rotation.Right);
                                break;
                            case 'U':
                                m.Rotate(Rotation.Flip);
                                break;
                        }
                        p.y += board.KillHeight + 1;
                        m.MoveTo(p);
                        break;
                    }
                }
            }
            return m;
        }

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
