using Radcliffe.ConsoleApp;

using System;
using System.Net;

namespace Console_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string phoneNumber = IO.MaskedInput("Phone Number (XXXXX) XXXXXX:", "(00000) 000000");
            IO.Print(phoneNumber);
            string ipAddress = IO.MaskedInput("IP Address xxx.xxx.xxx.xxx:", "000.000.000.000", CheckIP);
            IO.Print( ipAddress );
            string sortCode = IO.MaskedInput("Sort Code xx-xx-xx:", "00-00-00");
            IO.Print( sortCode );
            string postCode = IO.MaskedInput("Post Code LL00 0LL:", "LL00 0LL", null, null, true);
            IO.Print( postCode );
            Console.ReadLine();
        }

        public static bool CheckIP(string ipString)
        {
            return IPAddress.TryParse( ipString, out IPAddress? ipAddress);
        }
    }
}
