using DaVinci.ObjectCalisthenics;
using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

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
    }
}
