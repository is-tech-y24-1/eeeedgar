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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProviderI7Array)), Shared]
    public class CodeFixProviderI7Array : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerI7Array.DiagnosticId); }
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
            var returnStatement = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<ReturnStatementSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixI7Title,
                    // createChangedSolution: c => MakeUppercaseAsync(context.Document, declaration, c),
                    createChangedDocument: c => ChangeNullToEmptyCollectionAsync(context.Document, returnStatement, c), 
                    equivalenceKey: nameof(CodeFixResources.CodeFixI7Title)),
                diagnostic);
        }

        private async Task<Document> ChangeNullToEmptyCollectionAsync(Document contextDocument, ReturnStatementSyntax returnStatement, CancellationToken cancellationToken)
        {
            var tree = await contextDocument.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;

            var method = returnStatement.Ancestors().OfType<MethodDeclarationSyntax>().First();

            var returnType = method.ReturnType.DescendantNodesAndSelf().OfType<ArrayTypeSyntax>().First();

            var newReturnExpression = SyntaxFactory.InvocationExpression
            (
                SyntaxFactory.MemberAccessExpression
                (
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("Array"),
                    SyntaxFactory.GenericName
                        (
                            SyntaxFactory.Identifier("Empty")
                        )
                        .WithTypeArgumentList
                        (
                            SyntaxFactory.TypeArgumentList
                            (
                                
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>
                                (
                                    SyntaxFactory.PredefinedType
                                    (
                                        SyntaxFactory.ParseToken(returnType.ElementType.ToString())
                                    )
                                )
                            )
                        )
                )
            );

            var newRoot = root.ReplaceNode(returnStatement.Expression, newReturnExpression);

            return contextDocument.WithSyntaxRoot(newRoot);
        }
    }
}
