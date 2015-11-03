using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Oc2DoNotUseTheElseKeyword : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.2";
        protected override LocalizableString Title => "Don’t use the else keyword (Object Calisthenics Rule #2)";
        protected override LocalizableString MessageFormat => "The else keyword should be avoided.";
        protected override LocalizableString Description => "Rule #2 of Object Calisthenics is \"Don’t use the else keyword\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCodeBlockAction(AnalyzeCodeBlock);
        }

        private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        {
            foreach (var elseClause in context.CodeBlock.DescendantNodes().OfType<ElseClauseSyntax>())
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, elseClause.GetLocation()));
            }
        }
    }
}
