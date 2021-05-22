using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CityView.xaml
    /// </summary>
    public partial class CityView : UserControl
    {
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private SaleView saleView;
        public int CurrID;
        private City CurrCity;
        private bool isNew;
        public CityView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewCityGrid.Visibility = Visibility.Collapsed;
                EditCityGrid.Visibility = Visibility.Visible;
                EditCity.Visibility = Visibility.Collapsed;
                SaveCity.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Cities.GetCityByID(ID).Deleted)
                {
                    EditCity.Visibility = Visibility.Collapsed;
                    DelCity.Visibility = Visibility.Collapsed;
                }
                FillData();
            }



            if (!Right.CheckAccess("EditCity"))
            {
                EditCity.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelCity"))
            {
                DelCity.Visibility = Visibility.Collapsed;
            }
        }
        public CityView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.isNew = isNew;
            if (isNew)
            {
                ViewCityGrid.Visibility = Visibility.Collapsed;
                EditCityGrid.Visibility = Visibility.Visible;
                EditCity.Visibility = Visibility.Collapsed;
                SaveCity.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Cities.GetCityByID(ID).Deleted)
                {
                    EditCity.Visibility = Visibility.Collapsed;
                    DelCity.Visibility = Visibility.Collapsed;
                }
                FillData();
            }


            if (!Right.CheckAccess("EditCity"))
            {
                EditCity.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelCity"))
            {
                DelCity.Visibility = Visibility.Collapsed;
            }
        }

        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrCity = Cities.GetCityByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            NameBox.Text = CurrCity.Name;
            CountryBox.Text = CurrCity.Country;
            DescriptionBox.Text = CurrCity.Description;
        }

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Cities.GetIdsForFilters();
        //    });

        //    IdComboBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        //}


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
                UserControl usc = new CitiesControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindCityControl(secondWindow, saleView);
                secondWindow.GridMain.Children.Add(usc);
            }
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Cities.GetCityByID(CurrID).Deleted)
        //        {
        //            EditCity.Visibility = Visibility.Collapsed;
        //            DelCity.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditCity"))
        //            {
        //                EditCity.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelCity"))
        //            {
        //                DelCity.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //    }
        //}
        private void EditNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditNameBox.ToolTip = null;
            EditNameBox.Background = Brushes.Transparent;
        }

        private void EditCountryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditCountryBox.ToolTip = null;
            EditCountryBox.Background = Brushes.Transparent;
        }

        private void EditCity_Click(object sender, RoutedEventArgs e)
        {
            ViewCityGrid.Visibility = Visibility.Collapsed;
            EditCityGrid.Visibility = Visibility.Visible;
            EditCity.Visibility = Visibility.Collapsed;
            SaveCity.Visibility = Visibility.Visible;
            EditNameBox.Text = NameBox.Text;
            EditCountryBox.Text = CountryBox.Text;
            EditDescriptionBox.Text = DescriptionBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveCity_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrCity.Name = Validator.ValidateStringWithoutLower(EditNameBox.Text);
                CurrCity.Country = Validator.ValidateStringWithoutLower(EditCountryBox.Text);
                CurrCity.Description = Validator.ValidateStringWithoutLower(EditDescriptionBox.Text);

                result = CurrCity.EditCity();
            }
            else
            {
                CurrCity = new City(Validator.ValidateStringWithoutLower(EditNameBox.Text), Validator.ValidateStringWithoutLower(EditCountryBox.Text), 
                    Validator.ValidateStringWithoutLower(EditDescriptionBox.Text));

                result = CurrCity.AddCity();
            }



            if (result.Success)
            {
                CurrID = CurrCity.ID;
                FillData();
                ViewCityGrid.Visibility = Visibility.Visible;
                EditCityGrid.Visibility = Visibility.Collapsed;
                EditCity.Visibility = Visibility.Visible;
                SaveCity.Visibility = Visibility.Collapsed;
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
                if (result.Description.Contains("empty_country"))
                {
                    EditCountryBox.ToolTip = "Это поле не может быть пустым!";
                    EditCountryBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой город уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void DelCity_Click(object sender, RoutedEventArgs e)
        {
            if (CurrCity != null)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить город?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrCity.DelCity();
                    FillData();
                    EditCity.Visibility = Visibility.Collapsed;
                    DelCity.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SalesView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "citysales", CurrID);
            secondWindow.Show();
        }
        private void HotelsView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "cityhotels", CurrID);
            secondWindow.Show();
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Cities.GetCityByID(CurrID).Deleted)
                {
                    EditCity.Visibility = Visibility.Collapsed;
                    DelCity.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditCity"))
                    {
                        EditCity.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelCity"))
                    {
                        DelCity.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            City city = Cities.GetCityByID(CurrID + 1);
            if (city != null)
            {
                CurrID++;
                if (city.Deleted)
                {
                    EditCity.Visibility = Visibility.Collapsed;
                    DelCity.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditCity"))
                    {
                        EditCity.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelCity"))
                    {
                        DelCity.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }
    }
}
