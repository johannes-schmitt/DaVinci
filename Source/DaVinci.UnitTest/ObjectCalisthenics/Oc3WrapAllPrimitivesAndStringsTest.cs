using DaVinci.ObjectCalisthenics;
using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class Oc3WrapAllPrimitivesAndStringsTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Oc3WrapAllPrimitivesAndStrings();
        }

        [TestMethod]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestMethod]
        public void MethodTakesIntAsParameter_DiagnosticReported()
        {
            const string Code = @"
                class SomeClass
                {
                    public void DoSomething(int parameter)
                    {
                    }
                }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.3",
                Message = "'parameter' should be wrapped as it's a primitive.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 49) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodTakesUIntAsParameter_DiagnosticReported()
        {
            const string Code = @"
                class SomeClass
                {
                    public void DoSomething(uint parameter)
                    {
                    }
                }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.3",
                Message = "'parameter' should be wrapped as it's a primitive.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 50) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodTakesNonPrimitiveAsParameter_NoDiagnosticReported()
        {
            const string Code = @"
                class NonPrimitive
                {
                }

                class SomeClass
                {
                    public void DoSomething(NonPrimitive parameter)
                    {
                    }
                }";

            VerifyCSharpDiagnostic(Code);
        }

        [TestMethod]
        public void MethodTakesStringAsParameter_DiagnosticReported()
        {
            const string Code = @"
                class SomeClass
                {
                    public void DoSomething(string parameter)
                    {
                    }
                }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.3",
                Message = "'parameter' should be wrapped as it's a primitive.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 52) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodTakesObjectAsParameter_DiagnosticReported()
        {
            const string Code = @"
                class SomeClass
                {
                    public void DoSomething(object parameter)
                    {
                    }
                }";

            VerifyCSharpDiagnostic(Code);
        }
    }
}
