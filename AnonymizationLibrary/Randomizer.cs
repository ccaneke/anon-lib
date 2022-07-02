/* Copyright (C) Interneuron, Inc - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Chukwuemezie Aneke <ccanekedev@gmail.com>, May 2020
 */

using System;
using System.Text;
namespace AnonymizationLibrary
{
    public class Randomizer
    {
        /**
         * Randomizer class that generates a random string for both number strings and a string of letters
         */

        private readonly string alphabets;
        private readonly Random randomNumberGenerator;
        private int length;
        public string randomizedString;
        private bool isNumberString;
        public string randomizedNumberString { get; set;} 
        public char randomizedInitial { get; set; }
        private readonly bool _anyLetter;
        private readonly string input;
        private readonly string decimalNumbers;
        //private string randomizedMrn;
        public string randomizedMrn { get; set; }

        public Randomizer(string str)
        {
        alphabets = "abcdefghijklmnopqrstuvwxyz";
            randomNumberGenerator = new Random();
            length = str.Length;
            randomizedString = "";
            isNumberString = validNumberString(str);
            randomizedInitial = ' '; // white space character
            input = str;
            decimalNumbers = "0123456789";
            this.randomizedMrn = "";
            //_anyLetter =
            //str[0] == 'A' || str[0] == 'B' || str[0] = 'C' || 'D' || 'E' || 'F' || 'G' || 'H' || 'I' || 'J' || 'K' || 'L' || 'M'
                                            //'N' || 'O' || 'P' || 'Q' || 'R' || 'S' || 'T' || 'V
        }

        //pri

        // Todo: When idnumber is a letter string, need to determine whether a letter string is an idnumber propery value or not
        // * length of idnumber when it starts with T is always 7
        // update, hospital number is the mrn
        // loop through MT00770 and anonymize first character using only alphabets, second character alphabets or numbers, the rest
        // just numbers

            private bool isMRn()
        {// if calling this method returns true then build a randomized mrn string in the format first character is a letter, second
            // character can be a letter or number, and the rest of the characters are numbers

            // ignore case
            string input = this.input.ToUpper(); // override global variable
            string alphabets = this.alphabets.ToUpper(); // override global variable

            bool isAnyCharAnAlphabet = false;
            bool isAnyCharANumber = false;

            for (int index = 0; index < alphabets.Length/*input.Length*/; index++)
            {
                // if string contains ateast one alphabet
                if (input.Contains(alphabets[index])/*this.alphabets.Contains(input[index])*/)
                {
                    isAnyCharAnAlphabet = true; // true means atleast one character is a letter
                }
            }

            for (int index = 0; index < this.decimalNumbers.Length; index+=1)
            {
                // if input string contains a decimal number, there is atleast one digit in the string
                if (this.input.Contains(this.decimalNumbers[index])) {
                    isAnyCharANumber = true;
                }
            }

            return isAnyCharANumber &&/*||*/ isAnyCharAnAlphabet; // assumes mrn contains both letters and numbers
        }

        private bool validNumberString(string input)
        {
            
            long number;
            //int number; // did not work because If the string contains nonnumeric characters or the numeric value is too large or too small
            // for the particular type you have specified, TryParse returns false and sets the out parameter to zero. Therefore used long
            // source: https://docs.microsoft.com/en-gb/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
            //Console.WriteLine("number: " + number);
            //Console.WriteLine(Int32.TryParse(input, out number));
            //Console.WriteLine("number is: " + number);

            return long.TryParse(input, out number);
        }

        public void shuffleCharacters()
        {
            if (this.length == 1) // i.e. a single character initial already exists
            {
                this.shuffleInitial();
                Console.WriteLine("an initial");
            } else if (isNumberString)
            {
                this.shuffleNumber();
                Console.WriteLine("number string");
            } else if (isMRn()) // when not a number string means that the string may contain letters and numbers or just letters
            {
                this.shuffleMRN();
            } else // when the string does not contain letters and numbers, therefore the string contains just letters
            {
                this.shuffleLetters();
                Console.WriteLine("letter string");

            }
        }

        private void shuffleInitial()
        {
                this.randomizedInitial = this.alphabets[this.randomNumberGenerator.Next(alphabets.Length)];   

        }

        private void shuffleLetters()
        {
            StringBuilder stringbuilder = new StringBuilder();

            for (int index = 0; index < length; index++)
            {
                stringbuilder.Append(this.alphabets[this.randomNumberGenerator.Next(0, this.alphabets.Length)]);
            }

            randomizedString = stringbuilder.ToString();
        }

        private void shuffleNumber()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int index = 0; index < this.length; index++)
            {
                stringBuilder.Append("" + this.randomNumberGenerator.Next(10)); // radix of 10 for the decimal numeral system
            }

            Console.WriteLine(stringBuilder);
            Console.WriteLine("saaa");

            this.randomizedNumberString = stringBuilder.ToString();
        }

        private void/*string*/ shuffleMRN()
        {
            StringBuilder strBuilder = new StringBuilder();
            string numbersAndLetters = new String(this.decimalNumbers + this.alphabets);
            for (int index = 0; index < this.input.Length; index++)
            {
                if (index == 0)
                {
                    strBuilder.Append(this.alphabets[randomNumberGenerator.Next(this.alphabets.Length)]);
                }
                else if (index == 1) // when index is not 0, if index is 1
                {
                    strBuilder.Append((numbersAndLetters)[randomNumberGenerator.Next(numbersAndLetters.Length)]);
                }
                else // for the rest of the index just append a number
                {
                    strBuilder.Append(this.decimalNumbers[this.randomNumberGenerator.Next(this.decimalNumbers.Length)]);
                }
            }

            // toUpper and toLower methods work on number strings, not just letter strings
            this.randomizedMrn = strBuilder.ToString().ToUpper();

            //return strBuilder.ToString();
        }
    }
}
