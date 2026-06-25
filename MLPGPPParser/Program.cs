// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MLPGPPParser;

Console.WriteLine("Hello, World!");

var configPath = $@"{Environment.CurrentDirectory}\Resources\config.json";

ConsoleDrawer.DrawText("Loading config...");
if (File.Exists(configPath) == false) {
    CreateEmptyConfig();
    return ConsoleDrawer.DrawError("Config file not found,  created new one");
}

var configJson = File.ReadAllText(configPath);
var config = JsonSerializer.Deserialize<Config>(configJson);
if (config == null) {
    CreateEmptyConfig();
    return ConsoleDrawer.DrawError("Config file is invalid, created new one");
}

void CreateEmptyConfig() {
    var newConfig = new Config(
        "idk",
        "idk",
        "source!A3:X",
        "O:/MLGPP/s");
    
    var newConfigJson = JsonSerializer.Serialize(newConfig);
    Directory.CreateDirectory(configPath.Substring(0, configPath.LastIndexOf('\\'))); // (Resources\config.json)
    File.WriteAllText(configPath, newConfigJson);
}


var initializer = new SheetsService.Initializer();
initializer.ApiKey = config.APIToken;

var api = new SheetsService(initializer);

var request = api.Spreadsheets.Values.Get(config.sheetID, config.dataRange);

ConsoleDrawer.DrawText("Loading sheet data...");

var sheetData = ConsoleDrawer.RunTask(request.ExecuteAsync());

ConsoleDrawer.DrawText("Parsing data...");

var games = ConsoleDrawer.RunTask(Task.Run(() => Parser.ParseGames(sheetData.Values)));


ConsoleDrawer.DrawText("Done!");

return 0;
