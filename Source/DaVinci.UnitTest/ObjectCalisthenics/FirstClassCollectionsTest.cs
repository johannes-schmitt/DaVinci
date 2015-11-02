using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class FirstClassCollectionsTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.FirstClassCollections();
        }

        [TestMethod]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestMethod]
        public void ClassContainsListAndField_ReportDiagnostic()
        {
            const string Code = @"
            class SomeClass
            {
                private int someField;

                private System.Collections.Generic.List<int> list;

                public SomeClass()
                {
                    this.someField = 1;
                    this.list = new System.Collections.Generic.List<int>();
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 62) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassContainsTwoLists_ReportDiagnostic()
        {
            const string Code = @"
            class SomeClass
            {
                private System.Collections.Generic.List<int> firstList;

                private System.Collections.Generic.List<int> secondList;

                public SomeClass()
                {
                    this.firstList = new System.Collections.Generic.List<int>();
                    this.secondList = new System.Collections.Generic.List<int>();
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 62) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }
    }
}
