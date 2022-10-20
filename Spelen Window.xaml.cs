using System;
using System.Collections.Generic;
using System.IO;
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
        private float velocity = 0;
        private float gravity = 0.15f;
        ImageBrush imageBrush = new ImageBrush();

        public Spelen_Window()
        {
            InitializeComponent();

            // Vervangt enkele kleur rechthoeken met afbeeldingen
            /*foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    x.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(@"locatie van poppetje png (zoals mario.png in de afbeeldingen map)"))
                    };
                }
                else if ((string) x.Tag == "Blok")
                {
                    x.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(@"locatie van blok png (zoals brick.png in afbeeldingen map)")),
                        TileMode = TileMode.Tile,
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 50, 50)
                    };
                }
            }*/

            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            velocity += gravity;
            Canvas.SetTop(Speler, Canvas.GetTop(Speler) + velocity);
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    Rect speler = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (Rectangle y in SpelenCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string) y.Tag == "Blok")
                        {
                            Rect blok = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(blok))
                            {
                                if (speler.Top < blok.Top) // Speler raakt blok van boven
                                {
                                    Canvas.SetTop(Speler, blok.Top - speler.Height - 0.00001); // de 0.00001 voorkomt overlap voor horizontale beweging
                                    velocity = 0;
                                    if (jump) // Kan alleen springen als speler op blok staat
                                    {
                                        velocity = -gravity * 60;
                                    }
                                }
                                else if (speler.Bottom > blok.Bottom) // Speler raakt blok van onder
                                {
                                    Canvas.SetTop(Speler, blok.Top + blok.Height + 0.00001); // de 0.00001 voorkomt overlap voor horizontale beweging
                                    velocity = 0;
                                }
                            }
                        }
                    }
                }
            }
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (moveLeft && x.Name != "Speler")
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + 4);
                if (moveRight && x.Name != "Speler")
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 4);
            }
            // Links en rechts worden apart bepaald om te voorkomen dat de speler de verkeerde kant op wordt verplaatst
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    Rect speler = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (Rectangle y in SpelenCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string) y.Tag == "Blok")
                        {
                            Rect blok = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(blok))
                            {
                                foreach (Rectangle z in SpelenCanvas.Children.OfType<Rectangle>())
                                {
                                    Rect blok2 = new Rect(Canvas.GetLeft(z), Canvas.GetTop(z), z.Width, z.Height);
                                    if (speler.Left < blok.Left && blok2 != speler) // Speler raakt blok van links
                                    {
                                        Canvas.SetLeft(z, blok2.Left - blok.Left + speler.Right + 0.00001); // de 0.00001 voorkomt overlap voor verticale beweging
                                    }
                                    else if (speler.Right > blok.Right && blok2 != speler) // Speler raakt blok van rechts
                                    {
                                        Canvas.SetLeft(z, blok2.Right - blok.Right + speler.Left - blok2.Width - 0.00001); // de 0.00001 voorkomt overlap voor verticale beweging
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            // Speler kan spelen met pijltjestoetsen of WASD en springen met spatie
            if (e.Key == Key.Left || e.Key == Key.A)
                moveLeft = true;
            if (e.Key == Key.Right || e.Key == Key.D)
                moveRight = true;
            if (e.Key == Key.Up || e.Key == Key.Space || e.Key == Key.W)
                jump = true;
        }

        private void onClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            // Speler kan spelen met pijltjestoetsen of WASD en springen met spatie
            if (e.Key == Key.Left || e.Key == Key.A)
                moveLeft = false;
            if (e.Key == Key.Right || e.Key == Key.D)
                moveRight = false;
            if (e.Key == Key.Up || e.Key == Key.Space || e.Key == Key.W)
                jump = false;
        }
    }
}
