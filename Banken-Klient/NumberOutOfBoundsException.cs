using System;

namespace Banken_Klient
{
    public class NumberOutOfBoundsException : Exception
    {
        string message;

        public override string Message
        {
            get { return message; }
        }

        public NumberOutOfBoundsException()
        {
            message = "Nummret är antingen för stort eller för litet. Försök Igen.";
        }
    }
}