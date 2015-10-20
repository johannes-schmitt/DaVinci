using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class DoNotUseTheElseKeywordTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.DoNotUseTheElseKeyword();
        }

        [TestMethod]
        public void MethodContainsIfWithoutElse_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void Do()
                {
                    if (true)
                    {
                        System.Console.WriteLine();
                    }
                }
            }";

            VerifyCSharpDiagnostic(Code);
        }

        [TestMethod]
        public void MethodContainsIfWithElse_OneDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;

                private int secondField;

                public void Do()
                {
                    if (true)
                    {
                        System.Console.WriteLine();
                    }
                    else
                    {
                        System.Console.WriteLine();
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.2",
                Message = "The else keyword should be avoided.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 14, 21) }
            };
            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsIfWithTwoElses_TwoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;

                private int secondField;

                public void Do()
                {
                    if (true)
                    {
                        System.Console.WriteLine();
                    }
                    else if (false)
                    {
                        System.Console.WriteLine();
                    }
                    else
                    {
                        System.Console.WriteLine();
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.2",
                Message = "The else keyword should be avoided.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 14, 21) }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "DaVinci.OC.2",
                Message = "The else keyword should be avoided.",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 18, 21) }
            };
            VerifyCSharpDiagnostic(Code, expected, expected2);
        }
    }
}
