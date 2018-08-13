using System.IO;

namespace DFramework.Infrastructure
{
    /// <summary>
    ///  A helper class for Directory operations.
    /// </summary>
    public class DirectoryHelper
    {
        /// <summary>
        ///  Creates a new directory if it does not exists.
        /// </summary>
        /// <param name="directory">Directory to create</param>
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}