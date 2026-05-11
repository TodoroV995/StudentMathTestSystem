using MathProcessor.Interfaces;
using System.Data;
using System.Globalization;

namespace MathProcessor.Services
{
    public class MathExpressionProcessor : IMathExpressionProcessor
    {
        public Task<decimal> EvaluateAsync(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException("Expression cannot be empty.");
            }

            var dataTable = new DataTable();

            var result = dataTable.Compute(expression, null);

            decimal decimalResult = Convert.ToDecimal(result, CultureInfo.InvariantCulture);

            return Task.FromResult(decimalResult);
        }
    }
}