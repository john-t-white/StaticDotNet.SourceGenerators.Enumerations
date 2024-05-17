using System;
using System.ComponentModel.DataAnnotations;

namespace StaticDotNet.SourceGenerators.Enumerations.Example {

	[GenerateEnumExtensions]

#if NETSTANDARD2_0_OR_GREATER || NET6_0
	[EnumProperty( typeof( byte ), "valueAsByte")]
	[EnumProperty( typeof( byte ), "ValueAsByte")]
#else
	[EnumProperty<byte>( "ValueAsByte")]
#endif
	public enum BasicExample {

		[Display(Name = "Value Zero")]
		ValueZero = 0,
		ValueOne = 1,
		ValueTwo = 2,
		ValueThree = 3,
		ValueFour = 4,
		ValueFive = 5
	}
}