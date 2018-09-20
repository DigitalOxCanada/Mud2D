# Lesson Notes

1. Added Figgle Fonts Nuget package.

2. Created Intro using Figgle fonts.

3. Introduced compiler directives `#if DEBUG`

# Q&A

**Q** I see the Figgle reference `<PackageReference Include="Figgle" Version="0.3.0" />` in the .csproj file but how did it get there?
**A** From powershell using the dotnet cli you can issue the command line `dotnet add package Figgle`.  This will create the reference in the project file.  The Nuget gallery is found at https://www.nuget.org/packages/Figgle/
  
**Q** From dotnet cli how do I differentiate between debug and release builds?
**A** Using `dotnet build -c Release` will build the release version.  Using `dotnet run -c Release` executes the release build.  If you don't specific the `-c` the default is `Debug`.
