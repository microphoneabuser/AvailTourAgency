using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для HotelRoomAddWindow.xaml
    /// </summary>
    public partial class HotelRoomAddWindow : Window
    {
        HotelView userControl;
        Dictionary<int, string> FirstTypesOfRoom = new Dictionary<int, string>();
        Dictionary<int, string> SecondTypesOfRoom = new Dictionary<int, string>();
        public HotelRoomAddWindow(HotelView userControl)
        {
            InitializeComponent();
            this.userControl = userControl;

            string[] ft = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
            foreach (string t in ft)
            {
                FirstTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            FirstTypeBox.ItemsSource = FirstTypesOfRoom.Values;

            string[] st = ConfigurationManager.AppSettings["SecondTypes"].Split(';');
            foreach (string t in st)
            {
                SecondTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            SecondTypeBox.ItemsSource = SecondTypesOfRoom.Values;

        }

        private void FirstTypeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FirstTypeBox.ToolTip = null;
            FirstTypeBox.Background = Brushes.Transparent;
        }

        private void SecondTypeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SecondTypeBox.ToolTip = null;
            SecondTypeBox.Background = Brushes.Transparent;
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionBox.ToolTip = null;
            DescriptionBox.Background = Brushes.Transparent;
        }

        private void QuantityBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            QuantityBox.ToolTip = null;
            QuantityBox.Background = Brushes.Transparent;
        }

        private void SaveHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            HotelRoom hotelRoom = new HotelRoom();

            hotelRoom.FirstTypeOfRoom = FirstTypesOfRoom.Where(f => f.Value == FirstTypeBox.Text).FirstOrDefault().Key;
            hotelRoom.SecondTypeOfRoom = SecondTypesOfRoom.Where(f => f.Value == SecondTypeBox.Text).FirstOrDefault().Key;

            hotelRoom.Description = Validator.ValidateStringWithoutLower(DescriptionBox.Text);
            try { hotelRoom.Price24 = decimal.Parse(PriceBox.Text); } catch { hotelRoom.Price24 = 0; }
            try { hotelRoom.Quantity = int.Parse(QuantityBox.Text); } catch { hotelRoom.Quantity = 0; }

            hotelRoom.HotelID = userControl.CurrID;
            Result result = hotelRoom.AddHotelRoom();

            if (result.Success)
            {
                ExitMessageBox myMessageBox = new ExitMessageBox(this, "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                this.Close();
                userControl.ReturnFromAddingHotelRoom();
            }
            else
            {
                int errorCode = 0;
                if (result.Description.Contains("empty_firsttypeofroom"))
                {
                    FirstTypeBox.ToolTip = "Это поле не может быть пустым!";
                    FirstTypeBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_secondtypeofroom"))
                {
                    SecondTypeBox.ToolTip = "Это поле не может быть пустым!";
                    SecondTypeBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_price24"))
                {
                    PriceBox.ToolTip = "Это поле не может быть пустым!";
                    PriceBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_quantity"))
                {
                    QuantityBox.ToolTip = "Это поле не может быть пустым!";
                    QuantityBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("duplication"))
                {
                    errorCode = 1;
                }
                if (errorCode == 0)
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Заполните все обязательные поля!", true);
                    myMessageBox.ShowDialog();
                }
                else
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Такой номер уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;

        }

        private void PriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PriceBox.ToolTip = null;
            PriceBox.Background = Brushes.Transparent;
        }

        private void Dragger_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.Top = 0;
            }
            this.DragMove();
        }

        private void close_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_Button.Background = Brushes.RoyalBlue;

        }

        private void close_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.LightGray;

        }

        private void close_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.Transparent;

        }

        private void close_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();

        }

        private void FirstTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FirstTypeBox.ToolTip = null;
            FirstTypeBox.Background = Brushes.Transparent;
        }

        private void SecondTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SecondTypeBox.ToolTip = null;
            SecondTypeBox.Background = Brushes.Transparent;
        }
    }
}
