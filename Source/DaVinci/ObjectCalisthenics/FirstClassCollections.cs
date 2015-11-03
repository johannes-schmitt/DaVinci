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
                var collection = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().Select(f =>
                    new
                    {
                        IsCollection = IsCollection(f, context.SemanticModel),
                        Syntax = f
                    }
                );

                VerifyRule(context, collection);
            }
        }

        private bool IsCollection(FieldDeclarationSyntax field, SemanticModel semanticModel)
        {
            return field.ImplementsInterface<IEnumerable>(semanticModel);
        }

        private void VerifyRule(SemanticModelAnalysisContext context, IEnumerable<dynamic> fieldsInfo)
        {
            foreach (var violatedField in GetRuleViolations(fieldsInfo.ToList()))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, violatedField.Declaration.Variables[0].Identifier.GetLocation()));
            }
        }

        private IEnumerable<FieldDeclarationSyntax> GetRuleViolations(IList<dynamic> fieldsInfo)
        {
            if (fieldsInfo.Any(f => !f.IsCollection))
            {
                return fieldsInfo.Where(f => f.IsCollection).Select(f => f.Syntax as FieldDeclarationSyntax);
            }

            if (fieldsInfo.Any(f => f.IsCollection))
            {
                return fieldsInfo.Where(f => f.IsCollection).Skip(1).Select(f => f.Syntax as FieldDeclarationSyntax);
            }

            return new List<FieldDeclarationSyntax>();
        }
    }
}
