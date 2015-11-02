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
                var fields = new List< FieldDeclarationsInfo >();
 
                foreach (var fieldDeclaration in classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    fields.Add(new FieldDeclarationsInfo
                    {
                        Type = fieldDeclaration.ImplementsInterface<IEnumerable>(context.SemanticModel)
                                        ? FieldDeclarationType.Collection
                                        : FieldDeclarationType.NonCollection,
                        Syntax = fieldDeclaration
                    });
                }

                VerifyRule(context, fields);
            }
        }

        private void VerifyRule(SemanticModelAnalysisContext context, ICollection<FieldDeclarationsInfo> fields)
        {
            foreach (var violation in GetRuleViolations(fields))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, violation.Declaration.Variables[0].Identifier.GetLocation()));
            }
        }

        private IEnumerable<FieldDeclarationSyntax> GetRuleViolations(ICollection<FieldDeclarationsInfo> fields)
        {
            
            if (fields.Where( f => f.Type == FieldDeclarationType.NonCollection).Any())
            {
                return fields.Where(f => f.Type == FieldDeclarationType.Collection).Select(f => f.Syntax);
            }

            if (fields.Where(f => f.Type == FieldDeclarationType.Collection).Any())
            {
                return fields.Where(f => f.Type == FieldDeclarationType.Collection).Skip(1).Select(f => f.Syntax);
            }

            return new List<FieldDeclarationSyntax>();
        }

        internal enum FieldDeclarationType
        {
            Collection,
            NonCollection
        }

        internal class FieldDeclarationsInfo
        {
            public FieldDeclarationType Type;
            public FieldDeclarationSyntax Syntax;
        }
    }
}
