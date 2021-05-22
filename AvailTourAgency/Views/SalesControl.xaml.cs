using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для SalesControl.xaml
    /// </summary>
    public partial class SalesControl : UserControl
    {
        private ObservableCollection<SaleItem> SalesItems { get; set; } = new ObservableCollection<SaleItem>();
        private MainWindow mainWindow;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private int filterID;
        private string filterClient;
        private DateTime? filterFDateSale;
        private DateTime? filterSDateSale;
        private string filterCity;
        private string filterTour;
        private DateTime? filterFDateFly;
        private DateTime? filterSDateFly;
        private string filterHotel;
        private string filterFPrice;
        private string filterSPrice;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount;

        public SalesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            SalesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            SalesGrid.CurrentColumn = SalesGrid.Columns[0];
            SalesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);

            if (!Right.CheckAccess("AddSale"))
            {
                AddSale.Visibility = Visibility.Collapsed;
            }
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                SalesItems = Sales.GetSalesForTable(showNotDeleted, showDeleted, filterID, filterClient,
                    filterFDateSale, filterSDateSale, filterCity, filterTour,
                    filterFDateFly, filterSDateFly, filterHotel, filterFPrice, filterSPrice,
                    sorting, ascordesc, count, page, ref genCount); ;
            });


            if (SalesItems.Count == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "По вашему запросу ничего не найдено.", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                GlobalPageCount.Text = (Math.Ceiling((double)genCount / count)).ToString();

                SalesGrid.ItemsSource = SalesItems;

                ShowSortingAgain();
                if (first)
                {
                    FillDataForFilters();
                }

                CurrentPageNum.Items.Clear();
                for (int i = 1; i <= int.Parse(GlobalPageCount.Text); i++)
                {
                    CurrentPageNum.Items.Add(i);
                }
                CurrentPageNum.SelectedItem = page;
            }
            loadingGif.Visibility = Visibility.Collapsed;


        }

        private async void FillDataForFilters()
        {
            IEnumerable<string> cityCollection = null;

            CityFilterBox.Items.Clear();

            await Task.Run(() =>
            {
                cityCollection = Cities.GetCitiesForFilters();
            });

            CityFilterBox.Items.Add("");

            cityCollection.ToList().ForEach(c => CityFilterBox.Items.Add(c));
        }

        private void SalesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SalesGrid.SelectedIndex != -1)
            {
                int curId = (SalesGrid.SelectedItem as SaleItem).ID;
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new SaleView(mainWindow, curId, false);
                mainWindow.GridMain.Children.Add(usc);
            }
        }

        private void ButtonOpenFilter_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenFilter.Visibility = Visibility.Collapsed;
            ButtonCloseFilter.Visibility = Visibility.Visible;
            FilterGrid.Visibility = Visibility.Visible;
        }

        private void ButtonCloseFilter_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenFilter.Visibility = Visibility.Visible;
            ButtonCloseFilter.Visibility = Visibility.Collapsed;
            FilterGrid.Visibility = Visibility.Hidden;
        }

        private void CancelFilter_Click(object sender, RoutedEventArgs e)
        {
            IdFilterBox.Text = "";
            ClientTextBox.Text = "";
            FirstSaleDatePicker.SelectedDate = null;
            SecondSaleDatePicker.SelectedDate = null;
            CityFilterBox.SelectedItem = null;
            TourFilterBox.Text = "";
            FirstFlightDatePicker.SelectedDate = null;
            SecondFlightDatePicker.SelectedDate = null;
            HotelFilterBox.Text = "";
            FirstPrice.Text = "";
            SecondPrice.Text = "";
            NotDeletedCheckBox.IsChecked = true;
            DeletedCheckBox.IsChecked = false;

            filterID = 0;
            filterClient = null;
            filterFDateSale = null;
            filterSDateSale = null;
            filterCity = null;
            filterTour = null;
            filterFDateFly = null;
            filterSDateFly = null;
            filterHotel = null;
            filterFPrice = null;
            filterSPrice = null;
            showNotDeleted = true;
            showDeleted = false;
            sorting = "id";
            ascordesc = "desc";
            page = 1;
            genCount = 0;
            FillGrid(false);
        }

        private void AppendFilter_Click(object sender, RoutedEventArgs e)
        {
            AppendFilter();
        }
        public void AppendFilter()
        {
            filterID = Validator.ValidateInt(IdFilterBox.Text);
            filterClient = ClientTextBox.Text;
            filterFDateSale = FirstSaleDatePicker.SelectedDate;
            filterSDateSale = SecondSaleDatePicker.SelectedDate;
            filterCity = CityFilterBox.Text;
            filterTour = TourFilterBox.Text;
            filterFDateFly = FirstFlightDatePicker.SelectedDate;
            filterSDateFly = SecondFlightDatePicker.SelectedDate;
            filterHotel = HotelFilterBox.Text;
            filterFPrice = FirstPrice.Text;
            filterSPrice = SecondPrice.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { SalesGrid.CurrentColumn = SalesGrid.Columns[0]; }
            if (sorting == "client") { SalesGrid.CurrentColumn = SalesGrid.Columns[1]; }
            if (sorting == "saledate") { SalesGrid.CurrentColumn = SalesGrid.Columns[2]; }
            if (sorting == "city") { SalesGrid.CurrentColumn = SalesGrid.Columns[3]; }
            if (sorting == "tour") { SalesGrid.CurrentColumn = SalesGrid.Columns[4]; }
            if (sorting == "flydate") { SalesGrid.CurrentColumn = SalesGrid.Columns[5]; }
            if (sorting == "hotel") { SalesGrid.CurrentColumn = SalesGrid.Columns[6]; }
            if (sorting == "employee") { SalesGrid.CurrentColumn = SalesGrid.Columns[7]; }
            if (sorting == "price") { SalesGrid.CurrentColumn = SalesGrid.Columns[8]; }

            if (ascordesc == "desc") { SalesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { SalesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            var column = e.Column;
            column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            ascordesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";

            var columnName = e.Column.Header.ToString();

            if (columnName == "Номер") { sorting = "id"; }
            if (columnName == "Клиент") { sorting = "client"; }
            if (columnName == "Дата продажи") { sorting = "saledate"; }
            if (columnName == "Город") { sorting = "city"; }
            if (columnName == "Тур") { sorting = "tour"; }
            if (columnName == "Дата вылета") { sorting = "flydate"; }
            if (columnName == "Отель") { sorting = "hotel"; }
            if (columnName == "Сотрудник") { sorting = "employee"; }
            if (columnName == "Стоимость") { sorting = "price"; }

            FillGrid(false);
        }

        private void FirstPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void CurrentPageNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentPageNum.SelectedItem != null && page != (int)CurrentPageNum.SelectedItem)
            {
                page = int.Parse(CurrentPageNum.SelectedItem.ToString());
                FillGrid(false);
            }
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            page = 1;
            FillGrid(false);
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (page > 1)
            {
                page = page - 1;
                FillGrid(false);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (page < (Math.Ceiling((double)genCount / count)))
            {
                page = page + 1;
                FillGrid(false);
            }
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            page = (int)Math.Ceiling((double)genCount / count);
            FillGrid(false);
        }

        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NumberOfRecords.SelectedItem != null && count != (int)NumberOfRecords.SelectedItem)
            {
                count = int.Parse(NumberOfRecords.SelectedItem.ToString());
                FillGrid(false);
            }
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new SaleView(mainWindow, 0, true);
            mainWindow.GridMain.Children.Add(usc);
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
