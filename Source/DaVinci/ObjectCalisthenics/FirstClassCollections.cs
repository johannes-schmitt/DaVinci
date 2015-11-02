using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DaVinci.Extensions.Microsoft.CodeAnalysis.CSharp.Syntax;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.ObjectCalisthenics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FirstClassCollections : BaseDiagnosticAnalyzer
    {
        protected override string DiagnosticId => "DaVinci.OC.4";
        protected override LocalizableString Title => "First class collections (Object Calisthenics Rule #4)";
        protected override LocalizableString MessageFormat => "Consider wrapping the collection into a separate class.";
        protected override LocalizableString Description => "Rule #4 of Object Calisthenics is \"First class collections\".";
        protected override string Category => "Object Calisthenics";
        protected override DiagnosticSeverity DefaultSeverity => DiagnosticSeverity.Info;

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSemanticModelAction(AnalyzeSemanticModel);
        }

        private async void AnalyzeSemanticModel(SemanticModelAnalysisContext context)
        {
            var root = await context.SemanticModel.SyntaxTree.GetRootAsync();
            foreach (var classDeclaration in root.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>())
            {
                var fields = new Dictionary<FieldDeclarationType, IList<FieldDeclarationSyntax>>();
                fields[FieldDeclarationType.Collection] = new List<FieldDeclarationSyntax>();
                fields[FieldDeclarationType.NonCollection] = new List<FieldDeclarationSyntax>();

                foreach (var fieldDeclaration in classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    var fieldType = fieldDeclaration.ImplementsInterface<IEnumerable>(context.SemanticModel)
                                        ? FieldDeclarationType.Collection
                                        : FieldDeclarationType.NonCollection;

                    fields[fieldType].Add(fieldDeclaration);
                }

                VerifyRule(context, fields);
            }
        }

        private void VerifyRule(SemanticModelAnalysisContext context, IDictionary<FieldDeclarationType, IList<FieldDeclarationSyntax>> fields)
        {
            foreach (var violation in GetRuleViolations(fields))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, violation.Declaration.Variables[0].Identifier.GetLocation()));
            }
        }

        private IEnumerable<FieldDeclarationSyntax> GetRuleViolations(IDictionary<FieldDeclarationType, IList<FieldDeclarationSyntax>> fields)
        {
            if (fields[FieldDeclarationType.NonCollection].Any())
            {
                return fields[FieldDeclarationType.Collection];
            }

            if (fields[FieldDeclarationType.Collection].Any())
            {
                return fields[FieldDeclarationType.Collection].Skip(1);
            }

            return new List<FieldDeclarationSyntax>();
        }

        private enum FieldDeclarationType
        {
            Collection,
            NonCollection
        }
    }
}
