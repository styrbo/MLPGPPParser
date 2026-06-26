namespace MLPGPPParser;

public class Parser {
    public enum ColumnType : int {
        Id = 0,
        Name = 1,
        Version = 2,
        Platform = 3,
        Status = 6,
        Engine = 7,
        Source = 8,
        SourcePlatform = 9,
        ArchiveBuildId = 10,
        Distributing = 11,
        AgeRestriction = 12,
        ScreenshotsBucketId = 14,
        ShortDescription = 15,
        GenresOrTags = 16,
        Creator = 17,
        ReleaseData = 18,
        RecordTypeData = 19,
        Characters = 20,
        Playtime = 21,
        LastUpdate = 22,
    }

    public enum RecordType {
        None = 0,
        Game,
        Mod,
        GameSource,
        Lost,
        BuildBroken,
        DLC,
    }

    public Parser(ScreenshotsCollection screenshotsCollection) {
        _screenshotsCollection = screenshotsCollection;
    }

    private ScreenshotsCollection _screenshotsCollection;

    private static RecordType GetRecordType(string type) {
        switch (type) {
            case "Game":
                return RecordType.Game;
            case "MOD":
                return RecordType.Mod;
            case "Game Source":
                return RecordType.GameSource;
            case "Lost":
                return RecordType.Lost;
            case "Build Broken":
                return RecordType.BuildBroken;
            case "DLC":
                return RecordType.DLC;
            default:
                return RecordType.None;
        }
    }

    private static Tag GetTag(string tag) {
        tag = tag.ToLower();

        switch (tag) {
            case "visual novel":
                return Tag.visual_novel;
            case "puzzle":
                return Tag.puzzle;
            case "action":
                return Tag.action;
            case "rpg":
                return Tag.rpg;
            case "platformer":
                return Tag.platformer;
            case "jrpg":
                return Tag.jrpg;
            case "shoot 'em Up":
                return Tag.Shoot_em_Up;
            case "arcade":
                return Tag.arcade;
            case "point & click":
                return Tag.pointANDclick;
            case "tamagotchi":
                return Tag.tamagotchi;
            case "adventure":
                return Tag.adventure;
            case "shooter":
                return Tag.shooter;
            case "clicker":
                return Tag.clicker;
            case "karaoke":
                return Tag.karaoke;
            case "horror":
                return Tag.horror;
            case "detective":
                return Tag.detective;
            case "racing":
                return Tag.racing;
            case "open world":
                return Tag.open_world;
            case "side scroller":
                return Tag.side_scroller;
            case "survival":
                return Tag.survival;
            case "tower defence":
                return Tag.tower_defence;
            case "mmo":
                return Tag.mmo;
            case "fighting":
                return Tag.fighting;
            case "casual":
                return Tag.casual;
            case "sport":
                return Tag.sport;
            case "strategy":
                return Tag.strategy;
            case "humor":
                return Tag.humor;
            case "quest":
                return Tag.quest;
            case "sandbox":
                return Tag.sandbox;
            case "simulation":
                return Tag.simulation;
            case "dress up":
                return Tag.dress_up;
            case "social":
                return Tag.social;
            case "2d":
                return Tag._2d;
            case "3d":
                return Tag._3d;
            case "top down":
                return Tag.top_down;
            case "first person":
                return Tag.first_person;
            case "third person":
                return Tag.thrid_person;
            case "cash grab":
                return Tag.cash_grab;
            case "official":
                return Tag.official;
            case "anthro":
                return Tag.anthro;
            case "coop":
                return Tag.coop;
            case "online":
                return Tag.online;
            case "released":
                return Tag.Released;
            case "in Development":
                return Tag.In_Development;
            case "custom/none":
                return Tag.customORnone;
            case "flash":
                return Tag.flash;
            case "unity":
                return Tag.unity;
            case "ue":
                return Tag.ue;
            case "godot":
                return Tag.godot;
            case "gamemaker":
                return Tag.gamemaker;
            case "construct":
                return Tag.construct;
            case "rpgmaker":
                return Tag.rpgmaker;
            case "ren'py":
                return Tag.RenPy;
            case "blender":
                return Tag.blender;
            case "roblox":
                return Tag.roblox;
            case "html5":
                return Tag.html5;
            case "java":
                return Tag.Java;
            case "adventure game studio":
                return Tag.Adventure_Game_Studio;
            case "fm2k":
                return Tag.FM2K;
            case "z-engine":
                return Tag.Z_Engine;
            case "löve":
                return Tag.LoveEngine;
            case "clickteam fusion":
                return Tag.ClickteamFusion;
            case "m6":
                return Tag.M6;
            case "cmc":
                return Tag.CMC;
            case "secondary cast":
                return Tag.Secondary_cast;
            case "background":
                return Tag.Background;
            case "oc":
                return Tag.OC;
            case "<5 min":
                return Tag.LESS_5_min;
            case "5m-30m":
                return Tag.FROM_5m_TO_30m;
            case "30m-2h":
                return Tag.FROM_30m_TO_2h;
            case "2h-4h":
                return Tag.FROM_2h_TO_4h;
            case "4h-10h":
                return Tag.FROM_4h_TO_10h;
            case "10h+":
                return Tag.MORE_10h;
            default:
                return Tag.None;
        }
    }

