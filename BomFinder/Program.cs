using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using CommandLine;

namespace BomFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<FindOptions>(args)
                .WithParsed(RunFind);
        }

        private static void RunFind(FindOptions options)
        {
            var directory = string.IsNullOrWhiteSpace(options.Directory)?Environment.CurrentDirectory:options.Directory;

            var files = FindFiles(directory, options.SearchPattern);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            if (options.RemoveBOM)
            {
                Console.WriteLine("Removing BOM");

                files.AsParallel().ForAll(RemoveBOM);

                Console.WriteLine("BOM revomed on all files");
            }
        }

        private static string[] FindFiles(string directory, string searchPattern)
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories).AsParallel().Where(f => HasBom(f)).ToArray();
        }

        private static bool HasBom(string fileName)
        {
            using var fs = File.OpenRead(fileName);

            var bom = new byte[4];
            var bytesRead = fs.Read(bom, 0, 4);

            return bytesRead == 4 &&
                (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf);

        }

        private static void RemoveBOM(string fileName)
        {
            var contents = File.ReadAllText(fileName);
            File.WriteAllText(fileName, contents, new UTF8Encoding(false));
        }
    }
}
