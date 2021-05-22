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
    /// Логика взаимодействия для SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        private MainWindow mainWindow;
        private SaleView saleView;
        public SecondWindow(MainWindow mainWindow, SaleView saleView, string message)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.mainWindow = mainWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findclient")
            {
                usc = new FindClientControl(this, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "findcity")
            {
                usc = new FindCityControl(this, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "findtourist")
            {
                usc = new FindTouristControl(this, saleView);
                GridMain.Children.Add(usc);
            }
        }
        public SecondWindow(MainWindow mainWindow, SaleView saleView, string message, string filterCity)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.mainWindow = mainWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findhotel")
            {
                usc = new FindHotelControl(this, saleView, filterCity);
                GridMain.Children.Add(usc);
            }
            if (message == "findtour")
            {
                usc = new FindTourControl(this, saleView, filterCity);
                GridMain.Children.Add(usc);
            }
        }


        public SecondWindow(MainWindow mainWindow, SaleView saleView, string message, int tourID)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.mainWindow = mainWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findaddservice")
            {
                usc = new FindAddServiceControl(this, saleView, tourID);
                GridMain.Children.Add(usc);
            }
            if (message == "findhotelroom")
            {
                usc = new FindHotelRoomControl(this, saleView, tourID);//tourID в данном случае является ИД отеля
                GridMain.Children.Add(usc);
            }


            if (message == "clientview")
            {
                usc = new ClientView(this, tourID, false, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "cityview")
            {
                usc = new CityView(this, tourID, false, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "hotelview")
            {
                usc = new HotelView(this, tourID, false, saleView, "");
                GridMain.Children.Add(usc);
            }
            if (message == "tourview")
            {
                usc = new TourView(this, tourID, false, saleView, "");
                GridMain.Children.Add(usc);
            }
        }

        public SecondWindow(MainWindow mainWindow, string message, int ID)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "toursales")
            {
                usc = new TourSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "hotelsales")
            {
                usc = new HotelSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "clientsales")
            {
                usc = new ClientSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "addservicesales")
            {
                usc = new AddServiceSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "citysales")
            {
                usc = new CitySalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "employeesales")
            {
                usc = new EmployeeSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "cityhotels")
            {
                usc = new CityHotelsControl(this, ID);
                GridMain.Children.Add(usc);
            }
        }

        public SecondWindow(MainWindow mainWindow, AddServiceView addServiceView, string message)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findtour")
            {
                usc = new FindTourControl(this, addServiceView);
                GridMain.Children.Add(usc);
            }
        }













        SecondWindow secondWindow;


        public SecondWindow(SecondWindow secondWindow, SaleView saleView, string message)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.secondWindow = secondWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findclient")
            {
                usc = new FindClientControl(this, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "findcity")
            {
                usc = new FindCityControl(this, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "findtourist")
            {
                usc = new FindTouristControl(this, saleView);
                GridMain.Children.Add(usc);
            }
        }
        public SecondWindow(SecondWindow secondWindow, SaleView saleView, string message, string filterCity)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.secondWindow = secondWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findhotel")
            {
                usc = new FindHotelControl(this, saleView, filterCity);
                GridMain.Children.Add(usc);
            }
            if (message == "findtour")
            {
                usc = new FindTourControl(this, saleView, filterCity);
                GridMain.Children.Add(usc);
            }
        }


        public SecondWindow(SecondWindow secondWindow, SaleView saleView, string message, int tourID)
        {
            InitializeComponent();
            this.saleView = saleView;
            this.secondWindow = secondWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findaddservice")
            {
                usc = new FindAddServiceControl(this, saleView, tourID);
                GridMain.Children.Add(usc);
            }
            if (message == "findhotelroom")
            {
                usc = new FindHotelRoomControl(this, saleView, tourID);//tourID в данном случае является ИД отеля
                GridMain.Children.Add(usc);
            }


            if (message == "clientview")
            {
                usc = new ClientView(this, tourID, false, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "cityview")
            {
                usc = new CityView(this, tourID, false, saleView);
                GridMain.Children.Add(usc);
            }
            if (message == "hotelview")
            {
                usc = new HotelView(this, tourID, false, saleView, "");
                GridMain.Children.Add(usc);
            }
            if (message == "tourview")
            {
                usc = new TourView(this, tourID, false, saleView, "");
                GridMain.Children.Add(usc);
            }
        }

        public SecondWindow(SecondWindow secondWindow, string message, int ID)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "toursales")
            {
                usc = new TourSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "hotelsales")
            {
                usc = new HotelSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "clientsales")
            {
                usc = new ClientSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "addservicesales")
            {
                usc = new AddServiceSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "citysales")
            {
                usc = new CitySalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "employeesales")
            {
                usc = new EmployeeSalesControl(this, ID);
                GridMain.Children.Add(usc);
            }
            if (message == "cityhotels")
            {
                usc = new CityHotelsControl(this, ID);
                GridMain.Children.Add(usc);
            }
        }
        public SecondWindow(SecondWindow secondWindow, AddServiceView addServiceView, string message)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            UserControl usc = null;
            GridMain.Children.Clear();
            if (message == "findtour")
            {
                usc = new FindTourControl(this, addServiceView);
                GridMain.Children.Add(usc);
            }
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
            //ExitMessageBox myMessageBox = new ExitMessageBox(this, "Вы уверены, что хотите закрыть приложение?", false);
            //myMessageBox.Show();
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
    }
}
