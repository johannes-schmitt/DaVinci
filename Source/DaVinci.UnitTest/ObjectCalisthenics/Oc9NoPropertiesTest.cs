using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestFixture]
    public class Oc9NoPropertiesTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.Oc9NoProperties();
        }

        [Test]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [Test]
        public void ClassContainsOneProperty_OneDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                public int SomeProperty { get; set; }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinciOC9",
                Message = "\'SomeProperty\' is a property and should be avoided.",
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 4, 28)
                        }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
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