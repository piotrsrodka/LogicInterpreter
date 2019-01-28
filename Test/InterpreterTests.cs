using LogicalInterpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class InterpreterTests
    {
        [TestMethod]
        public void Complicated()
        {
            Assert.IsTrue(Evaluate(@"((1 == 1 && 4 > 5) || 0 != 1))"));
        }

        [TestMethod]
        public void FalseAndTrueOrTrueWithBrackets()
        {
            Assert.IsFalse(Evaluate(@"false && (true || true)"));
        }

        [TestMethod]
        public void FalseAndTrueOrTrue()
        {
            Assert.IsTrue(Evaluate(@"false && true || true"));
        }

        [TestMethod]
        public void TrueAndTrueOrFalse()
        {
            Assert.IsTrue(Evaluate(@"true && true || false"));
        }

        [TestMethod]
        public void TrueStringAndTrueWithBrackets()
        {
            Assert.IsTrue(Evaluate(@"(""Red"" == ""Red"" && 15 > 5)"));
        }

        [TestMethod]
        public void TrueAndTrueWithBrackets()
        {
            Assert.IsTrue(Evaluate("(1 == 1) && (15 > 5)"));
        }

        [TestMethod]
        public void TrueAndFalseWithBrackets()
        {
            Assert.IsFalse(Evaluate("(1 == 1) && (1 > 5)"));
        }

        [TestMethod]
        public void TrueAndFalse()
        {
            Assert.IsFalse(Evaluate("1 == 1 && 1 > 5"));
        }

        [TestMethod]
        public void TrueAndFalseLiterally()
        {
            Assert.IsFalse(Evaluate("true && false"));
        }

        [TestMethod]
        public void True()
        {
            Assert.IsTrue(Evaluate("true"));
        }

        [TestMethod]
        public void False()
        {
            Assert.IsFalse(Evaluate("false"));
        }

        [TestMethod]
        public void FalseOrTrue()
        {
            Assert.IsTrue(Evaluate("false || 1==1"));
        }

        [TestMethod]
        public void TrueOrTrue()
        {
            Assert.IsTrue(Evaluate("true || 1==1"));
        }

        [TestMethod]
        public void NotEqualTextsIsTrue()
        {
            Assert.IsTrue(Evaluate(@" ""Text"" != ""Text2"" "));
        }

        [TestMethod]
        public void NotEqualNumbersIsFalse()
        {
            Assert.IsFalse(Evaluate(@"1 != 1"));
        }

        [TestMethod]
        public void NotEqualNumbersIsTrue()
        {
            Assert.IsTrue(Evaluate(@"2 != 1"));
        }

        [TestMethod]
        public void GreaterIsTrue()
        {
            Assert.IsTrue(Evaluate(@"2 > 1"));
        }

        [TestMethod]
        public void Given_EqualNumbers_When_Comparing_Then_ItIsTrue()
        {
            Assert.IsTrue(Evaluate(@"1 == 1"));
        }

        [TestMethod]
        public void Given_EqualString_When_Comparing_Then_ItIsTrue()
        {
            Assert.IsTrue(Evaluate(@"""Text"" == ""Text"" "));
        }

        private static bool Evaluate(string input)
        {
            Scanner scanner = new Scanner(input);
            Interpreter interpreter = new Interpreter(scanner, null);
            var result = interpreter.LogicalExpression();
            return result;
        }
    }
}
