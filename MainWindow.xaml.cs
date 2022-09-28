using System;
using System.Collections.Generic;
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

namespace Arcade_Game_2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Main_Window_Stoppen_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Main_Window_Opties_Button_Click(object sender, RoutedEventArgs e)
        {
            Opties_Window opties_Window = new Opties_Window();
            opties_Window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void Main_Window_Score_Button_Click(object sender, RoutedEventArgs e)
        {
            Score_Window score_Window = new Score_Window();
            score_Window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void Main_Window_Spelen_Button_Click(object sender, RoutedEventArgs e)
        {
            Spelen_Window spelen_Window = new Spelen_Window();
            spelen_Window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
