using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using DaVinci.Extensions.Microsoft.CodeAnalysis.Diagnostics;
using DaVinci.Extensions.Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseOneLevelOfIndentationPerMethod : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.1";
        protected override LocalizableString Title => "Use one level of indentation per method (Object Calisthenics Rule #1).";
        protected override LocalizableString MessageFormat => "'{0}' contains more than 1 level of indentation.";
        protected override LocalizableString Description => "Rule #1 of Object Calisthenics is \"Use one level of indentation per method\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;
        protected override Uri HelpUri => new Uri("https://github.com/johannesschmitt/DaVinci/blob/master/Documentation/DaVinci.OC.1.md");

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCodeBlockAction<MethodDeclarationSyntax>(AnalyzeMethodCodeBlock);
        }

        private void AnalyzeMethodCodeBlock(CodeBlockAnalysisContext context, MethodDeclarationSyntax methodDeclarationSyntax)
        {
            if (HasMultipleIndentations(methodDeclarationSyntax.Body))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation(), methodDeclarationSyntax.Identifier.Text));
            }
        }

        private bool HasMultipleIndentations(BlockSyntax block)
        {
            return block.GetSubBlocks().Any(subBlock => subBlock.GetSubBlocks().Any());
        }
    }
}
