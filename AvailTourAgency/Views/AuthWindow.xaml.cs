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
using AvailTourAgency.Models;
using System.Configuration;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            //LoginBox.Text = ConfigurationManager.AppSettings["FirstTypes"];
        }

        private void minus_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            minus_Button.Background = Brushes.RoyalBlue;
        }

        private void recover_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            recover_Button.Background = Brushes.RoyalBlue;
        }

        private void close_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_Button.Background = Brushes.RoyalBlue;

        }

        private void close_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void recover_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;

        }

        private void minus_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.Top = 0;
            }
            this.DragMove();
        }

        private void minus_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            minus_Button.Background = Brushes.LightGray;
        }

        private void recover_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            recover_Button.Background = Brushes.LightGray;
        }

        private void close_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.LightGray;
        }

        private void minus_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            minus_Button.Background = Brushes.Transparent;
        }

        private void recover_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            recover_Button.Background = Brushes.Transparent;
        }

        private void close_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.Transparent;
        }

        private void LoginBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down) { PasswordBox.Focus(); }
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down) { LoginBox.Focus(); }
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginBox.Background = Brushes.Transparent;
            LoginBox.ToolTip = null;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.Background = Brushes.Transparent;
            PasswordBox.ToolTip = null;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "")
            {
                LoginBox.ToolTip = "Заполните поле!";
                LoginBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
            }
            if (PasswordBox.Password == "")
            {
                PasswordBox.ToolTip = "Заполните поле!";
                PasswordBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
            }
            else
            {
                loadingGif.Visibility = Visibility.Visible;

                Result result = new Result();

                string login = LoginBox.Text;
                string pass = PasswordBox.Password;

                await Task.Run(() => result = User.Login(login, pass));

                if (result.Success)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Неправильно введены логин или пароль.", true);
                    myMessageBox.ShowDialog();
                }
                loadingGif.Visibility = Visibility.Collapsed;   
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { LoginButton_Click(sender, e); }
        }
    }
}
