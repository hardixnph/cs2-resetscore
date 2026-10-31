# CS2 ResetScore

A CounterStrikeSharp plugin that lets players reset their score (kills, deaths, assists, damage, MVPs, score).

## Commands

| Chat | Console |
|------|---------|
| `!rs` / `/rs` | `css_rs` |
| `!resetscore` / `/resetscore` | `css_resetscore` |

## Installation

1. Build: `dotnet build src -c Release`
2. Copy the contents of `src/bin/Release/net10.0/` to
   `addons/counterstrikesharp/plugins/ResetScore/` (keep the `lang` folder next to `ResetScore.dll`).

## Messages

Edit `lang/en.json`:

```json
{
  "Prefix": "{green}[ResetScore]{default}",
  "ResetScore": "{lime}Your score has been reset!",
  "AlreadyReset": "{red}Your score is already 0!"
}
```

`Prefix` is added in front of every message automatically. Leave it as `""` to disable it.

### Colors

`{default}` `{white}` `{darkred}` `{green}` `{lightyellow}` `{lightblue}` `{olive}` `{lime}` `{red}`
`{lightpurple}` `{purple}` `{grey}` `{yellow}` `{gold}` `{silver}` `{blue}` `{darkblue}` `{bluegrey}`
`{magenta}` `{lightred}` `{orange}`
