using StaticDotNet.SourceGenerators.Enumerations.Infrastructure;

namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct EnumValueDescriptor( string Name, object Value, DisplayAttributeDescriptor? DisplayAttribute, EquatableArray<EnumPropertyValueAttributeDescriptor> EnumPropertyValueAttributes ) {

	public static EnumValueDescriptor FromFieldSymbol( IFieldSymbol fieldSymbol ) {

		if( fieldSymbol == null ) {
			throw new ArgumentNullException( nameof( fieldSymbol ) );
		}

		DisplayAttributeDescriptor? displayAttribute = null;
		List<EnumPropertyValueAttributeDescriptor> enumPropertyValueAttributes = [];

		foreach( AttributeData currentAttribute in fieldSymbol.GetAttributes() ) {

			if( currentAttribute.AttributeClass is not INamedTypeSymbol currentAttributeSymbol ) {
				break;
			}

			string currentAttributeUnboundedTypeName = currentAttributeSymbol.ToUnboundedDisplayString();
			if( currentAttributeUnboundedTypeName.Equals( DisplayAttributeDescriptor.AttributeTypeFullName, StringComparison.OrdinalIgnoreCase ) ) {

				displayAttribute = DisplayAttributeDescriptor.FromAttributeData( currentAttribute );
				continue;
			}

			if( currentAttributeUnboundedTypeName.Equals( EnumPropertyValueAttributeDescriptor.GenericAttributeTypeFullName, StringComparison.OrdinalIgnoreCase ) ) {
				var currentEnumPropertyValueAttribute = EnumPropertyValueAttributeDescriptor.FromGenericAttributeData( currentAttribute );
				enumPropertyValueAttributes.Add( currentEnumPropertyValueAttribute );
				continue;
			}

			if( currentAttributeUnboundedTypeName.Equals( EnumPropertyValueAttributeDescriptor.AttributeTypeFullName, StringComparison.OrdinalIgnoreCase ) ) {
				var currentEnumPropertyValueAttribute = EnumPropertyValueAttributeDescriptor.FromGenericAttributeData( currentAttribute );
				enumPropertyValueAttributes.Add( currentEnumPropertyValueAttribute );
				continue;
			}
		}

		string name = fieldSymbol.Name;
		object value = fieldSymbol.ConstantValue ?? throw new InvalidOperationException( "Enum field value is unexpectedly null." );

		return new( name, value, displayAttribute, new( [ .. enumPropertyValueAttributes ] ) );
	}
}
