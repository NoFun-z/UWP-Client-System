using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EmployeeLib;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Payroll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Salaried salaried1 = new Salaried("123456789", "Davy", "Jones", new DateTime(2021, 03, 08), Convert.ToDecimal(75000));
            salaried1.Phone = "159-753-4862";
            txtSalaried.Text = salaried1.ToString() + "\nPension: $" + salaried1.Pension();

            Hourly hourly1 = new Hourly("987654321", "Tobey", "Maguire", new DateTime(2014, 12, 25), 23, 60);
            hourly1.Phone = "310-285-5200";
            txtHourly.Text = hourly1.ToString() + "\nPension: $" + hourly1.Pension();

            Manager manager1 = new Manager("761943852", "Jackie", "Chen", new DateTime(2016, 07, 15), 62000, 3000, 25);
            manager1.Phone = "289-156-3547";
            txtManager.Text = manager1.ToString() + "\nCalculate pay (2 weeks): $" + manager1.CalculatePay() + "\nIncomeTax: $" + manager1.IncomeTax();

            Sales sales1 = new Sales("741852963", "Keanu", "Reeves", new DateTime(2015, 10, 21), 90000, 2000000, 475000);
            sales1.Phone = "xxx-xxx-xxxx";
            txtSales.Text = sales1.ToString() + "\nCalculate pay: $" + sales1.CalculatePay() + "\nUnionDues: $" + sales1.UnionDues();
        }

        private void txtSalaried_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtHourly_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtManager_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtSales_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
