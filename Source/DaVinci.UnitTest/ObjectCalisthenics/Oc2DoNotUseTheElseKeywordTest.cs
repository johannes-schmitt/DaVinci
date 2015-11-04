using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestFixture]
    public class Oc2DoNotUseTheElseKeywordTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.Oc2DoNotUseTheElseKeyword();
        }

        [Test]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [Test]
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

        [Test]
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
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 14, 21) }
            };
            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
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
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 14, 21) }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "DaVinci.OC.2",
                Message = "The else keyword should be avoided.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 18, 21) }
            };
            VerifyCSharpDiagnostic(Code, expected, expected2);
        }
    }
}
