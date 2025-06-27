using System.IO;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    public static class FileAsyncHelper
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var sr = new StreamReader(fs))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public static async Task WriteAllTextAsync(string path, string contents)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            using (var sw = new StreamWriter(fs))
            {
                await sw.WriteAsync(contents);
            }
        }
    }
}
