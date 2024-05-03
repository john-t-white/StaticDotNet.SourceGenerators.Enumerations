namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public sealed record EnumDescriptor( LanguageVersion LanguageVersion, string Accessibility, string Namespace, string Name, string UnderlyingType, EquatableArray<EnumPropertyAttributeDescriptor> EnumPropertyAttributes, EquatableArray<EnumValueDescriptor> EnumValue ) {

	public static EnumDescriptor FromINamedTypeSymbol( in INamedTypeSymbol enumSymbol, CSharpCompilation compilation ) {

		if( enumSymbol == null ) {
			throw new ArgumentNullException( nameof( enumSymbol ) );
		}

		if( compilation == null ) {
			throw new ArgumentNullException( nameof( compilation ) );
		}

		List<EnumPropertyAttributeDescriptor> enumPropertyAttributesList = [];

		foreach( AttributeData currentAttribute in enumSymbol.GetAttributes() ) {

			if( currentAttribute.AttributeClass is not INamedTypeSymbol currentAttributeSymbol ) {
				continue;
			}

			string currentAttributeTypeName = currentAttributeSymbol.ToUnboundedDisplayString();

			if( currentAttributeTypeName.Equals( EnumPropertyAttributeDescriptor.GenericAttributeTypeFullName, StringComparison.OrdinalIgnoreCase ) ) {
				var currentEnumPropertyAttribute = EnumPropertyAttributeDescriptor.FromGenericAttributeData( currentAttribute );
				enumPropertyAttributesList.Add( currentEnumPropertyAttribute );
			} else if( currentAttributeTypeName.Equals( EnumPropertyAttributeDescriptor.AttributeTypeFullName, StringComparison.OrdinalIgnoreCase ) ) {
				var currentEnumPropertyAttribute = EnumPropertyAttributeDescriptor.FromAttributeData( currentAttribute );
				enumPropertyAttributesList.Add( currentEnumPropertyAttribute );
			}
		}

		List<EnumValueDescriptor> enumValuesList = [];
		foreach( ISymbol currentEnumMember in enumSymbol.GetMembers() ) {

			if( currentEnumMember is not IFieldSymbol currentEnumField || currentEnumField.ConstantValue is null ) {
				continue;
			}

			var currentEnumValueDescriptor = EnumValueDescriptor.FromFieldSymbol( currentEnumField );
			enumValuesList.Add( currentEnumValueDescriptor );
		}

		string accessibility = enumSymbol.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public ? "public" : "internal";
		string name = enumSymbol.Name;
		string @namespace = enumSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : enumSymbol.ContainingNamespace.ToString();
		string underlyingType = enumSymbol.EnumUnderlyingType?.ToDisplayString( SymbolDisplayFormat.MinimallyQualifiedFormat ) ?? throw new InvalidOperationException( "Underlying type unexpectedly null." );


		return new( compilation.LanguageVersion, accessibility, @namespace, name, underlyingType, new( [ .. enumPropertyAttributesList ] ), new( [ .. enumValuesList ] ) );
	}
}
