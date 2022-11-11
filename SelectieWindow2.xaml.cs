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
    /// Interaction logic for SelectieWindow2.xaml
    /// </summary>
    public partial class SelectieWindow2 : Window
    {
        Window window;
        ImageSource imageSource1;
        ImageSource imageSource2;
        string spelerNaam1;

        public SelectieWindow2(Window window, ImageSource imageSource1, string spelerNaam1)
        {
            this.window = window;
            this.imageSource1 = imageSource1;
            this.spelerNaam1 = spelerNaam1;

            InitializeComponent();

            SpelenButton.Background = new SolidColorBrush(Color.FromArgb(153, 0, 0, 0));

            ImageSource[] imageSources = { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/roze.png")), // /bin/Debug/netcoreapp3.1/Images/rood.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/blauw.png")), // /bin/Debug/netcoreapp3.1/Images/blauw.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/groen.png")), // /bin/Debug/netcoreapp3.1/Images/groen.png
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/geel.png")) // /bin/Debug/netcoreapp3.1/Images/geel.png
            };
            for (int i = 0; i < 4; i++)
            {
                Image image = new Image { Source = imageSources[i] };
                if (i < 2)
                {
                    Grid.SetRow(image, 3);
                }
                else
                {
                    Grid.SetRow(image, 5);
                }
                if (i % 2 == 0)
                {
                    Grid.SetColumn(image, 1);
                    Grid.SetColumnSpan(image, 2);
                }
                else
                {
                    Grid.SetColumn(image, 4);
                    Grid.SetColumnSpan(image, 1);
                }
                SelectieGrid2.Children.Add(image);
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
                Arcade_Game_2022.HelpWindow helpWindow = new Arcade_Game_2022.HelpWindow(this, imageSource1, imageSource2, spelerNaam1, NaamInvoer.Text);
                helpWindow.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
            }
        }

        private void Optie1Click(object sender, RoutedEventArgs e)
        { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
            imageSource2 = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/roze.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            ResetKeuze();
            Optie1.Fill = new SolidColorBrush(Color.FromArgb(153, 255, 0, 255));
            Optie1Button.Opacity = 1.0;
            Optie1Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            if (NaamInvoer.Text != "" && NaamInvoer.Text != "Naam..." && NaamInvoer.Text.Length <= 10)
                SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie2Click(object sender, RoutedEventArgs e)
        { // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
            imageSource2 = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/blauw.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            ResetKeuze();
            Optie2.Fill = new SolidColorBrush(Color.FromArgb(153, 0, 0, 255));
            Optie2Button.Opacity = 1.0;
            Optie2Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
            if (NaamInvoer.Text != "" && NaamInvoer.Text != "Naam..." && NaamInvoer.Text.Length <= 10)
                SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie3Click(object sender, RoutedEventArgs e)
        {
            imageSource2 = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/groen.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            ResetKeuze();
            Optie3.Fill = new SolidColorBrush(Color.FromArgb(153, 0, 255, 0));
            Optie3Button.Opacity = 1.0;
            Optie3Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255));
            if (NaamInvoer.Text != "" && NaamInvoer.Text != "Naam..." && NaamInvoer.Text.Length <= 10)
                SpelenButton.Background = Brushes.LightGray;
        }
        private void Optie4Click(object sender, RoutedEventArgs e)
        {
            imageSource2 = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/geel.png")); // /bin/Debug/netcoreapp3.1/Images/mario.png
            ResetKeuze();
            Optie4.Fill = new SolidColorBrush(Color.FromArgb(153, 255, 255, 0));
            Optie4Button.Opacity = 1.0;
            Optie4Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            if (NaamInvoer.Text != "" && NaamInvoer.Text != "Naam..." && NaamInvoer.Text.Length <= 10)
                SpelenButton.Background = Brushes.LightGray;
        }

        private void ResetKeuze()
        {
            Optie1.Fill = Brushes.Transparent;
            Optie2.Fill = Brushes.Transparent;
            Optie3.Fill = Brushes.Transparent;
            Optie4.Fill = Brushes.Transparent;
            Optie1Button.Opacity = 0.4;
            Optie2Button.Opacity = 0.4;
            Optie3Button.Opacity = 0.4;
            Optie4Button.Opacity = 0.4;
            Optie1Button.Foreground = Brushes.Black;
            Optie2Button.Foreground = Brushes.Black;
            Optie3Button.Foreground = Brushes.Black;
            Optie4Button.Foreground = Brushes.Black;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            NaamInvoer.Foreground = Brushes.Black;
            if (imageSource2 != null)
                SpelenButton.Background = Brushes.LightGray;
            if (NaamInvoer.Text.Length > 10)
            {
                NaamInvoer.Foreground = Brushes.Red;
                SpelenButton.Background = new SolidColorBrush(Color.FromArgb(153, 0, 0, 0));
            }
        }
    }
}
