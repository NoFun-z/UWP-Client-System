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
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab4LocPham
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<EmployeeLibs> EmployeeCollection = new List<EmployeeLibs>();
        public List<EmployeeLibs> match = new List<EmployeeLibs>();
        public static string cboselected;
        public static string NameSin;

        public MainPage()
        {
            this.InitializeComponent();
            MyEmployee().Add(new Salaried("123456789", "Loc", "Pham", new DateTime(2022, 01, 18), 52000));
            MyEmployee().Add(new Salaried("987654321", "Davy", "McGuire", new DateTime(2020, 10, 7), 80000));
            MyEmployee().Add(new Manager("935478962", "John", "Varvatos", new DateTime(2012, 09, 2), 6475, 245, 25));
            MyEmployee().Add(new Manager("357951486", "Michael", "Jordan", new DateTime(2018, 10, 20), 15000, 0, 29));
            MyEmployee().Add(new Hourly("025896314", "Light", "Yagami", new DateTime(1991, 7, 16), 23.75, 45));
            MyEmployee().Add(new Salaried("852096374", "Tobey", "McGuire", new DateTime(1996, 08, 10), 204000));
            MyEmployee().Add(new Hourly("014796325", "George", "Harrison", new DateTime(2000, 11, 15), 18.75, 40));
            MyEmployee().Add(new Sales("354789651", "Karly", "Mitchell", new DateTime(2019, 09, 20), 58000, 600000, 495000));
            MyEmployee().Add(new Salaried("987321654", "Anna", "Pham", new DateTime(2021, 03, 7), 200000));
            MyEmployee().Add(new Sales("031846957", "Davion", "Knight", new DateTime(2004, 04, 30), 35000, 250000, 195000));

            //Combo box and List view
            lvEmployees.ItemsSource = EmployeeCollection;

            //Retrieving Text box
            NameSin = txtNameSin.Text;

            //Hide current text boxes
            txtCurrentFname.Visibility = Visibility.Collapsed;
            txtCurrentSin.Visibility = Visibility.Collapsed;

            EmployeePayroll<EmployeeLibs> a = new EmployeePayroll<EmployeeLibs>(DateTime.Now, EmployeeCollection);

            //Display PayPeriod info
            PayPeriodDisplay();
        }

        //The public method that return a collection of employee types
        public List<EmployeeLibs> MyEmployee()
        {
            return EmployeeCollection;
        }

        //PayPeriod operations
        private void PayPeriodDisplay()
        {
            //Reset the data
            txtPayPeriod.Text = "";
            lvHourlyPayPeriod.ItemsSource = null;

            //Initiate a new collection
            EmployeePayroll<EmployeeLibs> a2 = new EmployeePayroll<EmployeeLibs>(DateTime.Now, EmployeeCollection);

            //Display pay period
            foreach (string str in a2.ProcessPayroll())
            {
                txtPayPeriod.Text += $"{str}\n\n";
            }

            //Pay period listview display
            List<EmployeeLibs> paylist = new List<EmployeeLibs>();
            List<string> paystring = new List<string>();

            foreach (EmployeeLibs emp in EmployeeCollection) // Get all hourly type and add it to the new collection variable
            {
                if (emp.GetType() == typeof(Hourly))
                {
                    paylist.Add(emp);
                }
            }

            foreach (Hourly hours in paylist) // Comparing the hourly object formatted output to each element in the ProcessPayroll method.
            {
                foreach (string strs in a2.ProcessPayroll())
                {
                    if (string.Format("{0} {1}, ID#{2} -Net Pay: ${3} -Bonus: ${4} -Deductions: ${5}", hours.FirstName, hours.LastName, hours.Sin.Substring(0, 4), hours.CalculatePay(), hours.Bonus(), hours.Deduction()) == strs)
                    {
                        paystring.Add(string.Format("{0} {1}, ID#{2} -Net Pay: ${3} -Bonus: ${4} -Deductions: ${5}", hours.FirstName, hours.LastName, hours.Sin.Substring(0, 4), hours.CalculatePay(), hours.Bonus(), hours.Deduction()));
                    }
                }
            }

            lvHourlyPayPeriod.ItemsSource = paystring;
        }

        //Search button applies the combobox operations
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cboEmpTypes_SelectionChanged(null, null);
        }

        //Listview interactions
        private void lvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmployeeLibs selectedlist = (EmployeeLibs)lvEmployees.SelectedItem;

            //I reset the textbox when the list view is selected
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtSIN.Text = "";
            txtWage.Text = "";
            txt2ndOption.Text = "";
            txt3rdOption.Text = "";
            txtEmployeeInfo.Text = "";

            //Reset the current value in the current text blocks that will be used to compare data for updating queries
            txtCurrentFname.Text = "";
            txtCurrentSin.Text = "";

            //Do nothing if the listview is null
            if (selectedlist is null) return;

            //Displaying data
            txtEmployeeInfo.Text = string.Format("Employee Name: {0} {1}\nEmployee SIN: {2}\nEmployee binary wage : ${3}\nEmployee hired date: {4}", selectedlist.FirstName, selectedlist.LastName, selectedlist.Sin, selectedlist.CalculatePay(), selectedlist.HireDate.ToShortDateString());

            //Assigning the employees' names to the text boxes
            txtFirstName.Text = selectedlist.FirstName;
            txtLastName.Text = selectedlist.LastName;
            txtSIN.Text = selectedlist.Sin;

            //Assigning data to the current text boxes
            txtCurrentFname.Text = selectedlist.FirstName;
            txtCurrentSin.Text = selectedlist.Sin;

            //Get the type of one selected list item then cast the selectedlist back to that type then access and display that type's data
            if (lvEmployees.SelectedItem.GetType() == typeof(Salaried))
            {
                Salaried sa = (Salaried)selectedlist;
                txtWage.Text = sa.Amount.ToString();
            }
            else if (lvEmployees.SelectedItem.GetType() == typeof(Hourly))
            {
                Hourly h = (Hourly)selectedlist;
                txt2ndOption.Text = h.Rate.ToString();
                txt3rdOption.Text = h.Hours.ToString();
            }
            else if (lvEmployees.SelectedItem.GetType() == typeof(Manager))
            {
                Manager m = (Manager)selectedlist;
                txtWage.Text = m.Basic_Salary.ToString();
                txt2ndOption.Text = m.Salary_Allowances.ToString();
                txt3rdOption.Text = m.Work_Volume.ToString();
            }
            else if (lvEmployees.SelectedItem.GetType() == typeof(Sales))
            {
                Sales s = (Sales)selectedlist;
                txtWage.Text = s.CalculatePay().ToString();
                txt2ndOption.Text = s.Total_Sales.ToString();
                txt3rdOption.Text = s.Expenses.ToString();
            }
        }

        //Combobox interactions
        private void cboEmpTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboEmpTypes.SelectedIndex == -1) return;    //Do nothing if an employee type is not selected in the combobox

            lvEmployees.ItemsSource = null;     //clear out the previous items in the listview
            match.Clear();                      //reset the list of matches

            NameSin = txtNameSin.Text;          //retrieve the current value from the textbox
            cboselected = ((ComboBoxItem)cboEmpTypes.SelectedItem).Content.ToString();  //retrieve the current selection from the combobox

            // switch (cboEmpTypes.SelectedIndex)
            switch (cboselected)
            {
                case "Hourly":
                    txtWage.Visibility = Visibility.Collapsed;
                    txt2ndOption.Visibility = Visibility.Visible;
                    txt3rdOption.Visibility = Visibility.Visible;
                    txtTotalWage.Visibility = Visibility.Collapsed;
                    txt2ndOp.Visibility = Visibility.Visible;
                    txt3rdOp.Visibility = Visibility.Visible;

                    foreach (EmployeeLibs em in EmployeeCollection)
                    {
                        //first, make sure this employee is of the selected type.  Other types of employees are passed over.
                        if (em.GetType() != typeof(Hourly)) continue;

                        //second, make sure this employee's name or sin matches the value in the NameSin textbox
                        if (em.FirstName.Contains(NameSin) || em.LastName.Contains(NameSin) || string.Format("{0} {1}", em.FirstName, em.LastName) == NameSin || em.Sin.Contains(NameSin) || em.HireDate.ToShortDateString().Contains(((DateTimeOffset)dtDatePicker.SelectedDate).ToString("d")))
                        {
                            match.Add(em);      //this employee meets all the criteria to be included in the matches list.
                        }
                    }

                    break;
                case "Salaried":
                    txtWage.Visibility = Visibility.Visible;
                    txt2ndOption.Visibility = Visibility.Collapsed;
                    txt3rdOption.Visibility = Visibility.Collapsed;
                    txtTotalWage.Visibility = Visibility.Visible;
                    txt2ndOp.Visibility = Visibility.Collapsed;
                    txt3rdOp.Visibility = Visibility.Collapsed;

                    foreach (EmployeeLibs em in EmployeeCollection)
                    {
                        if (em.GetType() != typeof(Salaried)) continue;
                        if (em.FirstName.Contains(NameSin) || em.LastName.Contains(NameSin) || string.Format("{0} {1}", em.FirstName, em.LastName) == NameSin || em.Sin.Contains(NameSin) || em.HireDate.ToShortDateString().Contains(((DateTimeOffset)dtDatePicker.SelectedDate).ToString("d")))
                        {
                            match.Add(em);
                        }
                    }
                    break;
                case "Manager":
                    txtWage.Visibility = Visibility.Visible;
                    txt2ndOption.Visibility = Visibility.Visible;
                    txt3rdOption.Visibility = Visibility.Visible;
                    txtTotalWage.Visibility = Visibility.Visible;
                    txt2ndOp.Visibility = Visibility.Visible;
                    txt3rdOp.Visibility = Visibility.Visible;

                    foreach (EmployeeLibs em in EmployeeCollection)
                    {
                        if (em.GetType() != typeof(Manager)) continue;
                        if (em.FirstName.Contains(NameSin) || em.LastName.Contains(NameSin) || string.Format("{0} {1}", em.FirstName, em.LastName) == NameSin || em.Sin.Contains(NameSin) || em.HireDate.ToShortDateString().Contains(((DateTimeOffset)dtDatePicker.SelectedDate).ToString("d")))
                        {
                            match.Add(em);
                        }
                    }
                    break;
                case "Sales":
                    txtWage.Visibility = Visibility.Visible;
                    txt2ndOption.Visibility = Visibility.Visible;
                    txt3rdOption.Visibility = Visibility.Visible;
                    txtTotalWage.Visibility = Visibility.Visible;
                    txt2ndOp.Visibility = Visibility.Visible;
                    txt3rdOp.Visibility = Visibility.Visible;

                    foreach (EmployeeLibs em in EmployeeCollection)
                    {
                        if (em.GetType() != typeof(Sales)) continue;
                        if (em.FirstName.Contains(NameSin) || em.LastName.Contains(NameSin) || string.Format("{0} {1}", em.FirstName, em.LastName) == NameSin || em.Sin.Contains(NameSin) || em.HireDate.ToShortDateString().Contains(((DateTimeOffset)dtDatePicker.SelectedDate).ToString("d")))
                        {
                            match.Add(em);
                        }
                    }
                    break;

            }

            lvEmployees.ItemsSource = match;        //now that we have a new updated matches list, re-assign it to the listview.


            txtNameSin.Focus(FocusState.Keyboard);
        }

        //Update button
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string NewFName = txtFirstName.Text;
            string NewLName = txtLastName.Text;
            string SIN = txtSIN.Text;
            string Wage = txtWage.Text;
            string SecondOption = txt2ndOption.Text;
            string ThirdOption = txt3rdOption.Text;

            foreach (EmployeeLibs emps in EmployeeCollection)
            {
                if (emps.FirstName.Contains(txtCurrentFname.Text) && emps.Sin.Contains(txtCurrentSin.Text))
                {
                    if (NewFName != "" && NewFName != emps.FirstName)
                        emps.FirstName = NewFName;
                    if (NewLName != "" && NewLName != emps.LastName)
                        emps.LastName = NewLName;
                    if (SIN != "" && SIN != emps.Sin)
                        emps.Sin = SIN;
                    if (emps.GetType() == typeof(Salaried))
                    {
                        Salaried sa = (Salaried)emps;
                        if (Wage != "" && Wage != sa.Amount.ToString())
                            sa.Amount = Convert.ToDecimal(Wage);
                    }
                    if (emps.GetType() == typeof(Hourly))
                    {
                        Hourly h = (Hourly)emps;
                        if (SecondOption != "" && SecondOption != h.Rate.ToString())
                            h.Rate = Convert.ToDouble(SecondOption);
                        if (ThirdOption != "" && ThirdOption != h.Hours.ToString())
                            h.Hours = Convert.ToDouble(ThirdOption);
                    }
                    if (emps.GetType() == typeof(Manager))
                    {
                        Manager m = (Manager)emps;
                        if (Wage != "" && Wage != m.Basic_Salary.ToString())
                            m.Basic_Salary = Convert.ToDecimal(Wage);
                        if (SecondOption != "" && SecondOption != m.Salary_Allowances.ToString())
                            m.Salary_Allowances = Convert.ToDecimal(SecondOption);
                        if (ThirdOption != "" && ThirdOption != m.Work_Volume.ToString())
                            m.Work_Volume = Convert.ToDouble(ThirdOption);
                    }
                    if (emps.GetType() == typeof(Sales))
                    {
                        Sales s = (Sales)emps;
                        if (Wage != "" && Wage != s.Amount.ToString())
                            s.Amount = Convert.ToDecimal(Wage);
                        if (SecondOption != "" && SecondOption != s.Total_Sales.ToString())
                            s.Total_Sales = Convert.ToDecimal(SecondOption);
                        if (ThirdOption != "" && ThirdOption != s.Expenses.ToString())
                            s.Expenses = Convert.ToDecimal(ThirdOption);
                    }
                    break;
                }
            }
            //Refresh data source
            lvEmployees.ItemsSource = null;
            lvEmployees.ItemsSource = EmployeeCollection;
        }

        //Adding new employee operation
        private async void btnAddEmps_Click(object sender, RoutedEventArgs e)
        {
            //Return if something went wrong
            if (cboEmpTypes.SelectedIndex == -1) return;
            if (dtDatePicker.SelectedDate == null) return;

            //Grab the value from the text boxes
            string FName = txtFirstName.Text;
            string LName = txtLastName.Text;
            string SIN = txtSIN.Text;
            string Wage = txtWage.Text;
            string SecondOption = txt2ndOption.Text;
            string ThirdOption = txt3rdOption.Text;

            //Only execute if the selected item in the combo box is not null or no employee is selected in the list view
            if (cboEmpTypes.SelectedIndex != -1 && lvEmployees.SelectedItem == null)
            {
                switch (((ComboBoxItem)cboEmpTypes.SelectedItem).Content.ToString())
                {
                    case "Salaried":
                        try
                        {
                            if (FName != "" && LName != "" && SIN != "" && Wage != "" && dtDatePicker.SelectedDate != null && lvEmployees.SelectedItem == null)
                                EmployeeCollection.Add(new Salaried(SIN, FName, LName, DateTime.Parse(dtDatePicker.SelectedDate.ToString()), Convert.ToDecimal(Wage)));
                            else
                            {
                                MessageDialog msg = new MessageDialog("Adding new employees not avaiable when a specific employee is selected/No field should be left empty, please fill all required data");
                                await msg.ShowAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog mess = new MessageDialog(ex.Message);
                            await mess.ShowAsync();
                        }
                        break;
                    case "Hourly":
                        try
                        {
                            if (FName != "" && LName != "" && SIN != "" && SecondOption != "" && ThirdOption != "" && dtDatePicker.SelectedDate != null && lvEmployees.SelectedItem == null)
                                EmployeeCollection.Add(new Hourly(SIN, FName, LName, DateTime.Parse(dtDatePicker.SelectedDate.ToString()), Convert.ToDouble(SecondOption), Convert.ToDouble(ThirdOption)));
                            else
                            {
                                MessageDialog msg = new MessageDialog("Adding new employees not avaiable when a specific employee is selected/No field should be left empty, please fill all required data");
                                await msg.ShowAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog mess = new MessageDialog(ex.Message);
                            await mess.ShowAsync();
                        }
                        break;
                    case "Manager":
                        try
                        {
                            if (FName != "" && LName != "" && SIN != "" && Wage != "" && SecondOption != "" && ThirdOption != "" && dtDatePicker.SelectedDate != null && lvEmployees.SelectedItem == null)
                                EmployeeCollection.Add(new Manager(SIN, FName, LName, DateTime.Parse(dtDatePicker.SelectedDate.ToString()), Convert.ToDecimal(Wage), Convert.ToDecimal(SecondOption), Convert.ToDouble(ThirdOption)));
                            else
                            {
                                MessageDialog msg = new MessageDialog("Adding new employees not avaiable when a specific employee is selected/No field should be left empty, please fill all required data");
                                await msg.ShowAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog mess = new MessageDialog(ex.Message);
                            await mess.ShowAsync();
                        }
                        break;
                    case "Sales":
                        try
                        {
                            if (FName != "" && LName != "" && SIN != "" && Wage != "" && SecondOption != "" && ThirdOption != "" && dtDatePicker.SelectedDate != null && lvEmployees.SelectedItem == null)
                                EmployeeCollection.Add(new Sales(SIN, FName, LName, DateTime.Parse(dtDatePicker.SelectedDate.ToString()), Convert.ToDecimal(Wage), Convert.ToDecimal(SecondOption), Convert.ToDecimal(ThirdOption)));
                            else
                            {
                                MessageDialog msg = new MessageDialog("Adding new employees not avaiable when a specific employee is selected/No field should be left empty, please fill all required data");
                                await msg.ShowAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog mess = new MessageDialog(ex.Message);
                            await mess.ShowAsync();
                        }
                        break;
                }
            }

            //Refresh data source
            lvEmployees.ItemsSource = null;
            lvEmployees.ItemsSource = EmployeeCollection;
        }

        //Hourly Listview operation
        private void lvHourlyPayPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Return if nothing is selected
            if (lvHourlyPayPeriod.SelectedItem == null) return;

            //Reset the text box
            txtHoursWorked.Text = "";

            //Retrieve the selected item
            string hourly = (string)lvHourlyPayPeriod.SelectedItem;

            //Add the relevant data to the text box
            foreach (EmployeeLibs employee in EmployeeCollection)
            {
                if (string.Format("{0} {1}, ID#{2} -Net Pay: ${3} -Bonus: ${4} -Deductions: ${5}", employee.FirstName, employee.LastName, employee.Sin.Substring(0, 4), employee.CalculatePay(), employee.Bonus(), employee.Deduction()) == hourly)
                {
                    Hourly HEmployee = (Hourly)employee;
                    txtHoursWorked.Text = HEmployee.Hours.ToString();
                }
            }
        }

        //Keydown for the txtNameSin text box
        private void txtNameSin_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                cboEmpTypes_SelectionChanged(null, null);
        }

        //Updating Hourly Listview
        private void btnHoursWorked_Click(object sender, RoutedEventArgs e)
        {
            //Do nothing if the Hourly listview not selected
            if (lvHourlyPayPeriod.SelectedItem == null) return;

            //Retrieving the selected item in the hourly listview
            string lvHourlyItem = (string)lvHourlyPayPeriod.SelectedItem;

            //Retrieving the text box
            string HoursWorked = txtHoursWorked.Text;

            //Search the matching obj and update if the value is different
            foreach (EmployeeLibs em in EmployeeCollection)
            {
                if (em.GetType() == typeof(Hourly))
                {
                    Hourly HourlyEm = (Hourly)em;

                    if (string.Format("{0} {1}, ID#{2} -Net Pay: ${3} -Bonus: ${4} -Deductions: ${5}", HourlyEm.FirstName, HourlyEm.LastName, HourlyEm.Sin.Substring(0, 4), HourlyEm.CalculatePay(), HourlyEm.Bonus(), HourlyEm.Deduction()) == lvHourlyItem)
                    {
                        if (HourlyEm.Hours != Convert.ToDouble(txtHoursWorked.Text) && txtHoursWorked.Text != "") // Update if the entered hour worked not equal to the old one and is not empty
                        {
                            HourlyEm.Hours = Convert.ToDouble(txtHoursWorked.Text);

                            //Refresh the all data display
                            lvEmployees.ItemsSource = null;
                            lvEmployees.ItemsSource = EmployeeCollection;
                            PayPeriodDisplay();

                            break;
                        }
                    }
                }
            }
        }

        //Generic class
        public class EmployeePayroll<T>
        {
            //Fields
            private DateTime field1;
            private List<EmployeeLibs> field2 = new List<EmployeeLibs>();

            //Properties
            //Readonly properties
            //Total emps count
            public int TotalEmps => field2.Count;

            //Total pay
            public decimal TotalPay
            {
                get
                {
                    decimal totalpay = 0;
                    foreach (EmployeeLibs e in field2)
                    {
                        totalpay += e.CalculatePay();
                    }
                    return totalpay;
                }
            }

            //Total Bonus
            public decimal TotalBonus
            {
                get
                {
                    decimal totalbonus = 0;
                    foreach (EmployeeLibs e in field2)
                    {
                        totalbonus += e.Bonus();
                    }
                    return totalbonus;
                }
            }

            //Total Deduction
            public decimal TotalDeduction
            {
                get
                {
                    decimal totaldeduction = 0;
                    foreach (EmployeeLibs e in field2)
                    {
                        totaldeduction += e.Deduction();
                    }
                    return totaldeduction;
                }
            }

            //Constructors
            public EmployeePayroll(DateTime f1, List<EmployeeLibs> f2)
            {
                field1 = f1;
                field2 = f2;
            }

            //Methods
            public List<string> ProcessPayroll()
            {
                List<string> strings = new List<string>(10);
                foreach (EmployeeLibs o in field2)
                {
                    strings.Add(string.Format("{0} {1}, ID#{2} -Net Pay: ${3} -Bonus: ${4} -Deductions: ${5}", o.FirstName, o.LastName, o.Sin.Substring(0, 4), o.CalculatePay(), o.Bonus(), o.Deduction()));
                }

                return strings;
            }
        }
    }
}
