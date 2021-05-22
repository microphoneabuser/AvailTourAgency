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
    /// Логика взаимодействия для FindTouristControl.xaml
    /// </summary>
    public partial class FindTouristControl : UserControl
    {
        private ObservableCollection<TouristItem> TouristsItems { get; set; } = new ObservableCollection<TouristItem>();
        private SecondWindow secondWindow;
        private SaleView saleView;
        private int filterID;
        private string filterFIO;
        private string filterPhoneNumber;
        private DateTime? filterDateOfBirth;
        private string filterDocumentData;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount = 0;
        public FindTouristControl(SecondWindow secondWindow, SaleView saleView)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView= saleView;
            TouristsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            TouristsGrid.CurrentColumn = TouristsGrid.Columns[0];
            TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                TouristsItems = Tourists.GetAllTouristsForTable(showNotDeleted, showDeleted, filterID, filterFIO, filterPhoneNumber, filterDateOfBirth, filterDocumentData, sorting, ascordesc, count, page, ref genCount);
            });

            if (TouristsItems.Count == 0)
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

                TouristsGrid.ItemsSource = TouristsItems;

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

        //    IdFilterBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Tourists.GetIdsForFilters();
        //    });

        //    IdFilterBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdFilterBox.Items.Add(i));
        //}
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
            FIOBox.Text = "";
            PhoneNumberBox.Text = "";
            DateOfBirthBox.SelectedDate = null;
            DocumentDataBox.Text = "";

            filterID = 0;
            filterFIO = "";
            filterPhoneNumber = "";
            filterDateOfBirth = null;
            filterDocumentData = "";
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
            filterFIO = FIOBox.Text;
            filterPhoneNumber = PhoneNumberBox.Text;
            if (DateOfBirthBox.SelectedDate != null) filterDateOfBirth = DateOfBirthBox.SelectedDate.Value; else filterDateOfBirth = null;
            filterDocumentData = DocumentDataBox.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }

        public void ShowSortingAgain()
        {
            if (sorting == "id") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[0]; }
            if (sorting == "fio") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[1]; }
            if (sorting == "phonenumber") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[2]; }
            if (sorting == "dateofbirth") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[3]; }
            if (sorting == "documenttype") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[4]; }
            if (sorting == "documentdata") { TouristsGrid.CurrentColumn = TouristsGrid.Columns[5]; }

            if (ascordesc == "desc") { TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { TouristsGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header != null)
            {
                e.Handled = true;
                var column = e.Column;
                column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                ascordesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";
                var columnName = e.Column.Header.ToString();

                if (columnName == "Номер") { sorting = "id"; }
                if (columnName == "ФИО") { sorting = "fio"; }
                if (columnName == "Номер телефона") { sorting = "phonenumber"; }
                if (columnName == "Дата рождения") { sorting = "dateofbitrh"; }
                if (columnName == "Тип документа") { sorting = "documenttype"; }
                if (columnName == "Серия и номер документа") { sorting = "documentdata"; }

                FillGrid(false);
            }
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

        private void AddTourist_Click(object sender, RoutedEventArgs e)
        {
            secondWindow.GridMain.Children.Clear();
            UserControl usc = new TouristView(secondWindow, 0, true, saleView);
            secondWindow.GridMain.Children.Add(usc);
        }

        private void TouristsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TouristsGrid.SelectedIndex != -1)
            {
                int curId = (TouristsGrid.SelectedItem as TouristItem).ID;
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new TouristView(secondWindow, curId, false, saleView);
                secondWindow.GridMain.Children.Add(usc);
            }
        }

        private void ChooseTourist_Click(object sender, RoutedEventArgs e)
        {
            TouristItem gridItem = (sender as Button).DataContext as TouristItem;
            saleView.AddTouristItem(gridItem.ID);
            secondWindow.Close();
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
