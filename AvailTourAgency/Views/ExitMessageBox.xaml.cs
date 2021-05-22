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
using System.Windows.Shapes;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для MyMessageBox.xaml
    /// </summary>
    public partial class ExitMessageBox : Window
    {
        Window CurrentWindow;
        public ExitMessageBox(Window window, string text, bool isOk)
        {
            InitializeComponent();
            CurrentWindow = window;
            Text.Text = text;
            if (isOk)
            {
                YesNoGrid.Visibility = Visibility.Collapsed;
                OkGrid.Visibility = Visibility.Visible;
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (YesNoGrid.Visibility == Visibility.Visible)
                {
                    YesButton_Click(sender, e);
                }
                else
                {
                    NoButton_Click(sender, e);
                }
            }
        }
        private void close_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_Button.Background = Brushes.RoyalBlue;
        }

        private void close_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void close_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.LightGray;
        }
        private void close_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.Transparent;
        }
        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            CurrentWindow.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
