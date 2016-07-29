using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Oc9NoProperties : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinciOC9";
        protected override LocalizableString Title => "Properties are not allowed (Object Calisthenics Rule #9)";
        protected override LocalizableString MessageFormat => "'{0}' is a property and should be avoided.";
        protected override LocalizableString Description => "Rule #9 of Object Calisthenics is \"No getters/setters/properties\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var propertySymbol = (IPropertySymbol)context.Symbol;

            var diagnostic = Diagnostic.Create(Rule, propertySymbol.Locations[0], propertySymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
