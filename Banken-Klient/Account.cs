using System.Text;

namespace Banken_Klient
{
    public abstract class Account : Common
    {

        string name;
        int balance;
        int id;

        public int ID
        {
            get { return id; }
        }

        public int Balance
        {
            get { return balance; }
        }

        public string Name
        {
            get { return name; }
        }

        public Account(string text)
        {
            string[] splitedString = text.Split('@');

            name = splitedString[0];
            balance = int.Parse(splitedString[1]);
            id = int.Parse(splitedString[2]);
        }

        public Account(int balance, string name)
        {
            this.name = name;
            this.balance = balance;
            id = int.Parse(Program.RetriveId()![0]) + 1;

        }

        public Account(int balance, int? _, int id)
        {
            name = "";
            this.balance = balance;
            this.id = id;
        }
        
        public void ChangeBalance(int balance)
        {
            this.balance = balance;
        }

        public abstract void ShowAccount();

        public abstract string ToString(string code);

        public override byte[] ToByte(string code)
        {
            return Encoding.UTF8.GetBytes(ToString(code));
        }

        public override int GetId()
        {
            return id;
        }
    }
}