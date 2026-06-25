namespace MLPGPPParser;

[Serializable]
public record Config(
    string APIToken,
    string sheetID,
    string dataRange,
    string localScreenshotsDBPath);
