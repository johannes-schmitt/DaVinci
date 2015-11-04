using System.Net;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.Helpers
{
    internal static class HelpLinkVerifier
    {
        public static void VerifyAllHelpLinks(this DiagnosticAnalyzer analyzer)
        {
            foreach (var diagnostic in analyzer.SupportedDiagnostics)
            {
                diagnostic.VerifyThatHelpLinkContainsDiagnosticId();
                diagnostic.VerifyThatHelpLinkExists();
            }
        }

        private static void VerifyThatHelpLinkContainsDiagnosticId(this DiagnosticDescriptor diagnosticDescriptor)
        {
            var helpLinkUri = diagnosticDescriptor.HelpLinkUri;
            StringAssert.Contains(diagnosticDescriptor.Id, helpLinkUri);
        }

        private static void VerifyThatHelpLinkExists(this DiagnosticDescriptor diagnosticDescriptor)
        {
            var webRequest = WebRequest.CreateHttp(diagnosticDescriptor.HelpLinkUri);
            webRequest.Method = "HEAD";
            using (var response = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
