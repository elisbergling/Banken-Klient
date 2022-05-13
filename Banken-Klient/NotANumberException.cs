using System;

namespace Banken_Klient
{
    public class NotANumberException : Exception
    {
        string message;

        public override string Message
        {
            get { return message; }
        }

        public NotANumberException()
        {
            message = "Det där är ingen siffra. Försök igen.";
        }
    }
}