using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ChooseALicense.Models;

public class Rules
{
    public List<Rule> Permissions { get; set; } = new();
    public List<Rule> Conditions { get; set; } = new();
    public List<Rule> Limitations { get; set; } = new();

    public static Rules Load()
    {
        var yaml = typeof(Program).Assembly.GetManifestResourceStream("rules.yml");
        var reader = new StreamReader(yaml!);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var rules = deserializer.Deserialize<Rules>(reader);
        return rules;
    }
}

public class Rule
{
    public string Tag { get; set; } = "";
    public string Label { get; set; } = "";
    public string Description { get; set; } = "";
}