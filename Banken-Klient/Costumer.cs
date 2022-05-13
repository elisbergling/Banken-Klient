using System;
using System.Linq;
using System.Text;


namespace Banken_Klient
{
    public class Costumer : Common
    {
        string name;
        int userId;
        MyList<Account> accounts;
        
        public string Name
        {
            get { return name; }
        }

        public int UserId
        {
            get { return userId; }
        }

        public MyList<Account> Accounts
        {
            get { return accounts; }
        }

        public Costumer(string text, int? _)
        {
            //Delar upp strängen och tilldelar dess värden
            string[] splitedString = text.Split('@');

            name = splitedString[0];
            userId = int.Parse(splitedString[1]);
            accounts = new MyList<Account>();
        }

        public Costumer(string name)
        {
            this.name = name;
            //Hämtar tidigare högsta id-nummer och lägger till 1
            userId = int.Parse(Program.RetriveId()![1]) + 1;
            accounts = new MyList<Account>();
        }

        public void ShowCostumer()
        {
            Console.WriteLine("---------" + name + "----------");
            Console.WriteLine("ID:" + userId);
            Console.WriteLine("---------" + String.Concat(Enumerable.Repeat("-", name.Length)) + "----------");
        }


        public string ToString(string code)
        {
            return code + '@' + name + '@' + userId;
        }

        public override byte[] ToByte(string code)
        {
            return Encoding.UTF8.GetBytes(ToString(code));
        }

        public override int GetId()
        {
            return userId;
        }
    }
}