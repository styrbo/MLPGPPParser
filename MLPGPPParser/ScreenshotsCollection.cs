namespace MLPGPPParser;

public class ScreenshotsCollection {

    public record class ScreenshotBucketData(uint screenshotsCount, string[] screenshotsFormats);

    public ScreenshotBucketData[] buckets;
    
    public Screenshot GetHeroScreenshot(uint bucketID) {
        return new Screenshot(bucketID, 0, buckets[bucketID].screenshotsFormats[0]);
    }

    public List<Screenshot> GetOtherScreenshots(uint bucketID) {
        var list = new List<Screenshot>();
        var counts = buckets[bucketID].screenshotsCount;

        for (uint i = 1; i < counts; i++) {
            var screenshot = new Screenshot(bucketID, i, buckets[bucketID].screenshotsFormats[i]);
            list.Add(screenshot);
        }

        return list;
    }

    public static ScreenshotsCollection LoadScreenshots(string screenshotsDBPath) {
        List<ScreenshotBucketData> items = [];
        uint currentBucketID = 0;
        while (true) {
            var folderPath = Path.Combine(screenshotsDBPath, currentBucketID.ToString());

            if (Directory.Exists(folderPath) == false)
                break;

            var screenshots = ScreenshotsId(folderPath)
                .Order()
                .ToArray();

            if (screenshots.Length == 0)
                break;
            if (screenshots[0].Item1 != 0) {
                ConsoleDrawer.DrawError($"screenshots id's not correct for: {folderPath}");
                continue;
            }
            
            var formats = screenshots.Select(s => s.Item2).ToArray();
            
            items.Add(new ScreenshotBucketData((uint) screenshots.Length, formats));
            currentBucketID++;
        }
        
        
        ConsoleDrawer.DrawText($"Loaded {items.Count} screenshots buckets");

        return new ScreenshotsCollection {
            buckets = items.ToArray()
        };
    }

    private static IEnumerable<(uint, string)> ScreenshotsId(string folderPath) {
        var files = Directory.EnumerateFiles(folderPath);

        foreach (var file in files) {
            if (ValidateScreenshot(file, out var screenshotID, out var format))
                yield return (screenshotID, format);
        }
    }

    private static bool ValidateScreenshot(string filePath, out uint screenshotID, out string format) {
        screenshotID = 0;
        format = "";
        
        filePath = filePath.Replace('\\', '/');

        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        var splittedPath = filePath.Split('/');

        if (splittedPath.Length < 2)
            return false;

        var splitedFile = splittedPath[^1].Split('.');

        if (splitedFile.Length > 2)
            return false;
        
        format = splitedFile[^1]; 

        return uint.TryParse(splitedFile[0], out screenshotID);
    }
}
