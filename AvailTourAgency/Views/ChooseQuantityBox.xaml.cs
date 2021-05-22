using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AvailTourAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для ChooseQuantityBox.xaml
    /// </summary>
    public partial class ChooseQuantityBox : Window
    {
        SecondWindow secondWindow;
        SaleView saleView;
        int addServiceID;
        public ChooseQuantityBox(SecondWindow secondWindow, SaleView saleView, int addServiceID)
        {
            InitializeComponent();
            this.secondWindow = secondWindow;
            this.saleView = saleView;
            this.addServiceID = addServiceID;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NoButton_Click(sender, e);
            }
        }
        private void close_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_Button.Background = Brushes.RoyalBlue;
        }

        private void close_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void close_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.LightGray;
        }
        private void close_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            close_Button.Background = Brushes.Transparent;
        }
        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuantityBox.Value.HasValue)
            {
                this.Close();
                saleView.AddAdditionalService(addServiceID, QuantityBox.Value.Value);
                secondWindow.Close();
            }
            else
            {
                MyMessageBox myMessageBox = new MyMessageBox(saleView, "Ошибка", "Заполните поле!", true);
                myMessageBox.ShowDialog();
            }
        }

    }
}
