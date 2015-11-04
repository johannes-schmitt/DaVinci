using Microsoft.CodeAnalysis;

namespace DaVinci.Extensions.Microsoft.CodeAnalysis
{
    public static class SymbolExtensions
    {
        public static string ToFullQualifiedName(this ISymbol symbol)
        {
            var fullQualifiedNameFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
            return symbol.ToDisplayString(fullQualifiedNameFormat);
        }
    }
}
