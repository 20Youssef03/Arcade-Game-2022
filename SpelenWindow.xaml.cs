using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;

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
        private double levelSpeler1 = 0;
        private double levelSpeler2 = 0;
        private int punten = 0;
        private int puntenSpeler2 = 0;
        private int puntenSpeler1 = 0;
        private int levensSpeler1 = 4;
        private int levensSpeler2 = 4;
        private int huidigeSpeler = 1;
        private int aantalSpelers;
        private string spelerNaam1;
        private string spelerNaam2;
        private ImageSource imageSource1;
        private ImageSource imageSource2;
        private List<Rectangle> itemsToRemove = new List<Rectangle>();
        private string currentDirectory = Directory.GetCurrentDirectory();
        private bool winnaar = false;
        private bool uitgevoerd = false;
        public SpelenWindow(ImageSource imageSource1, ImageSource imageSource2, string spelerNaam1, string spelerNaam2)
        {
            InitializeComponent();

            if (spelerNaam2 != null)
            {
                aantalSpelers = 2;
            }
            else
            {
                aantalSpelers = 1;
            }

            this.spelerNaam1 = spelerNaam1;
            this.spelerNaam2 = spelerNaam2;
            this.imageSource1 = imageSource1;
            this.imageSource2 = imageSource2;

            SpelerTekst.Text = "Speler: " + spelerNaam1;

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
                        ImageSource = new BitmapImage(new Uri(currentDirectory + "/../../../Images/gras blok platform.jpg")), // /bin/Debug/netcoreapp3.1/Images/gras blok platform.png
                        TileMode = TileMode.Tile,
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 50, 50)
                    };
                }
                else if ((string)x.Tag == "Munt")
                {
                    x.Fill = new ImageBrush // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                    {
                        ImageSource = new BitmapImage(new Uri(currentDirectory + "/../../../Images/munt.png")), // /bin/Debug/netcoreapp3.1/Images/gras blok platform.png
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 35, 35)
                    };
                }
            }

            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();
            HerstartLevel(); // Geen idee waarom dit nodig is maar als het er niet staat doet multiplayer vreemd
        }

        private void GameEngine(object sender, EventArgs e)
        {
            VerticaleBeweging();

            // Horizontale beweging wordt apart bepaald om te voorkomen dat de speler de verkeerde kant op wordt verplaatst
            HorizontaleBeweging();

            if (huidigeSpeler == 1)
                LevensTekst.Text = "Levens: " + levensSpeler1.ToString();
            else
                LevensTekst.Text = "Levens: " + (levensSpeler2 - 1).ToString();
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
            if (e.Key == Key.Up || e.Key == Key.W)
                jump = true;
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
            if (e.Key == Key.Up || e.Key == Key.W)
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
            //foreach die rectangles van canvas removed 
            foreach (Rectangle r in itemsToRemove)
            {
                SpelenCanvas.Children.Remove(r);
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

                        //als speler munt raakt
                        if ((string)y.Tag == "Munt")
                        {
                            Rect Munt = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(Munt))
                            {
                                //komt er een punt bij 
                                punten += 1;
                                PuntenTekst.Text = "Punten: " + punten.ToString();
                                //verdwijnt de munt 
                                itemsToRemove.Add(y);
                            }
                        }
                        if ((string)y.Tag == "Obstakel")
                        {
                            Rect Obstakel = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(Obstakel))
                            {
                                HerstartLevel();
                                //Herstart level na botsing met obstakel
                            }
                        }
                        if ((string)y.Tag == "Finish")
                        {
                            Rect Finish = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(Finish))
                            {
                                VolgendLevel();
                            }
                        }
                        if((string)y.Tag == "End")
                        {
                            Rect End = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (speler.IntersectsWith(End))
                            {
                                if (huidigeSpeler == 1)
                                {
                                    winnaar = true;
                                    WinSpel();
                                }
                                else
                                {
                                    winnaar = true;
                                    WinSpel();
                                }
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
            if (huidigeSpeler == 1)
            {
                levelSpeler1 += 1;
            }
            else
            {
                levelSpeler2 += 1;
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
            if (huidigeSpeler == 1)
            {
                WisselSpeler2();
            }
            else
            {
                WisselSpeler1();
            }
            foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name != "Speler")
                {
                    if (aantalSpelers == 2)
                        Canvas.SetTop(x, Canvas.GetTop(x) - 1000 * level); // Beweeg alle blokken 1000 pixels omhoog zodat het volgende level de plaats van de vorige inneemt
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - levelPositie); // Beweeg alle blokken terug naar de horizontale startpositie
                }
                else
                    Canvas.SetTop(x, 631); // Plaats speler op grond
            }
            levelPositie = 0; // Zet positie teller terug op 0
            velocity = 0; // Voorkom dat de speler beweegt aan het begin van een level
            PuntenTekst.Text = "Punten: " + punten.ToString();
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
            if (huidigeSpeler == 1 && e != null)
            {
                levelSpeler1 = 0;
                levensSpeler1 = 3;
                punten = 0;
            }
            else if (e != null)
            {
                levelSpeler2 = 0;
                levensSpeler2 = 4;
                punten = 0;
            }
            LevensTekst.Text = "Levens: 3";
            LevelTekst.Text = "Level 1";
            levelPositie = 0; // Zet positie teller terug op 0
            velocity = 0; // Voorkom dat de speler beweegt aan het begin van een level
            PuntenTekst.Text = "Punten: " + punten.ToString(); //reset de punten naar 0
        }

        private void WinSpel()
        {
            /*string test = "hoi";
            string curdir = currentDirectory;
            string combi = System.IO.Path.Combine(curdir, "..\\..\\..\\");
            string fullpa = System.IO.Path.GetFullPath(combi);*/
            //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + fullpa + "Data\\Database1.mdf\";Integrated Security=True";
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stefa\\Desktop\\Arcade-Game-2022\\Data\\Database1.mdf;Integrated Security=True";
            string naam1;
            int score1;
            string naam2;
            int score2;
            if (huidigeSpeler == 1) // Speler 2 wint of niemand wint
            {
                naam2 = spelerNaam1;
                naam1 = spelerNaam2;
                HerstartLevel();
                score1 = puntenSpeler2;
                score2 = puntenSpeler1;
            }
            else // Speler 1 wint
            {
                naam2 = spelerNaam2;
                naam1 = spelerNaam1;
                HerstartLevel();
                score1 = puntenSpeler1;
                score2 = puntenSpeler2;
            }
            if (spelerNaam2 == null) // 1 speler
            {
                naam1 = spelerNaam1;
                score1 = puntenSpeler2;
            }
            aantalSpelers--;
            string gewonnenTekst;
            if (winnaar)
                gewonnenTekst = "Ja";
            else
            {
                gewonnenTekst = "Nee";
            }
            string query = String.Format("INSERT INTO [Highscores] ([Speler], [Score], [Datum], [Gewonnen]) VALUES ('{0}', '{1}', '{2}', '{3}')", naam1, score1, DateTime.Today.Date.ToString("yyyy-MM-dd"), gewonnenTekst);
            if (spelerNaam2 != null)
                query += String.Format(", ('{0}', '{1}', '{2}', '{3}')", naam2, score2, DateTime.Today.Date.ToString("yyyy-MM-dd"), "Nee");
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                connection.Open();
                if (!uitgevoerd)
                {
                    command.ExecuteNonQuery();
                    uitgevoerd = true;
                }
                connection.Close();
            }
            catch (IOException)
            {
                connection.Close();
            }
            Application.Current.Shutdown();
        }

        private void WisselSpeler1()
        {
            levensSpeler2--;
            LevensTekst.Text = "Levens: " + levensSpeler2;
            puntenSpeler1 = punten;
            if (levensSpeler2 == 0)
            {
                WinSpel();
            }
            if (aantalSpelers == 2)
            {
                SpelerTekst.Text = "Speler: " + spelerNaam2;
                punten = puntenSpeler2;
                foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
                {
                    if (x.Name == "Speler")
                    {
                        x.Fill = new ImageBrush
                        {
                            ImageSource = imageSource2
                        };
                    }
                }
                huidigeSpeler = 1;
                HerstartSpel(null, null);
                level = levelSpeler1;
            }
        }
        private void WisselSpeler2()
        {
            levensSpeler1--;
            LevensTekst.Text = "Levens: " + levensSpeler1;
            puntenSpeler2 = punten;
            if (levensSpeler1 == 0)
            {
                WinSpel();
            }
            if (aantalSpelers == 2)
            {
                SpelerTekst.Text = "Speler: " + spelerNaam1;
                punten = puntenSpeler1;
                foreach (Rectangle x in SpelenCanvas.Children.OfType<Rectangle>())
                {
                    if (x.Name == "Speler")
                    {
                        x.Fill = new ImageBrush
                        {
                            ImageSource = imageSource1
                        };
                    }
                }
                huidigeSpeler = 2;
                HerstartSpel(null, null);
                level = levelSpeler2;
            }
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            SoundPlayer player = new SoundPlayer(currentDirectory + "/../../../Singing nightingale. The best bird song..wav");
            player.Load();
            player.Play();
            //achtergrond muziek wordt afgespeeld na klikken op geluidsknop 
        }

        private void Stop_Button_Speel_Window_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
