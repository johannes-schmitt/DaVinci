using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class Oc8NoClassesWithMoreThanTwoFieldsTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.Oc8NoClassesWithMoreThanTwoFields();
        }

        [TestMethod]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestMethod]
        public void ClassHasThreeFields_OneDiagnosticReportedShowingThreeFields()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;
                private int secondField;
                private int thirdField;
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.8",
                Message = "\'SomeClass\' contains more than 2 fields (3).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 19) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassHasFourFields_OneDiagnosticReportedShowingFourFields()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;
                private int secondField;
                private int thirdField;
                private int fourthField;
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.8",
                Message = "\'SomeClass\' contains more than 2 fields (4).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 19) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassHasTwoFields_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;
                private int secondField;
            }";

            VerifyCSharpDiagnostic(Code);
        }

        [TestMethod]
        public void StructHasThreeFields_OneDiagnosticReportedShowingThreeFields()
        {
            const string Code = @"
            struct SomeStruct
            {
                private int firstField;
                private int secondField;
                private int thirdField;
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.8",
                Message = "\'SomeStruct\' contains more than 2 fields (3).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 20) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void StructHasFourFields_OneDiagnosticReportedShowingFourFields()
        {
            const string Code = @"
            struct SomeStruct
            {
                private int firstField;
                private int secondField;
                private int thirdField;
                private int fourthField;
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.8",
                Message = "\'SomeStruct\' contains more than 2 fields (4).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 20) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void StructHasTwoFields_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeStruct
            {
                private int firstField;
                private int secondField;
            }";

            VerifyCSharpDiagnostic(Code);
        }
    }
}