namespace ExpressionTreeTesting
{
    using SpreadsheetEngine;

    public class ExpressionTreeTests
    {
        // ──────────────────────────────────────────────
        // 1. CONSTRUCTION & CONSTANTS
        // ──────────────────────────────────────────────

        [Test]
        public void Constructor_SingleConstant_EvaluatesCorrectly()
        {
            var tree = new ExpressionTree("42");
            Assert.That(tree.Evaluate(), Is.EqualTo(42.0));
        }


        [Test]
        public void Constructor_Zero_EvaluatesCorrectly()
        {
            var tree = new ExpressionTree("0");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }

        // ──────────────────────────────────────────────
        // 2. ADDITION
        // ──────────────────────────────────────────────

        [Test]
        public void Addition_TwoConstants_ReturnsSum()
        {
            var tree = new ExpressionTree("3+4");
            Assert.That(tree.Evaluate(), Is.EqualTo(7.0));
        }

        [Test]
        public void Addition_MultipleConstants_ReturnsSum()
        {
            var tree = new ExpressionTree("1+2+3+4+5");
            Assert.That(tree.Evaluate(), Is.EqualTo(15.0));
        }

        [Test]
        public void Addition_WithVariables_DefaultsToZero()
        {
            // Variables not set → default 0
            var tree = new ExpressionTree("A+B+C");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }

        [Test]
        public void Addition_MixedConstantsAndVariables_EvaluatesCorrectly()
        {
            var tree = new ExpressionTree("A+5+B");
            tree.SetVariable("A", 3);
            tree.SetVariable("B", 2);
            Assert.That(tree.Evaluate(), Is.EqualTo(10.0));
        }

        // ──────────────────────────────────────────────
        // 3. SUBTRACTION
        // ──────────────────────────────────────────────

        [Test]
        public void Subtraction_TwoConstants_ReturnsDifference()
        {
            var tree = new ExpressionTree("10-3");
            Assert.That(tree.Evaluate(), Is.EqualTo(7.0));
        }

        [Test]
        public void Subtraction_MultipleConstants_EvaluatesLeftToRight()
        {
            // 20 - 5 - 3 should be (20-5)-3 = 12
            var tree = new ExpressionTree("20-5-3");
            Assert.That(tree.Evaluate(), Is.EqualTo(12.0));
        }

        [Test]
        public void Subtraction_ResultIsNegative_ReturnsCorrectValue()
        {
            var tree = new ExpressionTree("3-10");
            Assert.That(tree.Evaluate(), Is.EqualTo(-7.0));
        }

        [Test]
        public void Subtraction_WithVariables_EvaluatesCorrectly()
        {
            // Example from spec: "C2-9-B2-27"
            var tree = new ExpressionTree("C2-9-B2-27");
            tree.SetVariable("C2", 50);
            tree.SetVariable("B2", 4);
            Assert.That(tree.Evaluate(), Is.EqualTo(10.0)); // 50-9-4-27
        }

        // ──────────────────────────────────────────────
        // 4. MULTIPLICATION
        // ──────────────────────────────────────────────

        [Test]
        public void Multiplication_TwoConstants_ReturnsProduct()
        {
            var tree = new ExpressionTree("6*7");
            Assert.That(tree.Evaluate(), Is.EqualTo(42.0));
        }

        [Test]
        public void Multiplication_MultipleConstants_ReturnsProduct()
        {
            var tree = new ExpressionTree("2*3*4");
            Assert.That(tree.Evaluate(), Is.EqualTo(24.0));
        }

        [Test]
        public void Multiplication_ByZero_ReturnsZero()
        {
            var tree = new ExpressionTree("99*0");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }

        [Test]
        public void Multiplication_WithVariables_EvaluatesCorrectly()
        {
            var tree = new ExpressionTree("A*B*3");
            tree.SetVariable("A", 2);
            tree.SetVariable("B", 5);
            Assert.That(tree.Evaluate(), Is.EqualTo(30.0));
        }

        // ──────────────────────────────────────────────
        // 5. DIVISION
        // ──────────────────────────────────────────────

        [Test]
        public void Division_TwoConstants_ReturnsQuotient()
        {
            var tree = new ExpressionTree("20/4");
            Assert.That(tree.Evaluate(), Is.EqualTo(5.0));
        }

        [Test]
        public void Division_MultipleConstants_EvaluatesLeftToRight()
        {
            // 100 / 5 / 4 = (100/5)/4 = 5
            var tree = new ExpressionTree("100/5/4");
            Assert.That(tree.Evaluate(), Is.EqualTo(5.0));
        }

        [Test]
        public void Division_NonEvenResult_ReturnsDouble()
        {
            var tree = new ExpressionTree("10/3");
            Assert.That(tree.Evaluate(), Is.EqualTo(10.0 / 3.0).Within(1e-10));
        }

        [Test]
        public void Division_WithVariables_EvaluatesCorrectly()
        {
            var tree = new ExpressionTree("X/Y");
            tree.SetVariable("X", 100);
            tree.SetVariable("Y", 4);
            Assert.That(tree.Evaluate(), Is.EqualTo(25.0));
        }

        // ──────────────────────────────────────────────
        // 6. VARIABLES
        // ──────────────────────────────────────────────

        [Test]
        public void Variable_SingleCharacter_DefaultsToZero()
        {
            var tree = new ExpressionTree("X");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }

        [Test]
        public void Variable_MultiCharacter_SetAndEvaluate()
        {
            var tree = new ExpressionTree("Hello");
            tree.SetVariable("Hello", 99);
            Assert.That(tree.Evaluate(), Is.EqualTo(99.0));
        }

