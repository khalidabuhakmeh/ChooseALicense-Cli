using System.ComponentModel;
using ChooseALicense.Models;
using Spectre.Console;
using Spectre.Console.Cli;

#pragma warning disable CS8765

namespace ChooseALicense.Commands;

public class NewCommand : Command<NewCommand.Settings>
{
    private readonly LicenseInformation _licenseInformation;
    
    public NewCommand(LicenseInformation licenseInformation)
    {
        _licenseInformation = licenseInformation;
    }
    
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<id>")]
        [Description("id of the license. Use [underline]list[/] to find id values.")]
        public string Id { get; set; } = "";

        [CommandOption("-o|--output")] 
        [Description("relative file path to create the license file (include the file name). default: \"License.md\"")]
        public string? Output { get; set; } = "LICENSE.md";
        
        [CommandOption("-n|--name")]
        [Description("Name of the license holder (will replace any instance of [name] in license).\nAlso Reads Environment Variable: CHOOSELICENSE_NAME")]
        public string? Name { get; set; }
        
        [CommandOption("-y|--year")]
        [Description("Name of the license holder (will replace any instance of [name] in license).")]
        public string? Year { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var license = _licenseInformation.Licenses.Find(settings.Id);

        if (license is { })
        {
            var output = Path.Combine(Environment.CurrentDirectory, settings.Output ?? "LICENSE.md");
            var name = settings.Name ?? Environment.GetEnvironmentVariable("CHOOSELICENSE_NAME") ?? "[fullname]";
            var year = settings.Year ?? DateTime.Now.Year.ToString();
            
            var text = license.Text
                .Replace("[year]", year)
                .Replace("[fullname]", name);

            File.Delete(output);
            File.WriteAllText(output, text);
            
            AnsiConsole.MarkupLine($"[yellow]⚠️ This license is NOT LEGAL ADVICE. ⚠️[/]");
            AnsiConsole.MarkupLine($"✅ Successfully created the license [green]\"{license.Title.EscapeMarkup()}\"[/] at [green]{output.EscapeMarkup()}[/]");
            AnsiConsole.MarkupLine($"🤖 License assigned to \"{name.EscapeMarkup()}\" for \"{year.EscapeMarkup()}\".");

            return 0;
        }

        AnsiConsole.MarkupLine($"[red]License Id of {settings.Id} not found[/]");
        return 1;
    }
}