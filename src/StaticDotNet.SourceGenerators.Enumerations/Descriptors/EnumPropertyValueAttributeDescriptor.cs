namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct EnumPropertyValueAttributeDescriptor( INamedTypeSymbol Type, string Name, in TypedConstant Value, LocationDescriptor? Location ) {

	public const string GenericAttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyValueAttribute<>";
	public const string AttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyValueAttribute";

	public static EnumPropertyValueAttributeDescriptor FromGenericAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol type = attributeData.AttributeClass?.TypeArguments[0] as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );
		string name = attributeData.ConstructorArguments[ 0 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant value = attributeData.ConstructorArguments[ 1 ];
		LocationDescriptor? location = LocationDescriptor.FromAttributeData( attributeData );

		return new( type, name, value, location );
	}

	public static EnumPropertyValueAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol type = attributeData.ConstructorArguments[ 0 ].Value as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );
		string name = attributeData.ConstructorArguments[ 1 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant value = attributeData.ConstructorArguments[ 2 ];
		LocationDescriptor? location = LocationDescriptor.FromAttributeData( attributeData );

		return new( type, name, value, location );
	}
}
