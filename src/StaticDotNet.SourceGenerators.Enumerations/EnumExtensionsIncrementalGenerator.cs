using Microsoft.CodeAnalysis.CSharp;
using StaticDotNet.SourceGenerators.Enumerations.Source;
using System;
using System.Diagnostics;
using System.Threading;

namespace StaticDotNet.SourceGenerators.Enumerations;

[Generator]
public sealed class EnumExtensionsIncrementalGenerator
: IIncrementalGenerator {

	private const string ENUM_EXTENSIONS_ATTRIBUTES_FULL_NAME = "StaticDotNet.SourceGenerators.Enumerations.GenerateEnumExtensionsAttribute";

	private static readonly DiagnosticDescriptor stopwatchDiagnositcDescriptor = new( "stopwatch", "Stop Wiatch", "Time Taken: {0}ms", "Performance", DiagnosticSeverity.Warning, true );

	public void Initialize( IncrementalGeneratorInitializationContext context ) {

		IncrementalValuesProvider<EnumDescriptor> enumDescriptors = context.SyntaxProvider
			.ForAttributeWithMetadataName( ENUM_EXTENSIONS_ATTRIBUTES_FULL_NAME, ( node, _ ) => node is EnumDeclarationSyntax, GetEnumDescriptor );

		context.RegisterSourceOutput( enumDescriptors, static ( sourceProductionContext, currentEnumDescriptor ) => {

			//#if DEBUG
			//			if( !System.Diagnostics.Debugger.IsAttached ) {
			//				System.Diagnostics.Debugger.Launch();
			//			}
			//#endif

			var stopWatch = Stopwatch.StartNew();

			string fileNamePrefix = currentEnumDescriptor.Namespace.Length > 0
				? $"{currentEnumDescriptor.Namespace.Replace( '.', '_' )}_{currentEnumDescriptor.Name}"
				: currentEnumDescriptor.Name;

			string enumValueSource = EnumValueSourceGenerator.Generate( in currentEnumDescriptor );
			sourceProductionContext.AddSource( $"{fileNamePrefix}EnumValue.g.cs", SourceText.From( enumValueSource, Encoding.UTF8 ) );

			//string enumExtensionsSourceCode = EnumExtensionsSourceGenerator.Generate( in currentEnumDescriptor );
			//sourceProductionContext.AddSource( $"{fileNamePrefix}Extensions.g.cs", SourceText.From( enumExtensionsSourceCode, Encoding.UTF8 ) );

			stopWatch.Stop();

			var stopWatchDiagnostic = Diagnostic.Create( stopwatchDiagnositcDescriptor, Location.None, stopWatch.ElapsedMilliseconds );

			sourceProductionContext.ReportDiagnostic( stopWatchDiagnostic );
		} );
	}

	private static EnumDescriptor GetEnumDescriptor( GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken ) {

		if( context.TargetSymbol is not INamedTypeSymbol enumSymbol ) {
			throw new InvalidOperationException();
		}

		cancellationToken.ThrowIfCancellationRequested();

		CSharpCompilation compilation = context.SemanticModel.Compilation as CSharpCompilation ?? throw new InvalidOperationException();

		return EnumDescriptor.FromINamedTypeSymbol( enumSymbol, compilation );
	}
}
