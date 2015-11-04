using DaVinci.ObjectCalisthenics;
using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class Oc5UseOneDotPerLineTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Oc5UseOneDotPerLine();
        }

        [TestMethod]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestMethod]
        public void OneExpressionContains2Dots_DiagnosticIsReported()
        {
            const string Code = @"
           class Board {
                public String boardRepresentation() {
                    StringBuilder buf = new StringBuilder();

                    var somevariable = loc.current.substring(0, 1);
                    for (Location loc : squares()) {
                        buf.append(somevariable);
                    }

                    return buf.toString();
                }
             }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.5",
                Message = "\'loc.current.substring(0, 1)\' contains more than 1 dot per line.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 40) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void NoExpressionContains2Dots_NoDiagnosticIsReported()
        {
            const string Code = @"
           class Board {
                public String boardRepresentation() {
                    StringBuilder buf = new StringBuilder();

                    var somevariable = loc.substring(0, 1);
                    for (Location loc : squares()) {
                        buf.append(somevariable);
                    }

                    return buf.toString();
                }
             }";
           
            VerifyCSharpDiagnostic(Code);
        }

        [TestMethod]
        public void NestedExpressionContains2Dots_DiagnosticIsReported()
        {
            const string Code = @"
           class Board {
                public String boardRepresentation() {
                    StringBuilder buf = new StringBuilder();

                    for (Location loc : squares()) {
                        buf.append(loc.current.substring(0, 1));
                    }

                    return buf.toString();
                }
             }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.5",
                Message = "\'loc.current.substring(0, 1)\' contains more than 1 dot per line.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 36) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ThreeNestedExpressionContains2Dots_DiagnosticIsReported()
        {
            const string Code = @"
           class Board {
                public String boardRepresentation() {
                    StringBuilder buf = new StringBuilder();

                    for (Location loc : squares()) {
                        buf.append(loc.current.substring(point.getvalue(), point.getvalue()));
                    }

                    return buf.toString();
                }
             }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.5",
                Message = "\'loc.current.substring(point.getvalue(), point.getvalue())\' contains more than 1 dot per line.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 36) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }
    }
}
