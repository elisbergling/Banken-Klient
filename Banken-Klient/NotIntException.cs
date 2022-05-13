using System;

namespace Banken_Klient
{
    public class NotIntException : Exception
    {
        string message;

        public override string Message
        {
            get { return message; }
        }

        public NotIntException()
        {
            message = "Det måste vara en siffra. Försök Igen.";
        }
    }
}