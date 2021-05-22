using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для TourDatesPriceAddWindow.xaml
    /// </summary>
    public partial class TourDatesPriceAddWindow : Window
    {
        TourView userControl;
        Dictionary<int, string> FlightClasses = new Dictionary<int, string>();
        public TourDatesPriceAddWindow(TourView userControl)
        {
            InitializeComponent();
            this.userControl = userControl;

            string[] fc = ConfigurationManager.AppSettings["FlightClass"].Split(';');
            foreach (string c in fc)
            {
                FlightClasses.Add(int.Parse(c.Split(':')[0]), c.Split(':')[1]);
            }

            FlightClassBox.ItemsSource = FlightClasses.Values;
        }

        private void SaveTourDatesPrice_Click(object sender, RoutedEventArgs e)
        {
            TourDatesPrice tourDatesPrice = new TourDatesPrice();
            tourDatesPrice.Airline = Validator.ValidateStringWithoutLower(AirlineBox.Text);
            tourDatesPrice.Deleted = false;

            tourDatesPrice.FlightClass = FlightClasses.Where(f => f.Value == FlightClassBox.Text).FirstOrDefault().Key;

            tourDatesPrice.FlyDateThere = FirstDatePicker.SelectedDate;
            tourDatesPrice.FlyDateBack = SecondDatePicker.SelectedDate;
            try { tourDatesPrice.Length = (tourDatesPrice.FlyDateBack.Value.Date - tourDatesPrice.FlyDateThere.Value.Date).Days; } catch { tourDatesPrice.Length = 0; }
            tourDatesPrice.Luggage = Validator.ValidateStringWithoutLower(LuggageBox.Text);
            tourDatesPrice.Meals = Validator.ValidateStringWithoutLower(MealsBox.Text);
            tourDatesPrice.Plane = Validator.ValidateStringWithoutLower(PlaneBox.Text);
            try { tourDatesPrice.Price = Validator.ValidateDecimal(PriceBox.Text); } catch { tourDatesPrice.Price = 0; }
            try { tourDatesPrice.Quantity = int.Parse(QuantityBox.Text); } catch { tourDatesPrice.Quantity = 0; }
            tourDatesPrice.TourID = userControl.CurrID;
            Result result = tourDatesPrice.AddTourDatesPrice();

            if (result.Success)
            {
                ExitMessageBox myMessageBox = new ExitMessageBox(this, "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                this.Close();
                userControl.ReturnFromAddingTDP();
            }
            else
            {
                int errorCode = 0;
                if (result.Description.Contains("empty_flydatethere"))
                {
                    FirstDatePicker.ToolTip = "Это поле не может быть пустым!";
                    FirstDatePicker.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_flydateback"))
                {
                    SecondDatePicker.ToolTip = "Это поле не может быть пустым!";
                    SecondDatePicker.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_price"))
                {
                    PriceBox.ToolTip = "Это поле не может быть пустым!";
                    PriceBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_airline"))
                {
                    AirlineBox.ToolTip = "Это поле не может быть пустым!";
                    AirlineBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_plane"))
                {
                    PlaneBox.ToolTip = "Это поле не может быть пустым!";
                    PlaneBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_flightclass"))
                {
                    FlightClassBox.ToolTip = "Это поле не может быть пустым!";
                    FlightClassBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_luggage"))
                {
                    LuggageBox.ToolTip = "Это поле не может быть пустым!";
                    LuggageBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_meals"))
                {
                    MealsBox.ToolTip = "Это поле не может быть пустым!";
                    MealsBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                if (result.Description.Contains("wrong_dates"))
                {
                    errorCode = 2;
                }
                if (errorCode == 0)
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Заполните все обязательные поля!", true);
                    myMessageBox.ShowDialog();
                }
                else if (errorCode == 2)
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Некорректные даты!", true);
                    myMessageBox.ShowDialog();
                }
                else
                {
                    ExitMessageBox myMessageBox = new ExitMessageBox(this, "Такая вариация тура уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void close_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_Button.Background = Brushes.RoyalBlue;

        }

        private void close_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ExitMessageBox myMessageBox = new ExitMessageBox(this, "Вы уверены, что хотите закрыть приложение?", false);
            //myMessageBox.Show();
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
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.Top = 0;
            }
            this.DragMove();
        }

        private void FirstDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FirstDatePicker.ToolTip = null;
            FirstDatePicker.Background = Brushes.Transparent;
        }

        private void SecondDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SecondDatePicker.ToolTip = null;
            SecondDatePicker.Background = Brushes.Transparent;
        }

        private void PriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PriceBox.ToolTip = null;
            PriceBox.Background = Brushes.Transparent;
        }

        private void AirlineBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AirlineBox.ToolTip = null;
            AirlineBox.Background = Brushes.Transparent;
        }

        private void PlaneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaneBox.ToolTip = null;
            PlaneBox.Background = Brushes.Transparent;
        }

        private void FlightClassBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FlightClassBox.ToolTip = null;
            FlightClassBox.Background = Brushes.Transparent;
        }

        private void LuggageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LuggageBox.ToolTip = null;
            LuggageBox.Background = Brushes.Transparent;
        }

        private void MealsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MealsBox.ToolTip = null;
            MealsBox.Background = Brushes.Transparent;
        }

        private void QuantityBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            QuantityBox.ToolTip = null;
            QuantityBox.Background = Brushes.Transparent;
        }

        private void FlightClassBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlightClassBox.ToolTip = null;
            FlightClassBox.Background = Brushes.Transparent;
        }
    }
}
