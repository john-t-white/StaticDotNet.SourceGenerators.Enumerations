namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public sealed record EnumPropertyAttributeDescriptor( string Type, string Name, TypedConstant DefaultValue ) {

	public const string GenericAttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyAttribute<>";
	public const string AttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyAttribute";

	public static EnumPropertyAttributeDescriptor FromGenericAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		string name = attributeData.ConstructorArguments[ 0 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant defaultValue = attributeData.ConstructorArguments[ 1 ];

		INamedTypeSymbol namedTypeSymbol = defaultValue.Type as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );

		string type = namedTypeSymbol.ToDisplayString();

		return new( type, name, defaultValue );
	}

	public static EnumPropertyAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol namedTypeSymbol = attributeData.ConstructorArguments[ 0 ].Value as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );

		string type = namedTypeSymbol.ToDisplayString();
		string name = attributeData.ConstructorArguments[ 1 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant defaultValue = attributeData.ConstructorArguments[ 2 ];

		return new( type, name, defaultValue );
	}
}
