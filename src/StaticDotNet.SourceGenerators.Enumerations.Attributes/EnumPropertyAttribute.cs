namespace StaticDotNet.SourceGenerators.Enumerations;

[AttributeUsage( AttributeTargets.Enum, AllowMultiple = true )]
public sealed class EnumPropertyAttribute<T>( string name, T? defaultValue = default )
	: Attribute {

	public string Name { get; } = !string.IsNullOrWhiteSpace( name ) ? name : throw new ArgumentException( "Value cannot be null or white space.", nameof( name ) );

	public object? DefaultValue { get; } = defaultValue;
}

[AttributeUsage( AttributeTargets.Enum, AllowMultiple = true )]
public sealed class EnumPropertyAttribute( Type type, string name, object? defaultValue = null )
	: Attribute {

	public Type Type { get; } = type ?? throw new ArgumentNullException( nameof( type ) );

	public string Name { get; } = !string.IsNullOrWhiteSpace( name ) ? name : throw new ArgumentException( "Value cannot be null or white space.", nameof( name ) );

	public object? DefaultValue { get; } = defaultValue ?? ( type.IsValueType ? Activator.CreateInstance(type) : null );
}
