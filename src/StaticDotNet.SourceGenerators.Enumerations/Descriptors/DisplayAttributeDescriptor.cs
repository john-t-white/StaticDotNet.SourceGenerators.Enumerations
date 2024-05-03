namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public sealed record DisplayAttributeDescriptor( string? Name, string? ShortName ) {

	public const string AttributeTypeFullName = "System.ComponentModel.DataAnnotations.DisplayAttribute";

	public static DisplayAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		string? name = null;
		string? shortName = null;

		foreach( KeyValuePair<string, TypedConstant> currentNamedArgument in attributeData.NamedArguments ) {

			if( currentNamedArgument.Key == nameof( Name ) ) {
				name = currentNamedArgument.Value.Value as string;
			}

			if( currentNamedArgument.Key == nameof( ShortName ) ) {
				shortName = currentNamedArgument.Value.Value as string;
			}
		}

		return new( name, shortName );
	}
}
