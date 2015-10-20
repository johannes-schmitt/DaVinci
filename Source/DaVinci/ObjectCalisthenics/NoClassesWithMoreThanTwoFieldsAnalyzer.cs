using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NoClassesWithMoreThanTwoFieldsAnalyzer : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.8";
        protected override LocalizableString Title => "A Class should not have more than two fields (Object Calisthenics Rule #8).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than two fields ({1}).";
        protected override LocalizableString Description => "Rule #8 of Object Calisthenics is \"No classes with more than two instance variables\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Warning;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private async void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var root = await context.Tree.GetRootAsync();
            var classes = root.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>();
            foreach (var @class in classes)
            {
                var numberOfFields = @class.Members.Count(member => member is FieldDeclarationSyntax);
                if (numberOfFields > 2)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, @class.Identifier.GetLocation(), @class.Identifier.Text, numberOfFields));
                }
            }
        }
    }
}