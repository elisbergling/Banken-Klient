using System;
using System.Text;
using System.Net.Sockets;

namespace Banken_Klient
{
    internal class Program
    {
        static NetworkStream tcpStream = null!;

        public static void Main(string[] args)
        {
            const string address = "127.0.0.1";
            const int port = 8001;
            TcpClient tcpClient = new TcpClient();
            Connect(tcpClient, address, port);

            tcpStream = tcpClient.GetStream();

            Console.WriteLine("Välkommen till den här bank liknande applikationen");

            ChooseCostumerLoop(tcpClient);

            Console.WriteLine("Hejdå");
        }

        static void ChooseCostumerLoop(TcpClient tcpClient)
        {
            bool flag = true;

            while (flag)
            {
                Console.WriteLine("\nSkapa kund(1)");
                Console.WriteLine("Välj kund(2)");
                Console.WriteLine("Avsluta programmet(3)");
                int input = SecureInput(3);

                switch (input)
                {
                    case 1:
                        CreateCostumer();
                        break;
                    case 2:
                        if (ChooseCostumer())
                            flag = false;
                        break;
                    case 3:
                        End(tcpClient);
                        return;
                }
            }

            ChooseAccountLoop(tcpClient);
        }

        static void ChooseAccountLoop(TcpClient tcpClient)
        {
            bool flag = true;

            //Programloop
            while (flag)
            {
                Console.WriteLine("\nSkapa konto(1)");
                Console.WriteLine("Ta bort konto(2)");
                Console.WriteLine("Sätt in eller ta ut pengar(3)");
                Console.WriteLine("Visa konton(4)");
                Console.WriteLine("Byt kund(5)");
                Console.WriteLine("Ta bort mig som kund(6)");
                Console.WriteLine("Avsluta programmet(7)");
                int input = SecureInput(7);

                switch (input)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        DeleteAccount();
                        break;
                    case 3:
                        ChangeBalance();
                        break;
                    case 4:
                        ShowAccounts();
                        break;
                    case 5:
                        ChooseCostumerLoop(tcpClient);
                        flag = false;
                        break;
                    case 6:
                        DeleteCostumer();
                        ChooseCostumerLoop(tcpClient);
                        flag = false;
                        break;
                    case 7:
                        End(tcpClient);
                        flag = false;
                        break;
                }
            }
        }

        static void CreateCostumer()
        {
            string name = SecureInput2<string>("Vad heter du: ");
            Costumer c = new Costumer(name);

            byte[] cByte = c.ToByte("0");
            // Skicka iväg meddelandet:
            tcpStream.Write(cByte, 0, cByte.Length);
        }

        static bool ChooseCostumer()
        {
            MyList<Costumer>? costumers = ShowCostumers();
            if (costumers != null)
            {
                CurrentCostumer.Costumer = Check(costumers, "Vem vill du välja?");
                return true;
            }

            return false;
        }

