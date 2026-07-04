using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace task11
{
    public class ClassCalculator
    {
        public static ICalculator CreateCalculator()
        {
            string code = @"
            using task11;
            public class Calculator : ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }";

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(typeof(int).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
            };

            var option = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var comp = CSharpCompilation.Create("Calculator", new [] {syntaxTree}, references, option);

            

            using(var memStream = new MemoryStream())
            {
                var result = comp.Emit(memStream);

                memStream.Seek(0, SeekOrigin.Begin);
                Assembly ass = AssemblyLoadContext.Default.LoadFromStream(memStream);

                var calculator = (ICalculator)Activator.CreateInstance(ass.GetType("Calculator"));

                return calculator;
            }
        }
    }
}