    // op auto conversion from int to column type

    private static bool IsCellHasContent(string line) {
        if (string.IsNullOrWhiteSpace(line) == false && line != "?" && line != "~")
            return true;

        return false;
    }

    private static string GetDataFromCell(IList<object> line, ColumnType columnType) {
        if (line.Count <= (int)columnType)
            return string.Empty;
        
        var obj = line[(int)columnType];

        return (string)obj;
    }

    private static IEnumerable<Tag> GetTags(string lane, char separator = ',') {
        var tagsText = lane.Split(separator);
        foreach (var tagText in tagsText) {
            var tag = GetTag(tagText);

            if (tag != Tag.None) {
                yield return tag;
            }
        }
    }

    private static Tag GetDevelopmentStatusTag(string status) {
        switch (status) {
            case "released":
                return Tag.Released;
            default:
                return Tag.In_Development;
        }
    }

    public GameData[] ParseGames(IList<IList<object>> sheetData) {
        var games = new Dictionary<uint, GameData>();

        for (var index = 0; index < sheetData.Count; index++) {
            var row = sheetData[index];
            
            if (TryParseLine(games, row, out var errorMessage) == false) {
                ConsoleDrawer.DrawError($"can't parse row {index} {errorMessage}");
                
                continue;
            } else {
                ConsoleDrawer.DrawText($"parsed row {index}");
            }
        }

        return games.Values.ToArray();
    }

    public static bool IsValidGameEntry(IList<object> line, out string errorMessage) {
        if (line.All(o => string.IsNullOrWhiteSpace(o.ToString()))) {
            errorMessage = "empty lane";
            return false;
        }
        
        var idData = GetDataFromCell(line, ColumnType.Id);

        if (uint.TryParse(idData, out var id) == false) {
            errorMessage = "id is not a number";
            return false;
        }

        var name = GetDataFromCell(line, ColumnType.Name);
        if (string.IsNullOrWhiteSpace(name)) {
            errorMessage = "name is empty";
            return false;
        }

        var buildIdText = GetDataFromCell(line, ColumnType.ArchiveBuildId);
        if (IsCellHasContent(buildIdText) == false) {
            errorMessage = "no valid build id";
            return false;
        }
        
        var recordTypeText = GetDataFromCell(line, ColumnType.RecordTypeData);
        var recordType = GetRecordType(recordTypeText);
        if (recordType != RecordType.Game && recordType != RecordType.GameSource) {
            errorMessage = "not a game/source";
            return false;
        }

        errorMessage = "";
        return true;
    }

