using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Oc3WrapAllPrimitivesAndStrings : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinciOC3";
        protected override LocalizableString Title => "Wrap all primitives and strings (Object Calisthenics Rule #3).";
        protected override LocalizableString MessageFormat => "'{0}' should be wrapped as it's a primitive.";
        protected override LocalizableString Description => "Rule #3 of Object Calisthenics is \"Wrap all primitives and strings\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclarationSyntax = context.Node as MethodDeclarationSyntax;

            if (methodDeclarationSyntax == null)
            {
                return;
            }

            foreach (var parameterSyntax in methodDeclarationSyntax.DescendantNodes().OfType<ParameterSyntax>())

            {
                var predefinedTypeSyntax = parameterSyntax.Type as PredefinedTypeSyntax;
                if (predefinedTypeSyntax != null && predefinedTypeSyntax.Keyword.Text != "object")
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, parameterSyntax.Identifier.GetLocation(), parameterSyntax.Identifier.Text));
                }
            }
        }
    }
}
