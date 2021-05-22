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
using System.Configuration;


namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для TourView.xaml
    /// </summary>
    public partial class TourView : UserControl
    {
        //private ObservableCollection<SaleItem> Items { get; set; } = new ObservableCollection<SaleItem>();
        private ObservableCollection<TourDatesPriceItem> TourDatesPricesItems { get; set; } = new ObservableCollection<TourDatesPriceItem>();
        //private ObservableCollection<string> FlightClasses { get; set; } = FillFlightClassComboBox();
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private SaleView saleView;
        private string filterCity;
        public int CurrID;
        private Tour CurrTour;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascOrDesc = "desc";
        private bool isNew;
        List<int> idList = new List<int>();

        public TourView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewTourGrid.Visibility = Visibility.Collapsed;
                EditTourGrid.Visibility = Visibility.Visible;
                EditTour.Visibility = Visibility.Collapsed;
                SaveTour.Visibility = Visibility.Visible;
                FillCityComboBox();
            }
            else
            {
                CurrID = ID;
                if (Tours.GetTourByID(ID).Deleted)
                {
                    EditTour.Visibility = Visibility.Collapsed;
                    DelTour.Visibility = Visibility.Collapsed;
                    AddTourDatesPrice.Visibility = Visibility.Collapsed;
                    EditTourDatesPrice.Visibility = Visibility.Collapsed;
                    SaveTourDatesPrice.Visibility = Visibility.Collapsed;
                }
                TourDatesPricesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
                TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[0];
                TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
                FillData();
                FillTourDatesPricesGrid();
            }

            if (!Right.CheckAccess("EditTour"))
            {
                EditTour.Visibility = Visibility.Collapsed;
                EditTourDatesPrice.Visibility = Visibility.Collapsed;
                AddTourDatesPrice.Visibility = Visibility.Collapsed;
                DelTourDatesPrice.Visibility = Visibility.Collapsed;
                SaveTourDatesPrice.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelTour"))
            {
                DelTour.Visibility = Visibility.Collapsed;
            }
        }

        public TourView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView, string filterCity)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.filterCity = filterCity;
            this.isNew = isNew;
            if (isNew)
            {
                ViewTourGrid.Visibility = Visibility.Collapsed;
                EditTourGrid.Visibility = Visibility.Visible;
                EditTour.Visibility = Visibility.Collapsed;
                SaveTour.Visibility = Visibility.Visible;
                FillCityComboBox();
            }
            else
            {
                CurrID = ID;
                if (Tours.GetTourByID(ID).Deleted)
                {
                    EditTour.Visibility = Visibility.Collapsed;
                    DelTour.Visibility = Visibility.Collapsed;
                    AddTourDatesPrice.Visibility = Visibility.Collapsed;
                    EditTourDatesPrice.Visibility = Visibility.Collapsed;
                    SaveTourDatesPrice.Visibility = Visibility.Collapsed;
                }
                TourDatesPricesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
                TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[0];
                TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
                FillData();
                FillTourDatesPricesGrid();
            }

            if (!Right.CheckAccess("EditTour"))
            {
                EditTour.Visibility = Visibility.Collapsed;
                EditTourDatesPrice.Visibility = Visibility.Collapsed;
                AddTourDatesPrice.Visibility = Visibility.Collapsed;
                DelTourDatesPrice.Visibility = Visibility.Collapsed;
                SaveTourDatesPrice.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelTour"))
            {
                DelTour.Visibility = Visibility.Collapsed;
            }
        }
        public void ReturnFromAddingTDP()
        {
            if (Tours.GetTourByID(CurrID).Deleted)
            {
                EditTour.Visibility = Visibility.Collapsed;
                DelTour.Visibility = Visibility.Collapsed;
                AddTourDatesPrice.Visibility = Visibility.Collapsed;
                EditTourDatesPrice.Visibility = Visibility.Collapsed;
                SaveTourDatesPrice.Visibility = Visibility.Collapsed;
            }
            TourDatesPricesGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            TourDatesPricesGrid.CurrentColumn = TourDatesPricesGrid.Columns[0];
            TourDatesPricesGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            FillData();
            FillTourDatesPricesGrid();

        }
        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrTour = Tours.GetTourByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            NameBox.Text = CurrTour.Name;
            CityBox.Text = CurrTour.GetCity().Name;
            DescriptionBox.Text = CurrTour.Description;
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

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Tours.GetIdsForFilters();
        //    });

        //    IdComboBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        //}
        private async void FillCityComboBox()
        {
            IEnumerable<string> cityCollection = null;

            EditCityBox.Items.Clear();

            await Task.Run(() =>
            {
                cityCollection = Cities.GetCitiesForFilters();
            });

            EditCityBox.Items.Add("");

            cityCollection.ToList().ForEach(i => EditCityBox.Items.Add(i));

            EditCityBox.SelectedItem = CityBox.Text;
        }
        //public ObservableCollection<string> FillFlightClassComboBox()
        //{
        //    ObservableCollection<string> n = new ObservableCollection<string>();
        //    string[] fc = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
        //    foreach (string c in fc)
        //    {
        //        n.Add(c);
        //    }
        //    return n;
        //}
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
            if (mainWindow != null)
            {
                mainWindow.GridMain.Children.Clear();
                UserControl usc = new ToursControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindTourControl(secondWindow, saleView, filterCity);
                secondWindow.GridMain.Children.Add(usc);
            }
        }

        private void TourDatesPricesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ToursView_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new SalesControl(mainWindow);
            mainWindow.GridMain.Children.Add(usc);
        }

        

        private void DeletedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                if (DeletedCheckBox.IsChecked.Value)
                {
                    showDeleted = true;
                    FillTourDatesPricesGrid();
                }
                else
                {
                    showDeleted = false;
                    FillTourDatesPricesGrid();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы изменить вариации только что созданного тура, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Tours.GetTourByID(CurrID).Deleted)
        //        {
        //            EditTour.Visibility = Visibility.Collapsed;
        //            DelTour.Visibility = Visibility.Collapsed;
        //            AddTourDatesPrice.Visibility = Visibility.Collapsed;
        //            EditTourDatesPrice.Visibility = Visibility.Collapsed;
        //            SaveTourDatesPrice.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditTour"))
        //            {
        //                EditTour.Visibility = Visibility.Visible;
        //                EditTourDatesPrice.Visibility = Visibility.Visible;
        //                AddTourDatesPrice.Visibility = Visibility.Visible;
        //                DelTourDatesPrice.Visibility = Visibility.Visible;
        //                SaveTourDatesPrice.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelTour"))
        //            {
        //                DelTour.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //        FillTourDatesPricesGrid();
        //    }
        //}

        private void EditTour_Click(object sender, RoutedEventArgs e)
        {
            ViewTourGrid.Visibility = Visibility.Collapsed;
            EditTourGrid.Visibility = Visibility.Visible;
            EditTour.Visibility = Visibility.Collapsed;
            SaveTour.Visibility = Visibility.Visible;
            EditNameBox.Text = NameBox.Text;
            FillCityComboBox();
            EditDescriptionBox.Text = DescriptionBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveTour_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrTour.Name = Validator.ValidateStringWithoutLower(EditNameBox.Text);
                City city = Cities.GetCityByName(Validator.ValidateStringWithoutLower(EditCityBox.Text));
                if (city != null)
                {
                    CurrTour.CityID = city.ID;
                }
                else
                {
                    CurrTour.CityID = 0;
                }
                CurrTour.Description = Validator.ValidateStringWithoutLower(EditDescriptionBox.Text);

                result = CurrTour.EditTour();
            }
            else
            {
                City city = Cities.GetCityByName(Validator.ValidateStringWithoutLower(EditCityBox.Text));
                int cityID = 0;
                if (city != null)
                {
                    cityID = city.ID;
                }
                CurrTour = new Tour(Validator.ValidateStringWithoutLower(EditNameBox.Text), cityID, Validator.ValidateStringWithoutLower(EditDescriptionBox.Text));

                result = CurrTour.AddTour();
            }



            if (result.Success)
            {
                CurrID = CurrTour.ID;
                FillData();
                FillTourDatesPricesGrid();
                ViewTourGrid.Visibility = Visibility.Visible;
                EditTourGrid.Visibility = Visibility.Collapsed;
                EditTour.Visibility = Visibility.Visible;
                SaveTour.Visibility = Visibility.Collapsed;
                MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                EditingIndicator.Visibility = Visibility.Collapsed;
                isNew = false;
            }
            else
            {
                int errorCode = 0;
                if (result.Description.Contains("empty_name"))
                {
                    EditNameBox.ToolTip = "Это поле не может быть пустым!";
                    EditNameBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_city"))
                {
                    EditCityBox.ToolTip = "Это поле не может быть пустым!";
                    EditCityBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("duplication"))
                {
                    errorCode = 1;
                }
                if(errorCode == 0)
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

        private void EditNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditNameBox.ToolTip = null;
            EditNameBox.Background = Brushes.Transparent;
        }

        private void EditCityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditCityBox.ToolTip = null;
            EditCityBox.Background = Brushes.Transparent;
        }

        private void DelTour_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить тур?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrTour.DelTour();
                    FillData();
                    FillTourDatesPricesGrid();
                    EditTour.Visibility = Visibility.Collapsed;
                    DelTour.Visibility = Visibility.Collapsed;
                    AddTourDatesPrice.Visibility = Visibility.Collapsed;
                    EditTourDatesPrice.Visibility = Visibility.Collapsed;
                    SaveTourDatesPrice.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данный тур ещё не сохранён. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void AddTourDatesPrice_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы добавить вариации только что созданного тура, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                TourDatesPriceAddWindow tourDatesPriceAddWindow = new TourDatesPriceAddWindow(this);
                tourDatesPriceAddWindow.Show();
            }
        }
        private void EditTourDatesPrice_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (EditTourDatesPrice.IsChecked.Value)
            //{
            //    TourDatesPricesGrid.IsReadOnly = true;

            //}
            //else
            //{
            //    TourDatesPricesGrid.IsReadOnly = false;

            //}
        }
        private void DelTourDatesPrice_Click(object sender, RoutedEventArgs e)
        {
            if (idList.Count != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Подтвердите действие!", "Вы уверены, что хотите удалить выделенные данные вариаций туров?", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    foreach (int id in idList)
                    {
                        TourDatesPrice tdp = TourDatesPrices.GetTDPByID(id);
                        tdp.DelTourDatesPrice();
                    }
                    FillTourDatesPricesGrid();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выделенных записей.", "Сначала выберите вариации для последующего удаления.", true);
                myMessageBox.ShowDialog();
            }
        }

        private void checkTDP_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                TourDatesPriceItem gridItem = (sender as CheckBox).DataContext as TourDatesPriceItem;
                idList.Add(gridItem.ID);
            }
            if (!(sender as CheckBox).IsChecked.Value)
            {
                TourDatesPriceItem gridItem = (sender as CheckBox).DataContext as TourDatesPriceItem;
                idList.Remove(gridItem.ID);
            }
        }

        private void EditTourDatesPrice_Click(object sender, RoutedEventArgs e)
        {
            if (EditTourDatesPrice.IsChecked.Value)
            {
                TourDatesPricesGrid.IsReadOnly = false;

            }
            else
            {
                TourDatesPricesGrid.IsReadOnly = true;

            }
        }

        private void SaveTourDatesPrice_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                bool succeded = true;
                foreach (TourDatesPriceItem tourDatesPriceItem in TourDatesPricesItems)
                {
                    TourDatesPrice tdp = TourDatesPrices.GetTDPByID(tourDatesPriceItem.ID);
                    tdp.Length = tourDatesPriceItem.Length;
                    tdp.Luggage = Validator.ValidateStringWithoutLower(tourDatesPriceItem.Luggage);
                    tdp.Meals = Validator.ValidateStringWithoutLower(tourDatesPriceItem.Meals);
                    tdp.Plane = Validator.ValidateStringWithoutLower(tourDatesPriceItem.Plane);
                    tdp.Price = tourDatesPriceItem.Price;
                    tdp.Quantity = tourDatesPriceItem.Quantity;
                    tdp.Airline = Validator.ValidateStringWithoutLower(tourDatesPriceItem.Airline);
                    
                    tdp.FlightClass = tourDatesPriceItem.FlightClasses.Where(f => f.Value == tourDatesPriceItem.FlightClass).FirstOrDefault().Key;

                    tdp.FlyDateBack = DateTime.Parse(tourDatesPriceItem.FlyDateBack);
                    tdp.FlyDateThere = DateTime.Parse(tourDatesPriceItem.FlyDateThere);
                    if (tourDatesPriceItem.Length != 0 &&
                        tourDatesPriceItem.FlyDateThere != null && tourDatesPriceItem.FlyDateBack != null &&
                        tourDatesPriceItem.Length != (DateTime.Parse(tourDatesPriceItem.FlyDateBack).Date - DateTime.Parse(tourDatesPriceItem.FlyDateThere).Date).Days)
                    {
                        int l = (DateTime.Parse(tourDatesPriceItem.FlyDateBack).Date - DateTime.Parse(tourDatesPriceItem.FlyDateThere).Date).Days;
                        tdp.Length = l;
                    }

                    Result result = tdp.EditTourDatesPrice();

                    if (!result.Success)
                    {
                        succeded = false;
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
                        break;
                    }
                }
                if (succeded)
                {
                    FillTourDatesPricesGrid();
                    EditTourDatesPrice.IsChecked = false;
                    TourDatesPricesGrid.IsReadOnly = true;
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                    myMessageBox.ShowDialog();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы добавить вариации только что созданного тура, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }


        private void SalesViewButton_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "toursales", CurrID);
            secondWindow.Show();
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Tours.GetTourByID(CurrID).Deleted)
                {
                    EditTour.Visibility = Visibility.Collapsed;
                    DelTour.Visibility = Visibility.Collapsed;
                    AddTourDatesPrice.Visibility = Visibility.Collapsed;
                    EditTourDatesPrice.Visibility = Visibility.Collapsed;
                    SaveTourDatesPrice.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditTour"))
                    {
                        EditTour.Visibility = Visibility.Visible;
                        EditTourDatesPrice.Visibility = Visibility.Visible;
                        AddTourDatesPrice.Visibility = Visibility.Visible;
                        DelTourDatesPrice.Visibility = Visibility.Visible;
                        SaveTourDatesPrice.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelTour"))
                    {
                        DelTour.Visibility = Visibility.Visible;
                    }
                }
                FillData();
                FillTourDatesPricesGrid();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Tour tour = Tours.GetTourByID(CurrID + 1);
            if (tour != null)
            {
                CurrID++;
                if (tour.Deleted)
                {
                    EditTour.Visibility = Visibility.Collapsed;
                    DelTour.Visibility = Visibility.Collapsed;
                    AddTourDatesPrice.Visibility = Visibility.Collapsed;
                    EditTourDatesPrice.Visibility = Visibility.Collapsed;
                    SaveTourDatesPrice.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditTour"))
                    {
                        EditTour.Visibility = Visibility.Visible;
                        EditTourDatesPrice.Visibility = Visibility.Visible;
                        AddTourDatesPrice.Visibility = Visibility.Visible;
                        DelTourDatesPrice.Visibility = Visibility.Visible;
                        SaveTourDatesPrice.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelTour"))
                    {
                        DelTour.Visibility = Visibility.Visible;
                    }
                }
                FillData();
                FillTourDatesPricesGrid();
            }
        }
    }
}
