using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DaVinci.Extensions.Microsoft.CodeAnalysis.CSharp.Syntax
{
    internal static class FieldDeclarationSyntaxExtensions
    {
        public static bool ImplementsInterface<T>(this FieldDeclarationSyntax field, SemanticModel semanticModel)
        {
            var variableDeclaration = field.DescendantNodes().OfType<VariableDeclarationSyntax>().First();
            return IsCollectionField<T>(semanticModel, variableDeclaration);
        }

        private static bool IsCollectionField<T>(SemanticModel semanticModel, VariableDeclarationSyntax variableDeclaration)
        {
            var allInterfaces = semanticModel.GetTypeInfo(variableDeclaration.Type).Type.AllInterfaces;

            foreach (var @interface in allInterfaces)
            {
                var fullQualifiedInterfaceName =
                    @interface.ToDisplayString(
                        new SymbolDisplayFormat(
                            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces));

                if (fullQualifiedInterfaceName == typeof(T).FullName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
