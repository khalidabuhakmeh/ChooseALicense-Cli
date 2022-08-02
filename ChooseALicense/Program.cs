// See https://aka.ms/new-console-template for more information

// dotnet license --help
// dotnet license list
// dotnet license new <name> -o License.md
// dotnet license info <name>
// dotnet license search commercial

using ChooseALicense;
using ChooseALicense.Commands;
using ChooseALicense.Infrastructure;
using ChooseALicense.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);

registrar.RegisterLazy(typeof(LicenseInformation), () =>
    new LicenseInformation(Rules.Load(), Licenses.Load())
);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName(Defaults.CommandName);
    
    config.AddCommand<ListCommand>("list")
        .WithDescription("List supported licenses. Use -s to search. limited (-l) to 10 results by default.");
    config.AddCommand<NewCommand>("new")
        .WithDescription("Create a new license file using a specific license identifier");
    config.AddCommand<InfoCommand>("info")
        .WithDescription("Detailed information on a specific license");
    config.AddCommand<RulesCommand>("rules")
        .WithDescription("Lists each rule is and a detailed description.");
});

return app.Run(args);