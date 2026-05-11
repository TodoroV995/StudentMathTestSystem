namespace MathProcessor.Interfaces
{
    public interface IMathExpressionProcessor
    {
        Task<decimal> EvaluateAsync(string expression);
    }
}