namespace StaticDotNet.SourceGenerators.Enumerations.Extensions;

public static class NamedTypedSymbolExtensions {

	public static string ToUnboundedDisplayString( this INamedTypeSymbol namedTypeSymbol )
		=> namedTypeSymbol == null
			? throw new ArgumentNullException( nameof( namedTypeSymbol ) )
			: namedTypeSymbol.IsGenericType
				? namedTypeSymbol.ConstructUnboundGenericType().ToDisplayString()
				: namedTypeSymbol.ToDisplayString();
}
