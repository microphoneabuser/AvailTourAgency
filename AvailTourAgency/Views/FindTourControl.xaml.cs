using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для FindTourControl.xaml
    /// </summary>
    public partial class FindTourControl : UserControl
    {
        private ObservableCollection<TourItem> ToursItems { get; set; } = new ObservableCollection<TourItem>();
        private SecondWindow secondWindow;
        private SaleView saleView;
        private AddServiceView addServiceView;
        private int filterID;
        private string filterName;
        private string filterCity;
        private DateTime? filterFDate;
        private DateTime? filterSDate;
        private string filterFPrice;
        private string filterSPrice;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount = 0;


        public FindTourControl(SecondWindow secondWindow, SaleView saleView, string filterCity)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.filterCity = filterCity;
            ToursGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            ToursGrid.CurrentColumn = ToursGrid.Columns[0];
            ToursGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);
            //FillDataForFilters();
            //ToursGrid.ItemsSource = ToursItems;

            if (!Right.CheckAccess("AddTour"))
            {
                AddTour.Visibility = Visibility.Collapsed;
            }
        }


        public FindTourControl(SecondWindow secondWindow, AddServiceView addServiceView)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.addServiceView = addServiceView;
            ToursGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            ToursGrid.CurrentColumn = ToursGrid.Columns[0];
            ToursGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);
            //FillDataForFilters();
            //ToursGrid.ItemsSource = ToursItems;

            if (!Right.CheckAccess("AddTour"))
            {
                AddTour.Visibility = Visibility.Collapsed;
            }
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                ToursItems = Tours.GetToursForTable(showNotDeleted, showDeleted, filterID, filterName, filterCity, filterFDate, filterSDate, filterFPrice, filterSPrice, sorting, ascordesc, count, page, ref genCount);
            });

            if (ToursItems.Count == 0)
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

                ToursGrid.ItemsSource = ToursItems;

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
        //        idCollection = Tours.GetIdsForFilters();
        //        nameCollection = Tours.GetToursForFilters();
        //    });

        //    IdFilterBox.Items.Add("");
        //    NameFilterBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdFilterBox.Items.Add(i));
        //    nameCollection.ToList().ForEach(n => NameFilterBox.Items.Add(n));
        //}
        private void ToursGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ToursGrid.SelectedIndex != -1 && addServiceView == null)
            {
                int curId = (ToursGrid.SelectedItem as TourItem).ID;
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new TourView(secondWindow, curId, false, saleView, filterCity);
                secondWindow.GridMain.Children.Add(usc);
            }
        }

        private void OpenCards_Click(object sender, RoutedEventArgs e)
        {
            //mainWindow.GridMain.Children.Clear();
            //UserControl usc = new ToursControlCards();
            //mainWindow.GridMain.Children.Add(usc);
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
            FirstFlightDatePicker.SelectedDate = null;
            SecondFlightDatePicker.SelectedDate = null;
            FirstPrice.Text = "";
            SecondPrice.Text = "";
            NotDeletedCheckBox.IsChecked = true;
            DeletedCheckBox.IsChecked = false;

            filterID = 0;
            filterName = null;
            filterCity = null;
            filterFDate = null;
            filterSDate = null;
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
            filterName = NameFilterBox.Text;
            if (FirstFlightDatePicker.SelectedDate != null) { filterFDate = FirstFlightDatePicker.SelectedDate; }
            if (SecondFlightDatePicker.SelectedDate != null) { filterSDate = SecondFlightDatePicker.SelectedDate; }
            filterFPrice = FirstPrice.Text;
            filterSPrice = SecondPrice.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { ToursGrid.CurrentColumn = ToursGrid.Columns[0]; }
            if (sorting == "name") { ToursGrid.CurrentColumn = ToursGrid.Columns[1]; }
            if (sorting == "city") { ToursGrid.CurrentColumn = ToursGrid.Columns[2]; }

            if (ascordesc == "desc") { ToursGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { ToursGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
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

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            secondWindow.GridMain.Children.Clear();
            UserControl usc = new TourView(secondWindow, 0, true, saleView, filterCity);
            secondWindow.GridMain.Children.Add(usc);
        }

        private void ChooseTour_Click(object sender, RoutedEventArgs e)
        {
            if (addServiceView == null)
            {
                TourItem gridItem = (sender as Button).DataContext as TourItem;
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindTDPControl(secondWindow, gridItem.ID, saleView, filterCity);
                secondWindow.GridMain.Children.Add(usc);
            }
            else
            {
                TourItem gridItem = (sender as Button).DataContext as TourItem;
                addServiceView.ChangeTour(gridItem.ID);
                secondWindow.Close();
            }
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
