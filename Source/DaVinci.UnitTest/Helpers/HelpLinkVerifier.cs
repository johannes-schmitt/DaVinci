using System;
using System.Linq;
using System.Net;

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.Helpers
{
    internal static class HelpLinkVerifier
    {
        public static void VerifyAllHelpLinks(this DiagnosticAnalyzer analyzer)
        {
            foreach (var helpLinkUri in analyzer.SupportedDiagnostics.Select(a => new Uri(a.HelpLinkUri)))
            {
                var webRequest = WebRequest.CreateHttp(helpLinkUri);
                webRequest.Method = "HEAD";
                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }
    }
}
