using System.Globalization;

namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct LocationDescriptor( string FilePath, in TextSpan TextSpan, in LinePositionSpan LineSpan ) {

	public Location CreateLocation() => Location.Create( FilePath, TextSpan, LineSpan );

	public static LocationDescriptor? FromAttributeData( AttributeData attributeData ) {

		if ( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		Location? location = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();

		return location is null
			? null
			: FromLocation( location );
	}

	public static LocationDescriptor? FromNamedTypeSymbol( INamedTypeSymbol namedTypeSymbol ) {
		
		if ( namedTypeSymbol is null ) {
			throw new ArgumentNullException( nameof( namedTypeSymbol ) );
		}

		Location? location = namedTypeSymbol.Locations.Length > 0 ? namedTypeSymbol.Locations[ 0 ] : null;

		return location is null
			? null
			: FromLocation( location );
	}

	public static LocationDescriptor FromLocation( Location location ) {

		if( location is null ) {
			throw new ArgumentNullException( nameof( location ) );
		}

		string filePath = location.SourceTree?.FilePath ?? string.Empty;
		TextSpan textSpan = location.SourceSpan;
		LinePositionSpan lineSpan = location.GetLineSpan().Span;

		return new( filePath, textSpan, lineSpan );
	}
}
