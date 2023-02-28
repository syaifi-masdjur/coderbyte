using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace coderbyte
{
    public class StringToCalculate
    {

        //Allowed Operator *,/,+,-,**
        private static string[] operators = { "*", "/", "+", "-"};

        public static double smallExecuteOperation(string op, double op1, double op2)
        {
            double output = 0;
            switch (op)
            {
                case "*":
                    output = op1 * op2;
                    break;
                case "/":
                    output = op1 / op2;
                    break;
                case "+":
                    output = op1 + op2;
                    break;
                case "-":
                    output = op1 + op2;
                    break;
                case "**":
                    output = Math.Pow(op1, op2);
                    break;
            }
            return output;
        }


        public static int inlineExecuteOperation(string op)
        {
            var _output = op.Replace(" ", "");
            int output = 0;

            //replace space
            int ox = 0;
            int op1 = 0;
            int op2 = 0;
            int leftpos = 0, rightpos = 0;
            var bIsNumber = int.TryParse(_output, out ox);
            while (!bIsNumber)
            {
                //main process
                int mainIdx = 0;
                while (mainIdx < 4)
                {
                    string mainop = operators[mainIdx];
                    int mainpos = _output.IndexOf(mainop, 0);
                    if (mainpos == -1)
                    {
                        mainIdx++;
                        continue;
                    }

                    //find left number
                    int leftIdx = 0;
                    while (leftIdx < 4)
                    {
                        string leftop = operators[leftIdx];
                        leftpos = _output.LastIndexOf(leftop, mainpos - 1);
                        if (leftpos == -1)
                        {
                            leftIdx++;
                            continue;
                        }

                        var _op1 = _output.Substring(leftpos + 1, mainpos - leftpos - 1);
                        op1 = Convert.ToInt32(_op1);
                        if (op1 != 0)
                        {
                            leftIdx++;
                            break;
                        }
                        leftIdx++;
                    }

                    if (leftpos == -1)
                    {
                        var _op1 = _output.Substring(0, mainpos - leftpos - 1);
                        op1 = Convert.ToInt32(_op1);
                    }

                    //find left number
                    int rightIdx = 0;
                    while (rightIdx < 4)
                    {
                        string rightop = operators[rightIdx];
                        int amo = 2;
                        if(mainpos + 1 == _output.Length-1)
                        {
                            amo = 1;
                        }
                        rightpos = _output.IndexOf(rightop, mainpos+1, amo);//Error in here.. kyu?
                        if (rightpos == -1)
                        {
                            rightIdx++;
                            continue;
                        }
                        var _op2 = _output.Substring(rightpos - 1, 1);
                        op2 = Convert.ToInt32(_op2);
                        if (op2 != 0)
                        {
                            break;
                        }
                        rightIdx++;
                    }

                    if (rightpos == -1)
                    {
                        var _op2 = _output.Substring(mainpos + 1, _output.Length - mainpos - 1);
                        op2 = Convert.ToInt32(_op2);
                    }

                    var calc = smallExecuteOperation(mainop, op1, op2);
                    var toBeReplaced = $"{op1}{mainop}{op2}";
                    _output = _output.Replace(toBeReplaced, calc.ToString());
                    bIsNumber = int.TryParse(_output, out ox);
                    if (bIsNumber)
                    {
                        output = ox;
                        break;
                    }
                    mainIdx = 0;
                }
                if (bIsNumber)
                {
                    break;
                }
            }

            return output;

        }

        public static int StringToExpression(string op)
        {
            if (op == null)
                throw new ArgumentNullException();

            var _output = op.Replace(" ", "");
            int output = 0;
            string betweenBracket = _output;
            while (true)
            {
                //find left bracket
                int lBracket = betweenBracket.IndexOf('(', 0);
                if (lBracket == -1)
                {
                    output = inlineExecuteOperation(betweenBracket);
                    break;

                }
                int rBracket = betweenBracket.LastIndexOf(')', betweenBracket.Length-1, betweenBracket.Length - 1);

                //in between bracket
                betweenBracket = betweenBracket.Substring(lBracket+1, rBracket - lBracket-1);
                lBracket = betweenBracket.IndexOf('(', 0);
                if (lBracket == -1)
                {
                    int xoutput = inlineExecuteOperation(betweenBracket);
                    var toBeReplaced = $"({betweenBracket})";
                    _output = _output.Replace(toBeReplaced, xoutput.ToString());
                    betweenBracket = _output;
                }
            }

            return output;
        }
    }
}
