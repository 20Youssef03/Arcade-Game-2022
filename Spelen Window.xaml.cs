using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arcade_Game_2022
{
    /// <summary>
    /// Interaction logic for Spelen_Window.xaml
    /// </summary>
    public partial class Spelen_Window : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool jump = false;
        private float velocity = 0f;
        private float gravity = 9.81f;

        public Spelen_Window()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            velocity = gravity;
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    Rect speler = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (Rectangle y in SpelenCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string) y.Tag == "Block")
                        {
                            Rect block = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(block))
                            {
                                moveLeft = false;
                                moveRight = false;
                                velocity = 0;
                            }
                        }
                    }
                }
            }
            if (moveLeft)
                Canvas.SetLeft(Speler, Canvas.GetLeft(Speler) - 1);
            if (moveRight)
                Canvas.SetLeft(Speler, Canvas.GetLeft(Speler) + 1);
            Canvas.SetTop(Speler, Canvas.GetTop(Speler) + 0.1 * velocity);
        }

        private void Spelen_Window_Terug_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                moveLeft = true;
            if (e.Key == Key.Right)
                moveRight = true;
            if (e.Key == Key.Up || e.Key == Key.Space)
                jump = true;
        }

        private void onClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                moveLeft = false;
            if (e.Key == Key.Right)
                moveRight = false;
            if (e.Key == Key.Up || e.Key == Key.Space)
                jump = false;
        }
    }
}
