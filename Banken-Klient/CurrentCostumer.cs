namespace Banken_Klient
{
    public class CurrentCostumer
    {
        static Costumer? costumer;

        public static Costumer? Costumer
        {
            get { return costumer; }
            set { costumer = value; }
        }
    }
}