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
    /// Логика взаимодействия для CitiesControl.xaml
    /// </summary>
    public partial class CitiesControl : UserControl
    {
        private ObservableCollection<CityItem> CitiesItems { get; set; } = new ObservableCollection<CityItem>();
        private MainWindow mainWindow;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private int filterID;
        private string filterName;
        private string filterCountry;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount;
        public CitiesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            CitiesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            CitiesGrid.CurrentColumn = CitiesGrid.Columns[0];
            CitiesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);

            if (!Right.CheckAccess("AddCity"))
            {
                AddCity.Visibility = Visibility.Collapsed;
            }
        }


        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                CitiesItems = Cities.GetCitiesForTable(showNotDeleted, showDeleted, filterID, filterName, filterCountry,
                    sorting, ascordesc, count, page, ref genCount);
            });


            if (CitiesItems.Count == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "По вашему запросу ничего не найдено.", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                GlobalPageCount.Text = (Math.Ceiling((double)genCount / count)).ToString();

                CitiesGrid.ItemsSource = CitiesItems;

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
            IEnumerable<string> countryCollection = null;

            CountryFilterBox.Items.Clear();

            await Task.Run(() =>
            {
                countryCollection = Cities.GetCountriesForFilters();
            });

            CountryFilterBox.Items.Add("");

            countryCollection.ToList().ForEach(t => CountryFilterBox.Items.Add(t));
        }

        private void CitiesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CitiesGrid.SelectedIndex != -1)
            {
                int curId = (CitiesGrid.SelectedItem as CityItem).ID;
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new CityView(mainWindow, curId, false);
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
            NameTextBox.Text = "";
            CountryFilterBox.SelectedItem = null;
            NotDeletedCheckBox.IsChecked = true;
            DeletedCheckBox.IsChecked = false;

            filterID = 0;
            filterName = null;
            filterCountry = null;
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
            filterName = NameTextBox.Text;
            filterCountry = CountryFilterBox.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { CitiesGrid.CurrentColumn = CitiesGrid.Columns[0]; }
            if (sorting == "name") { CitiesGrid.CurrentColumn = CitiesGrid.Columns[1]; }
            if (sorting == "country") { CitiesGrid.CurrentColumn = CitiesGrid.Columns[2]; }
            if (sorting == "description") { CitiesGrid.CurrentColumn = CitiesGrid.Columns[3]; }

            if (ascordesc == "desc") { CitiesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { CitiesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
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
            if (columnName == "Страна") { sorting = "country"; }
            if (columnName == "Описание") { sorting = "description"; }

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

        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new CityView(mainWindow, 0, true);
            mainWindow.GridMain.Children.Add(usc);
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
