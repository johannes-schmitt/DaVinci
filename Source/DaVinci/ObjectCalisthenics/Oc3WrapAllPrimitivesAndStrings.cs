using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Oc3WrapAllPrimitivesAndStrings : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.3";
        protected override LocalizableString Title => "Wrap all primitives and strings (Object Calisthenics Rule #3).";
        protected override LocalizableString MessageFormat => "";
        protected override LocalizableString Description => "Rule #3 of Object Calisthenics is \"Wrap all primitives and strings\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
        }
    }
}
