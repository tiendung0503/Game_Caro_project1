using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Caro2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    var game = new Game();
        //    InitializeComponent();
        //    int sz = 20;
        //    int x = 0, y = 0;
        //    int w = game.GameSize * sz;
        //    var grid = new Grid {
        //        Background = Brushes.Red,
        //        Height = w,
        //        Width = w,
        //    };
        //    desk_caro.Child = grid;

        //    for(int i = 0; i < game.GameSize + 1; i++, x += sz)
        //    {
        //        var line = new Line() { 
        //            Stroke = Brushes.Red,
        //            StrokeThickness = 1,
        //            X1 = x, Y1 = 0,
        //            X2 = x, Y2 = w,
        //        };
        //        grid.Children.Add(line);
        //    }

        //    for (int i = 0; i < game.GameSize + 1; i++, y += sz)
        //    {
        //        var line = new Line()
        //        {
        //            Stroke = Brushes.Red,
        //            StrokeThickness = 1,
        //            X1 = 0,
        //            Y1 = y,
        //            X2 = w,
        //            Y2 = y,
        //        };
        //        grid.Children.Add(line);
        //    }

        //    game.Changed += (s, e) => {
        //        var m = game.CurrentMove;
        //        if (m.Player == Move.Empty)
        //        {

        //        }
        //    };

        //    game.Start();
        //    grid.MouseLeftButtonUp += (s, e) => {
        //        var pt = e.GetPosition(grid);
        //        int c = (int)pt.X / sz;
        //        int r = (int)pt.Y / sz;

        //        x = c * sz;
        //        y = r * sz;

        //        if (!game.TryPut(r, c)) return;
        //        var piece = new Canvas { 
        //            Width = sz,
        //            Height = sz,
        //            Margin = new Thickness(x,y,0,0),
        //            HorizontalAlignment = HorizontalAlignment.Left,
        //            VerticalAlignment = VerticalAlignment.Top,
        //        };
        //        if(game.Player == 'o')
        //        {
        //            piece.Children.Add(new Ellipse
        //            {
        //                Width = sz - 4,
        //                Height = sz - 4,
        //                Stroke = Brushes.Yellow,
        //                StrokeThickness = 2,
        //                Margin = new Thickness(2),
        //            });
        //        }

        //        else
        //        {
        //            piece.Children.Add(new Line
        //            {
        //                Stroke = Brushes.Green,
        //                StrokeThickness = 2,
        //                X1 = 4,
        //                Y1 = 4,
        //                X2 = sz - 4,
        //                Y2 = sz - 4,
        //            });
        //            piece.Children.Add(new Line
        //            {
        //                Stroke = Brushes.Green,
        //                StrokeThickness = 2,
        //                X1 = 4,
        //                Y1 = sz - 4,
        //                X2 = sz - 4,
        //                Y2 = 4,
        //            });
        //        }

        //        grid.Children.Add(piece);
        //        game.Swap();
        //    };

        //}

        public MainWindow()
        {
            InitializeComponent();
            CreateGame();
        }       
        void CreateGame()
        {
            var game = new Game();
            int sz = 20;
            int w = game.GameSize * sz;
            var grid = new Grid
            {
                Background = Brushes.Red,
                Height = w,
                Width = w,
            };
            desk_caro.Child = grid;

            game.GameOver += (s, e) => {
                var ts = new System.Threading.ThreadStart(() => MessageBox.Show(e.Move.Player + " win!!!"));
                new System.Threading.Thread(ts).Start();
            };

            for (int i = 0; i < game.GameSize; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int r = 0; r < game.GameSize; r++)
            {
                for (int c = 0; c < game.GameSize; c++)
                {
                    var piece = new Piece(game, r, c);
                    grid.Children.Add(piece);
                }
            }

            btnUndo.Click += (s, e) => game.Undo();


            game.Start();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CreateGame();
        }
    }
}
