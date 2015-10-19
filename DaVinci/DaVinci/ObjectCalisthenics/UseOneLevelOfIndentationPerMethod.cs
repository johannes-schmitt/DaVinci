using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseOneLevelOfIndentationPerMethod : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.1";
        protected override LocalizableString Title => "Use one level of indentation per method (Object Calisthenics Rule #1).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than 1 level of indentation.";
        protected override LocalizableString Description => "Rule #1 of Object Calisthenics is \"Use one level of indentation per method\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Warning;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCodeBlockAction(AnalyzeCodeBlock);
        }

        private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        {
            var methodDeclarationSyntax = context.CodeBlock as MethodDeclarationSyntax;
            if (methodDeclarationSyntax != null)
            {
                if (HasMultipleIndentations(methodDeclarationSyntax))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation(), methodDeclarationSyntax.Identifier.Text));
                }
            }
        }

        private bool HasMultipleIndentations(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            foreach (var blockSyntax in GetCandidates(methodDeclarationSyntax.Body.Statements))
            {
                if (ContainsControlStructure(blockSyntax))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<BlockSyntax> GetCandidates(SyntaxList<StatementSyntax> statements)
        {
            foreach (var statementSyntax in statements)
            {
                var blockSyntax = (statementSyntax as ForStatementSyntax)?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }

                blockSyntax = (statementSyntax as ForEachStatementSyntax)?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }

                blockSyntax = (statementSyntax as WhileStatementSyntax)?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }

                blockSyntax = (statementSyntax as DoStatementSyntax)?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }

                blockSyntax = (statementSyntax as IfStatementSyntax)?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }

                blockSyntax = (statementSyntax as IfStatementSyntax)?.Else?.Statement as BlockSyntax;
                if (blockSyntax != null)
                {
                    yield return blockSyntax;
                }
            }
        }

        public void Format()
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

        private bool ContainsControlStructure(BlockSyntax body)
        {
            foreach (var statementSyntax in body.Statements)
            {
                if (IsControlStructure(statementSyntax))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsControlStructure(StatementSyntax statementSyntax)
        {
            var type = statementSyntax.GetType();
            var controlStructures = new List<Type>
                             {
                                 typeof(ForStatementSyntax),
                                 typeof(ForEachStatementSyntax),
                                 typeof(WhileStatementSyntax),
                                 typeof(DoStatementSyntax),
                                 typeof(IfStatementSyntax)
                             };

            return controlStructures.Contains(type);
        }
    }
}
