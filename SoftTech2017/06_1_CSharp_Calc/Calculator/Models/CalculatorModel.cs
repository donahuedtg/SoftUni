using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calculator.Models
{
    public class CalculatorModel
    {
        public CalculatorModel()
        {
            this.Result = 0;
        }

        public decimal LeftOperand { get; set; }
        public decimal RightOperand { get; set; }
        public string Operator { get; set; }
        public decimal Result { get; set; }


        public static decimal CalculateResult(CalculatorModel calc)
        {
            decimal result = 0;

            switch (calc.Operator)
            {
                case "+":
                    result = calc.LeftOperand + calc.RightOperand;
                    break;
                case "-":
                    result = calc.LeftOperand - calc.RightOperand;
                    break;
                case "/":
                    result = calc.LeftOperand / calc.RightOperand;
                    break;
                case "*":
                    result = calc.LeftOperand * calc.RightOperand;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}