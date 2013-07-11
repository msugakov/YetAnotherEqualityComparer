using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("YetAnotherEqualityComparer")]
[assembly: AssemblyDescription("This library allows to define IEqualityComparer on arbitrary types " +
                               "to compare objects based on their members. Members are explicitly configured " +
                               "with strongly-typed fluent API. Members and are not limited to properties, " +
                               "these can be fields or even methods. The library allows to implement overrides " +
                               "of Equals and GetHashCode by using provided MemberEqualityComparer type minimizing " +
                               "boilerplate.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("M. Sugakov")]
[assembly: AssemblyProduct("YetAnotherEqualityComparer")]
[assembly: AssemblyCopyright("Copyright © M. Sugakov 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4c6fe746-1604-4b42-85cb-ba67d6df3236")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
