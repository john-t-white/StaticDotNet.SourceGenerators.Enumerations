namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct DisplayAttributeDescriptor( TypedConstant? Name, TypedConstant? ShortName ) {

	public const string AttributeTypeFullName = "System.ComponentModel.DataAnnotations.DisplayAttribute";

	public static DisplayAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		TypedConstant? name = null;
		TypedConstant? shortName = null;

		foreach( KeyValuePair<string, TypedConstant> currentNamedArgument in attributeData.NamedArguments ) {

			if( currentNamedArgument.Key == nameof( Name ) ) {
				name = currentNamedArgument.Value;
			}

			if( currentNamedArgument.Key == nameof( ShortName ) ) {
				shortName = currentNamedArgument.Value;
			}
		}

		return new( name, shortName );
	}
}
