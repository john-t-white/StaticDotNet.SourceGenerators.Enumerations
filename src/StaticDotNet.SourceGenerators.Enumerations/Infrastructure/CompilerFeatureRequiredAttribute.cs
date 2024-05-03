namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// </summary>
[AttributeUsage( AttributeTargets.All, AllowMultiple = true, Inherited = false )]
internal sealed class CompilerFeatureRequiredAttribute( string featureName ) : Attribute {

	public string FeatureName { get; } = featureName;
	public bool IsOptional { get; init; }

	public const string RefStructs = nameof( RefStructs );
	public const string RequiredMembers = nameof( RequiredMembers );
}
