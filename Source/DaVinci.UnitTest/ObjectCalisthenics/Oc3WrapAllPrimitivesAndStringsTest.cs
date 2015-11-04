﻿using DaVinci.ObjectCalisthenics;
using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestFixture]
    public class Oc3WrapAllPrimitivesAndStringsTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Oc3WrapAllPrimitivesAndStrings();
        }

        [Test]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [TestCase("decimal", true)]
        [TestCase("double", true)]
        [TestCase("float", true)]
        [TestCase("int", true)]
        [TestCase("uint", true)]
        [TestCase("long", true)]
        [TestCase("ulong", true)]
        [TestCase("object", false)]
        [TestCase("short", true)]
        [TestCase("ushort", true)]
        [TestCase("string", true)]
        public void MethodWithBuiltInParameter_DiagnosticReported(string parameterType, bool expectDiagnostic)
        {
            var code = @"
                class SomeClass
                {
                    public void DoSomething(" + parameterType + @" parameter)
                    {
                    }
                }";

            if (expectDiagnostic)
            {
                var expected = new DiagnosticResult
                {
                    Id = "DaVinci.OC.3",
                    Message = "'parameter' should be wrapped as it's a primitive.",
                    Severity = DiagnosticSeverity.Info,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 45 + parameterType.Length + 1) }
                };

                VerifyCSharpDiagnostic(code, expected);
            }
            else
            {
                VerifyCSharpDiagnostic(code);
            }
        }

        [Test]
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
    }
}