    public bool TryParseLine(IDictionary<uint, GameData> games, IList<object> line, out string errorMessage) {
        if (IsValidGameEntry(line, out errorMessage) == false) {
            return false;
        }

        var gameIdText = GetDataFromCell(line, ColumnType.Id);
        var gameID = uint.Parse(gameIdText);

        if (games.TryGetValue(gameID, out var gameData) == false) {
            var gameName = GetDataFromCell(line, ColumnType.Name);
            var shortDescription = GetDataFromCell(line, ColumnType.ShortDescription);
            var genreOrTags = GetDataFromCell(line, ColumnType.GenresOrTags);
            var statusData = GetDataFromCell(line, ColumnType.Status);
            var engineData = GetDataFromCell(line, ColumnType.Engine);
            var characterData = GetDataFromCell(line, ColumnType.Characters);
            var playtimeData = GetDataFromCell(line, ColumnType.Playtime);
            var authors = GetDataFromCell(line, ColumnType.Creator);

            var releaseData = GetDataFromCell(line, ColumnType.ReleaseData);

            var screenshotsBucketId = GetDataFromCell(line, ColumnType.ScreenshotsBucketId);

            var tags = GetTags(genreOrTags);

            if (IsCellHasContent(statusData)) {
                var statusTag = GetDevelopmentStatusTag(statusData);
                tags = tags.Append(statusTag);
            }

            if (IsCellHasContent(engineData)) {
                var engine = GetTag(engineData);
                tags = tags.Append(engine);
            }

            if (IsCellHasContent(characterData)) {
                var characters = GetTags(characterData);
                tags = tags.Concat(characters);
            }

            if (IsCellHasContent(playtimeData)) {
                var playtime = GetTag(playtimeData);
                tags = tags.Append(playtime);
            }

            Screenshot heroScreenshot = null;
            List<Screenshot> otherScreenshots = null;
            if (IsCellHasContent(screenshotsBucketId)
                && uint.TryParse(screenshotsBucketId, out var screenshotsBuckedId)) {
                heroScreenshot = _screenshotsCollection.GetHeroScreenshot(screenshotsBuckedId);
                otherScreenshots = _screenshotsCollection.GetOtherScreenshots(screenshotsBuckedId);
            }

            if (TryParseGameBuild(line, out var build, out var buildErrorMessage) == false) {
                errorMessage = buildErrorMessage;
                return false;
            }
            
            var sourceURL = GetDataFromCell(line, ColumnType.Source);

            if (tags.Contains(Tag.None)) {
                errorMessage = "no valid tags";
                return false;
            }

            gameData = new GameData(
                Id: gameID,
                Name: gameName,
                ShortDescription: shortDescription,
                Tags: new TagsCollection(tags),
                ReleaseDate: releaseData,
                SourceURL: sourceURL,
                Author: authors,
                HeroScreenshot: heroScreenshot,
                OtherScreenshots: otherScreenshots,
                new List<GameBuild> {
                    build
                }
            );
            
            games[gameID] = gameData;
            errorMessage = "";
            return true;
        }
        
        if (TryParseGameBuild(line, out var build1, out var buildErrorMessage1) == false) {
            errorMessage = buildErrorMessage1;
            return false;
        }
        
        gameData.Builds.Add(build1);
        errorMessage = "";
        return true;
    }

    private bool TryParseGameBuild(IList<object> line, out GameBuild build, out string errorMessage) {
        var buildIdText = GetDataFromCell(line, ColumnType.ArchiveBuildId);
        var version = GetDataFromCell(line, ColumnType.Version);
        var releaseDate = GetDataFromCell(line, ColumnType.ReleaseData);

        if (IsCellHasContent(buildIdText) == false ||
            uint.TryParse(buildIdText, out var buildId) == false) {
            errorMessage = "no valid build id";
            build = null;

            return false;
        }

        build = new GameBuild(buildId, version, releaseDate);
        errorMessage = "";
        return true;
    }
}
