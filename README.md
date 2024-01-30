# What this is
- Rewrite of anagramarama in C# (forked from Dulsi's version 0.7).
- Currently developed using on Dotnet 8.0.
- Aiming to use VSCode with extensions so that the same environment can be used in Windows, Linux, MacOS (and eventually ideally iOS if possible)
- A learning exercise for me as I am not a programmer by trade and this is mainly to understand c#, but also how to use VSCode and develop for free under multiple environments.
- This is not intended to be bug-free, secure, optimised when it reaches v1.


# Assumptions
- Trying to be close to original C source and structure from the latest modifications from Dulsi.
- Adding comments for readability and understanding 
- Trying to keep for now the folder architecture introduced by Dulci.
- As original, uses SDL2, but via C#

# Tips & learnings
## Installation of C#
- Follow Microsoft DotNet SDK installation

## Installation of SDL2 for C#
- [Jeremy Sayers - C# SDL Tutorial](https://jsayers.dev/c-sharp-sdl-tutorial-part-1-setup/)

## Commenting
- For C# Microsoft advises [XML-style documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments) for its source code 
- The C# extension for VSCode enable for the pre-population of comments for each method etc.. 

  - add a tick mark for `format on Type` to:
    - Windows: `File / Preference / Settings / Text Editor / Format On Type`
    - MacOs: `tbd`

  - Thereafter, a template will automatically pre-populate at the cursor position, ready for comments, when the text `\\\` is typed.

- Dotnet C# VSCode extensions allow XML documentation to be extracted automatically at compile time, and stored into a file.
  - The file, of name *project*.xml can then be found under `bin/Debug/` or `bin/release` depending on the compilation performed.
  - In the `projectname.csproj` file, add the following two lines within the `<PropertyGroup>` section:
```
<DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(AssemblyName).xml</DocumentationFile>

<GenerateDocumentationFile>true</GenerateDocumentationFile>
```

## Testing
- Currently being studied
  - [Info](https://www.linkedin.com/pulse/automated-unit-testing-mstest-vs-xunit-nunit-anar-solutions-1f#:~:text=Integration%3A%20MSTest%20has%20an%20advantage,custom%20test%20runners%20and%20reporters.) : Comparison of Automated Unit Testing that can be used with the C# Dev Kit extension. Summary: 
    - MSTest = (Microsoft) Limited cross platform support
    - xUnit = (Open Source) Prefer for optimising speed
    - NUnit = (Open Source) Prefer for cross platform
  - I chose `NUnit` as it is also the default selected in *CodiumAI* but `xUnit` looks like it would be just as valid anyway for this project.
- Unitary tests built with help of CodiumAI
  - [Unitary test setup](https://learn.microsoft.com/en-gb/dotnet/core/testing/unit-testing-with-nunit):
  - create folder `agUnitaryTests` at the same level of `ag`
    - This is the simplest way because otherwise 2x bin and 2x obj folders are created and they interfere with each other. See info [here](https://github.com/dotnet/core/issues/4837)
  - in terminal: `cd UnitaryTests`
  - in terminal, create the NUnit test project: `dotnet new nunit`
  - in terminal, refer it to the application: `dotnet add reference ../ag/ag.csproj`
  - in terminal, return to the folder where the ag.sln is: `cd ..`
  - in terminal, reference in the solution (sln) file: `dotnet sln add agUnitaryTests\UnitaryTests.csproj`



# VSCode extensions used for this development
- *Microsoft*: .Net Install Tool
- *Microsoft*: C# Base language support for c#
- *Microsoft*: C# Dev Kit
  - PERSONAL SIDE NOTE: When creating a C# Console project, this extension currently causes an issue preventin the creation the .VSCode folder by VSCode. This is the folder containing with the launch.json and tasks.json -> Disable // irrelevant when cloning this project
- *Microsoft*: Intellicode for C# Dev Kit
- *Microsoft*: GitHub Pull Requests and Issues
- *Matt Bierner*: Markdown Preview Github Styling
- *CodiumAI*: CodiumAI - Integrity Agent powered by GPT-3.5&4