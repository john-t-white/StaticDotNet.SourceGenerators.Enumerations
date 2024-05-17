using StaticDotNet.SourceGenerators.Enumerations.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StaticDotNet.SourceGenerators.Enumerations.Source;

public readonly record struct EnumerationSourceGeneratorContext( in EnumDescriptor EnumDescriptor ) {

	public bool SupportsNullability => EnumDescriptor.LanguageVersion >= LanguageVersion.CSharp8;

	public bool SupportsRecords => EnumDescriptor.LanguageVersion >= LanguageVersion.CSharp9;

	public bool SupportsFileScopedNamespaces => EnumDescriptor.LanguageVersion >= LanguageVersion.CSharp10;

	public static bool TryCreate( in SourceProductionContext sourceProductionContext, in EnumDescriptor enumDescriptor, [NotNullWhen( true )] out EnumerationSourceGeneratorContext? enumerationSourceGeneratorContext ) {

		enumerationSourceGeneratorContext = null;

		Dictionary<string, EnumerationProperty> enumerationProperties = new( enumDescriptor.EnumPropertyAttributes.Count, StringComparer.OrdinalIgnoreCase );

		foreach( EnumPropertyAttributeDescriptor currentEnumPropertyAttribute in enumDescriptor.EnumPropertyAttributes ) {

			if( enumerationProperties.ContainsKey( currentEnumPropertyAttribute.Name ) ) {
				EnumerationDiagnostic.ReportDuplicateEnumProperty( sourceProductionContext, enumDescriptor, currentEnumPropertyAttribute );

				return false;
			}

			var enumerationProperty = EnumerationProperty.Create( currentEnumPropertyAttribute );
			enumerationProperties.Add( currentEnumPropertyAttribute.Name, enumerationProperty );
		}

		enumerationSourceGeneratorContext = new( enumDescriptor );
		return true;
	}
}
