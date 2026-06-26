namespace MLPGPPParser;

public static class DBFormater {
    private const string ScreenshotsFolderLocalPath = @"/s/";
    private const string ScreenshotPlaceholder = "https://placehold.co/800x450/533483/eaeaea?text=Screenshot+1";

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
            characters = ["67", "pls fix that"],
            playtime = "erm oops",
            url = game.SourceURL,
            dateAdded = game.ReleaseDate,
            screenshots = GetScreenshotsPaths(game.OtherScreenshots)
        };

        return gameData;
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

    private static SerializedTagData[] FormatTags(TagsCollection tags) {
        var formattedTags = new SerializedTagData[tags.Tags.Count];
        var index = 0;

        foreach (var tag in tags.Tags) {
            formattedTags[index] = new SerializedTagData() {
                label = tag.Key.ToString(),
                type = tag.Value.ToString()
            };
            index++;
        }

        return formattedTags;
    }
}
