using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NoClassesWithMoreThanTwoFieldsAnalyzer : BaseDiagnosticAnalyzer
    {
        private const int MaximumNumberOfFields = 2;

        protected override string DiagnosticId => "DaVinci.OC.8";
        protected override LocalizableString Title => "A Class should not have more than " + MaximumNumberOfFields + " fields (Object Calisthenics Rule #8).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than " + MaximumNumberOfFields + " fields ({1}).";
        protected override LocalizableString Description => "Rule #8 of Object Calisthenics is \"No classes with more than " + MaximumNumberOfFields + " instance variables\".";
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
                AnalyzeClass(context, @class);
            }
        }

        private void AnalyzeClass(SyntaxTreeAnalysisContext context, ClassDeclarationSyntax @class)
        {
            var numberOfFields = @class.Members.Count(member => member is FieldDeclarationSyntax);
            if (numberOfFields > MaximumNumberOfFields)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, @class.Identifier.GetLocation(), @class.Identifier.Text, numberOfFields));
            }
        }
    }
}