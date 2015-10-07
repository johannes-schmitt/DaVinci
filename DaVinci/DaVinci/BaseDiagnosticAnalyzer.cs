using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci
{
    public abstract class BaseDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        asdfasdf
        as
            asdfsd
        protected abstract string DiagnosticId { get; }
        protected abstract LocalizableString Title { get; }
        protected abstract LocalizableString MessageFormat { get; }
        protected abstract LocalizableString Description { get; }
        protected abstract string Category { get; }
        protected abstract DiagnosticSeverity DefaultSeverity { get; }

        private DiagnosticDescriptor rule;

        internal DiagnosticDescriptor Rule
            =>
                this.rule
                ?? (this.rule =
                    new DiagnosticDescriptor(
                        DiagnosticId,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Warning,
                        true,
                        Description));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}