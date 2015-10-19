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
            if (methodDeclarationSyntax == null)
            {
                return;
            }

            if (HasMultipleIndentations(methodDeclarationSyntax.Body.Statements))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation(), methodDeclarationSyntax.Identifier.Text));
            }
        }

        private bool HasMultipleIndentations(SyntaxList<StatementSyntax> statements)
        {
            foreach (var block in GetBlocksWhichShouldNotContainControlStructures(statements))
            {
                if (ContainsControlStructure(block))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<BlockSyntax> GetBlocksWhichShouldNotContainControlStructures(SyntaxList<StatementSyntax> statements)
        {
            foreach (var statementSyntax in statements)
            {
                var block = (statementSyntax as ForStatementSyntax)?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as ForEachStatementSyntax)?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as WhileStatementSyntax)?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as DoStatementSyntax)?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as IfStatementSyntax)?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as IfStatementSyntax)?.Else?.Statement as BlockSyntax;
                if (block != null)
                {
                    yield return block;
                }

                block = (statementSyntax as TryStatementSyntax)?.Block;
                if (block != null)
                {
                    yield return block;
                }

                foreach (var tryStatement in (statementSyntax as TryStatementSyntax)?.Catches ?? new SyntaxList<CatchClauseSyntax>())
                {
                    block = tryStatement.Block;
                    if (block != null)
                    {
                        yield return block;
                    }
                }

                block = (statementSyntax as TryStatementSyntax)?.Finally?.Block;
                if (block != null)
                {
                    yield return block;
                }
            }
        }

        private bool ContainsControlStructure(BlockSyntax block)
        {
            foreach (var statementSyntax in block.Statements)
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
                                 typeof(IfStatementSyntax),
                                 typeof(TryStatementSyntax)
                             };

            return controlStructures.Contains(type);
        }
    }
}
