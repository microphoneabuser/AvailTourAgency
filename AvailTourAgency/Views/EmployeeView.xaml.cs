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
    public partial class EmployeeView : UserControl
    {
        private MainWindow mainWindow;
        public int CurrID;
        private Employee CurrEmployee;
        private bool isNew;
        private string fileName;
        public EmployeeView(MainWindow mainWindow, int ID, bool isNew)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.isNew = isNew;
            if (isNew)
            {
                ViewEmployeeGrid.Visibility = Visibility.Collapsed;
                EditEmployeeGrid.Visibility = Visibility.Visible;
                EditEmployee.Visibility = Visibility.Collapsed;
                SaveEmployee.Visibility = Visibility.Visible;
                FillPositionComboBox();
            }
            else
            {
                CurrID = ID;
                if (Employees.GetEmployeeByID(ID).Deleted)
                {
                    EditEmployee.Visibility = Visibility.Collapsed;
                    DelEmployee.Visibility = Visibility.Collapsed;
                }
                FillData();
            }

            if (!Right.CheckAccess("EditEmployee"))
            {
                EditEmployee.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("DelEmployee"))
            {
                DelEmployee.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("ActivateUser"))
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                EditLoginGrid.Visibility = Visibility.Collapsed;
                EditPassGrid.Visibility = Visibility.Collapsed;
                ChangePass.Visibility = Visibility.Collapsed;
            }
            if (!Right.CheckAccess("SeeEmployees"))
            {
                comeBackButton.Visibility = Visibility.Collapsed;
            }
        }
        private async void FillData()
        {
            await Task.Run(() =>
            {
                CurrEmployee = Employees.GetEmployeeByID(CurrID);
            });
            IdBox.Text = CurrID.ToString();
            FIOBox.Text = CurrEmployee.FIO;
            PassportBox.Text = CurrEmployee.Passport;
            PhoneNumberBox.Text = CurrEmployee.PhoneNumber;
            INNBox.Text = CurrEmployee.INN;
            PositionBox.Text = CurrEmployee.GetRole().Name;
            LoginBox.Text = CurrEmployee.Login;

            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(48);
            string imgUri = System.IO.Path.Combine(appDir, CurrEmployee.Image);

            Image.Source = new BitmapImage(new Uri(imgUri));
        }

        //private async void FillIdComboBox()
        //{
        //    IEnumerable<int> idCollection = null;

        //    IdComboBox.Items.Clear();

        //    await Task.Run(() =>
        //    {
        //        idCollection = Employees.GetIdsForFilters();
        //    });

        //    IdComboBox.Items.Add("");

        //    idCollection.ToList().ForEach(i => IdComboBox.Items.Add(i));
        //}
        private async void FillPositionComboBox()
        {
            IEnumerable<string> rolesCollection = null;

            EditPositionBox.Items.Clear();

            await Task.Run(() =>
            {
                rolesCollection = Roles.GetRolesForFilters();
            });

            EditPositionBox.Items.Add("");

            rolesCollection.ToList().ForEach(i => EditPositionBox.Items.Add(i));

            EditPositionBox.SelectedItem = PositionBox.Text;
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
            mainWindow.GridMain.Children.Clear();
            UserControl usc = new EmployeesControl(mainWindow);
            mainWindow.GridMain.Children.Add(usc);
        }
        private void ToursView_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(mainWindow, "employeesales", CurrID);
            secondWindow.Show();
        }

        //private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (IdComboBox.SelectedItem != null && IdComboBox.SelectedItem.ToString() != "")
        //    {
        //        CurrID = (int)IdComboBox.SelectedItem;
        //        if (Employees.GetEmployeeByID(CurrID).Deleted)
        //        {
        //            EditEmployee.Visibility = Visibility.Collapsed;
        //            DelEmployee.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            if (Right.CheckAccess("EditEmployee"))
        //            {
        //                EditEmployee.Visibility = Visibility.Visible;
        //            }
        //            if (Right.CheckAccess("DelEmployee"))
        //            {
        //                DelEmployee.Visibility = Visibility.Visible;
        //            }
        //        }
        //        FillData();
        //    }
        //}

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

        private void EditPositionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditPositionBox.ToolTip = null;
            EditPositionBox.Background = Brushes.Transparent;
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
            FillPositionComboBox();
            EditLoginBox.Text = LoginBox.Text;
            EditingIndicator.Visibility = Visibility.Visible;

            EditImage.Source = Image.Source;
        }

        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();

            if (!isNew)
            {
                CurrEmployee.FIO = Validator.ValidateStringWithoutLower(EditFIOBox.Text);
                CurrEmployee.Passport = Validator.ValidateStringWithoutLower(EditPassportBox.Text);
                CurrEmployee.PhoneNumber = Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text);
                CurrEmployee.INN = Validator.ValidateStringWithoutLower(EditINNBox.Text);
                Role role = Roles.GetRoleByName(EditPositionBox.Text);
                if (role != null)
                {
                    CurrEmployee.RoleID = role.ID;
                }
                else
                {
                    CurrEmployee.RoleID = 0;
                }
                CurrEmployee.Login = Validator.ValidateStringWithoutLower(EditLoginBox.Text);

                result = CurrEmployee.EditEmployee();
            }
            else
            {
                Role role = Roles.GetRoleByName(EditPositionBox.Text);
                int roleID = 0;
                if (role != null)
                {
                   roleID = role.ID;
                }
                CurrEmployee = new Employee(Validator.ValidateStringWithoutLower(EditFIOBox.Text), Validator.ValidateStringWithoutLower(EditPassportBox.Text),
                    Validator.ValidateStringWithoutLower(EditPhoneNumberBox.Text), Validator.ValidateStringWithoutLower(EditINNBox.Text), 
                    roleID, Validator.ValidateStringWithoutLower(EditLoginBox.Text));

                result = CurrEmployee.AddEmployee();

            }



            if (result.Success)
            {
                CurrID = CurrEmployee.ID;
                if (isNew && fileName != null && fileName != "")
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
                //if (result.Description.Contains("empty_login"))
                //{
                //    EditLoginBox.ToolTip = "Это поле не может быть пустым!";
                //    EditLoginBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                //}
                //if (result.Description.Contains("empty_pass"))
                //{
                //    EditPassBox.ToolTip = "Это поле не может быть пустым!";
                //    EditPassBox.Background = new SolidColorBrush(Color.FromArgb(100, 100, 200, 220));
                //}
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

        private void DelEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (CurrEmployee != null)
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Вы уверены, что хотите удалить сотрудника?", "Это действие будет невозможно отменить", false);
                myMessageBox.ShowDialog();

                if (myMessageBox.flag)
                {
                    CurrEmployee.DelEmployee();
                    FillData();
                    EditEmployee.Visibility = Visibility.Collapsed;
                    DelEmployee.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка", "Данный сотрудник еще не сохранён. Нажмите на кнопку \"Сохранить\"", true);
                myMessageBox.ShowDialog();
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

            if (!isNew)
            {
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
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(this, "Ошибка!", "Задать пароль сотрудника можно только после его сохранения.", true);
                myMessageBox.ShowDialog();
            }
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JPG (*.jpg)|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                if (!isNew)
                {
                    SetNewImage(openFileDialog.FileName);
                }
                else
                {
                    fileName = openFileDialog.FileName;

                    BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));

                    EditImage.Source = bitmapImage;
                }
            }
                //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
        private void SetNewImage(string path)
        {   
            CurrEmployee.SetImage(path);

            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(48);
            string imgUri = System.IO.Path.Combine(appDir, CurrEmployee.Image);

            BitmapImage bitmapImage = new BitmapImage(new Uri(imgUri));

            EditImage.Source = bitmapImage;

            mainWindow.FillImage();   
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrID - 1 != 0)
            {
                CurrID--;
                if (Employees.GetEmployeeByID(CurrID).Deleted)
                {
                    EditEmployee.Visibility = Visibility.Collapsed;
                    DelEmployee.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditEmployee"))
                    {
                        EditEmployee.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelEmployee"))
                    {
                        DelEmployee.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = Employees.GetEmployeeByID(CurrID + 1);
            if (employee != null)
            {
                CurrID++;
                if (employee.Deleted)
                {
                    EditEmployee.Visibility = Visibility.Collapsed;
                    DelEmployee.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Right.CheckAccess("EditEmployee"))
                    {
                        EditEmployee.Visibility = Visibility.Visible;
                    }
                    if (Right.CheckAccess("DelEmployee"))
                    {
                        DelEmployee.Visibility = Visibility.Visible;
                    }
                }
                FillData();
            }
        }
    }
}
