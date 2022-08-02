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
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var license = _licenseInformation.Licenses.Find(settings.Id);

        if (license is { })
        {
            var output = Path.Combine(Environment.CurrentDirectory, settings.Output ?? "LICENSE.md");
            File.Delete(output);
            File.WriteAllText(output, license.Text);
            AnsiConsole.MarkupLine($"Successfully created the license [green]\"{license.Title}\"[/] at [green]{output}[/]");
            AnsiConsole.MarkupLine($"[yellow]Next steps:\n{license.How.EscapeMarkup()}[/]");
            return 0;
        }

        AnsiConsole.MarkupLine($"[red]License Id of {settings.Id} not found[/]");
        return 1;
    }
}