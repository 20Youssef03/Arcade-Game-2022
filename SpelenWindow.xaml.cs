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

namespace ArcadeGame2022
{
    /// <summary>
    /// Interaction logic for SpelenWindow.xaml
    /// </summary>
    public partial class SpelenWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool jump = false;
        private float velocity = 0;
        private float gravity = 0.15f;

        public SpelenWindow(ImageSource imageSource)
        {
            InitializeComponent();

            // Vervangt enkele kleur rechthoeken met afbeeldingen
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    x.Fill = new ImageBrush
                    {
                        ImageSource = imageSource
                    };
                }
                else if ((string)x.Tag == "Blok")
                {
                    x.Fill = new ImageBrush // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/brick.png")), // /bin/Debug/netcoreapp3.1/Images/brick.png
                        TileMode = TileMode.Tile,
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 50, 50)
                    };
                }
            }

            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            VerticaleBeweging();

            // Horizontale beweging wordt apart bepaald om te voorkomen dat de speler de verkeerde kant op wordt verplaatst
            HorizontaleBeweging();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Speler kan spelen met pijltjestoetsen of WASD en springen met spatie
            if (e.Key == Key.Left || e.Key == Key.A)
                moveLeft = true;
            if (e.Key == Key.Right || e.Key == Key.D)
                moveRight = true;
            if (e.Key == Key.Up || e.Key == Key.Space || e.Key == Key.W)
                jump = true;
        }

        private void OnClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Speler kan spelen met pijltjestoetsen of WASD en springen met spatie
            if (e.Key == Key.Left || e.Key == Key.A)
                moveLeft = false;
            if (e.Key == Key.Right || e.Key == Key.D)
                moveRight = false;
            if (e.Key == Key.Up || e.Key == Key.Space || e.Key == Key.W)
                jump = false;
        }

        private void VerticaleBeweging()
        {
            // Beweeg de speler
            velocity += gravity;
            Canvas.SetTop(Speler, Canvas.GetTop(Speler) + velocity);

            // Kijk of de speler een blok raakt
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    Rect speler = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (Rectangle y in SpelenCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "Blok")
                        {
                            Rect blok = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(blok)) // Speler raakt blok
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
        }

        private void HorizontaleBeweging()
        {
            // Beweeg de speler
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (moveLeft && x.Name != "Speler")
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + 4);
                if (moveRight && x.Name != "Speler")
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 4);
            }

            // Kijk of de speler een blok raakt
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    Rect speler = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (Rectangle y in SpelenCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "Blok")
                        {
                            Rect blok = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(blok)) // Speler raakt blok
                            {
                                foreach (Rectangle z in SpelenCanvas.Children.OfType<Rectangle>()) // Zorg ervoor dat blokken op dezelfde plaats blijven ten opzichte van elkaar
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
    }
}
