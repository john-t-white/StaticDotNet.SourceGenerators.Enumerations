namespace StaticDotNet.SourceGenerators.Enumerations.Descriptors;

public readonly record struct EnumDescriptor( LanguageVersion LanguageVersion, string Accessibility, string Namespace, string Name, string UnderlyingType, bool HasFlags, EquatableArray<EnumPropertyAttributeDescriptor> EnumPropertyAttributes, EquatableArray<EnumValueDescriptor> EnumValues ) {

	private const string FLAGS_ATTRIBUTE_TYPE_FULL_NAME = "System.FlagsAttribute";

	public string FullName => Namespace.Length > 0
			? $"{Namespace}.{Name}"
			: Name;

	public static EnumDescriptor FromINamedTypeSymbol( INamedTypeSymbol enumSymbol, CSharpCompilation compilation ) {

		if( enumSymbol == null ) {
			throw new ArgumentNullException( nameof( enumSymbol ) );
		}

		if( compilation == null ) {
			throw new ArgumentNullException( nameof( compilation ) );
		}

		bool hasFlagsAttribute = false;
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
			} else if( currentAttributeTypeName.Equals( FLAGS_ATTRIBUTE_TYPE_FULL_NAME, StringComparison.OrdinalIgnoreCase ) ) {
				hasFlagsAttribute = true;
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

		return new( compilation.LanguageVersion, accessibility, @namespace, name, underlyingType, hasFlagsAttribute, new( [ .. enumPropertyAttributesList ] ), new( [ .. enumValuesList ] ) );
	}
}
