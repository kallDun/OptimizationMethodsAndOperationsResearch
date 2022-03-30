using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    class FunctionParser
    {
        public Table ToTable(string function, string equations)
        {
            string[] condition = function.Trim().Split(new char[] { '>' });

            string minormax = condition[1].Trim();
            string expression = condition[0].Substring(0, condition[0].Length - 1).Trim();

            string[] leftRightOfExp = expression.Split(new char[] { '=' });

            string leftPartOfExp = leftRightOfExp[0].Trim();
            string rightPartOfExp = leftRightOfExp[1].Trim();



            throw new NotImplementedException();
        }
    }
}
