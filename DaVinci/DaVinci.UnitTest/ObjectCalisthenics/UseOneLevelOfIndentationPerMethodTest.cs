﻿using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class UseOneLevelOfIndentationPerMethodTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.UseOneLevelOfIndentationPerMethod();
        }

        [TestMethod]
        public void MethodContainsNestedForLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                private readonly int[,] grid = new int[10, 15];

                public string Format()
                {
                    string result = string.Empty;

                    for (int column = 0; column < 10; column++)
                    {
                        for (int row = 0; row < 10; row++)
                        {
                            result += this.grid[row, column].ToString();
                        }
                    }

                    return result;
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'Format\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 31) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedForEachLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                private readonly string[] rows = new string[10];
                private readonly string[] colums = new string[10];

                public void Format()
                {
                    foreach (var row in this.rows)
                    {
                        foreach (var col in this.colums)
                        {
                            System.Console.WriteLine(row + col);
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'Format\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedWhileLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void Format()
                {
                    while (true)
                    {
                        while (true)
                        {
                            System.Console.WriteLine(string.Empty);
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'Format\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }
    }
}
