using System;
using System.Collections.Generic;
using System.IO;
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

namespace ArcadeGame2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
            ImageSource imageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/achtergrond.png")); // /bin/Debug/netcoreapp3.1/Images/achtergrond.png
            Image image = new Image { Source = imageSource };
            Grid.SetRow(image, 0);
            Grid.SetRowSpan(image, 7);
            Grid.SetColumn(image, 0);
            Grid.SetColumnSpan(image, 5);
            MainGrid.Children.Add(image);
        }

        private void StoppenButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OptiesButtonClick(object sender, RoutedEventArgs e)
        {
            OptiesWindow optiesWindow = new OptiesWindow(this);
            optiesWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void HighscoresButtonClick(object sender, RoutedEventArgs e)
        {
            ScoreWindow scoreWindow = new ScoreWindow(this);
            scoreWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void SpelenButtonClick(object sender, RoutedEventArgs e)
        {
            SelectieWindow selectieWindow = new SelectieWindow(this);
            selectieWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
