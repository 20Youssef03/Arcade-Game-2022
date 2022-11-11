using ArcadeGame2022;
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

namespace Arcade_Game_2022
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        Window window;
        ImageSource imageSource1;
        ImageSource imageSource2;
        string spelerNaam1;
        string spelerNaam2;

        public HelpWindow(Window window, ImageSource imageSource1, ImageSource imageSource2, string spelerNaam1, string spelerNaam2)
        {
            this.window = window;
            this.imageSource1 = imageSource1;
            this.imageSource2 = imageSource2;
            this.spelerNaam1 = spelerNaam1;
            this.spelerNaam2 = spelerNaam2;
            InitializeComponent();
            foreach (Rectangle x in HelpCanvas.Children.OfType<Rectangle>())
            {
                if (x.Name == "punt")
                {
                    x.Fill = new ImageBrush // Methode in de reader werkte niet, methode in Microsoft documentatie ook niet, vandaar de Directory.GetCurrentDirectory()
                    {
                        ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../punt.png")), // /Images/gras blok platform.jpg
                        Stretch = Stretch.Fill,
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 270, 270)
                    };
                }
            }
        }

        private void TerugButtonClick(object sender, RoutedEventArgs e)
        {
            window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void SpelenButtonClick(object sender, RoutedEventArgs e)
        {
            SpelenWindow spelenWindow = new SpelenWindow(imageSource1, imageSource2, spelerNaam1, spelerNaam2);
            spelenWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
