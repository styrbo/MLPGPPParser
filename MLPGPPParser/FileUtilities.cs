namespace MLPGPPParser;

public static class FileUtilities {
    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        var dir = new DirectoryInfo(sourceDir);
        Directory.CreateDirectory(destinationDir);

        // Copy files
        foreach (FileInfo file in dir.GetFiles())
            file.CopyTo(Path.Combine(destinationDir, file.Name), true);

        // Copy subdirectories recursively
        if (recursive)
            foreach (DirectoryInfo subDir in dir.GetDirectories())
                CopyDirectory(subDir.FullName, Path.Combine(destinationDir, subDir.Name), true);
    }
}
