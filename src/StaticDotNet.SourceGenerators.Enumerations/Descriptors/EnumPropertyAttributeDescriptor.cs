namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct EnumPropertyAttributeDescriptor( INamedTypeSymbol Type, string Name, in TypedConstant DefaultValue, LocationDescriptor? Location ) {

	public const string GenericAttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyAttribute<>";
	public const string AttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyAttribute";

	public static EnumPropertyAttributeDescriptor FromGenericAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol type = attributeData.AttributeClass?.TypeArguments[ 0 ] as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );
		string name = attributeData.ConstructorArguments[ 0 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant defaultValue = attributeData.ConstructorArguments[ 1 ];
		LocationDescriptor? location = LocationDescriptor.FromAttributeData( attributeData );

		return new( type, name, defaultValue, location );
	}

	public static EnumPropertyAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol type = attributeData.ConstructorArguments[ 0 ].Value as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );
		string name = attributeData.ConstructorArguments[ 1 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant defaultValue = attributeData.ConstructorArguments[ 2 ];
		LocationDescriptor? location = LocationDescriptor.FromAttributeData( attributeData );

		return new( type, name, defaultValue, location );
	}
}
