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
    /// Логика взаимодействия для HotelView.xaml
    /// </summary>
    public partial class HotelView : UserControl
    {
        private ObservableCollection<HotelRoomItem> HotelRoomsItems { get; set; } = new ObservableCollection<HotelRoomItem>();
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private SaleView saleView;
        private string filterCity;
        public int CurrID;
        private Hotel CurrHotel;
        private bool showDeleted = false;
        private string sorting = "id";
        private string ascOrDesc = "desc";
        private bool isNew;
        List<int> idList = new List<int>();

        private string message;
        private int cityID;

        public HotelView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewHotelGrid.Visibility = Visibility.Collapsed;
                EditHotelGrid.Visibility = Visibility.Visible;
                EditHotel.Visibility = Visibility.Collapsed;
                SaveHotel.Visibility = Visibility.Visible;
                FillCityComboBox();
            }
            else
            {
                CurrID = ID;
                if (Hotels.GetHotelByID(ID).Deleted)
                {
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                }
                HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
                HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
                HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
                FillData();
                FillHotelRoomsGrid();
            }

            if (!Right.CheckAccess("EditHotel"))
            {
                EditHotel.Visibility = Visibility.Collapsed;
                EditRoom.Visibility = Visibility.Collapsed;
                AddRoom.Visibility = Visibility.Collapsed;
                DelRoom.Visibility = Visibility.Collapsed;
                SaveRoom.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelHotel"))
            {
                DelHotel.Visibility = Visibility.Collapsed;
            }
        }

        public HotelView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView, string filterCity)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.filterCity = filterCity;
            this.isNew = isNew;
            if (isNew)
            {
                ViewHotelGrid.Visibility = Visibility.Collapsed;
                EditHotelGrid.Visibility = Visibility.Visible;
                EditHotel.Visibility = Visibility.Collapsed;
                SaveHotel.Visibility = Visibility.Visible;
                FillCityComboBox();
            }
            else
            {
                CurrID = ID;
                if (Hotels.GetHotelByID(ID).Deleted)
                {
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                }
                HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
                HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
                HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
                FillData();
                FillHotelRoomsGrid();
            }


            if (!Right.CheckAccess("EditHotel"))
            {
                EditHotel.Visibility = Visibility.Collapsed;
                EditRoom.Visibility = Visibility.Collapsed;
                AddRoom.Visibility = Visibility.Collapsed;
                DelRoom.Visibility = Visibility.Collapsed;
                SaveRoom.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelHotel"))
            {
                DelHotel.Visibility = Visibility.Collapsed;
            }
        }

        public HotelView(SecondWindow secondWindow, string message, int cityID, int ID, bool isNew)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.message = message;
            this.cityID = cityID;
            this.isNew = isNew;
            if (isNew)
            {
                ViewHotelGrid.Visibility = Visibility.Collapsed;
                EditHotelGrid.Visibility = Visibility.Visible;
                EditHotel.Visibility = Visibility.Collapsed;
                SaveHotel.Visibility = Visibility.Visible;
                FillCityComboBox();
            }
            else
            {
                CurrID = ID;
                if (Hotels.GetHotelByID(ID).Deleted)
                {
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                }
                HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
                HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
                HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
                FillData();
                FillHotelRoomsGrid();
            }


            if (!Right.CheckAccess("EditHotel"))
            {
                EditHotel.Visibility = Visibility.Collapsed;
                EditRoom.Visibility = Visibility.Collapsed;
                AddRoom.Visibility = Visibility.Collapsed;
                DelRoom.Visibility = Visibility.Collapsed;
                SaveRoom.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelHotel"))
            {
                DelHotel.Visibility = Visibility.Collapsed;
            }
        }

        public void ReturnFromAddingHotelRoom()
        {
            if (Hotels.GetHotelByID(CurrID).Deleted)
            {
                EditHotel.Visibility = Visibility.Collapsed;
                DelHotel.Visibility = Visibility.Collapsed;
                AddRoom.Visibility = Visibility.Collapsed;
                EditRoom.Visibility = Visibility.Collapsed;
                SaveRoom.Visibility = Visibility.Collapsed;
            }
            HotelRoomsGrid.Sorting += new DataGridSortingEventHandler(SortHandler);
            HotelRoomsGrid.CurrentColumn = HotelRoomsGrid.Columns[0];
            HotelRoomsGrid.CurrentColumn.SortDirection = ListSortDirection.Descending;
            FillData();
            FillHotelRoomsGrid();

        }
        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrHotel = Hotels.GetHotelByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            NameBox.Text = CurrHotel.Name;
            CityBox.Text = CurrHotel.GetCity().Name;
            FeaturesBox.Text = CurrHotel.Features;
            EntertainmentBox.Text = CurrHotel.Entertainment;
            MealsBox.Text = CurrHotel.Meals;
            InfrastructureBox.Text = CurrHotel.Infrastructure;
            DescriptionBox.Text = CurrHotel.Description;
            DateOfConstructionBox.Value = CurrHotel.DateOfConstruction;
            LastRenovationBox.Value = CurrHotel.LastRenovation;
            NumberOfBuildingsBox.Value = CurrHotel.NumberOfBuildings;
            NumberOfRoomsBox.Value = CurrHotel.NumberOfRooms;
            RatingBox.Value = CurrHotel.Rating;
        }

        private async void FillHotelRoomsGrid()
        {
            await Task.Run(() =>
            {
                HotelRoomsItems = HotelRooms.GetHotelRoomsForTable(showDeleted, CurrID, sorting, ascOrDesc);
            });
            HotelRoomsGrid.ItemsSource = HotelRoomsItems;
            ShowSortingAgain();
        }

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Hotels.GetIdsForFilters();
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


        private void SalesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (SalesGrid.SelectedIndex != -1)
            //{
            //    int curId = (SalesGrid.SelectedItem as SaleItem).ID;
            //    mainWindow.GridMain.Children.Clear();
            //    UserControl usc = new SaleView(mainWindow);
            //    mainWindow.GridMain.Children.Add(usc);
            //}
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
                UserControl usc = new HotelsControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else if (message == null)
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindHotelControl(secondWindow, saleView, filterCity);
                secondWindow.GridMain.Children.Add(usc);
            }
            else
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new CityHotelsControl(secondWindow, cityID);
                secondWindow.GridMain.Children.Add(usc);
            }
        }

        private void HotelsGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ToursView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "hotelsales", CurrID);
            secondWindow.Show();
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Hotels.GetHotelByID(CurrID).Deleted)
        //        {
        //            EditHotel.Visibility = Visibility.Collapsed;
        //            DelHotel.Visibility = Visibility.Collapsed;
        //            AddRoom.Visibility = Visibility.Collapsed;
        //            EditRoom.Visibility = Visibility.Collapsed;
        //            SaveRoom.Visibility = Visibility.Collapsed;
        //            DelRoom.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditHotel"))
        //            {
        //                EditHotel.Visibility = Visibility.Visible;
        //                AddRoom.Visibility = Visibility.Visible;
        //                EditRoom.Visibility = Visibility.Visible;
        //                SaveRoom.Visibility = Visibility.Visible;
        //                DelRoom.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelHotel"))
        //            {
        //                DelHotel.Visibility = Visibility.Visible;
                        
        //            }
        //        }
        //        FillData();
        //        FillHotelRoomsGrid();
        //    }
        //}

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

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID == 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы добавить номера только что созданного отеля, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
            else
            {
                HotelRoomAddWindow hotelRoomAddWindow = new HotelRoomAddWindow(this);
                hotelRoomAddWindow.Show();
            }
        }

        private void DelRoom_Click(object sender, RoutedEventArgs e)
        {
            if (idList.Count != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Подтвердите действие!", "Вы уверены, что хотите удалить выделенные данные номера отеля?", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    foreach (int id in idList)
                    {
                        HotelRoom hotelRoom = HotelRooms.GetHotelRoomByID(id);
                        hotelRoom.DelHotelRoom();
                    }
                    FillHotelRoomsGrid();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Нет выделенных записей.", "Сначала выберите номера для последующего удаления.", true);
                myMessageBox.ShowDialog();
            }
        }

        private void DeletedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                if (DeletedCheckBox.IsChecked.Value)
                {
                    showDeleted = true;
                    FillHotelRoomsGrid();
                }
                else
                {
                    showDeleted = false;
                    FillHotelRoomsGrid();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы изменить номера только что созданного отеля, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (EditRoom.IsChecked.Value)
            {
                HotelRoomsGrid.IsReadOnly = false;

            }
            else
            {
                HotelRoomsGrid.IsReadOnly = true;

            }
        }

        private void SaveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                bool succeded = true;
                foreach (HotelRoomItem hotelRoomItem in HotelRoomsItems)
                {
                    HotelRoom hotelRoom = HotelRooms.GetHotelRoomByID(hotelRoomItem.ID);

                    hotelRoom.FirstTypeOfRoom = hotelRoomItem.FirstTypesOfRoom.Where(f => f.Value == hotelRoomItem.FirstTypeOfRoom).FirstOrDefault().Key;
                    hotelRoom.SecondTypeOfRoom = hotelRoomItem.SecondTypesOfRoom.Where(f => f.Value == hotelRoomItem.SecondTypeOfRoom).FirstOrDefault().Key;

                    hotelRoom.Quantity = hotelRoomItem.Quantity;
                    hotelRoom.Price24 = hotelRoomItem.Price24;
                    hotelRoom.Description = Validator.ValidateStringWithoutLower(hotelRoomItem.Description);

                    Result result = hotelRoom.EditHotelRoom();

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
                            MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой номер уже существует!", true);
                            myMessageBox.ShowDialog();
                        }
                        break;
                    }
                }
                if (succeded)
                {
                    FillHotelRoomsGrid();
                    EditRoom.IsChecked = false;
                    HotelRoomsGrid.IsReadOnly = true;
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                    myMessageBox.ShowDialog();
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Чтобы добавить номера только что созданного отеля, нужно сохранить его. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void EditHotel_Click(object sender, RoutedEventArgs e)
        {
            ViewHotelGrid.Visibility = Visibility.Collapsed;
            EditHotelGrid.Visibility = Visibility.Visible;
            EditHotel.Visibility = Visibility.Collapsed;
            SaveHotel.Visibility = Visibility.Visible;
            EditNameBox.Text = NameBox.Text;
            FillCityComboBox();
            EditDescriptionBox.Text = DescriptionBox.Text;
            EditFeaturesBox.Text = FeaturesBox.Text;
            EditEntertainmentBox.Text = EntertainmentBox.Text;
            EditMealsBox.Text = MealsBox.Text;
            EditInfrastructureBox.Text = InfrastructureBox.Text;
            EditDescriptionBox.Text = DescriptionBox.Text;
            EditDateOfConstructionBox.Value = DateOfConstructionBox.Value;
            EditLastRenovationBox.Value = LastRenovationBox.Value;
            EditNumberOfBuildingsBox.Value = NumberOfBuildingsBox.Value;
            EditNumberOfRoomsBox.Value = NumberOfRoomsBox.Value;
            EditRatingBox.Value = RatingBox.Value;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveHotel_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrHotel.Name = Validator.ValidateStringWithoutLower(EditNameBox.Text);
                City city = Cities.GetCityByName(EditCityBox.Text);
                if (city != null)
                {
                    CurrHotel.CityID = city.ID;
                }
                else
                {
                    CurrHotel.CityID = 0;
                }
                CurrHotel.Features = Validator.ValidateStringWithoutLower(EditFeaturesBox.Text);
                CurrHotel.Entertainment = Validator.ValidateStringWithoutLower(EditEntertainmentBox.Text);
                CurrHotel.Meals = Validator.ValidateStringWithoutLower(EditMealsBox.Text);
                CurrHotel.Infrastructure = Validator.ValidateStringWithoutLower(EditInfrastructureBox.Text);
                CurrHotel.Description = Validator.ValidateStringWithoutLower(EditDescriptionBox.Text);
                CurrHotel.DateOfConstruction = EditDateOfConstructionBox.Value;
                CurrHotel.LastRenovation = EditLastRenovationBox.Value;
                CurrHotel.NumberOfBuildings = EditNumberOfBuildingsBox.Value;
                CurrHotel.NumberOfRooms = EditNumberOfRoomsBox.Value;
                CurrHotel.Rating = EditRatingBox.Value;

                result = CurrHotel.EditHotel();
            }
            else
            {
                City city = Cities.GetCityByName(EditCityBox.Text);
                int cityID = 0;
                if (city != null)
                {
                    cityID = city.ID;
                }
                CurrHotel = new Hotel(Validator.ValidateStringWithoutLower(EditNameBox.Text), cityID, Validator.ValidateStringWithoutLower(EditFeaturesBox.Text), 
                    Validator.ValidateStringWithoutLower(EditEntertainmentBox.Text), Validator.ValidateStringWithoutLower(EditMealsBox.Text), 
                    Validator.ValidateStringWithoutLower(EditInfrastructureBox.Text), Validator.ValidateStringWithoutLower(EditDescriptionBox.Text),
                    EditDateOfConstructionBox.Value, EditLastRenovationBox.Value, EditNumberOfBuildingsBox.Value, EditNumberOfRoomsBox.Value, EditRatingBox.Value);

                result = CurrHotel.AddHotel();
            }



            if (result.Success)
            {
                CurrID = CurrHotel.ID;
                FillData();
                FillHotelRoomsGrid();
                ViewHotelGrid.Visibility = Visibility.Visible;
                EditHotelGrid.Visibility = Visibility.Collapsed;
                EditHotel.Visibility = Visibility.Visible;
                SaveHotel.Visibility = Visibility.Collapsed;
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
                if (errorCode == 0)
                {
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Заполните все обязательные поля!", true);
                    myMessageBox.ShowDialog();
                }
                else
                {
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой отель уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void DelHotel_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID != 0)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить отель?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrHotel.DelHotel();
                    FillData();
                    FillHotelRoomsGrid();
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данный отель еще не сохранён. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void checkHotelRoom_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                HotelRoomItem gridItem = (sender as CheckBox).DataContext as HotelRoomItem;
                idList.Add(gridItem.ID);
            }
            if (!(sender as CheckBox).IsChecked.Value)
            {
                HotelRoomItem gridItem = (sender as CheckBox).DataContext as HotelRoomItem;
                idList.Remove(gridItem.ID);
            }
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Hotels.GetHotelByID(CurrID).Deleted)
                {
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                    DelRoom.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditHotel"))
                    {
                        EditHotel.Visibility = Visibility.Visible;
                        AddRoom.Visibility = Visibility.Visible;
                        EditRoom.Visibility = Visibility.Visible;
                        SaveRoom.Visibility = Visibility.Visible;
                        DelRoom.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelHotel"))
                    {
                        DelHotel.Visibility = Visibility.Visible;

                    }
                }
                FillData();
                FillHotelRoomsGrid();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Hotel hotel = Hotels.GetHotelByID(CurrID + 1);
            if (hotel != null)
            {
                CurrID++;
                if (hotel.Deleted)
                {
                    EditHotel.Visibility = Visibility.Collapsed;
                    DelHotel.Visibility = Visibility.Collapsed;
                    AddRoom.Visibility = Visibility.Collapsed;
                    EditRoom.Visibility = Visibility.Collapsed;
                    SaveRoom.Visibility = Visibility.Collapsed;
                    DelRoom.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditHotel"))
                    {
                        EditHotel.Visibility = Visibility.Visible;
                        AddRoom.Visibility = Visibility.Visible;
                        EditRoom.Visibility = Visibility.Visible;
                        SaveRoom.Visibility = Visibility.Visible;
                        DelRoom.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelHotel"))
                    {
                        DelHotel.Visibility = Visibility.Visible;

                    }
                }
                FillData();
                FillHotelRoomsGrid();
            }
        }
    }
}
