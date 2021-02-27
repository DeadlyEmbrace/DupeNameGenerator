using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DupeNameGenerator
{
    /// <summary>
    /// This class contains the name lists that names will be chosen from
    /// </summary>
    public static class NameLists
    {
        /// <summary>
        /// Populate name lists from the files
        /// </summary>
        public static void PopulatedNames()
        {
            Debug.Log("Loading names from files");
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), NameListDirectory);
            FemaleNames.AddRange(File.ReadAllLines(Path.Combine(path, "FemaleNames.txt")));
            MaleNames.AddRange(File.ReadAllLines(Path.Combine(path, "MaleNames.txt")));
            GenderNeutral.AddRange(File.ReadAllLines(Path.Combine(path, "GenderNeutralNames.txt")));
            Surnames.AddRange(File.ReadAllLines(Path.Combine(path, "Surnames.txt")));
            Titles.AddRange(File.ReadAllLines(Path.Combine(path, "Titles.txt")));
            Debug.Log("Loaded names from files");
        }

        /// <summary>
        /// Female name list
        /// </summary>
        public static List<string> FemaleNames = new List<string>();

        /// <summary>
        /// Male name list
        /// </summary>
        public static List<string> MaleNames = new List<string>();

        /// <summary>
        /// Gender neutral name list
        /// </summary>
        public static List<string> GenderNeutral = new List<string>();

        /// <summary>
        /// Surname list
        /// </summary>
        public static List<string> Surnames = new List<string>();

        /// <summary>
        /// Title list
        /// </summary>
        public static List<string> Titles = new List<string>();

        /// <summary>
        /// Directory where the name lists are stored
        /// </summary>
        private static string NameListDirectory = "NameLists";
    }
}