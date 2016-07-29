﻿using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestFixture]
    public class Oc4FirstClassCollectionsTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.Oc4FirstClassCollections();
        }

        [Test]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [Test]
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
                Id = "DaVinciOC4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 62) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
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
                Id = "DaVinciOC4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 62) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
        public void ClassContainsTwoListsAndAnotherField_ReportDiagnostic()
        {
            const string Code = @"
            class SomeClass
            {
                private System.Collections.Generic.List<int> firstList;
                private System.Collections.Generic.List<int> secondList;
                private int intField;

                public SomeClass(int val)
                {
                    this.firstList = new System.Collections.Generic.List<int>();
                    this.secondList = new System.Collections.Generic.List<int>();
                    this.intField = val;
                }
            }";

            var firstExpected = new DiagnosticResult
            {
                Id = "DaVinciOC4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 62)}
            };

            var secondExpected = new DiagnosticResult
            {
                Id = "DaVinciOC4",
                Message = "Consider wrapping the collection into a separate class.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 5, 62) }
            };

            VerifyCSharpDiagnostic(Code, firstExpected, secondExpected);
        }
    }
}
