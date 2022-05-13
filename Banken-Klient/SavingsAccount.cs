using System;

namespace Banken_Klient
{
    public class SavingsAccount : Account
    {
        
        int interest;

        public int Interest
        {
            get { return interest; }
        }

        public SavingsAccount(string text) : base(text)
        {
            string[] splitedString = text.Split('@');
            interest = int.Parse(splitedString[3]);
        }

        public SavingsAccount(int balance, string name, int interest) : base(balance, name)
        {
            this.interest = interest;
        }

        public override void ShowAccount()
        {
            Console.WriteLine("--------- Sparkonto ----------");
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Saldo: $" + Balance);
            Console.WriteLine("Ränta: " + interest + "%");
            Console.WriteLine("------------------------------");
        }

        public override string ToString(string code)
        {
            return code + '@' + Name + '@' + Balance + '@' + ID + '@' + interest + '@' + CurrentCostumer.Costumer!.UserId;
        }
    }
}