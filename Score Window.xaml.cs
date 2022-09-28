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

namespace Arcade_Game_2022
{
    /// <summary>
    /// Interaction logic for Score_Window.xaml
    /// </summary>
    public partial class Score_Window : Window
    {
        public Score_Window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Score_Window_Terug_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
