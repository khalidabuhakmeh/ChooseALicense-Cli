namespace ChooseALicense.Models;

public static class RulesExtensions
{
    public static string ToLabelBullets(this List<Rule> rules, List<string> values)
    {
        var empty = new Rule();
        var labels = values
            .Select(v => new { v, r = rules.FirstOrDefault(r => r.Tag == v) ?? empty })
            .Select(t => t.r.Label)
            .ToList();
        
        return string.Join("\n", labels.Select(l => $"[yellow]- {l}[/]"));
    }
}