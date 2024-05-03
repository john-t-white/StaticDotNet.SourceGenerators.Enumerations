namespace StaticDotNet.SourceGenerators.Enumerations.Extensions;

public static class StringExtensions {

	public static string ToCamelCase( this string value )
		=> value is null
			? throw new ArgumentNullException( nameof( value ) )
			: !string.IsNullOrWhiteSpace( value )
				? char.ToLowerInvariant( value[ 0 ] ) + value.Substring( 1 )
				: value;
}
