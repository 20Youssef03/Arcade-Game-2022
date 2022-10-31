using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Collections;
using System.Media;
using System.DirectoryServices;
using System.Windows.Markup;

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
        private double levelPositie = 0;
        private double level = 0;
        private int punten = 0;
        private string spelerNaam1;
        private string spelerNaam2;
   
        public SpelenWindow(ImageSource imageSource1, ImageSource imageSource2, string spelerNaam1, string spelerNaam2)
        {
            InitializeComponent();

            this.spelerNaam1 = spelerNaam1;
            this.spelerNaam2 = spelerNaam2;

            // Vervangt enkele kleur rechthoeken met afbeeldingen
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "Speler")
                {
                    x.Fill = new ImageBrush
                    {
                        ImageSource = imageSource1
                    };
                }
                else if ((string)x.Tag == "Blok")
                {
                    x.Fill = new ImageBrush // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/gras blok platform.jpg")), // /bin/Debug/netcoreapp3.1/Images/gras blok platform.png
                        TileMode = TileMode.Tile,
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 50, 50)
                    };
                }
            }

            PlaatsVijanden();

            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            VerticaleBeweging();

            // Horizontale beweging wordt apart bepaald om te voorkomen dat de speler de verkeerde kant op wordt verplaatst
            HorizontaleBeweging();

            BeweegVijanden();
        }

        /// <summary>
        /// Kijkt of de speler bepaalde toetsen indrukt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Speler kan spelen met pijltjestoetsen of WASD en springen met spatie
            if (e.Key == Key.Left || e.Key == Key.A)
                moveLeft = true;
            if (e.Key == Key.Right || e.Key == Key.D)
                moveRight = true;
            if (e.Key == Key.Up || e.Key == Key.Space || e.Key == Key.W)
                jump = true;

            // Tijdelijk, voor debuggen
            if (e.Key == Key.D1)
                VolgendLevel();
            if (e.Key == Key.D2)
                HerstartLevel();
            if (e.Key == Key.D3)
                HerstartSpel(null, null);
            if (e.Key == Key.D6)
            {
                punten += 1;
                PuntenTekst.Text = punten.ToString();
            }
            if (e.Key == Key.D7)
                WinSpel();
            if (e.Key == Key.D9)
                Application.Current.Shutdown();
        }

        /// <summary>
        /// Kijkt of de speler het venster heeft gesloten, en zo ja, sluit het hele spel af.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Kijkt of de speler een bepaalde toets nog ingedrukt houdt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Beweegt de speler verticaal en controleert of de speler een blok van boven of onder raakt, in welk geval de speler niet verder beweegt, en laat de speler springen als de spring toets wordt ingedrukt en een blok van boven wordt geraakt.
        /// </summary>
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

        /// <summary>
        /// Beweegt alle blokken horizontaal en controleert of de speler een blok van de zijkant raakt, in welk geval de blokken niet bewegen.
        /// </summary>
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

            // Houd horizontale beweging bij
            if (moveLeft)
                levelPositie += 4;
            if (moveRight)
                levelPositie -= 4;

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
                                if (speler.Left < blok.Left)
                                    levelPositie += 4;
                                else if (speler.Right > blok.Right)
                                    levelPositie -= 4;
                            }
                        }
                        
                        
                        if ((string)y.Tag == "Munt")
                        {
                            Rect Munt = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(Munt))
                            {
                                punten += 1;
                                PuntenTekst.Text = punten.ToString();
                              
                            }
                        }
                        
                    }
                    
                }
            }
        }

     

        /// <summary>
        /// Plaatst de speler op de grond en alle blokken terug naar de horizontale startpositie en 1000 pixels hoger, waardoor het volgende level in beeld komt.
        /// </summary>
        private void VolgendLevel()
        {
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name != "Speler")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 1000); // Beweeg alle blokken 1000 pixels omhoog zodat het volgende level de plaats van de vorige inneemt
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - levelPositie); // Beweeg alle blokken terug naar de horizontale startpositie
                }
                else
                    Canvas.SetTop(x, 631); // Plaats speler op grond
            }
            level += 1;
            LevelTekst.Text = "Level " + (level + 1);
            levelPositie = 0; // Zet positie teller terug op 0
            velocity = 0; // Voorkom dat de speler beweegt aan het begin van een level
        }

        /// <summary>
        /// Plaatst de speler op de grond en alle blokken terug naar de horizontale startpositie.
        /// </summary>
        private void HerstartLevel()
        {
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name != "Speler")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - levelPositie); // Beweeg alle blokken terug naar de horizontale startpositie
                }
                else
                    Canvas.SetTop(x, 631); // Plaats speler op grond
            }
            levelPositie = 0; // Zet positie teller terug op 0
            velocity = 0; // Voorkom dat de speler beweegt aan het begin van een level
            PlaatsVijanden();
            punten = 0;
            PuntenTekst.Text = punten.ToString(); //reset de punten naar 0 
        }

        /// <summary>
        /// Plaatst de speler op de grond en alle blokken terug naar de horizontale en verticale startpositie.
        /// </summary>
        private void HerstartSpel(object sender, RoutedEventArgs e)
        {
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name != "Speler")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 1000 * level); // Beweeg alle blokken 1000 pixels omhoog zodat het volgende level de plaats van de vorige inneemt
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - levelPositie); // Beweeg alle blokken terug naar de horizontale startpositie
                }
                else
                    Canvas.SetTop(x, 631); // Plaats speler op grond
            }
            level = 0;
            LevelTekst.Text = "Level 1";
            levelPositie = 0; // Zet positie teller terug op 0
            velocity = 0; // Voorkom dat de speler beweegt aan het begin van een level
            PlaatsVijanden();
            punten = 0;
            PuntenTekst.Text = punten.ToString(); //reset de punten naar 0
        }

        private void PlaatsVijanden()
        {
            // Plaats vijanden in de gewenste posities
            // Wordt uitgevoerd aan het begin en bij het resetten van een level/het spel
        }

        private void BeweegVijanden()
        {
            // Beweeg de vijanden als ze in beeld zijn (0 < x < 1536, 0 < y < 864)
            // Dit zal waarschijnlijk verschillen afhangend van watvoor vijanden we willen
        }

        private void VerticaleBewegingVijanden()
        {
            // Net als VerticaleBeweging() maar specifiek voor de vijanden
        }
        private void HorizontaleBewegingVijanden()
        {
            // Net als HorizontaleBeweging() maar specifiek voor de vijanden
        }

        private void WinSpel()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\")) + "Data\\Database1.mdf\";Integrated Security=True";
            string query = String.Format("INSERT INTO [Highscores] ([Speler], [Score], [Datum], [Gewonnen]) VALUES ('{0}', '{1}', '{2}', '{3}')", spelerNaam1, punten, DateTime.Today.Date.ToString("yyyy-MM-dd"), "Ja");
            string query2 = "";
            if (spelerNaam2 != null)
                query2 = String.Format("INSERT INTO [Highscores] ([Speler], [Score], [Datum], [Gewonnen]) VALUES ('{0}', '{1}', '{2}', '{3}')", spelerNaam2, punten, DateTime.Today.Date.ToString("yyyy-MM-dd"), "Ja");
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            SqlCommand command2 = new SqlCommand();
            try
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                if (query2 != "")
                {
                    command2.CommandText = query2;
                    command2.CommandType = CommandType.Text;
                    command2.Connection = connection;
                }
                connection.Open();
                command.ExecuteNonQuery();
                if (query2 != "")
                    command2.ExecuteNonQuery();
                connection.Close();
            }
            catch (IOException)
            {
                connection.Close();
            }
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            SoundPlayer player = new SoundPlayer(@"C:\Users\Lavar\source\repos\New folder (10)\Singing nightingale. The best bird song..wav");
            player.Load();
            player.Play();
            //achtergrond muziek wordt afgespeeld na klikken op geluidsknop 
        }
    }
}
