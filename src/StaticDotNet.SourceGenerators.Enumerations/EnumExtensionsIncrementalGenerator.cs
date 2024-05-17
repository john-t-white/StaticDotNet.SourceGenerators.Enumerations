using Microsoft.CodeAnalysis;
using StaticDotNet.SourceGenerators.Enumerations.Source;
using System.Diagnostics;

namespace StaticDotNet.SourceGenerators.Enumerations;

[Generator]
public sealed class EnumExtensionsIncrementalGenerator
: IIncrementalGenerator {

	private const string ENUM_EXTENSIONS_ATTRIBUTES_FULL_NAME = "StaticDotNet.SourceGenerators.Enumerations.GenerateEnumExtensionsAttribute";

	private static readonly DiagnosticDescriptor stopwatchDiagnositcDescriptor = new( "stopwatch", "Stop Wiatch", "Time Taken: {0}ms Date Ran: {1}", "Performance", DiagnosticSeverity.Warning, true );

	public void Initialize( IncrementalGeneratorInitializationContext context ) {

		IncrementalValuesProvider<EnumDescriptor> enumDescriptors = context.SyntaxProvider
			.ForAttributeWithMetadataName( ENUM_EXTENSIONS_ATTRIBUTES_FULL_NAME, ( node, _ ) => node is EnumDeclarationSyntax, GetEnumDescriptors );

		context.RegisterSourceOutput( enumDescriptors, GenerateSource );
	}

	private static EnumDescriptor GetEnumDescriptors( GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken ) {

		if( context.TargetSymbol is not INamedTypeSymbol enumSymbol ) {
			throw new InvalidOperationException();
		}

		cancellationToken.ThrowIfCancellationRequested();

		CSharpCompilation compilation = context.SemanticModel.Compilation as CSharpCompilation ?? throw new InvalidOperationException();

		return EnumDescriptor.FromINamedTypeSymbol( enumSymbol, compilation );
	}

	private static void GenerateSource( SourceProductionContext sourceProductionContext, EnumDescriptor enumDescriptor ) {

		var stopWatch = Stopwatch.StartNew();

		if( !EnumerationSourceGeneratorContext.TryCreate( in sourceProductionContext, in enumDescriptor, out EnumerationSourceGeneratorContext? generatorContext ) ) {
			return;
		}

		string fileNamePrefix = enumDescriptor.FullName.Replace( '.', '_' );

		string enumValueSource = EnumerationValueSourceGenerator.Generate( in enumDescriptor );
		sourceProductionContext.AddSource( $"{fileNamePrefix}Value.g.cs", SourceText.From( enumValueSource, Encoding.UTF8 ) );

		//string enumExtensionsSourceCode = EnumExtensionsSourceGenerator.Generate( in enumDescriptor );
		//sourceProductionContext.AddSource( $"{fileNamePrefix}Extensions.g.cs", SourceText.From( enumExtensionsSourceCode, Encoding.UTF8 ) );

		stopWatch.Stop();

		var stopWatchDiagnostic = Diagnostic.Create( stopwatchDiagnositcDescriptor, enumDescriptor.EnumPropertyAttributes.First().Location!.Value.CreateLocation() ?? Location.None, stopWatch.ElapsedMilliseconds, DateTime.Now );

		sourceProductionContext.ReportDiagnostic( stopWatchDiagnostic );
	}
}
