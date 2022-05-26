using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalyzerTemplate
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProviderI7Yield)), Shared]
    public class CodeFixProviderI7Yield : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerI7Yield.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var yieldStatement = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<YieldStatementSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixI7YieldTitle,
                    createChangedDocument: c => ChangeNullToBreakAsync(context.Document, yieldStatement, c), 
                    equivalenceKey: nameof(CodeFixResources.CodeFixI7YieldTitle)),
                diagnostic);
        }

        private async Task<Document> ChangeNullToBreakAsync(Document contextDocument, YieldStatementSyntax yieldStatement, CancellationToken cancellationToken)
        {
            var tree = await contextDocument.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;

            var newRoot = root.ReplaceNode(yieldStatement, GetYieldBreakStatement());

            return contextDocument.WithSyntaxRoot(newRoot);
        }

        private YieldStatementSyntax GetYieldBreakStatement()
        {
            return SyntaxFactory.YieldStatement
            (
                SyntaxKind.YieldBreakStatement
            );
        }
    }
}
