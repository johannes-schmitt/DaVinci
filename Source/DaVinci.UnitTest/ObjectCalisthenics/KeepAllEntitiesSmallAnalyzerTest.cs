using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class KeepAllEntitiesSmallAnalyzerTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.KeepAllEntitiesSmallAnalyzer();
        }

        [TestMethod]
        public void ClassHasFewLines_NoDiagnosticReported()
        {
            const string Code = @"
            class SomeClass
            {
                private int firstField;

                private int secondField;

                private int thirdField;
            }";

            VerifyCSharpDiagnostic(Code);
        }

        [TestMethod]
        public void ClassHasMoreThanFiftyLines_OneDiagnosticReported()
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

                private int i23;

                public SomeClass(int someInt)
                {
                    this.i1 = someInt;
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.7",
                Message = "\'SomeClass\' contains more than 50 lines (53).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 19) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassHasFiftyOneLines_OneDiagnosticReported()
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
                Id = "DaVinci.OC.7",
                Message = "\'SomeClass\' contains more than 50 lines (51).",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 19) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void ClassHasFiftyLines_NoDiagnosticReported()
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
    }
}
