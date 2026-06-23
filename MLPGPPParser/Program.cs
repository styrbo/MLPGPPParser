// See https://aka.ms/new-console-template for more information

using Google.Apis.Sheets.v4;
using MLPGPPParser;

Console.WriteLine("Hello, World!");


var initializer = new SheetsService.Initializer();
initializer.ApiKey = "<KEY>";

var api = new SheetsService(initializer);

var request = api.Spreadsheets.Get("id");

var sheet = await request.ExecuteAsync();

var gridData = sheet.Sheets[0].Data[0].RowData[0].Values[1].FormattedValue;

var test = new SerializedTagData();