        [Test]
        public void Variable_AlphanumericName_SetAndEvaluate()
        {
            // Example from spec: "A+B+C1+Hello+6"
            var tree = new ExpressionTree("A+B+C1+Hello+6");
            tree.SetVariable("A", 1);
            tree.SetVariable("B", 2);
            tree.SetVariable("C1", 3);
            tree.SetVariable("Hello", 4);
            Assert.That(tree.Evaluate(), Is.EqualTo(16.0)); // 1+2+3+4+6
        }

        [Test]
        public void Variable_SetTwice_UsesLatestValue()
        {
            var tree = new ExpressionTree("X+1");
            tree.SetVariable("X", 5);
            tree.SetVariable("X", 10);
            Assert.That(tree.Evaluate(), Is.EqualTo(11.0));
        }

        [Test]
        public void Variable_LowercaseName_SetAndEvaluate()
        {
            var tree = new ExpressionTree("abc+2");
            tree.SetVariable("abc", 8);
            Assert.That(tree.Evaluate(), Is.EqualTo(10.0));
        }

        // ──────────────────────────────────────────────
        // 7. NEW EXPRESSION CLEARS OLD VARIABLES
        // ──────────────────────────────────────────────

        [Test]
        public void NewExpression_ClearsOldVariables()
        {
            // First expression
            var tree1 = new ExpressionTree("A+1");
            tree1.SetVariable("A", 10);
            Assert.That(tree1.Evaluate(), Is.EqualTo(11.0));

            // New expression should have its own fresh variable set
            var tree2 = new ExpressionTree("A+1");
            // A is not set → defaults to 0
            Assert.That(tree2.Evaluate(), Is.EqualTo(1.0));
        }

        // ──────────────────────────────────────────────
        // 8. SPEC EXAMPLES
        // ──────────────────────────────────────────────

        [Test]
        public void SpecExample_A_Plus_B_Plus_C1_Plus_Hello_Plus_6()
        {
            var tree = new ExpressionTree("A+B+C1+Hello+6");
            tree.SetVariable("A", 1);
            tree.SetVariable("B", 2);
            tree.SetVariable("C1", 3);
            tree.SetVariable("Hello", 4);
            Assert.That(tree.Evaluate(), Is.EqualTo(16.0));
        }

        [Test]
        public void SpecExample_C2_Minus_9_Minus_B2_Minus_27()
        {
            var tree = new ExpressionTree("C2-9-B2-27");
            tree.SetVariable("C2", 50);
            tree.SetVariable("B2", 4);
            Assert.That(tree.Evaluate(), Is.EqualTo(10.0));
        }

        // ──────────────────────────────────────────────
        // 9. DEFAULT EXPRESSION (smoke test)
        // ──────────────────────────────────────────────

        [Test]
        public void DefaultExpression_CanSetVariablesAndEvaluate()
        {
            // The program must have a default expression like "A1+B1+C1"
            // This test simulates a user setting variables on that default expression.
            var tree = new ExpressionTree("A1+B1+C1");
            tree.SetVariable("A1", 5);
            tree.SetVariable("B1", 10);
            tree.SetVariable("C1", 15);
            Assert.That(tree.Evaluate(), Is.EqualTo(30.0));
        }

        // ──────────────────────────────────────────────
        // 10. LARGE EXPRESSIONS
        // ──────────────────────────────────────────────

        [Test]
        public void Addition_ManyOperands_ReturnsCorrectSum()
        {
            // 1+2+3+...+10 = 55
            var tree = new ExpressionTree("1+2+3+4+5+6+7+8+9+10");
            Assert.That(tree.Evaluate(), Is.EqualTo(55.0));
        }

        [Test]
        public void Multiplication_ManyOperands_ReturnsCorrectProduct()
        {
            // 2*2*2*2*2 = 32
            var tree = new ExpressionTree("2*2*2*2*2");
            Assert.That(tree.Evaluate(), Is.EqualTo(32.0));
        }


        // Edge cases

        [Test]

        public void EmptyExpression_EvaluatesToZero()
        {
            var tree = new ExpressionTree("");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }


        // HW 6 test cases
        [Test]
        public void PrecedenceWithoutParentheses()
        {
            var tree = new ExpressionTree("3+4*2");
            Assert.That(tree.Evaluate(), Is.EqualTo(11.0));
        }

        [Test]
        public void ParenthesesOverridePrecedence()
        {
            var tree = new ExpressionTree("(3+4)*2");
            Assert.That(tree.Evaluate(), Is.EqualTo(14.0));
        }

        [Test]
        public void NestedParentheses()
        {
            var tree = new ExpressionTree("((3+4)*2)");
            Assert.That(tree.Evaluate(), Is.EqualTo(14.0));
        }

        [Test]
        public void MultipleParentheses()
        {
            var tree = new ExpressionTree("(3+4)*(2+1)");
            Assert.That(tree.Evaluate(), Is.EqualTo(21.0));
        }

        [Test]
        public void DeeplyNestedParentheses()
        {
            var tree = new ExpressionTree("((2+3)*(4-1))");
            Assert.That(tree.Evaluate(), Is.EqualTo(15.0));
        }


        [Test]
        public void MismatchedParenthesesReturnsNull()
        {
            ExpressionTree tree = new ExpressionTree("(3+4*2");
            Assert.That(tree.Evaluate(), Is.EqualTo(0.0));
        }
    }
}
