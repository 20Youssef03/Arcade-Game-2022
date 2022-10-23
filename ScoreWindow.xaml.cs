using System;
using System.Collections.Generic;
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
            this.window = window;
            InitializeComponent();
        }

        private void TerugButtonClick(object sender, RoutedEventArgs e)
        {
            window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
