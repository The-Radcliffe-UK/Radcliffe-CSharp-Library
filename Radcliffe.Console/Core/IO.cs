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
    }
}
