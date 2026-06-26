namespace MLPGPPParser;

public class ScreenshotsCollection {
    
    public int[] CountPerBucket;

    public Screenshot GetHeroScreenshot(uint bucketID) {
        return new Screenshot(bucketID, 0);
    }

    public List<Screenshot> GetOtherScreenshots(uint bucketID) {
        var list = new List<Screenshot>();
        var counts = CountPerBucket[bucketID];

        for (uint i = 1; i < counts; i++) {
            var screenshot = new Screenshot(bucketID, i);
            list.Add(screenshot);
        }

        return list;
    }

    public static ScreenshotsCollection LoadScreenshots(string screenshotsDBPath) {
        List<int> items = [];
        uint currentBucketID = 0;
        while (true) {
            var folderPath = Path.Combine(screenshotsDBPath, currentBucketID.ToString());

            if (!Directory.Exists(folderPath) == false)
                break;

            var screenshots = ScreenshotsId(folderPath)
                .Order()
                .ToArray();

            if (screenshots.Length == 0)
                break;
            if (screenshots[0] != 0) {
                ConsoleDrawer.DrawError($"screenshots id's not correct for: {folderPath}");
                continue;
            }
            
            items.Add(screenshots.Length);
            currentBucketID++;
        }

        return new ScreenshotsCollection {
            CountPerBucket = items.ToArray(),
        };
    }

    private static IEnumerable<uint> ScreenshotsId(string folderPath) {
        var files = Directory.EnumerateFiles(folderPath);

        foreach (var file in files) {
            if (ValidateScreenshot(file, out var screenshotID))
                yield return screenshotID;
        }
    }

    private static bool ValidateScreenshot(string filePath, out uint screenshotID) {
        screenshotID = 0;

        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        var splittedPath = filePath.Split('/');

        if (splittedPath.Length < 2)
            return false;

        var splitedFile = splittedPath[^1].Split('.');

        if (splitedFile.Length > 2)
            return false;

        return uint.TryParse(splitedFile[0], out screenshotID);
    }
}
