using System;
using StringCalculator;
using NUnit.Framework;

namespace StringCalculatorTests
{
    public class StringCalculatorTests
    {
        [TestCase("", 0)]
        [TestCase("1", 1)]
        [TestCase("1,2", 3)]
        [TestCase("1,2,3,4,5", 15)]
        [TestCase("1\n2,3", 6)]
        [TestCase("//;\n1;2", 3)]
        public void Add_ValidInput_ValidOutput(string input, int expected)
        {
            ArrangeActAndAssert(input, expected);
        }

        [TestCase("-1", "negatives not allowed - (-1)")]
        [TestCase("-1,2,3,-4,5", "negatives not allowed - (-1 & -4)")]
        public void Add_NegativeNumbersInput_ThrowsException(string input, string exceptionMessage)
        {
            var stringCalculator = new StringCalculator.StringCalculator();
            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo(exceptionMessage),
                delegate { stringCalculator.Add(input); });

            var ex = Assert.Throws<ArgumentException>(
                delegate { stringCalculator.Add(input); });
            Assert.That(ex.Message, Is.EqualTo(exceptionMessage));
        }

        [TestCase("1,2,3,4,5", 15)]
        public void Add_CountTheCalls_ValidOutputAndTheCalls(string input, int expected)
        {
            var stringCalculator = new StringCalculator.StringCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(5, stringCalculator.CalledCount);
        }

        [TestCase("1,2,3,4,5", 15)]
        public void Add_CallsEventOnAdditionComplete(string input, int expected)
        {
            var stringCalculator = new StringCalculator.StringCalculator();
            stringCalculator.AdditionComplete += StringCalculator_AdditionComplete;
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        private void StringCalculator_AdditionComplete(object sender, StringCalculatorEventArgs e)
        {
            Assert.AreEqual("1,2,3,4,5", e.Equation);
            Assert.AreEqual(15, e.Sum);
        }

        [TestCase("2,1001", 2)]
        public void Add_IgnoresNumbersBiggerThan1000(string input, int expected)
        {
            ArrangeActAndAssert(input, expected);
        }

        [TestCase("//[***]\n1***2***3", 6)]
        public void Add_AllowsAnyLengthSeparators(string input, int expected)
        {
            ArrangeActAndAssert(input, expected);
        }

        [TestCase("//[*][%]\n1*2%3", 6)]
        public void Add_AllowMixedSeparators(string input, int expected)
        {
            ArrangeActAndAssert(input, expected);
        }

        [TestCase("//[**][%%]\n1**2%%3", 6)]
        public void Add_AllowMixedSeparatorStrings(string input, int expected)
        {
            ArrangeActAndAssert(input, expected);
        }

        private void ArrangeActAndAssert(string input, in int expected)
        {
            var stringCalculator = new StringCalculator.StringCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }
    }
}