using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Caro2
{
    internal class Piece : Canvas
    {
        public int Row
        {
            get => (int)GetValue(Grid.RowProperty);
            set => SetValue(Grid.RowProperty, value);
        }

        public int Column
        {
            get => (int)GetValue(Grid.ColumnProperty);
            set => SetValue(Grid.ColumnProperty, value);
        }
        public bool IsEmpty => Children.Count == 0;
        public void Clear() => Children.Clear();
        public void DrawO()
        {
            double sz = this.ActualWidth - 4;
            Children.Add(new Ellipse
            {
                Width = sz,
                Height = sz,
                Stroke = Brushes.Yellow,
                StrokeThickness = 2,
                Margin = new Thickness(2),
            });     
        }
        public void DrawX()
        {
            const double d = 4;
            double sz = this.ActualWidth - d;
            Children.Add(new Line
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                X1 = d,
                Y1 = d,
                X2 = sz,
                Y2 = sz,
            });
            Children.Add(new Line
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                X1 = d,
                Y1 = sz,
                X2 = sz,
                Y2 = d,
            });
        }
        public void Put(char player)
        {
            if (player == Move.O) DrawO(); else DrawX();
        }

        public Piece(Game game, int r, int c)
        {
            Row = r;
            Column = c;
            Background = Brushes.Pink;
            Margin = new Thickness(0.5);

            game.Changed += (s, e) => {
                if (e.Move.Row != r || e.Move.Column != c) return;

                switch (e.Move.Player)
                {
                    case Move.Empty: Clear(); break;
                    case Move.O: DrawO(); break;
                    case Move.X: DrawX(); break;
                }
            };
            game.GameOver += (s, e) => {
                Background = Brushes.Gray;
            };
            
            PreviewMouseLeftButtonUp += (s, e) => {
                if (Children.Count > 0) return;

                game.PutAndCheckGameOver(r, c);
            };
        }
    }
}
