using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Microsoft.Win32;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        private MainWindow mainWindow;
        public int CurrID;
        private Employee CurrEmployee;
        private string fileName;
        public AccountView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            
            CurrID = User.GetUser().ID;
            if (Employees.GetEmployeeByID(CurrID).Deleted)
            {
                EditEmployee.Visibility = Visibility.Collapsed;
            }
            FillData();
        }
        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrEmployee = Employees.GetEmployeeByID(CurrID);
            });
            FIOBox.Text = CurrEmployee.FIO;
            PassportBox.Text = CurrEmployee.Passport;
            PhoneNumberBox.Text = CurrEmployee.PhoneNumber;
            INNBox.Text = CurrEmployee.INN;
            PositionBox.Text = CurrEmployee.GetRole().Name;
            EditPositionBox.Text = PositionBox.Text;
            LoginBox.Text = CurrEmployee.Login;

            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.Length - 11, 10);
            string imgUri = System.IO.Path.Combine(appDir, CurrEmployee.Image);

            Image.Source = new BitmapImage(new Uri(imgUri));
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

        private void EditINNBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditINNBox.ToolTip = null;
            EditINNBox.Background = Brushes.Transparent;
        }

        private void EditLoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditLoginBox.ToolTip = null;
            EditLoginBox.Background = Brushes.Transparent;
        }

        private void EditPassBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditPassBox.ToolTip = null;
            EditPassBox.Background = Brushes.Transparent;
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            ViewEmployeeGrid.Visibility = Visibility.Collapsed;
            EditEmployeeGrid.Visibility = Visibility.Visible;
            EditEmployee.Visibility = Visibility.Collapsed;
            SaveEmployee.Visibility = Visibility.Visible;
            EditFIOBox.Text = FIOBox.Text;
            EditPassportBox.Text = PassportBox.Text;
            EditPhoneNumberBox.Text = PhoneNumberBox.Text;
            EditINNBox.Text = INNBox.Text;
            EditLoginBox.Text = LoginBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;

            EditImage.Source = Image.Source;
        }

        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            CurrEmployee.FIO = Validator.ValidateStringWithoutLower(EditFIOBox.Text);
            CurrEmployee.Passport = Validator.ValidateStringWithoutLower(EditPassportBox.Text);
            CurrEmployee.PhoneNumber = Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text);
            CurrEmployee.INN = Validator.ValidateStringWithoutLower(EditINNBox.Text);
            CurrEmployee.Login = Validator.ValidateStringWithoutLower(EditLoginBox.Text);

            result = CurrEmployee.EditEmployee();


            if (result.Success)
            {
                CurrID = CurrEmployee.ID;
                if (fileName != null && fileName != "")
                {
                    SetNewImage(fileName);
                }
                FillData();
                ViewEmployeeGrid.Visibility = Visibility.Visible;
                EditEmployeeGrid.Visibility = Visibility.Collapsed;
                EditEmployee.Visibility = Visibility.Visible;
                SaveEmployee.Visibility = Visibility.Collapsed;

                MyMessageBox myMessageBox = new MyMessageBox(this, "Успешно", "Данные успешно изменены.", true);
                myMessageBox.ShowDialog();
                EditingIndicator.Visibility = Visibility.Collapsed;
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
                if (result.Description.Contains("empty_role"))
                {
                    EditPositionBox.ToolTip = "Это поле не может быть пустым!";
                    EditPositionBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                if (result.Description.Contains("empty_inn"))
                {
                    EditINNBox.ToolTip = "Это поле не может быть пустым!";
                    EditINNBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                }
                else if (result.Description.Contains("wrong_format_inn"))
                {
                    EditINNBox.ToolTip = "Неверный формат ИНН";
                    EditINNBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
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
                    MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Такой сотрудник уже существует!", true);
                    myMessageBox.ShowDialog();
                }
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            if (EditPassGrid.Visibility == Visibility.Collapsed)
            {
                EditPassGrid.Visibility = Visibility.Visible;
            }
            else
            {
                EditPassGrid.Visibility = Visibility.Collapsed;
                EditPassBox.Text = "";
            }
        }

        private void SavePass_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();
            
            result = CurrEmployee.SetPass(EditPassBox.Text);
            if (result.Success)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Успех!", "Пароль успешно изменен.", true);
                myMessageBox.ShowDialog();

                EditPassGrid.Visibility = Visibility.Collapsed;
                EditPassBox.Text = "";
            }
            else
            {
                EditPassBox.ToolTip = "Заполните поле!";
                EditPassBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
            }
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JPG (*.jpg)|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                SetNewImage(openFileDialog.FileName);
            }
        }

        private void SetNewImage(string path)
        {   
            CurrEmployee.SetImage(path);

            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.Length - 11, 10);
            string imgUri = System.IO.Path.Combine(appDir, CurrEmployee.Image);

            BitmapImage bitmapImage = new BitmapImage(new Uri(imgUri));

            EditImage.Source = bitmapImage;

            mainWindow.FillImage();   
        }
    }
}
