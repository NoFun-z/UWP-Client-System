using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Loc Pham PROG1224 Lab 3 2022/09/09
namespace EmployeeLib
{
    public abstract class EmployeeLibs : IDeduction
    {
        public const string COMPANY = "The Kat Company";
        //Fields
        private string sin;
        private string first;
        private string last;
        private DateTime hire;
        private string email;
        private string phone;
        private Address address;
        private bool isActive;

        //Properties
        public string Sin
        {
            get { return sin; }
            set
            {
                if (value.Length == 9)
                {
                    sin = value;
                }
                else
                {
                    throw new Exception("sin must have 9 digits");
                }
            }
        }

        public string FirstName
        {
            get { return first; }
            set
            {
                if (value != "")
                {
                    first = value;
                }
                else
                {
                    throw new Exception("Invalid firstname");
                }
            }
        }

        public string LastName
        {
            get { return last; }
            set
            {
                if (value != "")
                {
                    last = value;
                }
                else
                {
                    throw new Exception("Invalid lastname");
                }
            }
        }

        public DateTime HireDate
        {
            get { return hire; }
            set
            {
                if (value.Year == DateTime.Now.Year && value.Month < DateTime.Now.Month || value.Year < DateTime.Now.Year)
                {
                    hire = value;
                }
                else
                {
                    throw new Exception("hiring date cannot exceed the current time");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }


        //Constructors
        public EmployeeLibs(string s)
        {
            Sin = s;
        }

        public EmployeeLibs(string s, string f, string l)
        {
            Sin = s;
            FirstName = f;
            LastName = l;
        }

        public EmployeeLibs(string s, string f, string l, DateTime hireddate)
        {
            Sin = s;
            FirstName = f;
            LastName = l;
            HireDate = hireddate;
        }

        public EmployeeLibs(string s, string f, string l, DateTime hireddate, Address Add, bool Active)
        {
            Sin = s;
            FirstName = f;
            LastName = l;
            HireDate = hireddate;
            address = Add;
            IsActive = Active;
        }

        public EmployeeLibs(string s, string f, string l, DateTime h, string e, string p, Address Add, bool Active)
        {
            Sin = s;
            FirstName = f;
            LastName = l;
            HireDate = h;
            Email = e;
            Phone = p;
            Address = Add;
            IsActive = Active;
        }

        //Methods
        public override string ToString()
        {
            return string.Format("Employee Full Name: {0} {1}", first, last);
        }

        public abstract decimal CalculatePay();

        public virtual decimal Bonus()
        {
            return 0;
        }

        public decimal IncomeTax()
        {
            decimal result = 0;
            decimal calculatepay = CalculatePay();
            if (calculatepay < 1800)
            {
                return result + calculatepay * Convert.ToDecimal(0.15);
            }
            else if (calculatepay < 3800 && calculatepay > 1799)
            {
                return result + calculatepay * Convert.ToDecimal(0.2);
            }
            else if (calculatepay > 3799 && calculatepay < 5800)
            {
                return result + calculatepay * Convert.ToDecimal(0.26);
            }
            else if (calculatepay > 8200)
            {
                return result + calculatepay * Convert.ToDecimal(0.33);
            }

            return result;
        }

        public virtual decimal Pension()
        {
            return 0;
        }

        public virtual decimal UnionDues()
        {
            return 0;
        }

        public virtual decimal Insurance()
        {
            return 0;
        }

        public decimal Deduction()
        {
            return 12950 / 12;
        }
    }


    //Hourly sealed class
    public sealed class Hourly : EmployeeLibs
    {
        //Fields
        private double rate;
        private double hours;

        //Delegate
        public delegate void DelegateMethod();
        //Event
        public event DelegateMethod HourlyDelegateEvent;

        //Properties
        public double Rate
        {
            get { return rate; }
            set
            {
                if (value > 0)
                {
                    rate = value;
                }
                else
                {
                    throw new Exception("Rate must be greater than 0");
                }
            }
        }

        public double Hours
        {
            get { return hours; }
            set
            {
                if (value > 0)
                {
                    hours = value;
                }
                else
                {
                    throw new Exception("hours must be greater than 0");
                }
            }
        }

        //Constructor
        public Hourly(string sin) : base(sin) { }

        public Hourly(string sin, string first, string last) : base(sin, first, last) { }

        public Hourly(string sin, string first, string last, DateTime hireddate) : base(sin, first, last, hireddate) { }

        public Hourly(string sin, string first, string last, DateTime hireddate, Address add, bool active) : base(sin, first, last, hireddate, add, active) { }

        public Hourly(string sin, string first, string last, DateTime hireddate, double r, double h) : base(sin, first, last, hireddate)
        {
            Rate = r;
            Hours = h;
        }

        public Hourly(string sin, string first, string last, DateTime hireddate, Address add, bool active, double r, double h) : base(sin, first, last, hireddate, add, active)
        {
            Rate = r;
            Hours = h;
        }

        //Methods
        public override string ToString()
        {
            return base.ToString() + string.Format("\nRate: ${0}\nHours: {1}", rate, hours);
        }

        public override decimal Bonus()
        {
            return base.Bonus() + Math.Round(Convert.ToDecimal(hours * rate * 4 * 0.05), 2);
        }

        public override decimal CalculatePay()
        {
            if (Convert.ToDecimal(hours * rate * 2) + (Bonus() / 2) > 5500)
            {
                HourlyDelegateEvent();
                return 0;
            }
            else
                return Convert.ToDecimal(hours * rate * 2) + (Bonus() / 2);
        }

        public override decimal Pension()
        {
            return (CalculatePay() * 26 - 55420) + Convert.ToDecimal(55420 /100 * 1.4 * 10); //https://www.pspp.ca/page/how-your-pension-is-calculated This is how I calculated pension
        }
    }


    //Derived class Salaried
    public class Salaried : EmployeeLibs
    {
        //Fields
        private decimal amount;

        //Delegate
        public delegate void DelegateMethod();
        //Event
        public event DelegateMethod SalariedDelegateEvent;

        //Properties
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (value > 0)
                {
                    amount = value;
                }
                else
                {
                    throw new Exception("Annual wage must be greater than 0");
                }
            }
        }

