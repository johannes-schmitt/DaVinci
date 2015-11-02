using System;
using System.Linq;

using DaVinci.Extensions.Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class KeepAllEntitiesSmallAnalyzer : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.7";
        protected override LocalizableString Title => "Keep all entities small (Object Calisthenics Rule #7).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than {2} lines ({1}).";
        protected override LocalizableString Description => "Rule #7 of Object Calisthenics is \"Keep all entities small\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;
        protected override Uri HelpUri => null;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private async void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var root = await context.Tree.GetRootAsync();
            AnalyzeSizeOfSyntaxNodes<ClassDeclarationSyntax>(context, root, 50);
            AnalyzeSizeOfSyntaxNodes<StructDeclarationSyntax>(context, root, 50);
            AnalyzeSizeOfSyntaxNodes<MethodDeclarationSyntax>(context, root, 15);
        }

        private void AnalyzeSizeOfSyntaxNodes<T>(SyntaxTreeAnalysisContext context, SyntaxNode root, int maxNumberOfLines) where T : SyntaxNode
        {
            foreach (var syntaxNode in root.DescendantNodesAndSelf().OfType<T>())
            {
                AnalyzeSizeOfSyntaxNode(context, syntaxNode, maxNumberOfLines);
            }
        }

        private void AnalyzeSizeOfSyntaxNode(SyntaxTreeAnalysisContext context, SyntaxNode syntaxNode, int maxNumberOfLines)
        {
            var numberOfLines = syntaxNode.GetNumberOfLines();
            if (numberOfLines > maxNumberOfLines)
            {
                ReportDiagnostic(context, syntaxNode, maxNumberOfLines, numberOfLines);
            }
        }

        private void ReportDiagnostic(SyntaxTreeAnalysisContext context, dynamic syntaxNode, int maxNumberOfLines, int numberOfLines)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, syntaxNode.Identifier.GetLocation(), syntaxNode.Identifier.Text, numberOfLines, maxNumberOfLines));
        }
    }
}
