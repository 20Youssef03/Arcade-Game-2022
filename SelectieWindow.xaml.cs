using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectieWindow.xaml
    /// </summary>
    public partial class SelectieWindow : Window
    {
        Window window;
        ImageSource imageSource;

        public SelectieWindow(Window window)
        {
            this.window = window;
            InitializeComponent();

            SpelenButton.Background = Brushes.DarkGray;

            ImageSource[] imageSources = { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/rood.png")), // /bin/Debug/netcoreapp3.1/Images/rood.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/blauw.png")), // /bin/Debug/netcoreapp3.1/Images/blauw.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/groen.png")), // /bin/Debug/netcoreapp3.1/Images/groen.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/geel.png")) // /bin/Debug/netcoreapp3.1/Images/geel.png
            };
            for (int i = 0; i < 4; i++)
            {
                Image image = new Image { Source = imageSources[i] };
                if (i < 2)
                {
                    Grid.SetRow(image, 1);
                }
                else
                {
                    Grid.SetRow(image, 3);
                }
                if (i % 2 == 0)
                {
                    Grid.SetColumn(image, 1);
                }
                else
                {
                    Grid.SetColumn(image, 3);
                }
                SelectieGrid.Children.Add(image);
            }
        }

        private void TerugButtonClick(object sender, RoutedEventArgs e)
        {
            window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void SpelenButtonClick(object sender, RoutedEventArgs e)
        {
            if (SpelenButton.Background == Brushes.LightGray)
            {
                SpelenWindow spelenWindow = new SpelenWindow(imageSource);
                spelenWindow.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
            }
        }

        private void Optie1Click(object sender, RoutedEventArgs e)
        { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
            imageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/rood.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            Optie1.Background = new SolidColorBrush(Color.FromArgb(51, 255, 0, 0));
            Optie2.Background = Brushes.Transparent;
            Optie3.Background = Brushes.Transparent;
            Optie4.Background = Brushes.Transparent;
            SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie2Click(object sender, RoutedEventArgs e)
        { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
            imageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/blauw.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            Optie1.Background = Brushes.Transparent;
            Optie2.Background = new SolidColorBrush(Color.FromArgb(51, 255, 0, 0));
            Optie3.Background = Brushes.Transparent;
            Optie4.Background = Brushes.Transparent;
            SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie3Click(object sender, RoutedEventArgs e)
        {
            imageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/groen.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            Optie1.Background = Brushes.Transparent;
            Optie2.Background = Brushes.Transparent;
            Optie3.Background = new SolidColorBrush(Color.FromArgb(51, 255, 0, 0));
            Optie4.Background = Brushes.Transparent;
            SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie4Click(object sender, RoutedEventArgs e)
        {
            imageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Images/geel.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            Optie1.Background = Brushes.Transparent;
            Optie2.Background = Brushes.Transparent;
            Optie3.Background = Brushes.Transparent;
            Optie4.Background = new SolidColorBrush(Color.FromArgb(51, 255, 0, 0));
            SpelenButton.Background = Brushes.LightGray;
        }
    }
}
