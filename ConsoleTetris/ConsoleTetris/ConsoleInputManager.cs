using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris;
using System.Windows.Input;

namespace ConsoleTetris
{
    class ConsoleInputManager : IInputManager
    {
        public ConsoleInputManager()
        {

        }

        ~ConsoleInputManager()
        {

        }

        private enum InputState
        {
            LeftDown,
            RightDown,
            None
        }
        
        public float RepeatDelay = .1f;
        public float RepeatRate = .045f;

        private InputState state = InputState.None;
        private float timeTillRepeat = 0;

        private void KeyDownEvent( object sender, KeyEventArgs e )
        {

        }

        public TetrisAction HandleInput(float deltaTime)
        {
            timeTillRepeat -= deltaTime;  
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if ( key.Key == ConsoleKey.Spacebar) return TetrisAction.Drop;
                else if ( key.Key == ConsoleKey.LeftArrow)
                {
                    state = InputState.LeftDown;
                    timeTillRepeat = RepeatDelay;
                    return TetrisAction.Left;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    state = InputState.RightDown;
                    timeTillRepeat = RepeatDelay;
                    return TetrisAction.Right;
                }
                else if (key.Key == ConsoleKey.DownArrow) return TetrisAction.Down;
                else if (key.Key == ConsoleKey.C) return TetrisAction.Hold;
                else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.X) return TetrisAction.RotateRight;
                else if (key.Key == ConsoleKey.Z) return TetrisAction.RotateLeft;
            }

            if (state == InputState.LeftDown)
            {
                if (!Keyboard.IsKeyDown( Key.Left ) )
                {
                    state = InputState.None;
                }
                else if (timeTillRepeat <= 0)
                {
                    timeTillRepeat += RepeatRate;
                    return TetrisAction.Left;
                }
            }
            else if (state == InputState.RightDown)
            {
                if (!Keyboard.IsKeyDown( Key.Left ))
                {
                    state = InputState.None;
                }
                else if (timeTillRepeat <= 0)
                {
                    timeTillRepeat += RepeatRate;
                    return TetrisAction.Right;
                }
            }

            return TetrisAction.None;
        }
    }
}
