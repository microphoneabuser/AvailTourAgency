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
    /// Логика взаимодействия для AddServiceView.xaml
    /// </summary>
    public partial class AddServiceView : UserControl
    {
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private SaleView saleView;
        private int tourID;
        public int CurrID;
        private AddService CurrAddService;
        private bool isNew;
        public AddServiceView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewAddServiceGrid.Visibility = Visibility.Collapsed;
                EditAddServiceGrid.Visibility = Visibility.Visible;
                EditAddService.Visibility = Visibility.Collapsed;
                SaveAddService.Visibility = Visibility.Visible;
                //FillTourComboBox();
            }
            else
            {
                CurrID = ID;
                if (AddServices.GetAddServiceByID(ID).Deleted)
                {
                    EditAddService.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                }
                FillData();
            }


            if (!Right.CheckAccess("EditService"))
            {
                EditAddService.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelService"))
            {
                DelAddService.Visibility = Visibility.Collapsed;
            }
        }
        public AddServiceView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView, int tourID)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.tourID = tourID;
            this.isNew = isNew;
            if (isNew)
            {
                ViewAddServiceGrid.Visibility = Visibility.Collapsed;
                EditAddServiceGrid.Visibility = Visibility.Visible;
                EditAddService.Visibility = Visibility.Collapsed;
                SaveAddService.Visibility = Visibility.Visible;
                //FillTourComboBox();
            }
            else
            {
                CurrID = ID;
                if (AddServices.GetAddServiceByID(ID).Deleted)
                {
                    EditAddService.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                }
                FillData();
            }


            if (!Right.CheckAccess("EditService"))
            {
                EditAddService.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelService"))
            {
                DelAddService.Visibility = Visibility.Collapsed;
            }
        }
        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrAddService = AddServices.GetAddServiceByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            NameBox.Text = CurrAddService.Name;
            PriceBox.Text = CurrAddService.Price.ToString();
            TourBox.Text = CurrAddService.GetTour().Name;
            DescriptionBox.Text = CurrAddService.Description;
        }

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = AddServices.GetIdsForFilters();
        //    });

        //    IdComboBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        //}

        //private async void FillTourComboBox()
        //{
        //    IEnumerable<string> tourCollection = null;

        //    EditTourBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        tourCollection = Tours.GetToursForFilters();
        //    });

        //    EditTourBox.Items.Add("");

        //    tourCollection.ToList().ForEach(i => EditTourBox.Items.Add(i));

        //    EditTourBox.SelectedItem = TourBox.Text;
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
                UserControl usc = new AddServicesControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindAddServiceControl(secondWindow, saleView, tourID);
                secondWindow.GridMain.Children.Add(usc);
            }
        }
        private void ToursView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "addservicesales", CurrID);
            secondWindow.Show();
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (AddServices.GetAddServiceByID(CurrID).Deleted)
        //        {
        //            EditAddService.Visibility = Visibility.Collapsed;
        //            DelAddService.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditService"))
        //            {
        //                EditAddService.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelService"))
        //            {
        //                DelAddService.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //    }
        //}

        private void TourBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditTourBox.ToolTip = null;
            EditTourBox.Background = Brushes.Transparent;
        }

        private void EditPriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void EditNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditNameBox.ToolTip = null;
            EditNameBox.Background = Brushes.Transparent;
        }

        private void EditPriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditPriceBox.ToolTip = null;
            EditPriceBox.Background = Brushes.Transparent;
        }

        private void EditDescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditDescriptionBox.ToolTip = null;
            EditDescriptionBox.Background = Brushes.Transparent;
        }

        private void EditAddService_Click(object sender, RoutedEventArgs e)
        {
            ViewAddServiceGrid.Visibility = Visibility.Collapsed;
            EditAddServiceGrid.Visibility = Visibility.Visible;
            EditAddService.Visibility = Visibility.Collapsed;
            SaveAddService.Visibility = Visibility.Visible;
            EditNameBox.Text = NameBox.Text;
            EditPriceBox.Text = PriceBox.Text;
            EditTourBox.Text = TourBox.Text;
            //FillTourComboBox();
            EditDescriptionBox.Text = DescriptionBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveAddService_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrAddService.Name = Validator.ValidateStringWithoutLower(EditNameBox.Text);
                try { CurrAddService.Price = Validator.ValidateDecimal(EditPriceBox.Text); } catch { CurrAddService.Price = 0; }
                Tour tour = Tours.GetTourByName(EditTourBox.Text);
                if (tour != null)
                {
                    CurrAddService.TourID = tour.ID;
                }
                else
                {
                    CurrAddService.TourID = 0;
                }
                CurrAddService.Description = EditDescriptionBox.Text;

                result = CurrAddService.EditAddService();
            }
            else
            {
                decimal price = 0;
                try { price = decimal.Parse(EditPriceBox.Text); } catch { price = 0; }
                Tour tour = Tours.GetTourByName(EditTourBox.Text);
                int tourID = 0;
                if (tour != null)
                {
                    tourID = tour.ID;
                }
                CurrAddService = new AddService(Validator.ValidateStringWithoutLower(EditNameBox.Text), price, tourID, Validator.ValidateStringWithoutLower(EditDescriptionBox.Text));

                result = CurrAddService.AddAddService();
            }



            if (result.Success)
            {
                CurrID = CurrAddService.ID;
                FillData();
                ViewAddServiceGrid.Visibility = Visibility.Visible;
                EditAddServiceGrid.Visibility = Visibility.Collapsed;
                EditAddService.Visibility = Visibility.Visible;
                SaveAddService.Visibility = Visibility.Collapsed;
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
                if (result.Description.Contains("empty_price"))
                {
                    EditPriceBox.ToolTip = "Это поле не может быть пустым!";
                    EditPriceBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такая услуга уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void DelAddService_Click(object sender, RoutedEventArgs e)
        {
            if (CurrAddService != null)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить услугу?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrAddService.DelAddService();
                    FillData();
                    EditAddService.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данная услуга еще не сохранена. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void FindTour_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                SecondWindow secondWindow = new SecondWindow(mainWindow, this, "findtour");
                secondWindow.Show();
            }
            else
            {
                SecondWindow secondWindowNew = new SecondWindow(secondWindow, this, "findtour");
                secondWindowNew.Show();
            }
        }
        public void ChangeTour(int tourID)
        {
            EditTourBox.Text = $"{Tours.GetTourByID(tourID).Name}";
        }

        private void ClearTour_Click(object sender, RoutedEventArgs e)
        {
            EditTourBox.Text = "";
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (AddServices.GetAddServiceByID(CurrID).Deleted)
                {
                    EditAddService.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditService"))
                    {
                        EditAddService.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelService"))
                    {
                        DelAddService.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            AddService addService = AddServices.GetAddServiceByID(CurrID + 1);
            if (addService != null)
            {
                CurrID++;
                if (addService.Deleted)
                {
                    EditAddService.Visibility = Visibility.Collapsed;
                    DelAddService.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditService"))
                    {
                        EditAddService.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelService"))
                    {
                        DelAddService.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }
    }
}
