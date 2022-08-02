using System.ComponentModel;
using ChooseALicense.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using License = ChooseALicense.Models.License;
using Rule = Spectre.Console.Rule;

#pragma warning disable CS8765

namespace ChooseALicense.Commands;

public class InfoCommand : Command<InfoCommand.Settings>
{
    private readonly LicenseInformation _licenseInformation;

    public InfoCommand(LicenseInformation licenseInformation)
    {
        _licenseInformation = licenseInformation;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<id>")] 
        [Description("id of the license. Use [underline]list[/] to find id values.")]
        public string Id { get; set; } = "";
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (_licenseInformation.Licenses.Find(settings.Id) is { } license)
        {
            var rules = _licenseInformation.Rules;

            // add panel
            AnsiConsole.Write(new Rule("CLI Command").LeftAligned());
            AnsiConsole.MarkupLine($"[dim]$>[/] [italic bold green]{Defaults.CommandName} new {license.Id}[/]");
            AnsiConsole.Write(new Rule($"General Info - {license.Title} ({license.Id})").LeftAligned());
            AnsiConsole.MarkupLine($"[yellow]{license.Description}[/]");
            AnsiConsole.Write(new Table()
                .RoundedBorder()
                .AddColumns(
                    nameof(License.Permissions),
                    nameof(License.Conditions),
                    nameof(License.Limitations)
                )
                .AddRow(
                    rules.Permissions.ToLabelBullets(license.Permissions),
                    rules.Conditions.ToLabelBullets(license.Conditions),
                    rules.Limitations.ToLabelBullets(license.Limitations)
                ));
            AnsiConsole.Write(new Rule("License Text").LeftAligned());
            AnsiConsole.MarkupLine($"[bold]{license.Text.EscapeMarkup()}[/]");

            return 0;
        }

        AnsiConsole.MarkupLine($"[red]License Id of {settings.Id} not found[/]");
        return 1;
    }
}