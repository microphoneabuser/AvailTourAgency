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
    /// Логика взаимодействия для ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        private MainWindow mainWindow;
        private SecondWindow secondWindow;
        private SaleView saleView;
        public int CurrID;
        private Client CurrClient;
        private bool isNew;
        public ClientView(MainWindow mainWindow, int ID, bool isNew) //конструктор для вызова из основной формы
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewClientGrid.Visibility = Visibility.Collapsed;
                EditClientGrid.Visibility = Visibility.Visible;
                EditClient.Visibility = Visibility.Collapsed;
                SaveClient.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Clients.GetClientByID(ID).Deleted)
                {
                    EditClient.Visibility = Visibility.Collapsed;
                    DelClient.Visibility = Visibility.Collapsed;
                }
                FillData();
            }

            if (!Right.CheckAccess("EditClient"))
            {
                EditClient.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelClient"))
            {
                DelClient.Visibility = Visibility.Collapsed;
            }
        }
        public ClientView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView)//конструктор для вызова из второстепенной формы
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.isNew = isNew;
            this.saleView = saleView;
            if (isNew)
            {
                ViewClientGrid.Visibility = Visibility.Collapsed;
                EditClientGrid.Visibility = Visibility.Visible;
                EditClient.Visibility = Visibility.Collapsed;
                SaveClient.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Clients.GetClientByID(ID).Deleted)
                {
                    EditClient.Visibility = Visibility.Collapsed;
                    DelClient.Visibility = Visibility.Collapsed;
                }
                FillData();
            }

            if (!Right.CheckAccess("EditClient"))
            {
                EditClient.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelClient"))
            {
                DelClient.Visibility = Visibility.Collapsed;
            }
        }

        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrClient = Clients.GetClientByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            FIOBox.Text = CurrClient.FIO;
            PassportBox.Text = CurrClient.Passport;
            PhoneNumberBox.Text = CurrClient.PhoneNumber;
            CommentBox.Text = CurrClient.Comment;
        }

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Clients.GetIdsForFilters();
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
                UserControl usc = new ClientsControl(mainWindow);
                mainWindow.GridMain.Children.Add(usc);
            }
            else
            {
                secondWindow.GridMain.Children.Clear();
                UserControl usc = new FindClientControl(secondWindow, saleView);
                secondWindow.GridMain.Children.Add(usc);
            }
        }
        private void ToursView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "clientsales", CurrID);
            secondWindow.Show();
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Clients.GetClientByID(CurrID).Deleted)
        //        {
        //            EditClient.Visibility = Visibility.Collapsed;
        //            DelClient.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditClient"))
        //            {
        //                EditClient.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelClient"))
        //            {
        //                DelClient.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //    }
        //}

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            ViewClientGrid.Visibility = Visibility.Collapsed;
            EditClientGrid.Visibility = Visibility.Visible;
            EditClient.Visibility = Visibility.Collapsed;
            SaveClient.Visibility = Visibility.Visible;
            EditFIOBox.Text = FIOBox.Text;
            EditPassportBox.Text = PassportBox.Text;
            EditPhoneNumberBox.Text = PhoneNumberBox.Text;
            EditCommentBox.Text = CommentBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrClient.FIO = Validator.ValidateStringWithoutLower(EditFIOBox.Text);
                CurrClient.Passport = Validator.ValidateStringWithoutLower(EditPassportBox.Text);
                CurrClient.PhoneNumber = Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text);
                CurrClient.Comment = Validator.ValidateStringWithoutLower(EditCommentBox.Text);

                result = CurrClient.EditClient();
            }
            else
            {
                CurrClient = new Client(Validator.ValidateStringWithoutLower(EditFIOBox.Text), Validator.ValidateStringWithoutLower(EditPassportBox.Text),
                    Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text), Validator.ValidateStringWithoutLower(EditCommentBox.Text));

                result = CurrClient.AddClient();
            }



            if (result.Success)
            {
                CurrID = CurrClient.ID;
                FillData();
                ViewClientGrid.Visibility = Visibility.Visible;
                EditClientGrid.Visibility = Visibility.Collapsed;
                EditClient.Visibility = Visibility.Visible;
                SaveClient.Visibility = Visibility.Collapsed;
                MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                EditingIndicator.Visibility = Visibility.Collapsed;
                isNew = false;
            }
            else
            {
                int errorCode = 0;
                if (result.Description.Contains("empty_fio"))
                {
                    EditFIOBox.ToolTip = "Это поле не может быть пустым!";
                    EditFIOBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_passport"))
                {
                    EditPassportBox.ToolTip = "Это поле не может быть пустым!";
                    EditPassportBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                else if (result.Description.Contains("wrong_format_passport"))
                {
                    EditPassportBox.ToolTip = "Неверный формат паспорта";
                    EditPassportBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_phonenumber"))
                {
                    EditPhoneNumberBox.ToolTip = "Это поле не может быть пустым!";
                    EditPhoneNumberBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                else if (result.Description.Contains("wrong_format_phonenumber"))
                {
                    EditPhoneNumberBox.ToolTip = "Неверный формат номера телефона";
                    EditPhoneNumberBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой клиент уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void DelClient_Click(object sender, RoutedEventArgs e)
        {
            if (CurrClient != null)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить клиента?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrClient.DelClient();
                    FillData();
                    EditClient.Visibility = Visibility.Collapsed;
                    DelClient.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данный клиент еще не сохранён. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void EditFIOBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditFIOBox.ToolTip = null;
            EditFIOBox.Background = Brushes.Transparent;
        }

        private void EditPassportBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditPassportBox.ToolTip = null;
            EditPassportBox.Background = Brushes.Transparent;
        }

        private void EditPhoneNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditPhoneNumberBox.ToolTip = null;
            EditPhoneNumberBox.Background = Brushes.Transparent;
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Clients.GetClientByID(CurrID).Deleted)
                {
                    EditClient.Visibility = Visibility.Collapsed;
                    DelClient.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditClient"))
                    {
                        EditClient.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelClient"))
                    {
                        DelClient.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Client client = Clients.GetClientByID(CurrID + 1);
            if (client != null)
            {
                CurrID++;
                if (client.Deleted)
                {
                    EditClient.Visibility = Visibility.Collapsed;
                    DelClient.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditClient"))
                    {
                        EditClient.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelClient"))
                    {
                        DelClient.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }
    }
}
