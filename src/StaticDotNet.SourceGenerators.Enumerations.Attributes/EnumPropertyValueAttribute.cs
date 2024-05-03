namespace StaticDotNet.SourceGenerators.Enumerations;

[AttributeUsage( AttributeTargets.Field, AllowMultiple = true )]
public sealed class EnumPropertyValueAttribute<T>( string name, T value )
	: Attribute {

	public string Name { get; } = !string.IsNullOrWhiteSpace( name ) ? name : throw new ArgumentException( "Value cannot be null or white space.", nameof( name ) );

	public T Value { get; } = value;
}

[AttributeUsage( AttributeTargets.Field, AllowMultiple = true )]
public sealed class EnumPropertyValueAttribute( Type type, string name, object? value = null )
	: Attribute {

	public Type Type { get; } = type ?? throw new ArgumentNullException( nameof( type ) );

	public string Name { get; } = !string.IsNullOrWhiteSpace( name ) ? name : throw new ArgumentException( "Value cannot be null or white space.", nameof( name ) );

	public object? Value { get; } = value;
}
