namespace MLPGPPParser;

public static class DBFormater {
    private const string ScreenshotsFolderLocalPath = @"/s/";
    private const string ScreenshotPlaceholder = "https://derpicdn.net/img/view/2023/10/17/3221228.png";//"https://placehold.co/800x450/533483/eaeaea?text=Screenshot+1";

    private static string GetMainDownloadURL(uint buildID) {
        return $"https://games.mare.by/{buildID}";
    }

    private static string GetLabelText(Label label) {
        switch (label) {
            case Label.Genre:
                return "genre";
            case Label.Tags:
                return "other";
            default:
                throw new ArgumentOutOfRangeException(nameof(label), label, null);
        }
    }

    private static string GetPlatformText(Tag platform) {
        switch (platform) {
            case Tag.Platform_Web:
                return "Web";
            case Tag.Platform_Windows:
                return "Windows";
            case Tag.Platform_OSX:
                return "OSX";
            case Tag.Platform_WindowsX32:
                return "Windows";
            case Tag.Platform_Android:
                return "Android";
            case Tag.Platform_Linux:
                return "Linux";
            case Tag.Platform_Ios:
                return "IOS";
            case Tag.Platform_Ps4:
                return "Playstation 4";
            case Tag.Platform_Xboxone:
                return "XBox One";
            case Tag.Platform_Switch:
                return "Nintendo Switch";
            case Tag.Platform_SNES:
                return "SNES";
            case Tag.Platform_NintendoDS:
                return "Nintendo DS";
            case Tag.Platform_GBA:
                return "GBA";
            case Tag.Platform_GameBoy:
                return "Game Boy";
            case Tag.Platform_Roblox:
                return "Roblox";
            case Tag.Platform_Source_Code:
                return "Source code";
            case Tag.Platform_VR:
                return "VR";
            case Tag.Platform_Flash:
                return "Flash";
            default:
                throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
        }
    }

    public static SerializedGamesCollectionData FormatGames(Config cfg, GameData[] games) {
        var formatedGames = new GameSerializedData[games.Length];

        for (int i = 0; i < games.Length; i++) {
            var game = games[i];
            formatedGames[i] = FormatGame(game);
        }

        return new SerializedGamesCollectionData() {
            games = formatedGames,
        };
    }

    private static GameSerializedData FormatGame(GameData game) {
        
        
        
        var gameData = new GameSerializedData() {
            id = (int)game.Id,
            title = game.Name,
            author = game.Author,
            thumbnail = GetScreenshotPath(game.HeroScreenshot),
            shortDescription = game.ShortDescription,
            fullDescription = game.ShortDescription,
            tags = FormatTags(game.Tags),
            characters = FormatCharacters(game.Tags),
            playtime = FormatPlaytime(game.playtime),
            status = FormatStatus(game),
            engine = FormatEngine(game),
            platforms = FormatPlatforms(game),
            url = game.SourceURL,
            dateAdded = game.ReleaseDate,
            downloads = FormatDownloads(game),
            screenshots = GetScreenshotsPaths(game.OtherScreenshots)
        };

        return gameData;
    }

    private static SerializedDownloadData[] FormatDownloads(GameData game) {
        if (game.Builds == null || game.Builds.Count == 0) {
            ConsoleDrawer.DrawError($"no builds for {game.Id}");
            return null;
        }
        
        var result = game.Builds
            .Where(build => build != null)
            .Select(build => new SerializedDownloadData {
                label = GetPlatformText(build.platform),
                url = GetMainDownloadURL(build.Id),
                version = build.Version
            })
            .ToArray();

        if (result.Length == 0)
            return null;

        return result;
    }

    private static string[] FormatPlatforms(GameData game) {
        if(game.Builds == null || game.Builds.Count == 0) {
            ConsoleDrawer.DrawError($"no builds for {game.Id}");
            return [];
        }

        return game.Builds
            .Where(build => build != null)
            .Select(build => build.platform)
            .Select(GetPlatformText)
            .Distinct()
            .ToArray();
    }

    private static string FormatEngine(GameData game) {
        if (game.Tags.Tags.TryGetValue(Label.Engine, out var engineTags)) {
            var engine = engineTags[0];
            return engine.ToString();
        }
        
        ConsoleDrawer.DrawError($"unknown engine: for {game.Id}");
        return "unknown";
    }

    private static string FormatStatus(GameData game) {
        if(game.Tags.Tags.TryGetValue(Label.Status, out var values) == false)
            return "released"; // assume status

        var value = values[0];

        if (value == Tag.In_Development)
            return "in_development";
        else if (value == Tag.Released)
            return "released";
        
        ConsoleDrawer.DrawError($"unknown status: for {game.Id}");
        return "unknown";
    }
    
    private static string FormatPlaytime(Tag playtimeTag) {
        switch (playtimeTag) {
            case Tag.LESS_5_min:
                return "<5 min";
            case Tag.FROM_5m_TO_30m:
                return "5m-30m";
            case Tag.FROM_30m_TO_2h:
                return "30m-2h";
            case Tag.FROM_2h_TO_4h:
                return "2h-4h";
            case Tag.FROM_4h_TO_10h:
                return "4h-10h";
            case Tag.MORE_10h:
                return "10h+";
            default:
                return "unknown";
        }
    }

    private static string[] FormatCharacters(TagsCollection collection) {
        if(collection.Tags.TryGetValue(Label.Character, out var tags) == false)
            return [];
        
        var characters = tags
            .Select(t => t.ToString())
            .ToArray();
        
        return characters;
    }

    private static string[] GetScreenshotsPaths(List<Screenshot>? screenshots) {
        if (screenshots == null) {
            return [];
        }

        var screenshotsPaths = new string[screenshots.Count];
        var index = 0;

        foreach (var screenshot in screenshots) {
            screenshotsPaths[index] = GetScreenshotPath(screenshot);
            index++;
        }

        return screenshotsPaths;
    }

    private static string GetScreenshotPath(Screenshot? screenshot) {
        if (screenshot == null) {
            return ScreenshotPlaceholder;
        }

        var pathToScreenshot = $"{ScreenshotsFolderLocalPath}{screenshot.BucketId}/{screenshot.Id}.{screenshot.format}";

        return pathToScreenshot;
    }

    private static SerializedTagData[] FormatTags(TagsCollection collection) {
        var labelsForUse = new[] {
            Label.Genre,
            Label.Tags,
        };

        var tagsForUse = collection.Tags
            .Where(keypair => labelsForUse.Contains(keypair.Key))
            .ToDictionary();


        var formattedTags = new List<SerializedTagData>();
        
        foreach (var keyPair in tagsForUse) {
            foreach (var tag in keyPair.Value) {
                formattedTags.Add(new SerializedTagData {
                    label = GetLabelText(keyPair.Key),
                    type = tag.ToString()
                });
            }
        }

        return formattedTags.ToArray();
    }
}
