using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class NoPropertiesAnalyzerTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.NoPropertiesAnalyzer();
        }

        [TestMethod]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestMethod]
        public void ClassContainsOneProperty_OneDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                public int SomeProperty { get; set; }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.9",
                Message = "\'SomeProperty\' is a property and should be avoided.",
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 4, 28)
                        }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassContainsNoProperties_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
            }";

            VerifyCSharpDiagnostic(Code);
        }
    }
}