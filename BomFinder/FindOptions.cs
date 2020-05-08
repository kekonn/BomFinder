using System;
using CommandLine;

namespace BomFinder
{
    [Verb("find", isDefault: true, HelpText = "Find files and print them")]
    public class FindOptions
    {
        [Option('s',"search-pattern", Required = true, HelpText = "The pattern that defines what files to look for, i.e. \"*.cs\"")]
        public string SearchPattern { get; set; }

        [Option('d',"directory", Required = false, Default = "", HelpText = "The directory to search. Defaults to current directory if unspecified.")]
        public string Directory { get; set; }

        [Option('r',"remove-bom", Required = false, Default = false, HelpText = "When true, removes the BOM from the found files.")]
        public bool RemoveBOM { get; set; }
    }
}
