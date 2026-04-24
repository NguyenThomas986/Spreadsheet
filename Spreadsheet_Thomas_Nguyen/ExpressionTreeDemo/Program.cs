using SpreadsheetEngine;

class Program
{
    static void Main(string[] args)
    {
        int result = 0;
        string? variableName = null;
        string currentExpression = "A1+B1+C1";
        double declaredValue = 0.0;
        ExpressionTree demoTree = new(currentExpression);
        bool quit = false;

        while (!quit)
        {
            PrintMenu(currentExpression);
            if (!int.TryParse(Console.ReadLine(), out result))
                continue;

            if (result == 1)
            {
                Console.WriteLine("enter the expression");
                currentExpression = Console.ReadLine();
                demoTree = new ExpressionTree(currentExpression);
            }
            else if (result == 2)
            {
                Console.WriteLine("enter the variable");
                variableName = Console.ReadLine();
                Console.WriteLine("enter the value");
                declaredValue = Convert.ToDouble(Console.ReadLine());
                demoTree.SetVariable(variableName, declaredValue);
            }
            else if (result == 3)
            {
                Console.WriteLine(demoTree.Evaluate());
            }
            else if (result == 4)
            {
                quit = true;
            }
        }
    }

    static void PrintMenu(string currentExpression)
    {
        Console.WriteLine("menu current expression: {0}", currentExpression);
        Console.WriteLine("1. enter a new expression");
        Console.WriteLine("2. set a variable value");
        Console.WriteLine("3. evaluate tree");
        Console.WriteLine("4. quit");
    }
}