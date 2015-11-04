﻿using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Oc5UseOneDotPerLine : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.5";
        protected override LocalizableString Title => "Use one dot per line (Object Calisthenics Rule #6).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than 1 dot per line.";
        protected override LocalizableString Description => "Rule #5 of Object Calisthenics is \"Use one dot per line\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeLine, SyntaxKind.SimpleMemberAccessExpression);
        }

        private void AnalyzeLine(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.DescendantTokens().Count(st => st.IsKind(SyntaxKind.DotToken)) > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), context.Node.Parent.GetText()));
            }
        }
    }
}
