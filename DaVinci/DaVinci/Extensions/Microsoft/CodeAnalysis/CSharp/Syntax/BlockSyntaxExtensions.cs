using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DaVinci.Extensions.Microsoft.CodeAnalysis.CSharp.Syntax
{
    internal static class BlockSyntaxExtensions
    {
        public static IEnumerable<BlockSyntax> GetSubBlocks(this BlockSyntax block)
        {
            return block.GetNullableSubBlocks().Where(b => b != null);
        }

        private static IEnumerable<BlockSyntax> GetNullableSubBlocks(this BlockSyntax block)
        {
            foreach (var statementSyntax in block.Statements)
            {
                yield return (statementSyntax as ForStatementSyntax)?.Statement as BlockSyntax;
                yield return (statementSyntax as ForEachStatementSyntax)?.Statement as BlockSyntax;
                yield return (statementSyntax as WhileStatementSyntax)?.Statement as BlockSyntax;
                yield return (statementSyntax as DoStatementSyntax)?.Statement as BlockSyntax;
                yield return (statementSyntax as IfStatementSyntax)?.Statement as BlockSyntax;
                yield return (statementSyntax as IfStatementSyntax)?.Else?.Statement as BlockSyntax;
                yield return (statementSyntax as TryStatementSyntax)?.Block;
                yield return (statementSyntax as TryStatementSyntax)?.Finally?.Block;

                foreach (var tryStatement in (statementSyntax as TryStatementSyntax)?.Catches ?? new SyntaxList<CatchClauseSyntax>())
                {
                    yield return tryStatement.Block;
                }
            }
        }
    }
}
