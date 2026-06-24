namespace MLPGPPParser;

public static class Parser {
    
    
    public static GameData[] ParseGames(IList<IList<object>> sheetData) {
        var games = new Dictionary<uint, GameData>();

        foreach (var row in sheetData) {
            if (TryParseLine(games, row, out var errorMessage) == false) {
        
            }
        }
        
        Thread.Sleep(6769);

        return [];
    }
    
    public static bool TryParseLine(IDictionary<uint, GameData> games, IList<object> line, out string errorMessage) {
        if (line.All(o => string.IsNullOrWhiteSpace(o.ToString()))) {
            errorMessage = "empty lane";
            return false;
        }

        if (uint.TryParse(line[0].ToString(), out var id) == false) {
            errorMessage = "id is not a number";
            return false;
        }

        var name = line[1].ToString();
        if (string.IsNullOrWhiteSpace(name)) {
            errorMessage = "name is empty";
            return false;
        }

        var version = line[2].ToString();

        var platformString = line[3].ToString();
        var statusString = line[4].ToString();


        errorMessage = "";
        return true;
    }
    
    
    // lol we did not have them
    /* 

    private bool ValidatePlatform(string platform, out string errorMessage) {
        if (string.IsNullOrWhiteSpace(platform)) {
            errorMessage = "platform is empty";
            return false;
        }

        switch (platform) {
            case "?":
            case "web":
            case "windows":
            case "OSX":
            case "windows_x32":
                case 
                errorMessage = "";
                return true;
            default:
                errorMessage = $"platform {platform} is not supported by Parser";
        }
    }
    */
}
