using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;

namespace ChooseALicense.Models;

public class License
{
    [YamlMember(Alias = "spdx-id")]
    public string Id { get; set; }= "";
    [YamlMember(Alias = "title")]
    public string Title { get; set; }= "";
    [YamlMember(Alias = "description")]
    public string Description { get; set; }= "";
    [YamlMember(Alias = "how")]
    public string How { get; set; }= "";

    [YamlMember(Alias = "permissions")]
    public List<string> Permissions { get; set; } = new();
    [YamlMember(Alias = "conditions")]
    public List<string> Conditions { get; set; } = new();
    [YamlMember(Alias = "limitations")]
    public List<string> Limitations { get; set; } = new();
    
    [YamlIgnore]
    public string Text { get; set; } = "";
}

public class Licenses
{
    public List<License> All { get; set; } = new();

    public License? Find(string? id)
    {
        if (id is null)
            return null;
        
        var term = id?.Trim();
        var license = All.Find(l => l.Id.Equals(term, StringComparison.InvariantCultureIgnoreCase));
        return license;
    }

    public static Licenses Load()
    {
        var assembly = typeof(Program).Assembly;
        var streams = assembly
            .GetManifestResourceNames()
            .Where(n => n.Contains("Licenses"))
            .Select(n => assembly.GetManifestResourceStream(n))
            .ToList();
        
        var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();

        var licenses = new Licenses();
        foreach (var stream in streams)
        {
            using var streamReader = new StreamReader(stream!);
            var text = streamReader.ReadToEnd();
            var document = Markdown.Parse(text, pipeline);
            var license = GetFrontMatter<License>(document);
            license.Text = Markdown.ToPlainText(text, pipeline);
            
            licenses.All.Add(license);
        }

        return licenses;
    }

    private static T GetFrontMatter<T>(MarkdownDocument document)
    {
        var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
        
        var block = document
            .Descendants<YamlFrontMatterBlock>()
            .FirstOrDefault();

        var yaml =
            block!
                // this is not a mistake
                // we have to call .Lines 2x
                .Lines // StringLineGroup[]
                .Lines // StringLine[]
                .OrderByDescending(x => x.Line)
                .Select(x => $"{x}\n")
                .ToList()
                .Select(x => x.Replace("---", string.Empty))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Aggregate((s, agg) => agg + s);

        return deserializer.Deserialize<T>(yaml);
    }
}