using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalcLibrary;

namespace CalcTest
{
    [TestClass]
    public class CalcTest
    {
        [TestMethod]
        public void CalculateSum()
        {
            string[] a = Calc.GetOperands("23+4,5", 10);
            Assert.AreEqual("23", a[0]);
            Assert.AreEqual("4,5", a[1]);
        }

        [TestMethod]
        public void CalculateDifference()
        {
            string[] a = Calc.GetOperands("23-4,5", 10);
            Assert.AreEqual("23", a[0]);
            Assert.AreEqual("-4,5", a[1]);
        }

        [TestMethod]
        public void ManyMinusesGetOperands1()
        {
            string[] a = Calc.GetOperands("  23  -----  ---4,5    ", 10);
            Assert.AreEqual("23", a[0]);
            Assert.AreEqual("4,5", a[1]);
        }

        [TestMethod]
        public void ManyMinusesGetOperands2()
        {
            string[] a = Calc.GetOperands("  23  -----  --- 4,5    ", 10);
            Assert.AreEqual("23", a[0]);
            Assert.AreEqual("4,5", a[1]);
        }

        [TestMethod]
        public void ManyMinusesGetOperation1()
        {
            string a = Calc.GetOperation("  23  -----  ---4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void ManyMinusesGetOperation2()
        {
            string a = Calc.GetOperation("  23  ---4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void ManyMinusesGetOperation3()
        {
            string a = Calc.GetOperation("  23-------4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void SeparateMinusesGetOperation()
        {
            string a = Calc.GetOperation("  23 -- - ---4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void ManyPlusesGetOperation1()
        {
            string a = Calc.GetOperation("  23+++++4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void ManyPlusesGetOperation2()
        {
            string a = Calc.GetOperation("  23+ ++ + +4,5    ", 10);
            Assert.AreEqual("+", a);
        }

        [TestMethod]
        public void GetOperands()
        {
            string[] a = Calc.GetOperands("  23 * -4,5    ", 10);
            Assert.AreEqual("23", a[0]);
            Assert.AreEqual("-4,5", a[1]);
        }

        [TestMethod]
        public void GetMultiply()
        {
            string a = Calc.GetOperation("  23 * -4,5    ", 10);
            Assert.AreEqual("*", a);
        }

        [TestMethod]
        public void GetDivision()
        {
            string a = Calc.GetOperation("  23/ -4,5    ", 10);
            Assert.AreEqual("/", a);
        }

        [TestMethod]
        public void SumResult()
        {
            Assert.AreEqual("27", Calc.DoOperation("5,5+21,5", 10));
        }

        [TestMethod]
        public void SumResultManyPluses()
        {
            Assert.AreEqual("37,5", Calc.DoOperation("16 ++ + + ++ 21,5", 10));
        }

        [TestMethod]
        public void SumResultDifference1()
        {
            Assert.AreEqual("-5,5", Calc.DoOperation("16-----21,5", 10));
        }

        [TestMethod]
        public void SumResultDifference2()
        {
            Assert.AreEqual("5,5", Calc.DoOperation("-16----21,5", 10));
        }

        [TestMethod]
        public void MultiplyMinusResult()
        {
            Assert.AreEqual("4", Calc.DoOperation("-2 *- 2", 10));
        }

        [TestMethod]
        public void DivisionResult()
        {
            Assert.AreEqual("-1", Calc.DoOperation("2 /- 2", 10));
        }

        [TestMethod]
        public void DifferenceResult()
        {
            Assert.AreEqual("-4", Calc.DoOperation("- 2 - 2", 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongMultiplyOperation()
        {
            string a = Calc.GetOperation("26**5", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongDivisionOperation()
        {
            string a = Calc.GetOperation("26////5", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongMinusOperation1()
        {
            string a = Calc.GetOperation("26-+-5", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongMinusOperation2()
        {
            string a = Calc.GetOperation("26-*-5", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongExpressionForm1()
        {
            string a = Calc.GetOperation("/26*5", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DivideByZero()
        {
            string a = Calc.DoOperation("26/000", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void WrongExpressionForm2()
        {
            string a = Calc.GetOperation("26*5-", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotEnoughNumbers()
        {
            string a = Calc.DoOperation("26-", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NoNumbers()
        {
            string[] a = Calc.GetOperands("-", 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyNumbers()
        {
            string[] a = Calc.GetOperands("9-6*5", 10);
        }

    }
}
