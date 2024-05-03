using Microsoft.CodeAnalysis;

namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public sealed record EnumPropertyValueAttributeDescriptor( string Type, string Name, TypedConstant Value ) {

	public const string GenericAttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyValueAttribute<>";
	public const string AttributeTypeFullName = "StaticDotNet.SourceGenerators.Enumerations.EnumPropertyValueAttribute";

	public static EnumPropertyValueAttributeDescriptor FromGenericAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		string name = attributeData.ConstructorArguments[ 0 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant value = attributeData.ConstructorArguments[ 1 ];

		INamedTypeSymbol namedTypeSymbol = value.Type as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );
		string type = namedTypeSymbol.ToDisplayString();

		return new( type, name, value );
	}

	public static EnumPropertyValueAttributeDescriptor FromAttributeData( AttributeData attributeData ) {

		if( attributeData is null ) {
			throw new ArgumentNullException( nameof( attributeData ) );
		}

		INamedTypeSymbol namedTypeSymbol = attributeData.ConstructorArguments[ 0 ].Value as INamedTypeSymbol ?? throw new InvalidOperationException( "Type is unexpectedly null." );

		string type = namedTypeSymbol.ToDisplayString();
		string name = attributeData.ConstructorArguments[ 1 ].Value as string ?? throw new InvalidOperationException( "Name is unexpectedly null." );
		TypedConstant value = attributeData.ConstructorArguments[ 2 ];

		return new( type, name, value );
	}
}
