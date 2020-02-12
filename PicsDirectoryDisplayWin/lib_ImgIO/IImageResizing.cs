namespace PicsDirectoryDisplayWin.lib_ImgIO
{
    using System.IO;

    /// <summary>
    /// Represents an image resizer.
    /// </summary>
    public interface IImageResizing
    {
        ImageResizing Resize(int width, int height);
        ImageResizing Quality(int quality);
        void Save(string path, bool dispose);
        MemoryStream ToStream();
    }
}
