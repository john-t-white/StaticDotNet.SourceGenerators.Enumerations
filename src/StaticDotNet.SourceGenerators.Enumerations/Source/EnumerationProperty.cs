namespace StaticDotNet.SourceGenerators.Enumerations.Source;

public class EnumerationProperty( INamedTypeSymbol Type, string PropertyName, string ArgumentName ) {

	public static EnumerationProperty Create( in EnumPropertyAttributeDescriptor enumPropertyAttributeDescriptor ) {
		return new( enumPropertyAttributeDescriptor.Type, enumPropertyAttributeDescriptor.Name, enumPropertyAttributeDescriptor.Name.ToCamelCase() );
	}
}
