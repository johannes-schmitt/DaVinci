using System;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci
{
    public abstract class BaseDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        protected abstract string DiagnosticId { get; }
        protected abstract LocalizableString Title { get; }
        protected abstract LocalizableString MessageFormat { get; }
        protected abstract LocalizableString Description { get; }
        protected abstract string Category { get; }
        protected abstract DiagnosticSeverity DefaultSeverity { get; }

        private Uri HelpUri => new Uri($"https://github.com/johannesschmitt/DaVinci/blob/master/Documentation/{DiagnosticId}.md");

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
                        DefaultSeverity,
                        true,
                        Description,
                        HelpUri?.AbsoluteUri));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}