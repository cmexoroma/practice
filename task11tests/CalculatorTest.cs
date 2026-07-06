using Xunit;
using task11;

namespace task11tests;

public class CalculatorTest
{
    static ICalculator calculator = ClassCalculator.CreateCalculator(); 
    [Fact]
    public void AddTest()
    {
        Assert.Equal(11, calculator.Add(6, 5));
    }

    [Fact]
    public void MinusTest()
    {
        Assert.Equal(1, calculator.Minus(6, 5));
    }

    [Fact]
    public void MulTest()
    {
        Assert.Equal(30, calculator.Mul(6, 5));   
    }

    [Fact]
    public void DivTest()
    {
        Assert.Equal(2, calculator.Div(10, 5));
    }
}
