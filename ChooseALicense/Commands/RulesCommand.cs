using ChooseALicense.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using Rule = Spectre.Console.Rule;

#pragma warning disable CS8765

namespace ChooseALicense.Commands;

public class RulesCommand : Command<RulesCommand.Settings>
{
    private readonly LicenseInformation _licenseInformation;

    public RulesCommand(LicenseInformation licenseInformation)
    {
        _licenseInformation = licenseInformation;
    }

    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var rules = _licenseInformation.Rules;
        var dictionary = new Dictionary<string, List<Models.Rule>>
        {
            { nameof(Rules.Conditions), rules.Conditions },
            { nameof(Rules.Limitations), rules.Limitations },
            { nameof(Rules.Permissions), rules.Permissions },
        };

        foreach (var (key, values) in dictionary)
        {
            AnsiConsole.Write(new Rule(key).LeftAligned());
            foreach (var rule in values)
            {
                AnsiConsole.MarkupLine($"‣ [bold italic green]{rule.Label}[/]: [yellow italic]{rule.Description}[/]");
            }
        }

        return 0;
    }
}