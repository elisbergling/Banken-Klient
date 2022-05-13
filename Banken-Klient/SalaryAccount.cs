using System;

namespace Banken_Klient
{
    public class SalaryAccount : Account
    {
        int tax;

        public int Tax
        {
            get { return tax; }
        }

        public SalaryAccount(string text) : base(text)
        {
            string[] splitedString = text.Split('@');
            tax = int.Parse(splitedString[3]);
        }

        public SalaryAccount(int balance, string name, int tax) : base(balance, name)
        {
            this.tax = tax;
        }
   
        public override void ShowAccount()
        {
            Console.WriteLine("--------- Lönekonto ----------");
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Saldo: $" + Balance);
            Console.WriteLine("Skatt: " + tax + "%");
            Console.WriteLine("------------------------------");
        }

        public override string ToString(string code)
        {
            return code + '@' + Name + '@' + Balance + '@' + ID + '@' + tax + '@' + CurrentCostumer.Costumer!.UserId;

        }
    }
}