using DaVinci.Test.Helpers;
using DaVinci.Test.Verifiers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DaVinci.Test.ObjectCalisthenics
{
    [TestClass]
    public class UseOneLevelOfIndentationPerMethodTest : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DaVinci.ObjectCalisthenics.UseOneLevelOfIndentationPerMethod();
        }

        [TestMethod]
        public void MethodContainsNestedForLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                private readonly int[,] grid = new int[10, 15];

                public string SomeMethod()
                {
                    string result = string.Empty;

                    for (int column = 0; column < 10; column++)
                    {
                        for (int row = 0; row < 10; row++)
                        {
                            result += this.grid[row, column].ToString();
                        }
                    }

                    return result;
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 31) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedForEachLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                private readonly string[] rows = new string[10];
                private readonly string[] colums = new string[10];

                public void SomeMethod()
                {
                    foreach (var row in this.rows)
                    {
                        foreach (var col in this.colums)
                        {
                            System.Console.WriteLine(row + col);
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedWhileLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    while (true)
                    {
                        while (true)
                        {
                            System.Console.WriteLine(string.Empty);
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedDoWhileLoop_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    do
                    {
                        do
                        {
                            System.Console.WriteLine(string.Empty);
                        }
                        while (true);
                    }
                    while (true);
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsNestedIfStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    if (true)
                    {
                        if (!false)
                        {
                            System.Console.WriteLine(string.Empty);
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsControlStructureInElseStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    if (new Random().Next(1) == 1)
                    {
                        System.Console.WriteLine(string.Empty);
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            System.Console.WriteLine();
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsControlStructureInTryStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    try
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            System.Console.WriteLine();
                        }
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine();
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsControlStructureInCatchStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    try
                    {
                        System.Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            System.Console.WriteLine();
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsControlStructureInSecondCatchStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    try
                    {
                        System.Console.WriteLine();
                    }
                    catch (ArgumentException)
                    {
                        System.Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            System.Console.WriteLine();
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsControlStructureInFinallyStatement_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    try
                    {
                        System.Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine();
                    }
                    finally
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            System.Console.WriteLine();
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsTryStatementInControlStructure_DiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            System.Console.WriteLine();
                        }
                        catch (Exception)
                        {
                            System.Console.WriteLine();
                        }
                    }
                }
            }";

            var expected = new DiagnosticResult
            {
                Id = "DaVinci.OC.1",
                Message = "\'SomeMethod\' contains more than 1 level of indentation.",
                Severity = DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 29) }
            };

            VerifyCSharpDiagnostic(Code, expected);
        }

        [TestMethod]
        public void MethodContainsOnlyOneIndentation_NoDiagnosticIsReported()
        {
            const string Code = @"
            class SomeClass
            {
                public void SomeMethod()
                {
                    for (int i = 0; i < 10; i++)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                        System.Console.WriteLine();
                    }

                    if (true)
                    {
                        System.Console.WriteLine();
                    }
                    else
                    {
                        System.Console.WriteLine();
                    }

                    try
                    {
                        System.Console.WriteLine();
                    }
                    catch (ArgumentException ex)
                    {
                        System.Console.WriteLine(ex);
                    }
                    catch
                    {
                        System.Console.WriteLine(ex);
                    }
                    finally
                    {
                        System.Console.WriteLine();
                    }
                }
            }";

            VerifyCSharpDiagnostic(Code);
        }
    }
}
