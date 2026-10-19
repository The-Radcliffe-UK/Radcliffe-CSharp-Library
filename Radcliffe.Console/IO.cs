using Radcliffe.Core.Data;
using Radcliffe.Core.EventSystem;

using System;
namespace Radcliffe.ConsoleApp
{
    public class IO
    {
        public static void Print(string msg, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine( msg );
            } else
            {
                Console.Write( msg );
            }
        }

        public string? Input(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        public static int? IntInput(string msg)
        {
            Console.Write(msg);
            int? returnValue = null;

            string? inputValue = Console.ReadLine();
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
            Console.Write(msg);
            float? returnValue = null;

            string? inputValue = Console.ReadLine();
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

        public static string HiddenInput(string msg, bool hideInput = true, char coverChar = '*')
        {
            Console.Write( msg );
            if(!msg.EndsWith(' ' ))
            {
                Console.Write( " " );
            }
            
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey( intercept: true );

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring( 0, password.Length - 1 );
                    if (hideInput)
                    {
                        Console.Write( "\b \b" ); // Remove last asterisk
                    }
                } else if (!char.IsControl( key.KeyChar ))
                {
                    password += key.KeyChar;
                    if (hideInput)
                    {
                       Console.Write( coverChar );
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        public static string MaskedInput(string msg, string mask, Func<string, bool>? validation = null, Event<string>? validationError = null, bool forceUpper = false, bool forceLower = false)
        {
            Console.Write( msg );
            if (!msg.EndsWith( ' ' ))
            {
                Console.Write( " " );
            }

            InputMask returnValue = new (mask, true, forceUpper, forceLower);
            string currentInput = "";
            int currentCharacter = 0;
            ConsoleKeyInfo key;
            bool complete = false;

            bool checkLiterals = true;
            while (checkLiterals)
            {
                if (returnValue.IsLiteralAtPosition(currentCharacter, out char nextChar))
                {
                    Console.Write(nextChar);
                    currentInput += nextChar;
                    currentCharacter++;
                }
                else
                {
                    checkLiterals = false;
                }
            }

            do
            {
                key = Console.ReadKey( intercept: true );

                if (key.Key == ConsoleKey.Backspace && currentInput.Length > 0 && currentCharacter > 0)
                {
                    currentInput = currentInput.Substring( 0, currentInput.Length - 1 );
                    Console.Write( "\b \b" ); // Remove last character
                    currentCharacter--;
                    checkLiterals = true && currentCharacter > 0;
                    while (checkLiterals)
                    {
                        if (returnValue.IsLiteralAtPosition( currentCharacter, out char nextChar ))
                        {
                            currentInput = currentInput.Substring( 0, currentInput.Length - 1 );
                            Console.Write( "\b \b" ); // Remove last character
                            currentCharacter--;
                        } else
                        {
                            checkLiterals = false;
                        }
                    }
                    checkLiterals = true;
                    while (checkLiterals)
                    {
                        if (returnValue.IsLiteralAtPosition( currentCharacter, out char nextChar ))
                        {
                            Console.Write( nextChar );
                            currentInput += nextChar;
                            currentCharacter++;
                        } else
                        {
                            checkLiterals = false;
                        }
                    }
                } else if (!char.IsControl( key.KeyChar ))
                {
                    char input = key.KeyChar;
                    bool valid = returnValue.TryFormatCharAtPosition(input, currentCharacter, out char formatedInput);
                    if (valid)
                    {
                        Console.Write( formatedInput );
                        currentInput += formatedInput;
                        currentCharacter++;
                        checkLiterals = true;
                        while (checkLiterals)
                        {
                            if (returnValue.IsLiteralAtPosition( currentCharacter, out char nextChar ))
                            {
                                Console.Write( nextChar );
                                currentInput += nextChar;
                                currentCharacter++;
                            } else
                            {
                                checkLiterals = false;
                            }
                        }
                    } else
                    {
                        Console.Beep();
                    }
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    bool valid = validation == null || validation(currentInput);
                    complete = currentInput.Length == returnValue.mask.Length && valid;
                    if (!complete)
                    {
                        Console.Beep();
                        if(validationError != null)
                        {
                            if(currentInput.Length != returnValue.mask.Length)
                            {
                                validationError.Raise( "Not Enough Characters Entered" );
                            }else if (!valid)
                            {
                                validationError.Raise( "Invalid Data Entered" );
                            } else
                            {
                                validationError.Raise( "Input Error, please try again" );
                            }
                        }
                    }
                }
            } while (!complete);
            
            Console.WriteLine();
            return currentInput;
        }
    }
}