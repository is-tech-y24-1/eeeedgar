using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerI7Array : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerI7Array";
        
        private static readonly LocalizableString Title = new LocalizableResourceString(
            nameof(Resources.AnalyzerI7Title),
            Resources.ResourceManager,
            typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
            nameof(Resources.AnalyzerI7MessageFormat),
            Resources.ResourceManager,
            typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(
            nameof(Resources.AnalyzerI7Description),
            Resources.ResourceManager,
            typeof(Resources));
        private const string Category = "Refactoring";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            
            
            context.RegisterSyntaxNodeAction(AnalyzeNode, ReturnStatement);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var returnStatement = (ReturnStatementSyntax) context.Node;

            if (returnStatement.Expression is null)
                return;
            
            if (!returnStatement.Expression.IsKind(NullLiteralExpression))
                return;
            
            
            var method = returnStatement.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            if (method is null)
                return;
            
            var arrayType = method.ReturnType.DescendantNodesAndSelf().OfType<ArrayTypeSyntax>().FirstOrDefault();
            if (arrayType is null)
                return;

            var diagnostic = Diagnostic.Create(Rule, returnStatement.Expression.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
