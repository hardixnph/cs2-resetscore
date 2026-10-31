using System.Reflection;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace ResetScore;

public class ResetScore : BasePlugin
{
    public override string ModuleName => "ResetScore";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Hardix";
    public override string ModuleDescription => "Reset your score with !rs or !resetscore";

    // {tag} -> chat color code, built from every public static member of ChatColors ({green}, {red}, {lime}, ...)
    private static readonly Dictionary<string, string> ColorTags = typeof(ChatColors)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(f => (f.Name, Value: f.GetValue(null)))
        .Concat(typeof(ChatColors)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Select(p => (p.Name, Value: p.GetValue(null))))
        .Where(c => c.Value is char or string)
        .GroupBy(c => c.Name.ToLowerInvariant())
        .ToDictionary(g => $"{{{g.Key}}}", g => g.First().Value!.ToString()!, StringComparer.OrdinalIgnoreCase);

    [ConsoleCommand("css_rs", "Reset your score")]
    [ConsoleCommand("css_resetscore", "Reset your score")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnResetScoreCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid)
            return;

        var stats = player.ActionTrackingServices?.MatchStats;
        if (stats == null)
            return;

        if (stats.Kills == 0 && stats.Deaths == 0 && stats.Assists == 0 && stats.Damage == 0
            && player.Score == 0 && player.MVPs == 0)
        {
            player.PrintToChat(FormatMessage("AlreadyReset"));
            return;
        }

        stats.Kills = 0;
        stats.Deaths = 0;
        stats.Assists = 0;
        stats.Damage = 0;
        player.Score = 0;
        player.MVPs = 0;

        Utilities.SetStateChanged(player, "CCSPlayerController", "m_pActionTrackingServices");
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_iScore");
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_iMVPs");

        player.PrintToChat(FormatMessage("ResetScore"));
    }

    private string FormatMessage(string key)
    {
        string prefix = Localizer["Prefix"];
        string message = Localizer[key];
        string full = string.IsNullOrWhiteSpace(prefix) ? message : $"{prefix} {message}";

        // Leading space is required, otherwise CS2 ignores a color code at the very start of the message.
        return $" {ReplaceColorTags(full)}";
    }

    private static string ReplaceColorTags(string input)
    {
        foreach (var (tag, color) in ColorTags)
            input = input.Replace(tag, color, StringComparison.OrdinalIgnoreCase);

        return input;
    }
}
