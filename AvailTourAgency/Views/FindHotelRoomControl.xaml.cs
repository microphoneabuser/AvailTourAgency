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
    /// Логика взаимодействия для FindHotelRoomControl.xaml
    /// </summary>
    public partial class FindHotelRoomControl : UserControl
    {
        //private ObservableCollection<SaleItem> Items { get; set; } = new ObservableCollection<SaleItem>();
        private ObservableCollection<HotelRoomItem> HotelRoomsItems { get; set; } = new ObservableCollection<HotelRoomItem>();
        private SecondWindow secondWindow;
        private SaleView saleView;
        public int CurrID;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascOrDesc = "desc";

        public FindHotelRoomControl(SecondWindow secondWindow, SaleView saleView, int hotelID)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.CurrID = hotelID;
            HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
            HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            FillIdComboBox();
            IdComboBox.SelectedItem = hotelID;
            FillHotelRoomsGrid();
        }
        private async void FillHotelRoomsGrid()
        {
            await Task.Run(() =>
            {
                HotelRoomsItems = HotelRooms.GetHotelRoomsForTableForSale(showDeleted, CurrID, sorting, ascOrDesc);
            });
            HotelRoomsGrid.ItemsSource = HotelRoomsItems;
            ShowSortingAgain();
        }

        private async void FillIdComboBox()
        {
            IEnumerable<int> idCollection = null;

            IdComboBox.Items.Clear();

            await Task.Run(() =>
            {
                idCollection = Hotels.GetIdsForFilters();
            });

            IdComboBox.Items.Add("");

            idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0]; }
            if (sorting == "firsttype") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[1]; }
            if (sorting == "secondtype") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[2]; }
            if (sorting == "quantity") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[3]; }
            if (sorting == "price24") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[4]; }
            if (sorting == "description") { HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[5]; }

            if (ascOrDesc == "desc") { HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
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
                if (columnName == "Первый тип") { sorting = "firsttype"; }
                if (columnName == "Второй тип") { sorting = "secondtype"; }
                if (columnName == "Количество") { sorting = "quantity"; }
                if (columnName == "Цена за сутки") { sorting = "price24"; }
                if (columnName == "Описание") { sorting = "description"; }

                FillHotelRoomsGrid();
            }
        }

        private void ChooseTDP_Click(object sender, RoutedEventArgs e)
        {
            TourDatesPriceItem gridItem = (sender as Button).DataContext as TourDatesPriceItem;
            saleView.ChangeTour(CurrID, gridItem.ID);
            secondWindow.Close();
        }

        private void ChooseHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            HotelRoomItem gridItem = (sender as Button).DataContext as HotelRoomItem;
            saleView.AddHotelRoomItem(gridItem.ID);
            secondWindow.Close();
        }
    }
}
