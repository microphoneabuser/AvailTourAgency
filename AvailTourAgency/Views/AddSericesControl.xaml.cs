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
    /// Логика взаимодействия для AddServicesControl.xaml
    /// </summary>
    public partial class AddServicesControl : UserControl
    {
        private ObservableCollection<AddServiceItem> AddServicesItems { get; set; } = new ObservableCollection<AddServiceItem>();
        private MainWindow mainWindow;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private int filterID;
        private string filterName;
        private string filterFPrice;
        private string filterSPrice;
        private string filterTour;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount;
        public AddServicesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            AddServicesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[0];
            AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);

            if (!Right.CheckAccess("AddService"))
            {
                AddAddService.Visibility = Visibility.Collapsed;
            }
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                AddServicesItems = AddServices.GetAddServicesForTable(showNotDeleted, showDeleted, filterID, filterName,
                    filterFPrice, filterSPrice, filterTour,
                    sorting, ascordesc, count, page, ref genCount);
            });


            if (AddServicesItems.Count == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "По вашему запросу ничего не найдено.", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                GlobalPageCount.Text = (Math.Ceiling((double)genCount / count)).ToString();

                AddServicesGrid.ItemsSource = AddServicesItems;

                ShowSortingAgain();
                //if (first)
                //{
                //    FillDataForFilters();
                //}

                CurrentPageNum.Items.Clear();
                for (int i = 1; i <= int.Parse(GlobalPageCount.Text); i++)
                {
                    CurrentPageNum.Items.Add(i);
                }
                CurrentPageNum.SelectedItem = page;
            }
            loadingGif.Visibility = Visibility.Collapsed;
        }

        //private async void FillDataForFilters()
        //{
        //    IEnumerable<int> idCollection = null;
        //    IEnumerable<string> tourCollection = null;

        //    IdFilterBox.Items.Clear();
        //    TourFilterBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = AddServices.GetIdsForFilters();
        //        tourCollection = Tours.GetToursForFilters();
        //    });

        //    IdFilterBox.Items.Add("");
        //    TourFilterBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdFilterBox.Items.Add(i));
        //    tourCollection.ToList().ForEach(t => TourFilterBox.Items.Add(t));
        //}

        private void AddServicesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AddServicesGrid.SelectedIndex != -1)
            {
                int curId = (AddServicesGrid.SelectedItem as AddServiceItem).ID;
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new AddServiceView(mainWindow, curId, false);
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
            FirstPrice.Text = "";
            SecondPrice.Text = "";
            TourFilterBox.Text = "";
            NotDeletedCheckBox.IsChecked = true;
            DeletedCheckBox.IsChecked = false;

            filterID = 0;
            filterName = null;
            filterFPrice = null;
            filterSPrice = null;
            filterTour = null;
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
            filterFPrice = FirstPrice.Text;
            filterSPrice = SecondPrice.Text;
            filterTour = TourFilterBox.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }
        public void ShowSortingAgain()
        {
            if (sorting == "id") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[0]; }
            if (sorting == "name") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[1]; }
            if (sorting == "price") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[2]; }
            if (sorting == "description") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[3]; }
            if (sorting == "tour") { AddServicesGrid.CurrentColumn = AddServicesGrid.Columns[4]; }

            if (ascordesc == "desc") { AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { AddServicesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
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
            if (columnName == "Цена") { sorting = "price"; }
            if (columnName == "Описание") { sorting = "description"; }
            if (columnName == "Тур") { sorting = "tour"; }

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

        private void AddAddService_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new AddServiceView(mainWindow, 0, true);
            mainWindow.GridMain.Children.Add(usc);
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