        //Constructor
        public Salaried(string sin) : base(sin) { }

        public Salaried(string sin, string first, string last) : base(sin, first, last) { }

        public Salaried(string sin, string first, string last, DateTime hireddate) : base(sin, first, last, hireddate) { }

        public Salaried(string sin, string first, string last, DateTime hireddate, Address add, bool active) : base(sin, first, last, hireddate, add, active) { }

        public Salaried(string sin, string first, string last, DateTime hireddate, decimal a) : base(sin, first, last, hireddate)
        {
            Amount = a;
        }

        public Salaried(string sin, string first, string last, DateTime hireddate, Address add, bool active, decimal a) : base(sin, first, last, hireddate, add, active)
        {
            Amount = a;
        }

        //Methods
        public override string ToString()
        {
            return base.ToString() + string.Format("\nAnnual Wage: ${0}", amount);
        }

        public override decimal CalculatePay()
        {
            if (Math.Round(Convert.ToDecimal(amount / 26), 2) > 980000)
            {
                SalariedDelegateEvent();
                return 0;
            }
            else
                return Math.Round(Convert.ToDecimal(amount / 26), 2);
        }

        public override decimal Pension()
        {
            return (amount - 55420) + Convert.ToDecimal(55420 / 100 * 1.4 * 10);
        }
    }


    //Struct Address
    public struct Address
    {
        //Fields
        private string street;
        private string city;
        private string province;
        private string postal_code;

        //Properties
        public string Street
        {
            get { return street; }
            set
            {
                if (value != "")
                {
                    street = value;
                }
                else
                {
                    throw new Exception("Invalid street input");
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value != "")
                {
                    city = value;
                }
                else
                {
                    throw new Exception("Invalid city input");
                }
            }
        }

        public string Province
        {
            get { return province; }
            set
            {
                if (value != "")
                {
                    province = value;
                }
                else
                {
                    throw new Exception("Invalid province input");
                }
            }
        }

        public string Postal_Code
        {
            get { return postal_code; }
            set
            {
                if (value != "")
                {
                    postal_code = value;
                }
                else
                {
                    throw new Exception("Invalid postal code input");
                }
            }
        }

        //Constructor
        public Address(string s, string c, string p, string pos) : this()
        {
            Street = s;
            City = c;
            Province = p;
            Postal_Code = pos;
        }

        //Method
        public override string ToString()
        {
            return string.Format("Address: {0}, {1}, {2} - {3}", Street, City, Province, Postal_Code);
        }
    }


    //Base Employee derived class Manager
    public class Manager : EmployeeLibs
    {
        //Fields
        private decimal basic_salary;
        private decimal salary_allowances;
        private double work_volume;

        //Delegate
        public delegate void DelegateMethod();
        //Event
        public event DelegateMethod ManagerDelegateEvent;

        //Properties
        public decimal Basic_Salary
        {
            get { return basic_salary; }
            set
            {
                if (value > 0)
                {
                    basic_salary = value;
                }
                else
                {
                    throw new Exception("Salary must be greater than 0");
                }
            }
        }

        public decimal Salary_Allowances
        {
            get { return salary_allowances; }
            set
            {
                if (value >= 0)
                {
                    salary_allowances = value;
                }
                else
                {
                    throw new Exception("Salary allowances must be equal or greater than 0");
                }
            }
        }

