namespace StaticDotNet.SourceGenerators.Enumerations.Source;

public static class EnumerationValueSourceGenerator {

	public static string Generate( in EnumDescriptor enumDescriptor )
		=> enumDescriptor.LanguageVersion >= LanguageVersion.CSharp10
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
			#nullable enable

			[ExcludeFromCodeCoverage]
			{{enumDescriptor.Accessibility}} sealed record {{enumDescriptor.Name}}Value( {{enumDescriptor.Name}} Value, string Name, {{enumDescriptor.UnderlyingType}} UnderlyingValue, string DisplayName, string DisplayShortName
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.Append( $", {currentEnumPropertyAttribute.Type.ToDisplayString()} {currentEnumPropertyAttribute.Name}" );
		}

		_ = sourceCodeStringBuilder.AppendLine( $$"""
			 );

			#nullable disable
			""" );

		return sourceCodeStringBuilder.ToString();
	}

	private static string GenerateBasic( in EnumDescriptor enumDescriptor ) {

		StringBuilder sourceCodeStringBuilder = new();

		_ = sourceCodeStringBuilder
			.AppendLine( "using System.Diagnostics.CodeAnalysis;\r\n" );

		if( enumDescriptor.Namespace.Length > 0 ) {
			_ = sourceCodeStringBuilder.AppendLine( $"namespace {enumDescriptor.Namespace} {{\r\n" );
		}

		if( enumDescriptor.LanguageVersion >= LanguageVersion.CSharp8 ) {
			_ = sourceCodeStringBuilder.AppendLine( "#nullable enable\r\n" );
		}

		_ = sourceCodeStringBuilder.Append( $$"""
				[ExcludeFromCodeCoverage]
				{{enumDescriptor.Accessibility}} sealed class {{enumDescriptor.Name}}Value {

					public {{enumDescriptor.Name}}Value( {{enumDescriptor.Name}} value, string name, {{enumDescriptor.UnderlyingType}} underlyingValue, string displayName, string displayShortName
			""" );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {
			_ = sourceCodeStringBuilder.Append( $", {currentEnumPropertyAttribute.Type.ToDisplayString()} {currentEnumPropertyAttribute.Name.ToCamelCase()}" );
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
						public {{currentEnumPropertyAttribute.Type.ToDisplayString()}} {{currentEnumPropertyAttribute.Name}} { get; }
				""" );
		}

		_ = sourceCodeStringBuilder.AppendLine( "}" );

		if( enumDescriptor.LanguageVersion >= LanguageVersion.CSharp8 ) {
			_ = sourceCodeStringBuilder.AppendLine( "\r\n#nullable disable" );
		}

		if( enumDescriptor.Namespace.Length > 0 ) {
			_ = sourceCodeStringBuilder.AppendLine( "}" );
		}

		return sourceCodeStringBuilder.ToString();
	}
}
