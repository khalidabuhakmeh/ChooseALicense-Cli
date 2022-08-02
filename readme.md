# Choose A License – .NET CLI Tool

![Choose A License Tool Logo](ChooseALicense/icon.png)

Finding the right license for your project can range from being tedious to downright difficult. This .NET CLI tool brings the information found at [Choose A License](https://choosealicense.com) to your command line environment.

- View a glossary rules around permissions, limitations, and conditions
- Search for licenses based on title and identifier
- Create a new license using the `id` of the license
- View detailed information about a license by `id`

**Note, these aren't ALL the licenses in the world. That list is extensive and frankly too much.**

## Getting Started

You can install the tool from the command line using the following command. It is recommended to install it as a **global** tool since it can be used in any context.

```bash
dotnet tool install -g choose-license
```

As an example, you can create an MIT license file using the following command.

```bash
choose-license new mit
```

You can also choose the output path (from the current directory) using the `-o` flag, but just be sure to include the file name.

## Dependencies and Credit

- Choose A License - https://choosealicense.com
- Spectre.Console - https://spectreconsole.net/
- MarkDig - https://github.com/xoofx/markdig
- YamlDotNet - https://github.com/aaubry/YamlDotNet