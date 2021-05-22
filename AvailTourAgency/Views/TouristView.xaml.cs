using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    /// Логика взаимодействия для TouristView.xaml
    /// </summary>
    public partial class TouristView : UserControl
    {
        private SecondWindow secondWindow;
        private SaleView saleView;
        public int CurrID;
        private Tourist CurrTourist;
        private bool isNew;
        Dictionary<int, string> DocumentTypes = new Dictionary<int, string>();

        public TouristView(SecondWindow secondWindow, int ID, bool isNew, SaleView saleView)//конструктор для вызова из второстепенной формы
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.isNew = isNew;
            this.saleView = saleView;
            if (isNew)
            {
                ViewTouristGrid.Visibility = Visibility.Collapsed;
                EditTouristGrid.Visibility = Visibility.Visible;
                EditTourist.Visibility = Visibility.Collapsed;
                SaveTourist.Visibility = Visibility.Visible;
            }
            else
            {
                CurrID = ID;
                if (Tourists.GetTouristByID(ID).Deleted)
                {
                    EditTourist.Visibility = Visibility.Collapsed;
                    DelTourist.Visibility = Visibility.Collapsed;
                }
                FillData();
            }

            string[] fc = ConfigurationManager.AppSettings["DocumentTypes"].Split(';');
            foreach (string c in fc)
            {
                DocumentTypes.Add(int.Parse(c.Split(':')[0]), c.Split(':')[1]);
            }

            EditDocumentTypeBox.ItemsSource = DocumentTypes.Values;
        }

        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrTourist = Tourists.GetTouristByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            FIOBox.Text = CurrTourist.FIO;
            DateOfBitrhBox.Text = CurrTourist.DateOfBirth.Value.ToShortDateString();
            PhoneNumberBox.Text = CurrTourist.PhoneNumber;
            DocumentTypeBox.Text = DocumentTypes[CurrTourist.DocumentType];
            DocumentDataBox.Text = CurrTourist.DocumentData;
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
            secondWindow.GridMain.Children.Clear();
            UserControl usc = new FindTouristControl(secondWindow, saleView);
            secondWindow.GridMain.Children.Add(usc);   
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Tourists.GetTouristByID(CurrID).Deleted)
        //        {
        //            EditTourist.Visibility = Visibility.Collapsed;
        //            DelTourist.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            EditTourist.Visibility = Visibility.Visible;
        //            DelTourist.Visibility = Visibility.Visible;
        //        }
        //        FillData();
        //    }
        //}
        private void EditFIOBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditFIOBox.ToolTip = null;
            EditFIOBox.Background = Brushes.Transparent;
        }

        private void EditDocumentDataBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditDocumentDataBox.ToolTip = null;
            EditDocumentDataBox.Background = Brushes.Transparent;
        }
        private void EditDocumentTypeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditDocumentTypeBox.ToolTip = null;
            EditDocumentTypeBox.Background = Brushes.Transparent;
        }
        private void EditPhoneNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditPhoneNumberBox.ToolTip = null;
            EditPhoneNumberBox.Background = Brushes.Transparent;
        }
        private void EditDateOfBirthBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EditDateOfBirthBox.ToolTip = null;
            EditDateOfBirthBox.Background = Brushes.Transparent;
        }

        private void EditTourist_Click(object sender, RoutedEventArgs e)
        {
            ViewTouristGrid.Visibility = Visibility.Collapsed;
            EditTouristGrid.Visibility = Visibility.Visible;
            EditTourist.Visibility = Visibility.Collapsed;
            SaveTourist.Visibility = Visibility.Visible;
            EditFIOBox.Text = FIOBox.Text;
            EditPhoneNumberBox.Text = PhoneNumberBox.Text;
            EditDateOfBirthBox.SelectedDate = DateTime.Parse(DateOfBitrhBox.Text);
            EditDocumentTypeBox.Text = DocumentTypeBox.Text;
            EditDocumentDataBox.Text = DocumentDataBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;
        }

        private void SaveTourist_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrTourist.FIO = Validator.ValidateStringWithoutLower(EditFIOBox.Text);
                CurrTourist.PhoneNumber = Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text);
                CurrTourist.DateOfBirth = EditDateOfBirthBox.SelectedDate;

                CurrTourist.DocumentType = DocumentTypes.Where(f => f.Value == EditDocumentTypeBox.Text).FirstOrDefault().Key;

                CurrTourist.DocumentData = Validator.ValidateStringWithoutLower(EditDocumentDataBox.Text);

                result = CurrTourist.EditTourist();
            }
            else
            {
                CurrTourist = new Tourist(Validator.ValidateStringWithoutLower(EditFIOBox.Text), Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text),
                    EditDateOfBirthBox.SelectedDate, DocumentTypes.Where(f => f.Value == EditDocumentTypeBox.Text).FirstOrDefault().Key, Validator.ValidateStringWithoutLower(EditDocumentDataBox.Text));

                result = CurrTourist.AddTourist();
            }



            if (result.Success)
            {
                CurrID = CurrTourist.ID;
                FillData();
                ViewTouristGrid.Visibility = Visibility.Visible;
                EditTouristGrid.Visibility = Visibility.Collapsed;
                EditTourist.Visibility = Visibility.Visible;
                SaveTourist.Visibility = Visibility.Collapsed;
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
                if (result.Description.Contains("empty_dateofbirth"))
                {
                    EditDateOfBirthBox.ToolTip = "Это поле не может бвть пустым!";
                    EditDateOfBirthBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_documenttype"))
                {
                    EditDocumentTypeBox.ToolTip = "Это поле не может быть пустым";
                    EditDocumentTypeBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_documentdata"))
                {
                    EditDocumentDataBox.ToolTip = "Это поле не может быть пустым";
                    EditDocumentDataBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой турист уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void DelTourist_Click(object sender, RoutedEventArgs e)
        {
            if (CurrTourist != null)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить туриста?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrTourist.DelTourist();
                    FillData();
                    EditTourist.Visibility = Visibility.Collapsed;
                    DelTourist.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данный турист ещё не сохранен. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
            }
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Tourists.GetTouristByID(CurrID).Deleted)
                {
                    EditTourist.Visibility = Visibility.Collapsed;
                    DelTourist.Visibility = Visibility.Collapsed;
                }
                else
                {
                    EditTourist.Visibility = Visibility.Visible;
                    DelTourist.Visibility = Visibility.Visible;
                }
                FillData();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Tourist tourist = Tourists.GetTouristByID(CurrID + 1);
            if (tourist != null)
            {
                CurrID++;
                if (tourist.Deleted)
                {
                    EditTourist.Visibility = Visibility.Collapsed;
                    DelTourist.Visibility = Visibility.Collapsed;
                }
                else
                {
                    EditTourist.Visibility = Visibility.Visible;
                    DelTourist.Visibility = Visibility.Visible;
                }
                FillData();
            }
        }

        private void EditDocumentTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditDocumentTypeBox.ToolTip = null;
            EditDocumentTypeBox.Background = Brushes.Transparent;
        }
    }
}
