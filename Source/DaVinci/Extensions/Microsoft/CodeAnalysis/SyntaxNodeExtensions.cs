using Microsoft.CodeAnalysis;

namespace DaVinci.Extensions.Microsoft.CodeAnalysis
{
    internal static class SyntaxNodeExtensions
    {
        public static int GetNumberOfLines(this SyntaxNode node)
        {
            var nodeSpan = node.SyntaxTree.GetMappedLineSpan(node.Span);
            var numberOfLines = nodeSpan.EndLinePosition.Line - nodeSpan.StartLinePosition.Line + 1;
            return numberOfLines;
        }
    }
}
