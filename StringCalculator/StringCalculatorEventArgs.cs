using System;

namespace StringCalculator
{
    public class StringCalculatorEventArgs : EventArgs
    {
        public string Equation { get; set; }
        public int Sum { get; set; }
    }
}