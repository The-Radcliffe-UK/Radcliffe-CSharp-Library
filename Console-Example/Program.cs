using Radcliffe.Console.Core;

namespace Console_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string phoneNumber = IO.MaskedInput("(00000) 000000", true);
            IO.Print(phoneNumber);
            System.Console.ReadLine();
        }
    }
}
