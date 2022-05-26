using System.Collections.Generic;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProviderO8)), Shared]
    public class CodeFixProviderO8 : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerO8.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var methodIdentifier = "ToString";
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var invocation = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<InvocationExpressionSyntax>().First();

            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken);

            var toStringPosition = invocation.Expression.ChildNodes().OfType<IdentifierNameSyntax>()
                .TakeWhile(identifier => !identifier.Identifier.ValueText.Equals(methodIdentifier)).Count();

            var objectIdentifier =
                invocation.Expression.ChildNodes().OfType<IdentifierNameSyntax>().ToList()[toStringPosition - 1];

            var objectType = semanticModel.GetTypeInfo(objectIdentifier).Type;

            var syntaxReference = objectType.DeclaringSyntaxReferences.FirstOrDefault();
            if (!(await syntaxReference.GetSyntaxAsync(context.CancellationToken) is ClassDeclarationSyntax
                    declaration))
                return;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixO8Title,
                    createChangedDocument: c => GenerateToStringOverride(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixO8Title)),
                diagnostic);
        }

        private async Task<Document> GenerateToStringOverride(Document contextDocument,
            ClassDeclarationSyntax classDeclaration, CancellationToken cancellationToken)
        {
            var tree = await contextDocument.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;

            var lastChild = classDeclaration.ChildNodes().Last();

            CompilationUnitSyntax newRoot;
            if (lastChild == null)
            {
                newRoot = root.ReplaceNode(classDeclaration,
                    GetClassDeclarationWithToStringOverride(classDeclaration.Identifier.Text));
            }
            else
            {
                var nodesToInsert = new List<SyntaxNode> {GetToStringDeclaration()};
                newRoot = root.InsertNodesAfter(lastChild, nodesToInsert).NormalizeWhitespace();
            }


            return contextDocument.WithSyntaxRoot(newRoot);
        }

        private MethodDeclarationSyntax GetToStringDeclaration()
        {
            return
                SyntaxFactory.MethodDeclaration
                    (
                        SyntaxFactory.PredefinedType
                        (
                            SyntaxFactory.Token(SyntaxKind.StringKeyword)
                        ),
                        SyntaxFactory.Identifier("ToString")
                    )
                    .WithModifiers
                    (
                        SyntaxFactory.TokenList
                        (
                            new[]
                            {
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                                SyntaxFactory.Token(SyntaxKind.OverrideKeyword)
                            }
                        )
                    )
                    .WithBody
                    (
                        SyntaxFactory.Block
                        (
                            SyntaxFactory.SingletonList<StatementSyntax>
                            (
                                SyntaxFactory.ReturnStatement
                                (
                                    SyntaxFactory.InvocationExpression
                                    (
                                        SyntaxFactory.MemberAccessExpression
                                        (
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.BaseExpression(),
                                            SyntaxFactory.IdentifierName("ToString")
                                        )
                                    )
                                )
                            )
                        )
                    );
        }

        private ClassDeclarationSyntax GetClassDeclarationWithToStringOverride(string classIdentifier)
        {
            return SyntaxFactory.ClassDeclaration(classIdentifier)
                .WithMembers
                (
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>
                    (
                        GetToStringDeclaration()
                    )
                );
        }
    }
}
