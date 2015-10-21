using System.Linq;

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
        protected override LocalizableString MessageFormat => "'{0}' contains more than 50 lines ({1}).";
        protected override LocalizableString Description => "Rule #7 of Object Calisthenics is \"Keep all entities small\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

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
            var classSpan = @class.SyntaxTree.GetMappedLineSpan(@class.Span);
            var numberOfLines = classSpan.EndLinePosition.Line - classSpan.StartLinePosition.Line + 1;
            if (numberOfLines > 50)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, @class.Identifier.GetLocation(), @class.Identifier.Text, numberOfLines));
            }
        }
    }
}
