namespace System.IO
{
    public static class IOExtensions
    {
        public static FileInfo[] GetFilesSafe(this DirectoryInfo directoryInfo, string searchPattern)
        {
            try
            {
                return directoryInfo.GetFiles(searchPattern);
            }
            catch (DirectoryNotFoundException)
            {
                return Array.Empty<FileInfo>();
            }
        }

        public static DirectoryInfo[] GetDirectoriesSafe(this DirectoryInfo directoryInfo, string searchPattern)
        {
            try
            {
                return directoryInfo.GetDirectories(searchPattern);
            }
            catch (DirectoryNotFoundException)
            {
                return Array.Empty<DirectoryInfo>();
            }
        }
    }
}
