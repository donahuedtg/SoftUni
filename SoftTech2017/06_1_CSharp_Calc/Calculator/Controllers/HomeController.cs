using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calculator.Models;


namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(CalculatorModel calculator)
        {
            return View(calculator);
        }

        [HttpPost]
        public ActionResult Calculate(CalculatorModel calculator)
        {
            //calculator.Result = CalculatorModel.CalculateResult(calculator);
            calculator.Result = CalculateResult2(calculator);
            return RedirectToAction("Index", calculator);

        }


        private decimal CalculateResult2(CalculatorModel calc)
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