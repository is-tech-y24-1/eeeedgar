using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerO8 : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerO8";

        private static readonly LocalizableString Title = new LocalizableResourceString(
            nameof(Resources.AnalyzerO8Title),
            Resources.ResourceManager,
            typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
            nameof(Resources.AnalyzerO8MessageFormat),
            Resources.ResourceManager,
            typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(
            nameof(Resources.AnalyzerO8Description),
            Resources.ResourceManager,
            typeof(Resources));


        private const string Category = "Refactoring";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            
            context.RegisterSyntaxNodeAction(AnalyzeNode, InvocationExpression);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            const string methodIdentifier = "ToString";
            var invocation = (InvocationExpressionSyntax) context.Node;

            var toStringPosition = invocation.Expression.ChildNodes().OfType<IdentifierNameSyntax>()
                .TakeWhile(identifier => !identifier.Identifier.ValueText.Equals(methodIdentifier)).Count();

            var toStringIdentifier =
                invocation.Expression.ChildNodes().OfType<IdentifierNameSyntax>().ToList()[toStringPosition];


            if (toStringPosition == 0)
                // todo invocation in a class declaration
                return;

            var objectIdentifier =
                invocation.Expression.ChildNodes().OfType<IdentifierNameSyntax>().ToList()[toStringPosition - 1];

            var objectType = context.SemanticModel.GetTypeInfo(objectIdentifier).Type;

            var syntaxReference = objectType.DeclaringSyntaxReferences.FirstOrDefault();

            if (!(syntaxReference.GetSyntax(context.CancellationToken) is ClassDeclarationSyntax declaration))
                return;

            if (declaration.ChildNodes().OfType<MethodDeclarationSyntax>()
                .Any(m => m.Identifier.Text.Equals(methodIdentifier)))
                return;


            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation());
            context.ReportDiagnostic(diagnostic);

            // todo check type's base types
            // while (type != null)
            // {
            //     ...
            //     type = type.BaseType;
            // }
            //
        }
    }
}