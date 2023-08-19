using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Caro2
{
    class Move
    {
        public const char Empty = '\0';
        public const char X = 'x';
        public const char O = 'o';

        public int Row { get; set; }
        public int Column { get; set; }
        public char Player { get; set; }

        public void Update(char[,] data, char value)
        {
            data[Row, Column] = Player = value;
        }
    }
    class History : Stack<Move> { }

    class CaroEventArg : EventArgs
    {
        public Move Move { get; set; }
    }
    internal class Game
    {
        char[,] data;
        public int GameSize { get; set; } = 20;
        public Move CurrentMove { get; private set; }
        public char Player { get; private set; }

        public event EventHandler<CaroEventArg> Changed;
        public event EventHandler<CaroEventArg> GameOver;

        public void RaiseChanged() => Changed?.Invoke(this, new CaroEventArg { Move = CurrentMove });

        History backward = new History();
        History forward = new History();
        public void Start()
        {
            data = new char[GameSize, GameSize];
            Player = Move.X;
        
            backward = new History();
            forward = new History();
        }

        public void Undo()
        {
            if (backward.Count > 0)
            {
                CurrentMove = backward.Pop();
                CurrentMove.Update(data, Move.Empty);

                RaiseChanged();
                Swap();
            }
        }
        public void Swap() => Player = Player == Move.X ? Move.O : Move.X;
        
        int calc(char player, int row, int col, int dr, int dc, bool invert)
        {
            int r = row + dr;
            int c = col + dc;
            int s = 0;
            while (r >= 0 && r < GameSize && c >= 0 && c < GameSize && data[r, c] == player)
            {
                ++s;
                r += dr;
                c += dc;
            }
            if (invert) s += calc(player, row, col, -dr, -dc, false);
            return s;
        }
        bool isWin(char player, int row, int col)
        {
            Func<int, int, bool> over = (dr, dc) => calc(player, row, col, dr, dc, true) >= 4;
            return over(0, 1) || over(1, 0) || over(1, 1) || over(-1, 1);
        }
        
        public bool PutAndCheckGameOver(int r, int c)
        {
            CurrentMove = new Move { 
                Row = r, Column = c,
            };
            CurrentMove.Update(data, Player);
            RaiseChanged();

            backward.Push(CurrentMove);
            
            if (isWin(Player, r, c))
            {
                GameOver?.Invoke(this, new CaroEventArg { Move = CurrentMove });
                return true;
            }

            Swap();
            return false;
        }
    }
}
