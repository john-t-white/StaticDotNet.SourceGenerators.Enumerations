using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace StaticDotNet.SourceGenerators.Enumerations.Diagnostics;

public static class EnumerationDiagnostic {

	private static readonly DiagnosticDescriptor duplicateEnumPropertyDiagnosticDescriptor = new( "SDNENUM001", "Duplicate EnumPropertyAttribute", "An EnumPropertyAttribute with the name '{0}' already exists on '{1}' in file '{2}", "Compiler", DiagnosticSeverity.Error, true );

	public static void ReportDuplicateEnumProperty( in SourceProductionContext sourceProductionContext, in EnumDescriptor enumDescriptor, in EnumPropertyAttributeDescriptor enumPropertyAttributeDescriptor ) {

		Location location = enumPropertyAttributeDescriptor.Location.HasValue
			? enumPropertyAttributeDescriptor.Location.Value.CreateLocation()
			: Location.None;

		var duplicateDiagnostic = Diagnostic.Create( duplicateEnumPropertyDiagnosticDescriptor, location, enumPropertyAttributeDescriptor.Name, enumDescriptor.FullName, location.ToString() );

		sourceProductionContext.ReportDiagnostic( duplicateDiagnostic );
	}
}
