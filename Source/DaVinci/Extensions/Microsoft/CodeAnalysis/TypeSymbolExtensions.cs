using System.Linq;

using Microsoft.CodeAnalysis;

namespace DaVinci.Extensions.Microsoft.CodeAnalysis
{
    public static class TypeSymbolExtensions
    {
        public static bool ImplementsInterface<T>(this ITypeSymbol typeSymbol)
        {
            return typeSymbol.AllInterfaces
                .Select(@interface => @interface.ToFullQualifiedName())
                .Any(fullQualifiedInterfaceName => fullQualifiedInterfaceName == typeof(T).FullName);
        }
    }
}
