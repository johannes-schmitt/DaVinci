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
            var variableType = semanticModel.GetTypeInfo(variableDeclaration.Type).Type;
            return variableType.ImplementsInterface<T>();
        }
    }
}
