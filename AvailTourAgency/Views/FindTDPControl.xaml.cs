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
    /// Логика взаимодействия для FindTDPControl.xaml
    /// </summary>
    public partial class FindTDPControl : UserControl
    {
        //private ObservableCollection<SaleItem> Items { get; set; } = new ObservableCollection<SaleItem>();
        private ObservableCollection<TourDatesPriceItem> TourDatesPricesItems { get; set; } = new ObservableCollection<TourDatesPriceItem>();
        private SecondWindow secondWindow;
        private SaleView saleView;
        private string filterCity;
        public int CurrID;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascOrDesc = "desc";
        List<int> idList = new List<int>();

        public FindTDPControl(SecondWindow secondWindow, int ID, SaleView saleView, string filterCity)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.filterCity = filterCity;
            CurrID = ID;
            TourDatesPricesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[0];
            TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            FillIdComboBox();
            IdComboBox.SelectedItem = ID;
            FillTourDatesPricesGrid();
        }
        private async void FillTourDatesPricesGrid()
        {
            await Task.Run(() =>
            {
                TourDatesPricesItems = TourDatesPrices.GetTourDatesPricesForTable(showDeleted, CurrID, sorting, ascOrDesc);
            });
            TourDatesPricesGrid.ItemsSource = TourDatesPricesItems;
            ShowSortingAgain();
        }

        private async void FillIdComboBox()
        {
            IEnumerable<int> idCollection = null;

            IdComboBox.Items.Clear();

            await Task.Run(() =>
            {
                idCollection = Tours.GetIdsForFilters();
            });

            IdComboBox.Items.Add("");

            idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[0]; }
            if (sorting == "flydatethere") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[1]; }
            if (sorting == "flydateback") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[2]; }
            if (sorting == "length") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[3]; }
            if (sorting == "price") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[4]; }
            if (sorting == "airline") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[5]; }
            if (sorting == "plane") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[6]; }
            if (sorting == "flightclass") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[7]; }
            if (sorting == "luggage") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[8]; }
            if (sorting == "meals") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[9]; }
            if (sorting == "quantity") { TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[10]; }

            if (ascOrDesc == "desc") { TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header != null)
            {
                e.Handled = true;
                var column = e.Column;
                column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                ascOrDesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";
                var columnName = e.Column.Header.ToString();
                if (columnName == "Номер") { sorting = "id"; }
                if (columnName == "Дата вылета") { sorting = "flydatethere"; }
                if (columnName == "Дата вылета обратно") { sorting = "flydateback"; }
                if (columnName == "Длительность") { sorting = "length"; }
                if (columnName == "Цена") { sorting = "price"; }
                if (columnName == "Авиакомпания") { sorting = "airline"; }
                if (columnName == "Самолет") { sorting = "plane"; }
                if (columnName == "Класс перелета") { sorting = "flightclass"; }
                if (columnName == "Багаж") { sorting = "luggage"; }
                if (columnName == "Питание") { sorting = "meals"; }
                if (columnName == "Количество") { sorting = "quantity"; }
                FillTourDatesPricesGrid();
            }
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
            secondWindow.GridMain.Children.Clear();
            UserControl usc = new FindTourControl(secondWindow, saleView, filterCity);
            secondWindow.GridMain.Children.Add(usc);
        }

        private void ChooseTDP_Click(object sender, RoutedEventArgs e)
        {
            TourDatesPriceItem gridItem = (sender as Button).DataContext as TourDatesPriceItem;
            saleView.ChangeTour(CurrID, gridItem.ID);
            secondWindow.Close();
        }
    }
}
