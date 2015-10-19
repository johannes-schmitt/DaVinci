using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DaVinci.Extensions
{
    internal static class AnalysisContextExtensions
    {
        public static void RegisterCodeBlockAction<T>(this AnalysisContext context, Action<CodeBlockAnalysisContext, T> action) where T : SyntaxNode
        {
            context.RegisterCodeBlockAction((analysisContext =>
                {
                    var codeBlock = analysisContext.CodeBlock as T;
                    if (codeBlock != null)
                    {
                        action(analysisContext, codeBlock);
                    }
                }));
        }
    }
}
