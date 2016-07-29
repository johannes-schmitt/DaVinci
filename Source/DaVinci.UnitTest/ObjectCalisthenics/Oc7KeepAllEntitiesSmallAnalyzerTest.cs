using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestFixture]
    public class Oc7KeepAllEntitiesSmallAnalyzerTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.Oc7KeepAllEntitiesSmallAnalyzer();
        }

        [Test]
        public void HelpLinkUriExists()
        {
            var analyzer = GetCSharpDiagnosticAnalyzer();
            analyzer.VerifyAllHelpLinks();
        }

        [Test]
        public void ClassHasExactlyOneLineTooMuch_OneDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int i1;

                private int i2;

                private int i3;

                private int i4;

                private int i5;

                private int i6;

                private int i7;

                private int i8;

                private int i9;

                private int i10;

                private int i11;

                private int i12;

                private int i13;

                private int i14;

                private int i15;

                private int i16;

                private int i17;

                private int i18;

                private int i19;

                private int i20;

                private int i21;

                private int i22;

                public SomeClass(int someInt)
                {
                    this.i1 = someInt;
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinciOC7",
                Message = "\'SomeClass\' contains more than 50 lines (51).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 19) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
        public void ClassHasExactlyMaximumNumberOfLines_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int i1;

                private int i2;

                private int i3;

                private int i4;

                private int i5;

                private int i6;

                private int i7;

                private int i8;

                private int i9;

                private int i10;

                private int i11;

                private int i12;

                private int i13;

                private int i14;

                private int i15;

                private int i16;

                private int i17;

                private int i18;

                private int i19;

                private int i20;

                private int i21;

                public SomeClass(int someInt)
                {
                    this.i1 = someInt;
                    this.i2 = someInt;
                }
            }";

            VerifyCSharpDiagnostic(Code);
        }

        [Test]
        public void StructHasExactlyOneLineTooMuch_OneDiagnosticReported()
        {
            const string Code = @"
            struct SomeStruct
            {
                private int i1;

                private int i2;

                private int i3;

                private int i4;

                private int i5;

                private int i6;

                private int i7;

                private int i8;

                private int i9;

                private int i10;

                private int i11;

                private int i12;

                private int i13;

                private int i14;

                private int i15;

                private int i16;

                private int i17;

                private int i18;

                private int i19;

                private int i20;

                private int i21;

                private int i22;

                public SomeStruct(int someInt)
                {
                    this.i1 = someInt;
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinciOC7",
                Message = "\'SomeStruct\' contains more than 50 lines (51).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 20) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
        public void StructHasExactlyMaximumNumberOfLines_NoDiagnosticReported()
        {
            const string Code = @"
            struct SomeStruct
            {
                private int i1;

                private int i2;

                private int i3;

                private int i4;

                private int i5;

                private int i6;

                private int i7;

                private int i8;

                private int i9;

                private int i10;

                private int i11;

                private int i12;

                private int i13;

                private int i14;

                private int i15;

                private int i16;

                private int i17;

                private int i18;

                private int i19;

                private int i20;

                private int i21;

                public SomeStruct(int someInt)
                {
                    this.i1 = someInt;
                    this.i2 = someInt;
                }
            }";

            VerifyCSharpDiagnostic(Code);
        }

        [Test]
        public void MethodHasExactlyOneLineTooMuch_OneDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                void SomeMethod()
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinciOC7",
                Message = "\'SomeMethod\' contains more than 15 lines (16).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 22) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [Test]
        public void MethodHasExactlyMaximumNumberOfLines_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                void SomeMethod()
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                }
            }";

            VerifyCSharpDiagnostic(Code);
        }
    }
}
