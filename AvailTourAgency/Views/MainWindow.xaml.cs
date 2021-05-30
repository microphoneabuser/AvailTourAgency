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
using AvailTourAgency.Models;
using System.IO;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GridMain.Children.Clear();
            UserControl usc = new SalesControl(this);
            GridMain.Children.Add(usc);

            if (!Right.CheckAccess("SeeEmployees"))
            {
                EmployeesItem.Visibility = Visibility.Collapsed;
            }
            FillImage();

        }
        public void FillImage()
        {
            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.Length - 11, 10);
            string imgUri = System.IO.Path.Combine(appDir, Employees.GetEmployeeByID(User.GetUser().ID).Image);
            if (imgUri != null && imgUri != "")
            {
                UserIcon.Visibility = Visibility.Collapsed;
                UserImage.Visibility = Visibility.Visible;

                Uri uri = new Uri(imgUri);

                UserImage.Source = new BitmapImage(uri);
            }
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
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
            ExitMessageBox myMessageBox = new ExitMessageBox(this, "Вы уверены, что хотите закрыть приложение?", false);
            myMessageBox.Show();
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "SalesItem":
                    usc = new SalesControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "ClientsItem":
                    usc = new ClientsControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "EmployeesItem":
                    usc = new EmployeesControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "CitiesItem":
                    usc = new CitiesControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "AddServicesItem":
                    usc = new AddServicesControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "HotelsItem":
                    usc = new HotelsControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case "ToursItem":
                    usc = new ToursControl(this);
                    GridMain.Children.Add(usc);
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                default:
                    break;
            }
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            UserControl usc = new AccountView(this);
            GridMain.Children.Add(usc);
            ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }
    }
}
