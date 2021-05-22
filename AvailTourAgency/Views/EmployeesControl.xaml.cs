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
    /// Логика взаимодействия для EmployeesControl.xaml
    /// </summary>
    public partial class EmployeesControl : UserControl
    {
        private ObservableCollection<EmployeeItem> EmployeesItems { get; set; } = new ObservableCollection<EmployeeItem>();
        private MainWindow mainWindow;
        private int filterID;
        private string filterFIO;
        private string filterPassport;
        private string filterPhoneNumber;
        private string filterPosition;
        private string filterINN;
        private string filterLogin;
        private bool showNotDeleted = true;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascordesc = "desc";
        private int count = 5;
        private int page = 1;
        private int genCount = 0;
        public EmployeesControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            EmployeesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[0];
            EmployeesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            NumberOfRecords.Items.Clear();
            NumberOfRecords.Items.Add(5);
            NumberOfRecords.Items.Add(10);
            NumberOfRecords.Items.Add(20);
            NumberOfRecords.Items.Add(50);
            NumberOfRecords.Items.Add(100);
            NumberOfRecords.SelectedItem = NumberOfRecords.Items[0];
            FillGrid(true);

            if (!Right.CheckAccess("AddEmployee"))
            {
                AddEmployee.Visibility = Visibility.Collapsed;
            }
        }

        public async void FillGrid(bool first)
        {
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                EmployeesItems = Employees.GetEmployeesForTable(showNotDeleted, showDeleted, filterID, filterFIO, filterPassport, filterPhoneNumber,
                    filterPosition, filterINN, filterLogin, sorting, ascordesc, count, page, ref genCount);
            });

            if (EmployeesItems.Count == 0)
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

                EmployeesGrid.ItemsSource = EmployeesItems;

                ShowSortingAgain();
                if (first)
                {
                    FillDataForFilters();
                }
            }
            loadingGif.Visibility = Visibility.Collapsed;

        }

        private async void FillDataForFilters()
        {
            IEnumerable<string> positionCollection = null;

            PositionFilterBox.Items.Clear();

            await Task.Run(() =>
            {
                positionCollection = Roles.GetRolesForFilters();
            });

            PositionFilterBox.Items.Add("");

            positionCollection.ToList().ForEach(i => PositionFilterBox.Items.Add(i));
        }

        private void EmployeesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (EmployeesGrid.SelectedIndex != -1)
            {
                int curId = (EmployeesGrid.SelectedItem as EmployeeItem).ID;
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new EmployeeView(mainWindow, curId, false);
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
            IdFilterBox.Text = null;
            FIOTextBox.Text = "";
            PassportBox.Text = "";
            PhoneNumberBox.Text = "";
            PositionFilterBox.SelectedItem = null;
            INNTextBox.Text = "";
            LoginTextBox.Text = "";

            filterID = 0;
            filterFIO = "";
            filterPassport = "";
            filterPhoneNumber = "";
            filterPosition = "";
            filterINN = "";
            filterLogin = "";
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
            filterFIO = FIOTextBox.Text;
            filterPassport = PassportBox.Text;
            filterPhoneNumber = PhoneNumberBox.Text;
            filterPosition = PositionFilterBox.Text;
            filterINN = INNTextBox.Text;
            filterLogin = LoginTextBox.Text;
            showNotDeleted = NotDeletedCheckBox.IsChecked.Value;
            showDeleted = DeletedCheckBox.IsChecked.Value;
            FillGrid(false);
        }

        public void ShowSortingAgain()
        {
            if (sorting == "id") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[1]; }
            if (sorting == "fio") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[2]; }
            if (sorting == "passport") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[3]; }
            if (sorting == "phonenumber") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[4]; }
            if (sorting == "position") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[5]; }
            if (sorting == "inn") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[6]; }
            if (sorting == "login") { EmployeesGrid.CurrentColumn = EmployeesGrid.Columns[7]; }

            if (ascordesc == "desc") { EmployeesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending; }
            else { EmployeesGrid.CurrentColumn.SortDirection = ListSortDirection.Ascending; }
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            var column = e.Column;
            column.SortDirection = (column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            ascordesc = (column.SortDirection == ListSortDirection.Descending) ? "desc" : "asc";

            var columnName = e.Column.Header.ToString();
            if (columnName == "Номер") { sorting = "id"; }
            if (columnName == "ФИО") { sorting = "fio"; }
            if (columnName == "Паспорт") { sorting = "passport"; }
            if (columnName == "Номер телефона") { sorting = "phonenumber"; }
            if (columnName == "Должность") { sorting = "position"; }
            if (columnName == "ИНН") { sorting = "inn"; }
            if (columnName == "Логин") { sorting = "login"; }
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

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new EmployeeView(mainWindow, 0, true);
            mainWindow.GridMain.Children.Add(usc);
        }

        private void IdFilterBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
