namespace StaticDotNet.SourceGenerators.Enumerations.Source;

public static class EnumValueSourceGenerator {

	public static string Generate( in EnumDescriptor enumDescriptor )
		=> enumDescriptor is null
			? throw new ArgumentNullException( nameof( enumDescriptor ) )
			: enumDescriptor.LanguageVersion >= LanguageVersion.CSharp10
				? GenerateCSharp10OrHigher( in enumDescriptor )
				: GenerateBasic( in enumDescriptor );

	private static string GenerateCSharp10OrHigher( in EnumDescriptor enumDescriptor ) {

		StringBuilder sourceCodeStringBuilder = new();

		_ = sourceCodeStringBuilder
			.AppendLine( "using System.Diagnostics.CodeAnalysis;\r\n" );

		if( enumDescriptor.Namespace.Length > 0 ) {
			_ = sourceCodeStringBuilder.AppendLine( $"namespace {enumDescriptor.Namespace};\r\n" );
		}

		_ = sourceCodeStringBuilder.Append( $$"""
			[ExcludeFromCodeCoverage]
			{{enumDescriptor.Accessibility}} sealed record {{enumDescriptor.Name}}EnumValue( {{enumDescriptor.Name}} Value, string Name, {{enumDescriptor.UnderlyingType}} UnderlyingValue, string DisplayName, string DisplayShortName
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.Append( $", {currentEnumPropertyAttribute.Type} {currentEnumPropertyAttribute.Name}" );
		}

		_ = sourceCodeStringBuilder.AppendLine( " );" );

		return sourceCodeStringBuilder.ToString();
	}

	private static string GenerateBasic( in EnumDescriptor enumDescriptor ) {

		StringBuilder sourceCodeStringBuilder = new();

		_ = sourceCodeStringBuilder
			.AppendLine( "using System.Diagnostics.CodeAnalysis;\r\n" );

		if( enumDescriptor.Namespace.Length > 0 ) {
			_ = sourceCodeStringBuilder.AppendLine( $"namespace {enumDescriptor.Namespace} {{\r\n" );
		}

		_ = sourceCodeStringBuilder.Append( $$"""
				[ExcludeFromCodeCoverage]
				{{enumDescriptor.Accessibility}} sealed class {{enumDescriptor.Name}}EnumValue {

					public {{enumDescriptor.Name}}EnumValue( {{enumDescriptor.Name}} value, string name, {{enumDescriptor.UnderlyingType}} underlyingValue, string displayName, string displayShortName
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.Append( $", {currentEnumPropertyAttribute.Type} {currentEnumPropertyAttribute.Name.ToCamelCase()}" );
		}

		_ = sourceCodeStringBuilder.AppendLine( $$""" 
			 ) {
						Value = value;
						Name = name;
						UnderlyingValue = underlyingValue;
						DisplayName = displayName;
						DisplayShortName = displayShortName;
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.AppendLine( $$"""
							{{currentEnumPropertyAttribute.Name}} = {{currentEnumPropertyAttribute.Name.ToCamelCase()}};
				""" );
		}

		_ = sourceCodeStringBuilder.AppendLine( $$"""
					}

					public {{enumDescriptor.Name}} Value { get; }

					public string Name { get; }

					public {{enumDescriptor.UnderlyingType}} UnderlyingValue { get; }

					public string DisplayName { get; }
					
					public string DisplayShortName { get; }
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.AppendLine()
				.AppendLine( $$"""
						public {{currentEnumPropertyAttribute.Type}} {{currentEnumPropertyAttribute.Name}} { get; }
				""" );
		}

		_ = sourceCodeStringBuilder.AppendLine( "	}" );

		if( enumDescriptor.Namespace.Length > 0 ) {
			_ = sourceCodeStringBuilder.AppendLine( "}" );
		}

		return sourceCodeStringBuilder.ToString();
	}
}
