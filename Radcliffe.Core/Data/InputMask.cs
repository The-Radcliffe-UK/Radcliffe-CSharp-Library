using System;
using System.Text;
using System.Threading.Tasks;

namespace Radcliffe.Core.Data {
    /// <summary>
    /// Provides input validation and formatting using a mask pattern inspired by Microsoft Access-style masks.
    /// Supports flexible input formats, strict vs non-strict parsing, and casing enforcement.
    ///
    /// <para>
    /// Mask Syntax:
    /// Each character in the mask defines a rule or literal:
    /// <list type="table">
    /// <item><term>L</term><description>Letter (A-Z, a-z) required.</description></item>
    /// <item><term>?</term><description>Letter (A-Z, a-z) optional or space.</description></item>
    /// <item><term>0</term><description>Digit (0-9) required.</description></item>
    /// <item><term>9</term><description>Digit (0-9) optional or space.</description></item>
    /// <item><term>#</term><description>Digit, space, plus or minus sign allowed.</description></item>
    /// <item><term>A</term><description>Alphanumeric (letter or digit) required.</description></item>
    /// <item><term>a</term><description>Alphanumeric optional or space.</description></item>
    /// <item><term>C</term><description>Any character, including space. Optional.</description></item>
    /// <item><term>Other characters</term><description>Are treated as literals and must match the input unless strict mode is disabled.</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Example usage:
    /// <code>
    /// var mask = new InputMask("LL00 0LL", strict: false, forceUpper: true);
    /// if (mask.TryFormat("ab122ab", out var result))
    ///     Console.WriteLine(result); // Output: AB12 2AB
    /// </code>
    /// </para>
    /// </summary>
    public class InputMask {
        public string Mask { get; }
        public bool Strict { get; }
        public bool ForceUpper { get; }
        public bool ForceLower { get; }

        public InputMask(string mask, bool strict = false, bool forceUpper = false, bool forceLower = false)
        {
            Mask = mask;
            Strict = strict;
            ForceUpper = forceUpper;
            ForceLower = forceLower;

            if (ForceUpper && ForceLower)
            {
                throw new ArgumentException( "Cannot force both upper and lower casing." );
            }
        }

        /// <summary>
        /// The total length of the input including literal characters.
        /// This is the full length of the formatted string.
        /// </summary>
        public int FormattedLength => Mask.Length;

        /// <summary>
        /// The total length of the input *excluding* literals.
        /// This counts only required and optional data characters.
        /// </summary>
        //public int UnformattedLength => Mask.Count(c =>c is 'L' or '?' or '0' or '9' or '#' or 'A' or 'a' or 'C' );

        /// <summary>
        /// Attempts to validate and format a single character at a specific position in the mask.
        /// </summary>
        /// <param name="inputChar">The character to test.</param>
        /// <param name="position">The position in the mask.</param>
        /// <param name="formattedChar">Outputs the correctly formatted character, e.g. uppercased.</param>
        /// <returns>True if valid for the position; false otherwise.</returns>
        public bool TryFormatCharAtPosition(char inputChar, int position, out char formattedChar)
        {
            formattedChar = inputChar;

            if (position < 0 || position >= Mask.Length)
            {
                return false;
            }

            char maskChar = Mask[position];

            if (!IsMaskChar( maskChar ))
            {
                // Must match literal if strict; else accept and just move on
                bool valid = (!Strict || inputChar == maskChar);
                formattedChar = maskChar;
                return valid;
            }

            if (!IsValidCharForMask( maskChar, inputChar ))
            {
                return false;
            }

            formattedChar = FormatChar( inputChar );
            return true;
        }

        /// <summary>
        /// Attempts to validate and format the input based on the mask.
        /// </summary>
        public bool TryFormat(string input, out string formatted)
        {
            formatted = string.Empty;
            if (string.IsNullOrEmpty( input ))
            {
                return false;
            }

            StringBuilder output = new();
            int inputIndex = 0;

            for (int maskIndex = 0; maskIndex < Mask.Length; maskIndex++)
            {
                if (inputIndex >= input.Length && IsMaskChar( Mask[maskIndex] ))
                {
                    return false; // input ended too soon
                }

                char maskChar = Mask[maskIndex];

                if (IsMaskChar( maskChar ))
                {
                    // Skip non-mask characters in input
                    while (inputIndex < input.Length && !IsValidCharForMask( maskChar, input[inputIndex] ))
                    {
                        if (Strict)
                        {
                            return false;
                        }

                        inputIndex++;
                    }

                    if (inputIndex >= input.Length)
                    {
                        return false;
                    }

                    char inputChar = input[inputIndex++];

                    if (!IsValidCharForMask( maskChar, inputChar ))
                    {
                        return false;
                    }

                    output.Append( FormatChar( inputChar ) );
                } else
                {
                    // Literal character
                    if (Strict)
                    {
                        if (inputIndex >= input.Length || input[inputIndex] != maskChar)
                        {
                            return false;
                        }

                        inputIndex++;
                    } else
                    {
                        // Try to skip matching literal in input if it's there
                        if (inputIndex < input.Length && input[inputIndex] == maskChar)
                        {
                            inputIndex++;
                        }
                    }

                    output.Append( maskChar );
                }
            }

            // If we’ve got leftover input, reject
            if (inputIndex < input.Length)
            {
                return false;
            }

            formatted = output.ToString();
            return true;
        }

        /// <summary>
        /// Validates input without returning formatted output.
        /// </summary>
        public bool Validate(string input) => TryFormat( input, out _ );

        /// <summary>
        /// Helper: Checks if a character is a mask symbol or a literal.
        /// </summary>
        private bool IsMaskChar(char c) => "L?0#9AaC".Contains( c );

        /// <summary>
        /// Helper: Validates a character against a mask symbol.
        /// </summary>
        private bool IsValidCharForMask(char mask, char input)
        {
            return mask switch {
                'L' => char.IsLetter( input ),
                '?' => input == ' ' || char.IsLetter( input ),
                '0' => char.IsDigit( input ),
                '9' => input == ' ' || char.IsDigit( input ),
                '#' => input == ' ' || char.IsDigit( input ) || input == '+' || input == '-',
                'A' => char.IsLetterOrDigit( input ),
                'a' => input == ' ' || char.IsLetterOrDigit( input ),
                'C' => true, // any char
                _ => false
            };
        }

        /// <summary>
        /// Formats character based on casing rules.
        /// </summary>
        private char FormatChar(char c)
        {
            if (ForceUpper)
            {
                return char.ToUpper( c );
            }

            if (ForceLower)
            {
                return char.ToLower( c );
            }

            return c;
        }
    }
}