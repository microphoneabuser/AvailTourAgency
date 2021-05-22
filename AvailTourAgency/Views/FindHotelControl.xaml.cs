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
    /// Логика взаимодействия для FindHotelControl.xaml
    /// </summary>
    public partial class FindHotelControl : UserControl
    {
        private ObservableCollection<HotelItem> HotelsItems { get; set; } = new ObservableCollection<HotelItem>();
        private SecondWindow secondWindow;
        private SaleView saleView;
        private int filterID;
        private string filterName;
        private string filterCity;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount = 0;
        public FindHotelControl(SecondWindow secondWindow, SaleView saleView, string filterCity)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.filterCity = filterCity;
            HotelsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            HotelsGrid.CurrentColumn = HotelsGrid.Columns[0];
            HotelsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);

            if (!Right.CheckAccess("AddHotel"))
            {
                AddHotel.Visibility = Visibility.Collapsed;
            }
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                HotelsItems = Hotels.GetHotelsForTable(showNotDeleted, showDeleted, filterID, filterName, filterCity, sorting, ascordesc, count, page, ref genCount);
            });

            if (HotelsItems.Count == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "По вашему запросу ничего не найдено.", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                GlobalPageCount.Text = (Math.Ceiling((double)genCount / count)).ToString();

                CurrentPageNum.Items.Clear();
                for (int i = 1; i <= int.Parse(GlobalPageCount.Text); i++)
                {
                    CurrentPageNum.Items.Add(i);
                }
                CurrentPageNum.SelectedItem = page;

                HotelsGrid.ItemsSource = HotelsItems;

                ShowSortingAgain();
                //if (first)
                //{
                //    FillDataForFilters();
                //}
            }
            loadingGif.Visibility = Visibility.Collapsed;

        }

        //private async void FillDataForFilters()
        //{
        //    IEnumerable<int> idCollection = null;
        //    IEnumerable<string> nameCollection = null;

        //    IdFilterBox.Items.Clear();
        //    NameFilterBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Hotels.GetIdsForFilters();
        //        nameCollection = Hotels.GetHotelsForFilters();
        //    });

        //    IdFilterBox.Items.Add("");
        //    NameFilterBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdFilterBox.Items.Add(i));
        //    nameCollection.ToList().ForEach(n => NameFilterBox.Items.Add(n));
        //}


        private void HotelsGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (HotelsGrid.SelectedIndex != -1)
            {
                int curId = (HotelsGrid.SelectedItem as HotelItem).ID;
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new HotelView(secondWindow, curId, false, saleView, filterCity);
                secondWindow.GridMain.Children.Add(usc);
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
            NameFilterBox.Text = "";

            filterID = 0;
            filterName = null;
            filterCity = null;
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
            filterName = NameFilterBox.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }

        public void ShowSortingAgain()
        {
            if (sorting == "id") { HotelsGrid.CurrentColumn = HotelsGrid.Columns[0]; }
            if (sorting == "name") { HotelsGrid.CurrentColumn = HotelsGrid.Columns[1]; }
            if (sorting == "city") { HotelsGrid.CurrentColumn = HotelsGrid.Columns[2]; }
            if (sorting == "numberofbuildings") { HotelsGrid.CurrentColumn = HotelsGrid.Columns[3]; }
            if (sorting == "numberofrooms") { HotelsGrid.CurrentColumn = HotelsGrid.Columns[4]; }

            if (ascordesc == "desc") { HotelsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { HotelsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            var column = e.Column;
            column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            ascordesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";

            var columnName = e.Column.Header.ToString();
            if (columnName == "Номер") { sorting = "id"; }
            if (columnName == "Название") { sorting = "name"; }
            if (columnName == "Город") { sorting = "city"; }
            if (columnName == "Количество корпусов") { sorting = "numberofbuildings"; }
            if (columnName == "Количество номеров") { sorting = "numberofrooms"; }
            FillGrid(false);
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

        private void AddHotel_Click(object sender, RoutedEventArgs e)
        {
            secondWindow.GridMain.Children.Clear();
            UserControl usc = new HotelView(secondWindow, 0, true, saleView, filterCity);
            secondWindow.GridMain.Children.Add(usc);
        }

        private void ChooseHotel_Click(object sender, RoutedEventArgs e)
        {
            HotelItem gridItem = (sender as Button).DataContext as HotelItem;
            saleView.ChangeHotel(gridItem.ID);  
            secondWindow.Close();
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
