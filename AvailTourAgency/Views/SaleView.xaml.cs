using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvailTourAgency.Models;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для SaleView.xaml
    /// </summary>
    public partial class SaleView : UserControl
    {
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private string message;

        private int tourID;
        private int hotelID;
        private int clientID;
        private int addServiceID;
        private int cityID;
        private int employeeID;


        public int CurrID;
        private Sale CurrSale;
        private bool isNew;
        private bool touristsShowDeleted = false;
        private string touristsSorting = "id";
        private string touristsAscOrDesc = "desc";
        private bool roomsShowDeleted = false;
        private string roomsSorting = "id";
        private string roomsAscOrDesc = "desc";
        private bool addServicesShowDeleted = false;
        private string addServicesSorting = "id";
        private string addServicesAscOrDesc = "desc";

        private List<int> addServicesIdList = new List<int>();
        private List<int> hotelRoomsIdList = new List<int>();
        private List<int> touristsIdList = new List<int>();

        private int choosenClientID;
        private int choosenCityID;
        private int choosenHotelID;
        private int choosenTourID;
        private int choosenTDPID;

        private ObservableCollection<TouristItem> TouristsItems { get; set; } = new ObservableCollection<TouristItem>();
        private ObservableCollection<HotelRoomItem> RoomsItems { get; set; } = new ObservableCollection<HotelRoomItem>();
        private ObservableCollection<AddServiceItem> AddServicesItems { get; set; } = new ObservableCollection<AddServiceItem>();


        private ObservableCollection<TouristItem> OldTouristsItems { get; set; } = new ObservableCollection<TouristItem>();
        private ObservableCollection<HotelRoomItem> OldRoomsItems { get; set; } = new ObservableCollection<HotelRoomItem>();
        private ObservableCollection<AddServiceItem> OldAddServicesItems { get; set; } = new ObservableCollection<AddServiceItem>();



        public SaleView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;

            TouristsGrid.Sorting += new DataGridSortingEventHandler(TouristsSortHandler);
            TouristsGrid.CurrentColumn = TouristsGrid.Columns[0];
            TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(HotelRoomsSortHandler);
            HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
            HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            AddServicesGrid.Sorting += new DataGridSortingEventHandler(AddServicesSortHandler);
            AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[0];
            AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditTouristsGrid.Sorting += new DataGridSortingEventHandler(TouristsSortHandler);
            EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[0];
            EditTouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditHotelRoomsGrid.Sorting += new DataGridSortingEventHandler(HotelRoomsSortHandler);
            EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[0];
            EditHotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditAddServicesGrid.Sorting += new DataGridSortingEventHandler(AddServicesSortHandler);
            EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[0];
            EditAddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            if (isNew)
            {
                SaleViewPanel.Visibility = Visibility.Collapsed;
                SaleEditPanel.Visibility = Visibility.Visible;
                EditSale.Visibility = Visibility.Collapsed;
                SaveSale.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Sales.GetSaleByID(ID).Deleted)
                {
                    EditSale.Visibility = Visibility.Collapsed;
                    DelSale.Visibility = Visibility.Collapsed;
                }

                FillData();
                FillTouristsGrid();
                FillHotelRoomsGrid();
                FillAddServicesGrid();
                OldRoomsItems = RoomsItems;
                OldTouristsItems = TouristsItems;
                OldAddServicesItems = AddServicesItems;
            }


            if (!Right.CheckAccess("EditSale"))
            {
                EditSale.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelSale"))
            {
                DelSale.Visibility = Visibility.Collapsed;
            }
        }



        public SaleView(SecondWindow secondWindow, string message, int tourID, int ID, bool isNew)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.message = message;
            this.isNew = isNew;

            if (message == "toursales")
            {
                this.tourID = tourID;
            }
            if (message == "hotelsales")
            {
                this.hotelID = tourID;
            }
            if (message == "clientsales")
            {
                this.clientID = tourID;
            }
            if (message == "addservicesales")
            {
                this.addServiceID = tourID;
            }
            if (message == "citysales")
            {
                this.cityID = tourID;
            }
            if (message == "employeesales")
            {
                this.employeeID = tourID;
            }

            TouristsGrid.Sorting += new DataGridSortingEventHandler(TouristsSortHandler);
            TouristsGrid.CurrentColumn = TouristsGrid.Columns[0];
            TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(HotelRoomsSortHandler);
            HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
            HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            AddServicesGrid.Sorting += new DataGridSortingEventHandler(AddServicesSortHandler);
            AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[0];
            AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditTouristsGrid.Sorting += new DataGridSortingEventHandler(TouristsSortHandler);
            EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[0];
            EditTouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditHotelRoomsGrid.Sorting += new DataGridSortingEventHandler(HotelRoomsSortHandler);
            EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[0];
            EditHotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            EditAddServicesGrid.Sorting += new DataGridSortingEventHandler(AddServicesSortHandler);
            EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[0];
            EditAddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;

            if (isNew)
            {
                SaleViewPanel.Visibility = Visibility.Collapsed;
                SaleEditPanel.Visibility = Visibility.Visible;
                EditSale.Visibility = Visibility.Collapsed;
                SaveSale.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Sales.GetSaleByID(ID).Deleted)
                {
                    EditSale.Visibility = Visibility.Collapsed;
                    DelSale.Visibility = Visibility.Collapsed;
                }

                FillData();
                FillTouristsGrid();
                FillHotelRoomsGrid();
                FillAddServicesGrid();
                OldRoomsItems = RoomsItems;
                OldTouristsItems = TouristsItems;
                OldAddServicesItems = AddServicesItems;
            }

            if (!Right.CheckAccess("EditSale"))
            {
                EditSale.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelSale"))
            {
                DelSale.Visibility = Visibility.Collapsed;
            }
        }


        private void FillData()
        {
            CurrSale = Sales.GetSaleByID(CurrID);
            
            IdBox.Text = CurrID.ToString();

            SaleDateBox.Text = CurrSale.GetSaleDate().ToString();

            PriceBox.Text = CurrSale.Price.ToString();
            PaymentMethodBox.Text = CurrSale.PaymentMethod;
            EmployeeBox.Text = CurrSale.GetEmployee().FIO;
            if (CurrSale.GetClient() != null) ClientBox.Text = CurrSale.GetClient().FIO;
            if (CurrSale.GetCity() != null) CityBox.Text = CurrSale.GetCity().Name;
            if (CurrSale.GetHotel() != null) HotelBox.Text = CurrSale.GetHotel().Name;
            if (CurrSale.GetTour() != null && CurrSale.GetTourDatesPrice() != null && CurrSale.GetTour().Name != "")  TourBox.Text = $"{CurrSale.GetTour().Name} (Вариация: {CurrSale.GetTourDatesPrice().ID})";
        }

        private void FillHotelRoomsGrid()
        {
            RoomsItems = HotelRooms.GetHotelRoomsInSaleForTable(roomsShowDeleted, CurrID, roomsSorting, roomsAscOrDesc);
            HotelRoomsGrid.ItemsSource = RoomsItems;
            HotelRoomsShowSortingAgain();
        }
        private void FillTouristsGrid()
        {
            TouristsItems = Tourists.GetTouristsForTable(touristsShowDeleted, CurrID, touristsSorting, touristsAscOrDesc);
            TouristsGrid.ItemsSource = TouristsItems;
            TouristsShowSortingAgain();
        }
        private void FillAddServicesGrid()
        {
            AddServicesItems = AddServices.GetAddServicesInSaleForTable(addServicesShowDeleted, CurrID, touristsSorting, touristsAscOrDesc);
            AddServicesGrid.ItemsSource = AddServicesItems;
            AddServicesShowSortingAgain();
        }

        private void FillEditHotelRoomsGrid()
        {
            RoomsItems = HotelRooms.GetHotelRoomsInSaleForTable(roomsShowDeleted, CurrID, roomsSorting, roomsAscOrDesc);
            EditHotelRoomsGrid.ItemsSource = RoomsItems;
            HotelRoomsShowSortingAgain();
        }
        private void FillEditTouristsGrid()
        { 
            TouristsItems = Tourists.GetTouristsForTable(touristsShowDeleted, CurrID, touristsSorting, touristsAscOrDesc);
            EditTouristsGrid.ItemsSource = TouristsItems;
            TouristsShowSortingAgain();
        }
        private void FillEditAddServicesGrid()
        {
            AddServicesItems = AddServices.GetAddServicesInSaleForTable(addServicesShowDeleted, CurrID, touristsSorting, touristsAscOrDesc);
            EditAddServicesGrid.ItemsSource = AddServicesItems;
            AddServicesShowSortingAgain();
        }

        //private void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

            
        //    idCollection = Sales.GetIdsForFilters();
            

        //    IdComboBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        //}


        void TouristsSortHandler(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header != null)
            {
                e.Handled = true;
                var column = e.Column;
                column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                touristsAscOrDesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";
                var columnName = e.Column.Header.ToString();

                if (columnName == "Номер") { touristsSorting = "id"; }
                if (columnName == "ФИО") { touristsSorting = "fio"; }
                if (columnName == "Номер телефона") { touristsSorting = "phonenumber"; }
                if (columnName == "Дата рождения") { touristsSorting = "dateofbitrh"; }
                if (columnName == "Тип документа") { touristsSorting = "documenttype"; }
                if (columnName == "Серия и номер документа") { touristsSorting = "documentdata"; }

                FillTouristsGrid();
            }
        }

        public void TouristsShowSortingAgain()
        {
            if (touristsSorting == "id") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[0]; }
            if (touristsSorting == "fio") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[1]; }
            if (touristsSorting == "phonenumber") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[2]; }
            if (touristsSorting == "dateofbirth") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[3]; }
            if (touristsSorting == "documenttype") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[4]; }
            if (touristsSorting == "documentdata") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[5]; }

            if (touristsAscOrDesc == "desc") { TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        public void EditTouristsShowSortingAgain()
        {
            if (touristsSorting == "id") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[0]; }
            if (touristsSorting == "fio") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[1]; }
            if (touristsSorting == "phonenumber") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[2]; }
            if (touristsSorting == "dateofbirth") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[3]; }
            if (touristsSorting == "documenttype") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[4]; }
            if (touristsSorting == "documentdata") { EditTouristsGrid.CurrentColumn = EditTouristsGrid.Columns[5]; }

            if (touristsAscOrDesc == "desc") { EditTouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { EditTouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }


        void HotelRoomsSortHandler(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header != null)
            {
                e.Handled = true;
                var column = e.Column;
                column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                roomsAscOrDesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";
                var columnName = e.Column.Header.ToString();

                if (columnName == "Номер") { roomsSorting = "id"; }
                if (columnName == "Первый тип") { roomsSorting = "firsttypeofroom"; }
                if (columnName == "Второй тип") { roomsSorting = "secondtypeofroom"; }
                if (columnName == "Цена за сутки") { roomsSorting = "price24"; }
                if (columnName == "Описание") { roomsSorting = "description"; }

                FillHotelRoomsGrid();
            }
        }

        public void HotelRoomsShowSortingAgain()
        {
            if (roomsSorting == "id") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0]; }
            if (roomsSorting == "firsttypeofroom") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[1]; }
            if (roomsSorting == "secondtypeofroom") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[2]; }
            if (roomsSorting == "price24") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[3]; }
            if (roomsSorting == "description") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[4]; }

            if (roomsAscOrDesc == "desc") { HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        public void EditHotelRoomsShowSortingAgain()
        {
            if (roomsSorting == "id") { EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[0]; }
            if (roomsSorting == "firsttypeofroom") { EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[1]; }
            if (roomsSorting == "secondtypeofroom") { EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[2]; }
            if (roomsSorting == "price24") { EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[3]; }
            if (roomsSorting == "description") { EditHotelRoomsGrid.CurrentColumn = EditHotelRoomsGrid.Columns[4]; }

            if (roomsAscOrDesc == "desc") { EditHotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { EditHotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void AddServicesSortHandler(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header != null)
            {
                e.Handled = true;
                var column = e.Column;
                column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                addServicesAscOrDesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";
                var columnName = e.Column.Header.ToString();

                if (columnName == "Номер") { addServicesSorting = "id"; }
                if (columnName == "Название") { addServicesSorting = "name"; }
                if (columnName == "Цена") { addServicesSorting = "price"; }
                if (columnName == "Количество") { addServicesSorting = "quantity"; }

                FillAddServicesGrid();
            }
        }

        public void AddServicesShowSortingAgain()
        {
            if (addServicesSorting == "id") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[0]; }
            if (addServicesSorting == "name") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[1]; }
            if (addServicesSorting == "price") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[2]; }
            if (addServicesSorting == "quantity") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[3]; }

            if (roomsAscOrDesc == "desc") { AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }
        public void EditAddServicesShowSortingAgain()
        {
            if (addServicesSorting == "id") { EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[0]; }
            if (addServicesSorting == "name") { EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[1]; }
            if (addServicesSorting == "price") { EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[2]; }
            if (addServicesSorting == "quantity") { EditAddServicesGrid.CurrentColumn = EditAddServicesGrid.Columns[3]; }

            if (roomsAscOrDesc == "desc") { EditAddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { EditAddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        private void comeBackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            comeBackButton.Foreground = Brushes.DarkBlue;
        }

        private void comeBackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            comeBackButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 53, 128, 191));
        }

        private void comeBackButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            comeBackButton.Foreground = Brushes.CadetBlue;
        }

        private void comeBackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new SalesControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else
            {
                if (message == "toursales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new TourSalesControl(secondWindow, tourID);
                    secondWindow.GridMain.Children.Add(usc);
                }
                if (message == "hotelsales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new HotelSalesControl(secondWindow, hotelID);
                    secondWindow.GridMain.Children.Add(usc);
                }
                if (message == "clientsales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new ClientSalesControl(secondWindow, clientID);
                    secondWindow.GridMain.Children.Add(usc);
                }
                if (message == "addservicesales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new AddServiceSalesControl(secondWindow, addServiceID);
                    secondWindow.GridMain.Children.Add(usc);
                }
                if (message == "citysales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new CitySalesControl(secondWindow, cityID);
                    secondWindow.GridMain.Children.Add(usc);
                }
                if (message == "employeesales")
                {
                    secondWindow.GridMain.Children.Clear();
                    UserControl usc = new EmployeeSalesControl(secondWindow, employeeID);
                    secondWindow.GridMain.Children.Add(usc);
                }
            }
        }
        
        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Sales.GetSaleByID(CurrID).Deleted)
        //        {
        //            EditSale.Visibility = Visibility.Collapsed;
        //            DelSale.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditSale"))
        //            {
        //                EditSale.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelSale"))
        //            {
        //                DelSale.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //        FillHotelRoomsGrid();
        //        FillTouristsGrid();
        //        FillAddServicesGrid();
        //        OldRoomsItems = RoomsItems;
        //        OldTouristsItems = TouristsItems;
        //        OldAddServicesItems = AddServicesItems;
        //    }
        //}

        private void EditSale_Click(object sender, RoutedEventArgs e)
        {
            SaleViewPanel.Visibility = Visibility.Collapsed;
            SaleEditPanel.Visibility = Visibility.Visible;
            EditSale.Visibility = Visibility.Collapsed;
            SaveSale.Visibility = Visibility.Visible;
            EditSaleDateBox.Text = SaleDateBox.Text;
            EditPriceBox.Text = PriceBox.Text;
            EditPaymentMethodBox.Text = PaymentMethodBox.Text;
            EditEmployeeBox.Text = EmployeeBox.Text;
            EditClientBox.Text = ClientBox.Text;
            EditCityBox.Text = CityBox.Text;
            EditHotelBox.Text = HotelBox.Text;
            EditTourBox.Text = TourBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;

            choosenClientID = CurrSale.ClientID;
            choosenCityID = CurrSale.GetCity().ID;
            choosenHotelID = CurrSale.GetHotel().ID;
            choosenTourID = CurrSale.GetTour().ID;
            choosenTDPID = CurrSale.GetTourDatesPrice().ID;

            FillEditTouristsGrid();
            FillEditHotelRoomsGrid();
            FillEditAddServicesGrid();
        }

        private void DelSale_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить продажу?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrSale.DelSale();
                    FillData();
                    FillTouristsGrid();
                    FillHotelRoomsGrid();
                    FillAddServicesGrid();
                    FillEditTouristsGrid();
                    FillEditHotelRoomsGrid();
                    FillEditAddServicesGrid();
                    EditSale.Visibility = Visibility.Collapsed;
                    DelSale.Visibility = Visibility.Collapsed;
                    AddAddService.Visibility = Visibility.Collapsed;
                    AddHotelRoom.Visibility = Visibility.Collapsed;
                    AddTourist.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                    DelHotelRoom.Visibility = Visibility.Collapsed;
                    DelTourist.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данная продажа ещё не сохранена. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void EditPriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void SaveSale_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrSale.Price = Validator.ValidateDecimal(EditPriceBox.Text);
                CurrSale.PaymentMethod = EditPaymentMethodBox.Text;
                CurrSale.ClientID = choosenClientID;
                CurrSale.TourDatesPriceID = choosenTDPID;
                CurrSale.EditDate = new DateTime(DateTime.UtcNow.Ticks);//ticks чтобы убрать "Z" в конце даты (из-за этого все рушится к чертям)
                
                result = CurrSale.EditSale();
            }
            else
            {
                CurrSale = new Sale();
                CurrSale.SaleDate = new DateTime(DateTime.UtcNow.Ticks);//ticks чтобы убрать "Z" в конце даты (из-за этого все рушится к чертям)
                CurrSale.EditDate = new DateTime(DateTime.UtcNow.Ticks);//ticks чтобы убрать "Z" в конце даты (из-за этого все рушится к чертям)

                CurrSale.EmployeeID = User.GetUser().ID; 
                
                CurrSale.Price = Validator.ValidateDecimal(EditPriceBox.Text);
                CurrSale.PaymentMethod = EditPaymentMethodBox.Text;
                CurrSale.ClientID = choosenClientID;
                CurrSale.TourDatesPriceID = choosenTDPID;

                result = CurrSale.AddSale();
            }



            if (result.Success)
            {
                CurrSale.EditAddServicesInSale(AddServicesItems, OldAddServicesItems);
                CurrSale.EditHotelRoomsInSale(RoomsItems, OldRoomsItems);
                CurrSale.EditTouristsInSale(TouristsItems, OldTouristsItems);
                OldAddServicesItems = AddServicesItems;
                OldRoomsItems = RoomsItems;
                OldTouristsItems = TouristsItems;

                CurrID = CurrSale.ID;
                FillData();
                FillTouristsGrid();
                FillHotelRoomsGrid();
                FillAddServicesGrid();
                FillEditTouristsGrid();
                FillEditHotelRoomsGrid();
                FillEditAddServicesGrid();
                SaleViewPanel.Visibility = Visibility.Visible;
                SaleEditPanel.Visibility = Visibility.Collapsed;
                EditSale.Visibility = Visibility.Visible;
                SaveSale.Visibility = Visibility.Collapsed;
                MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                EditingIndicator.Visibility = Visibility.Collapsed;
                isNew = false;
            }
            else
            {
                int errorCode = 0;
                if (result.Description.Contains("duplication"))
                {
                    errorCode = 1;
                }
                if (errorCode == 0)
                {
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Заполните все обязательные поля!", true);
                    myMessageBox.ShowDialog();
                }
                else
                {
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой тур уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void CheckAddService_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                AddServiceItem gridItem = (sender as CheckBox).DataContext as AddServiceItem;
                addServicesIdList.Add(gridItem.ID);
            }
            if (!(sender as CheckBox).IsChecked.Value)
            {
                AddServiceItem gridItem = (sender as CheckBox).DataContext as AddServiceItem;
                addServicesIdList.Remove(gridItem.ID);
            }
        }

        private void CheckHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                HotelRoomItem gridItem = (sender as CheckBox).DataContext as HotelRoomItem;
                hotelRoomsIdList.Add(gridItem.ID);
            }
            if (!(sender as CheckBox).IsChecked.Value)
            {
                HotelRoomItem gridItem = (sender as CheckBox).DataContext as HotelRoomItem;
                hotelRoomsIdList.Remove(gridItem.ID);
            }
        }

        private void CheckTourist_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                TouristItem gridItem = (sender as CheckBox).DataContext as TouristItem;
                touristsIdList.Add(gridItem.ID);
            }
            if (!(sender as CheckBox).IsChecked.Value)
            {
                TouristItem gridItem = (sender as CheckBox).DataContext as TouristItem;
                touristsIdList.Add(gridItem.ID);
            }
        }

        private void FindClient_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findclient");
                secondWindow.Show();
            }
            else
            {
                SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findclient");
                secondWindowNew.Show();
            }
        }

        private void FindCity_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findcity");
                secondWindow.Show();
            }
            else
            {
                SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findcity");
                secondWindowNew.Show();
            }
        }

        private void FindHotel_Click(object sender, RoutedEventArgs e)
        {
            if (EditCityBox.Text == "")
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выбранного города!", "Перед тем как выбрать отель нужно выбрать город!", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findhotel", EditCityBox.Text);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findhotel", EditCityBox.Text);
                    secondWindowNew.Show();
                }
            }
        }

        private void FindTour_Click(object sender, RoutedEventArgs e)
        {
            if (EditCityBox.Text == "")
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выбранного города!", "Перед тем как выбрать тур нужно выбрать город!", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findtour", EditCityBox.Text);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findtour", EditCityBox.Text);
                    secondWindowNew.Show();
                }
            }
        }

        private void AddHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            if (EditHotelBox.Text == "")
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выбранного отеля!", "Перед тем как выбрать номера отеля нужно выбрать отель!", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findhotelroom", choosenHotelID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findhotelroom", choosenHotelID);
                    secondWindowNew.Show();
                }
            }
        }
        public void AddHotelRoomItem(int hotelRoomID)
        {
            HotelRoom hotelRoom = HotelRooms.GetHotelRoomByID(hotelRoomID);


            Dictionary<int, string> FirstTypesOfRoom = new Dictionary<int, string>();
            string[] ft = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
            foreach (string t in ft)
            {
                FirstTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            Dictionary<int, string> SecondTypesOfRoom = new Dictionary<int, string>();
            string[] st = ConfigurationManager.AppSettings["SecondTypes"].Split(';');
            foreach (string t in st)
            {
                SecondTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }


            RoomsItems.Add(new HotelRoomItem
            {
                ID = hotelRoom.ID,
                FirstTypeOfRoom = FirstTypesOfRoom[hotelRoom.FirstTypeOfRoom],
                SecondTypeOfRoom = SecondTypesOfRoom[hotelRoom.SecondTypeOfRoom],
                Price24 = hotelRoom.Price24,
                Description = hotelRoom.Description
            });
            EditHotelRoomsGrid.ItemsSource = RoomsItems;
            EditPriceBox.Text = CalcTotalPrice().ToString();
        }
        private void DelHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            if (hotelRoomsIdList.Count != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Подтвердите действие!", "Вы уверены, что хотите удалить выделенные номера из продажи?", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    foreach (int id in hotelRoomsIdList)
                    {
                        RoomsItems.Remove(RoomsItems.Where(r => r.ID == id).FirstOrDefault());
                    }
                    EditHotelRoomsGrid.ItemsSource = RoomsItems;
                    EditPriceBox.Text = CalcTotalPrice().ToString();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выделенных записей.", "Сначала выберите номера для последующего удаления их из продажи.", true);
                myMessageBox.ShowDialog();
            }
        }

        private void AddAddService_Click(object sender, RoutedEventArgs e)
        {
            if (EditTourBox.Text == "")
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выбранного тура!", "Перед тем как выбрать дополнительные услуги нужно выбрать тур!", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findaddservice", choosenTourID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findaddservice", choosenTourID);
                    secondWindowNew.Show();
                }
            }
        }
        public void AddAdditionalService(int addServiceID, int quantity)
        {
            AddService addService = AddServices.GetAddServiceByID(addServiceID);
            AddServicesItems.Add(new AddServiceItem
            {
                ID = addService.ID,
                Name = addService.Name, 
                Price = addService.Price.ToString(),
                Quantity = quantity
            });
            EditAddServicesGrid.ItemsSource = AddServicesItems;
            EditPriceBox.Text = CalcTotalPrice().ToString();
        }
        private void DelAddService_Click(object sender, RoutedEventArgs e)
        {
            if (addServicesIdList.Count != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Подтвердите действие!", "Вы уверены, что хотите удалить выделенные услуги из продажи?", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    foreach (int id in addServicesIdList)
                    {
                        AddServicesItems.Remove(AddServicesItems.Where(a => a.ID == id).FirstOrDefault());
                    }
                    EditAddServicesGrid.ItemsSource = AddServicesItems;
                    EditPriceBox.Text = CalcTotalPrice().ToString();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выделенных записей.", "Сначала выберите услуги для последующего удаления их из продажи.", true);
                myMessageBox.ShowDialog();
            }
        }

        private void AddTourist_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findtourist");
                secondWindow.Show();
            }
            else
            {
                SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findtourist");
                secondWindowNew.Show();
            }
        }
        public void AddTouristItem(int touristID)
        {
            Tourist tourist = Tourists.GetTouristByID(touristID);

            Dictionary<int, string> DocumentTypes = new Dictionary<int, string>();
            string[] ft = ConfigurationManager.AppSettings["DocumentTypes"].Split(';');
            foreach (string t in ft)
            {
                DocumentTypes.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            TouristsItems.Add(new TouristItem
            {
                ID = tourist.ID,
                FIO = tourist.FIO,
                PhoneNumber = tourist.PhoneNumber,
                DateOfBirth = tourist.DateOfBirth.Value.ToShortDateString(),
                DocumentType = DocumentTypes[tourist.DocumentType],
                DocumentData = tourist.DocumentData
            });
            EditTouristsGrid.ItemsSource = TouristsItems;
            EditPriceBox.Text = CalcTotalPrice().ToString();
        }
        private void DelTourist_Click(object sender, RoutedEventArgs e)
        {
            if (touristsIdList.Count != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Подтвердите действие!", "Вы уверены, что хотите удалить выделенных туристов из продажи?", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    foreach (int id in touristsIdList)
                    {
                        TouristsItems.Remove(TouristsItems.Where(t => t.ID == id).FirstOrDefault());
                    }
                    EditTouristsGrid.ItemsSource = TouristsItems;
                    EditPriceBox.Text = CalcTotalPrice().ToString();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выделенных записей.", "Сначала выберите туристов для последующего удаления их из продажи.", true);
                myMessageBox.ShowDialog();
            }
        }


        public void ChangeClient(int clientID)
        {
            choosenClientID = clientID;
            EditClientBox.Text = Clients.GetClientByID(choosenClientID).FIO;
        }
        public void ChangeCity(int cityID)
        {
            choosenCityID = cityID;
            EditCityBox.Text = Cities.GetCityByID(choosenCityID).Name;
            if (choosenHotelID != 0 && choosenCityID != Hotels.GetHotelByID(choosenHotelID).CityID)
            {
                choosenHotelID = 0;
                EditHotelBox.Text = "";
                RoomsItems = new ObservableCollection<HotelRoomItem>();
                EditHotelRoomsGrid.ItemsSource = RoomsItems;
                EditPriceBox.Text = CalcTotalPrice().ToString();
            }
            if (choosenTourID != 0 && choosenCityID != Tours.GetTourByID(choosenTourID).CityID)
            {
                choosenTourID = 0;
                EditTourBox.Text = "";
            }
        }
        public void ChangeHotel(int hotelID)
        {
            if (choosenHotelID != hotelID)
            {
                RoomsItems = new ObservableCollection<HotelRoomItem>();
                EditHotelRoomsGrid.ItemsSource = RoomsItems;
                choosenHotelID = hotelID;
                EditHotelBox.Text = Hotels.GetHotelByID(choosenHotelID).Name;
                EditPriceBox.Text = CalcTotalPrice().ToString();
            }
        }
        public void ChangeTour(int tourID, int tdpID)
        {
            choosenTourID = tourID;
            choosenTDPID = tdpID;
            EditTourBox.Text = $"{Tours.GetTourByID(choosenTourID).Name} (Вариация: {choosenTDPID})";
            EditPriceBox.Text = CalcTotalPrice().ToString();
        }

        public decimal CalcTotalPrice()
        {
            decimal price = 0;
            if (choosenTDPID != 0 && TouristsItems != null && TouristsItems.Count != 0)
            {
                price += TourDatesPrices.GetTDPByID(choosenTDPID).Price * TouristsItems.Count;
            }
            if (RoomsItems != null && RoomsItems.Count != 0 && choosenTDPID != 0)
            {
                foreach (HotelRoomItem hr in RoomsItems)
                {
                    price += hr.Price24 * TourDatesPrices.GetTDPByID(choosenTDPID).Length;
                }
            }
            if (AddServicesItems != null && AddServicesItems.Count != 0)
            {
                foreach (AddServiceItem a in AddServicesItems)
                {
                    price += Convert.ToDecimal(decimal.Parse(a.Price) * a.Quantity);
                }
            }
            return price;
        }

        private void CityBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrSale.GetCity().Name != "")
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "cityview", CurrSale.GetCity().ID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "cityview", CurrSale.GetCity().ID);
                    secondWindowNew.Show();
                }
            }
        }

        private void ClientBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrSale.ClientID != 0)
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "clientview", CurrSale.ClientID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "clientview", CurrSale.ClientID);
                    secondWindowNew.Show();
                }
            }
        }

        private void HotelBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrSale.GetHotel().Name != "")
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "hotelview", CurrSale.GetHotel().ID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "hotelview", CurrSale.GetHotel().ID);
                    secondWindowNew.Show();
                }
            }
        }

        private void TourBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrSale.TourDatesPriceID != 0)
            {
                if (mainWindow != null)
                {
                    SecondWindow secondWindow = new SecondWindow(mainWindow, this, "tourview", CurrSale.GetTour().ID);
                    secondWindow.Show();
                }
                else
                {
                    SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "tourview", CurrSale.GetTour().ID);
                    secondWindowNew.Show();
                }
            }
        }

        private void ClientBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ClientBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 200, 191));
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ClientBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ClientBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 53, 128, 191));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void CityBox_MouseEnter(object sender, MouseEventArgs e)
        {
            CityBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 200, 191));
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void CityBox_MouseLeave(object sender, MouseEventArgs e)
        {
            CityBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 53, 128, 191));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HotelBox_MouseEnter(object sender, MouseEventArgs e)
        {
            HotelBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 200, 191));
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void HotelBox_MouseLeave(object sender, MouseEventArgs e)
        {
            HotelBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 53, 128, 191));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TourBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TourBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 200, 191));
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void TourBox_MouseLeave(object sender, MouseEventArgs e)
        {
            TourBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 53, 128, 191));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Sales.GetSaleByID(CurrID).Deleted)
                {
                    EditSale.Visibility = Visibility.Collapsed;
                    DelSale.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditSale"))
                    {
                        EditSale.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelSale"))
                    {
                        DelSale.Visibility = Visibility.Visible;
                    }
                }
                FillData();
                FillHotelRoomsGrid();
                FillTouristsGrid();
                FillAddServicesGrid();
                OldRoomsItems = RoomsItems;
                OldTouristsItems = TouristsItems;
                OldAddServicesItems = AddServicesItems;
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Sale sale = Sales.GetSaleByID(CurrID + 1);
            if (sale != null)
            {
                CurrID++;
                if (sale.Deleted)
                {
                    EditSale.Visibility = Visibility.Collapsed;
                    DelSale.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditSale"))
                    {
                        EditSale.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelSale"))
                    {
                        DelSale.Visibility = Visibility.Visible;
                    }
                }
                FillData();
                FillHotelRoomsGrid();
                FillTouristsGrid();
                FillAddServicesGrid();
                OldRoomsItems = RoomsItems;
                OldTouristsItems = TouristsItems;
                OldAddServicesItems = AddServicesItems;
            }
        }
    }
}
