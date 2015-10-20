using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DaVinci.Extensions
{
    internal static class BlockSyntaxExtensions
    {
        public static IEnumerable<BlockSyntax> GetSubBlocks(this BlockSyntax block)
        {
            foreach (var statementSyntax in block.Statements)
            {
                var subBlock = (statementSyntax as ForStatementSyntax)?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as ForEachStatementSyntax)?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as WhileStatementSyntax)?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as DoStatementSyntax)?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as IfStatementSyntax)?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as IfStatementSyntax)?.Else?.Statement as BlockSyntax;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                subBlock = (statementSyntax as TryStatementSyntax)?.Block;
                if (subBlock != null)
                {
                    yield return subBlock;
                }

                foreach (var tryStatement in (statementSyntax as TryStatementSyntax)?.Catches ?? new SyntaxList<CatchClauseSyntax>())
                {
                    subBlock = tryStatement.Block;
                    if (subBlock != null)
                    {
                        yield return subBlock;
                    }
                }

                subBlock = (statementSyntax as TryStatementSyntax)?.Finally?.Block;
                if (subBlock != null)
                {
                    yield return subBlock;
                }
            }
        }
    }
}
