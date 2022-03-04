using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MemberAccessExpressionSyntaxAnalyzer: DiagnosticAnalyzer
    {
        private const string Title = "Declare explicit type for local declarations.";
        public const string MessageFormat = "Local '{0}' is implicitly typed. Consider specifying its type explicitly in the declaration.";
        private const string Description = "Declare explicit type for local declarations.";

        internal static DiagnosticDescriptor Rule =
                new DiagnosticDescriptor(DiagnosticIds.SyntaxNodeAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    DiagnosticCategories.Stateless,
                    DiagnosticSeverity.Error,
                    true,
                    Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            MemberAccessExpressionSyntax memberAccessExpression = (MemberAccessExpressionSyntax)context.Node;

            var identifierSyntaxNodes = memberAccessExpression.ChildNodes().Where(x => x.Kind() == SyntaxKind.IdentifierName);

            var node = memberAccessExpression.ChildNodes()?.OfType<IdentifierNameSyntax>()?.Last();
            if (node==null)
            {
                return;
            }
     
            ISymbol symbool = context.SemanticModel.GetDeclaredSymbol(node);
            if (symbool.Kind != SymbolKind.Field)
            {
                return;
            }
            
            
            
            
            // Find implicitly typed variable declarations.
            VariableDeclarationSyntax declaration = (VariableDeclarationSyntax)context.Node;
            if (declaration.Type.IsVar)
            {
                foreach (VariableDeclaratorSyntax variable in declaration.Variables)
                {
                    // For all such locals, report a diagnostic.
                    context.ReportDiagnostic(Diagnostic.Create(Rule,
                        variable.GetLocation(),
                        variable.Identifier.ValueText));
                }
            }
        }
    }
}