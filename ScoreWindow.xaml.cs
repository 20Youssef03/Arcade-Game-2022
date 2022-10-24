using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ArcadeGame2022
{
    /// <summary>
    /// Interaction logic for Score_Window.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        Window window;
        public ScoreWindow(Window window)
        {
            InitializeComponent();

            this.window = window;
            ScoresOphalen("Dag");
        }

        private void TerugButtonClick(object sender, RoutedEventArgs e)
        {
            window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void DagButtonClick(object snder, RoutedEventArgs e)
        {
            ScoresOphalen("Dag");
        }
        private void WeekButtonClick(object snder, RoutedEventArgs e)
        {
            ScoresOphalen("Week");
        }
        private void MaandButtonClick(object snder, RoutedEventArgs e)
        {
            ScoresOphalen("Maand");
        }

        private void ScoresOphalen(string DagWeekMaand)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\")) + "Data\\Database1.mdf\";Integrated Security=True";
            string dagen;
            switch (DagWeekMaand)
            {
                case "Dag":
                    dagen = "1";
                    break;
                case "Week":
                    dagen = "7";
                    break;
                case "Maand":
                    dagen = "30";
                    break;
                default:
                    dagen = "0";
                    break;
            }
            string query = String.Format("SELECT [Speler], [Score], [Gewonnen], [Datum], datediff(day, [Datum], getdate()) FROM [Highscores] WHERE datediff(day, [Datum], getdate()) < -" + dagen + " ORDER BY [Score] desc");
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                TitelTekst.Text = DagWeekMaand + " scores";
                string lijstNamen = "\nNaam\n";
                string lijstScores = "\nScore\n";
                string lijstGewonnen = "\nGewonnen\n";
                string lijstData = "\nData\n";
                while (reader.Read())
                {
                    lijstNamen += "\n" + reader.GetString(0);
                    lijstScores += "\n" + reader.GetInt32(4);
                    lijstGewonnen += "\n" + reader.GetString(2);
                    lijstData += "\n" + reader.GetDateTime(3).ToString("yyyy-MM-dd");
                }
                connection.Close();
                LijstNamen.Text = lijstNamen;
                LijstScores.Text = lijstScores;
                LijstGewonnen.Text = lijstGewonnen;
                LijstData.Text = lijstData;
            }
            catch (IOException)
            {
                connection.Close();
            }
        }
    }
}
