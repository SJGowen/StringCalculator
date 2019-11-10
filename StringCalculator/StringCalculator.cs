using System;
using System.Collections.Generic;

namespace StringCalculator
{
    public class StringCalculator
    {
        public int CalledCount { get; private set; }

        private string Separator { get; set; } = ",";

        public event EventHandler<StringCalculatorEventArgs> AdditionComplete;

        public int Add(string stringNumbers)
        {
            if (stringNumbers == string.Empty) return 0;

            stringNumbers = CheckForDefinedSeparators(stringNumbers);

            stringNumbers = stringNumbers.Replace("\n", Separator);

            var stringArray = stringNumbers.Split(Separator);

            ThrowExceptionIfListContainsNegatives(stringArray);

            return AddNumbers(stringArray);
        }

        private string CheckForDefinedSeparators(string stringNumbers)
        {
            if (stringNumbers[0] == '/' && stringNumbers[1] == '/' && stringNumbers[2] == '[')
            {
                var definedSeparators = GetDefinedSeparators(stringNumbers);
                stringNumbers = stringNumbers.Substring(stringNumbers.IndexOf('\n') + 1);
                foreach (var definedSeparator in definedSeparators)
                {
                    stringNumbers = stringNumbers.Replace(definedSeparator, Separator);
                }
            }
            else if (stringNumbers[0] == '/' && stringNumbers[1] == '/' && stringNumbers[3] == '\n')
            {
                Separator = stringNumbers[2].ToString();
                stringNumbers = stringNumbers.Substring(4);
            }

            return stringNumbers;
        }

        private static IEnumerable<string> GetDefinedSeparators(string stringNumbers)
        {
            var separators = new List<string>();
            var charCount = 2;
            while (stringNumbers[charCount] != '\n')
            {
                if (stringNumbers[charCount] != '[')
                {
                    var separator = string.Empty;
                    while (stringNumbers[charCount] != ']')
                    {
                        separator += stringNumbers[charCount];
                        charCount++;
                    }

                    separators.Add(separator);
                }

                charCount++;
            }

            return separators;
        }

        private static void ThrowExceptionIfListContainsNegatives(IEnumerable<string> numberList)
        {
            var negativeNumbers = new List<string>();

            foreach (var number in numberList)
            {
                if (int.Parse(number) < 0)
                {
                    negativeNumbers.Add(number);
                }
            }

            if (negativeNumbers.Count > 0)
            {
                throw new ArgumentException($"negatives not allowed - ({string.Join(" & ", negativeNumbers)})");
            }
        }

        private int AddNumbers(IEnumerable<string> collection)
        {
            var result = 0;
            var numbersAsString = string.Empty;
            CalledCount = 0;

            foreach (var item in collection)
            {
                if (int.TryParse(item, out var intItem) && (intItem <= 1000))
                {
                    result += intItem;
                    CalledCount++;
                    numbersAsString = AppendCommaIfNotEmpty(numbersAsString);
                    numbersAsString += item;
                }
            }

            OnAdditionComplete(numbersAsString, result);
            return result;
        }

        private static string AppendCommaIfNotEmpty(string numbersAsString)
        {
            if (numbersAsString.Length != 0)
            {
                numbersAsString += ",";
            }

            return numbersAsString;
        }

        private void OnAdditionComplete(string equation, int sum)
        {
            AdditionComplete?.Invoke(this, new StringCalculatorEventArgs { Equation = equation, Sum = sum });
        }
    }
}