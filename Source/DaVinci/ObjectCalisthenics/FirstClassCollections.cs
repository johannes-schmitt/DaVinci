using System.Collections.Generic;
using System.Linq;

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
                var collectionFields = new List<FieldDeclarationSyntax>();
                var nonCollectionFields = new List<FieldDeclarationSyntax>();
                foreach (var fieldDeclaration in classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    foreach (var variableDeclaration in fieldDeclaration.DescendantNodes().OfType<VariableDeclarationSyntax>())
                    {
                        var isCollectionField = false;
                        var allInterfaces = context.SemanticModel.GetTypeInfo(variableDeclaration.Type).Type.AllInterfaces;

                        foreach (var @interface in allInterfaces)
                        {
                            var fullQualifiedInterfaceName = @interface.ToDisplayString(new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces));
                            if (fullQualifiedInterfaceName == "System.Collections.IEnumerable")
                            {
                                isCollectionField = true;
                            }
                        }

                        if (isCollectionField)
                        {
                            collectionFields.Add(fieldDeclaration);
                        }
                        else
                        {
                            nonCollectionFields.Add(fieldDeclaration);
                        }
                    }
                }

                if (nonCollectionFields.Any())
                {
                    foreach (var collectionField in collectionFields)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, collectionField.Declaration.Variables[0].Identifier.GetLocation()));
                    }
                }
            }
        }
    }
}
