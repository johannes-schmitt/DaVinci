using System;
using System.Collections.Generic;
using System.Linq;

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
                if (ContainsNestedControlFlows(methodDeclarationSyntax))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation(), methodDeclarationSyntax.Identifier.Text));
                }
            }
        }

        private bool ContainsNestedControlFlows(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            foreach (var statement in methodDeclarationSyntax.Body.Statements.OfType<ForStatementSyntax>())
            {
                if (ContainsControlStructure((BlockSyntax)statement.Statement))
                {
                    return true;
                }
            }

            foreach (var statement in methodDeclarationSyntax.Body.Statements.OfType<ForEachStatementSyntax>())
            {
                if (ContainsControlStructure((BlockSyntax)statement.Statement))
                {
                    return true;
                }
            }

            foreach (var statement in methodDeclarationSyntax.Body.Statements.OfType<WhileStatementSyntax>())
            {
                if (ContainsControlStructure((BlockSyntax)statement.Statement))
                {
                    return true;
                }
            }

            return false;
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
                                 typeof(WhileStatementSyntax)
                             };

            return controlStructures.Contains(type);
        }
    }
}
