using System;
using System.Text.RegularExpressions;

namespace Radcliffe.Console.Core
{
    public class IO
    {
        public static void Print(string msg)
        {
            System.Console.WriteLine(msg);
        }

        public string? Input(string msg)
        {
            System.Console.Write(msg);
            return System.Console.ReadLine();
        }

        public static int? IntInput(string msg)
        {
            System.Console.Write(msg);
            int? returnValue = null;

            string? inputValue = System.Console.ReadLine();
            if(inputValue == null)
            {
                returnValue = null;
            }
            else
            {
                if(int.TryParse(inputValue, out int value))
                {
                    returnValue = value;
                }
                else
                {
                    returnValue = null;
                }
            }

            return returnValue;
        }

        public static bool IntRangeCheck(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }

        public static float? FloatInput(string msg)
        {
            System.Console.Write(msg);
            float? returnValue = null;

            string? inputValue = System.Console.ReadLine();
            if (inputValue == null)
            {
                returnValue = null;
            }
            else
            {
                if (float.TryParse(inputValue, out float value))
                {
                    returnValue = value;
                }
                else
                {
                    returnValue = null;
                }
            }

            return returnValue;
        }

        public static bool FloatRangeCheck(float value, float min, float max)
        {
            return (value >= min) && (value <= max);
        }

        public static string PasswordInput(bool maskInput = true)
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = System.Console.ReadKey( intercept: true );

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring( 0, password.Length - 1 );
                    if (maskInput)
                    {
                        System.Console.Write( "\b \b" ); // Remove last asterisk
                    }
                } else if (!char.IsControl( key.KeyChar ))
                {
                    password += key.KeyChar;
                    if (maskInput)
                    {
                        System.Console.Write( "*" );
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            System.Console.WriteLine();
            return password;
        }
    }
}