        static void CreateAccount()
        {
            Account? a = null;
            string name = SecureInput2<string>("Vad vill du att ditt konto ska heta: ");
            int balance = SecureInput2<int>("Hur mycket pengar vill du ha på kontot: ");
            string sort;
            while (true)
            {
                try
                {
                    sort = SecureInput2<string>("Vill du skapa ett spar eller lönekonto (s/l): ");

                    //Kollar så att antingen s eller l är angivet
                    if (sort != "s" && sort != "S" && sort != "l" && sort != "L")
                        throw new Exception("Du måste ange antingen \"s\" eller \"l\"");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (sort == "s" || sort == "S")
            {
                int interest;
                while (true)
                {
                    try
                    {
                        interest = SecureInput2<int>("Vilken ränta vill du ha i procent: ");
                        //Ser till att räntan inte är högre än 10%
                        if (interest >= 10) throw new Exception("Räntan får inte vara 10% eller mer. Försök igen");
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                a = new SavingsAccount(balance, name, interest);
            }
            else if (sort == "l" || sort == "L")
            {
                int tax;
                while (true)
                {
                    try
                    {
                        tax = SecureInput2<int>("Vilken ränta vill du ha i procent: ");
                        //Ser till att skatten inte är lägre än 10%
                        if (tax <= 10) throw new Exception("Räntan får inte vara 10% eller mindre. Försök igen");
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                a = new SalaryAccount(balance, name, tax);
            }

            byte[] aByte = a!.ToByte("1");
            // Skicka iväg meddelandet:
            tcpStream.Write(aByte, 0, aByte.Length);
        }

        static void DeleteAccount()
        {
            MyList<Account>? accounts = ShowAccounts();
            if (accounts != null)
            {
                Account a = Check(accounts, "Vilket konto vill du ta bort");
                byte[] mByte = a.ToByte("3");
                // Skicka iväg meddelandet:
                tcpStream.Write(mByte, 0, mByte.Length);
            }
        }

        static void DeleteCostumer()
        {
            byte[] aByte = CurrentCostumer.Costumer!.ToByte("2");
            // Skicka iväg meddelandet:
            tcpStream.Write(aByte, 0, aByte.Length);
            CurrentCostumer.Costumer = null;
        }

        static void ChangeBalance()
        {
            MyList<Account>? accounts = ShowAccounts();
            if (accounts != null)
            {
                Account a = Check(accounts, "Vilket konto vill du ändra pengar på");
                int amount = SecureInput2<int>("Vad är den nya summan: ");
                a.ChangeBalance(amount);
                byte[] aByte = a.ToByte("6");
                // Skicka iväg meddelandet:
                tcpStream.Write(aByte, 0, aByte.Length);
            }
        }

        static MyList<Account>? ShowAccounts()
        {
            MyList<Account>? accounts = RetriveAccounts();
            if (accounts != null)
            {
                //Visar kontonen
                for (int i = 0; i < accounts.Count; i++)
                {
                    accounts[i].ShowAccount();
                }

                return accounts;
            }

            return null;
        }

        static MyList<Costumer>? ShowCostumers()
        {
            MyList<Costumer>? costumers = RetriveCostumers();

            if (costumers != null)
            {
                //Visar kunderna
                for (int i = 0; i < costumers.Count; i++)
                {
                    costumers[i].ShowCostumer();
                }

                return costumers;
            }


            return null;
        }

        public static string[] RetriveId()
        {
            byte[] mByte = Encoding.UTF8.GetBytes("7");
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            // Tag emot meddelande från servern:
            byte[] bRead = new byte[256];
            int bReadSize = tcpStream.Read(bRead, 0, bRead.Length);
            // Konvertera meddelandet till ett string-objekt och skriv ut:
            string read = "";
            for (int i = 0; i < bReadSize; i++)
                read += Convert.ToChar(bRead[i]);
            //Delar upp strängen
            char[] pattern = {'$'}; //$ används som skiljetecken
            string[] splitedString =
                read.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
            return splitedString;
        }

        static string[]? Retrive()
        {
            // Tag emot meddelande från servern:
            byte[] bRead = new byte[256];
            int bReadSize = tcpStream.Read(bRead, 0, bRead.Length);
            // Konvertera meddelandet till ett string-objekt och skriv ut:
            string read = "";
            for (int i = 0; i < bReadSize; i++)
                read += Convert.ToChar(bRead[i]);
            if (read == "0")
            {
                Console.WriteLine("\nDet fanns inget att hämta. Skapa något först.");
                return null;
            }

            //Delar upp strängen
            char[] pattern = {'$'}; //$ används som skiljetecken
            string[] splitedString =
                read.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
            return splitedString;
        }

        static MyList<Costumer>? RetriveCostumers()
        {
            byte[] mByte = Encoding.UTF8.GetBytes("4");
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);

            string[]? splitedString = Retrive();
            MyList<Costumer> costumers = new MyList<Costumer>();
            //Kolla ifall det finns meddelanden
            if (splitedString != null)
            {
                //Visar kontonen
                foreach (string split in splitedString)
                {
                    costumers.Add(new Costumer(split, null));
                }

                Console.WriteLine("\nKonton hämtades");
                return costumers;
            }

            return null;
        }

        static MyList<Account>? RetriveAccounts()
        {
            byte[] mByte = Encoding.UTF8.GetBytes("5" + "@" + CurrentCostumer.Costumer!.UserId);
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            //Hämta meddelandet
            string[]? splitedString = Retrive();
            MyList<Account> accounts = new MyList<Account>();
            //Kolla ifall det finns meddelanden
            if (splitedString != null)
            {
                //Visar kontonen
                foreach (var split in splitedString)
                {
                    if (split[3] < 10)
                    {
                        accounts.Add(new SavingsAccount(split));
                    }
                    else
                    {
                        accounts.Add(new SalaryAccount(split));
                    }
                }

                Console.WriteLine("\nKonton hämtades");
                return accounts;
            }

            return null;
        }

        static void End(TcpClient tcpClient)
        {
            byte[] mByte = Encoding.UTF8.GetBytes("8"); //8 används som meddelande för att avsluta Servern
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            // Stäng anslutningen:
            tcpClient.Close();
        }


        static void Connect(TcpClient tcpClient, string address, int port)
        {
            try
            {
                // Anslut till servern:
                Console.WriteLine("Ansluter...");
                tcpClient.Connect(address, port);
                Console.WriteLine("Ansluten!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Programmet stängs av. Hejdå");
                //Stänger av programmet ifall det inte finns någon tillgänglig server
                Environment.Exit(0);
            }
        }

        static int SecureInput(int max)
        {
            while (true)
            {
                try
                {
                    //Mata in en sträng från konsollen
                    Console.Write("\nVälj vad du vill göra: ");
                    string? input = Console.ReadLine();
                    int number;

                    //Se ifall strängen är koverterbar till int. Om den är det ges variablen number, inputs värde.
                    if (!int.TryParse(input, out number))
                        throw new NotANumberException();

                    //Se ifall nummret ligger inom rätt värden
                    if (number > max || number < 1)
                        throw new NumberOutOfBoundsException();

                    return number;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static T SecureInput2<T>(string text)
        {
            while (true)
            {
                try
                {
                    Console.Write(text);
                    string input = Console.ReadLine()!;

                    //Kollar ifall det är en int som ska matas in
                    if (typeof(T) == typeof(int))
                    {
                        int intInput;
                        if (!int.TryParse(input, out intInput)) throw new NotIntException();
                        return (T) (object) intInput;
                    }

                    if (input.Contains("@"))
                        throw new Exception("Snabel-a \"@\"tecknet för inte användas. Försök igen");


                    return (T) (object) input;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static T Check<T>(MyList<T> list, string text)
        {
            while (true)
            {
                try
                {
                    int id = SecureInput2<int>(text + " (skriv id-nummer): ");

                    T? a = default(T);

                    //Kollar ifall ett obekt med det givna id-nummret finns
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (((list[i] as Common)!).GetId() == id)
                        {
                            a = list[i];
                        }
                    }

                    //Kasta en exception ifall det angivna id-nummret inte finns
                    if (a == null) throw new Exception("Något med det id-nummeret verkar inte finnas. Försök igen");
                    return a;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}