        public double Work_Volume
        {
            get { return work_volume; }
            set
            {
                if (value > 0)
                {
                    work_volume = value;
                }
                else
                {
                    throw new Exception("work volume must be greater than 0");
                }
            }
        }

        //Constructors
        public Manager(string sin) : base(sin) { }

        public Manager(string sin, string first, string last) : base(sin, first, last) { }

        public Manager(string sin, string first, string last, DateTime hireddate) : base(sin, first, last, hireddate) { }

        public Manager(string sin, string first, string last, DateTime hireddate, Address add, bool active) : base(sin, first, last, hireddate, add, active) { }

        public Manager(string sin, string first, string last, DateTime hireddate, decimal bs, decimal sa, double wv) : base(sin, first, last, hireddate)
        {
            Basic_Salary = bs;
            Salary_Allowances = sa;
            Work_Volume = wv;
        }

        public Manager(string sin, string first, string last, DateTime hireddate, Address add, bool active, decimal bs, decimal sa, double wv) : base(sin, first, last, hireddate, add, active)
        {
            Basic_Salary = bs;
            Salary_Allowances = sa;
            Work_Volume = wv;
        }

        //Methods
        public override string ToString()
        {
            return base.ToString() + string.Format("\nMonthly Salary: ${0}\nSalary Allowances: ${1}\nWork Volume (Dates): {2}", basic_salary, salary_allowances, work_volume);
        }

        public override decimal CalculatePay()
        {
            if (Math.Round((basic_salary + salary_allowances) / 26 * Convert.ToDecimal(work_volume / 2), 2) > 9500)
            {
                ManagerDelegateEvent();
                return 0;
            }
            else
                return Math.Round((basic_salary + salary_allowances) / 26 * Convert.ToDecimal(work_volume / 2), 2); //Total pay for manager per month = sum of wage and allowances / 26 days of the month and * the realistic dates of work
        }

        public override decimal Insurance()
        {
            return Convert.ToDecimal(469.20);
        }
    }


    //Public Salaried derived class Sales
    public sealed class Sales : Salaried
    {
        //Fields
        private decimal total_sales;
        private decimal expenses;

        //Delegate
        public delegate void SalesDelegateMethod();
        //Event
        public event SalesDelegateMethod SalesDelegateEvent;

        //Properties
        public decimal Total_Sales
        {
            get { return total_sales; }
            set
            {
                if (value > 0)
                {
                    total_sales = value;
                }
                else
                {
                    throw new Exception("sales must be greater than 0");
                }
            }
        }

        public decimal Expenses
        {
            get { return expenses; }
            set
            {
                if (value > 0)
                {
                    expenses = value;
                }
                else
                {
                    throw new Exception("expenses must be greater than 0");
                }
            }
        }

        //Constructors
        public Sales(string sin, string first, string last, DateTime hireddate) : base(sin, first, last, hireddate) { }

        public Sales(string sin, string first, string last, DateTime hireddate, Address add, bool active) : base(sin, first, last, hireddate, add, active) { }

        public Sales(string sin, string first, string last, DateTime hireddate, decimal a) : base(sin, first, last, hireddate, a) { }

        public Sales(string sin, string first, string last, DateTime hireddate, Address add, bool active, decimal a) : base(sin, first, last, hireddate, add, active, a) { }

        public Sales(string sin, string first, string last, DateTime hireddate, decimal a, decimal s, decimal e) : base(sin, first, last, hireddate, a)
        {
            Total_Sales = s;
            Expenses = e;
        }

        public Sales(string sin, string first, string last, DateTime hireddate, Address add, bool active, decimal a, decimal s, decimal e) : base(sin, first, last, hireddate, add, active, a)
        {
            Total_Sales = s;
            Expenses = e;
        }

        //Methods
        public override string ToString()
        {
            return base.ToString() + string.Format("\nTotal sales: ${0}\nExpenses: ${1}", total_sales, expenses);
        }

        public override decimal CalculatePay()
        {
            return Math.Round(base.CalculatePay() + ((total_sales - expenses) * Convert.ToDecimal(0.02) / 26) + Bonus(), 2);//Total pay of sales in 1 year = annual wage + 2% of total sales
        }

        public override decimal Bonus()
        {
            if (Amount / 26 * Convert.ToDecimal(0.02) > 1200)
            {
                SalesDelegateEvent();
                return 0;
            }
            else
                return base.Bonus() + Amount / 26 * Convert.ToDecimal(0.02);
        }

        public override decimal UnionDues()
        {
            return Math.Round(CalculatePay() /100 * Convert.ToDecimal(1.55), 2);
        }
    }

    //Interface
    public interface IDeduction
    {
        decimal IncomeTax();
        decimal Pension();
        decimal UnionDues();
        decimal Insurance();
    }
}
