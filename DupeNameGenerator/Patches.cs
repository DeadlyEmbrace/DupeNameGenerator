using System;
using System.Collections.Generic;
using Harmony;

namespace DupeNameGenerator
{
    public class Patches
    {
        public static class Mod_OnLoad
        {
            /// <summary>
            /// Occurs when the mod is loaded
            /// </summary>
            public static void OnLoad()
            {
                NameLists.PopulatedNames();
            }
        }

        /// <summary>
        /// Hooks into the <see cref="MinionStartingStats"/> constructor and change the name
        /// </summary>
        [HarmonyPatch(typeof(MinionStartingStats), MethodType.Constructor, new[] { typeof(bool), typeof(string) })]
        public class DupeRandomNameGenerationPatch
        {
            /// <summary>
            /// Occurs after the constructor has executed
            /// </summary>
            /// <param name="__instance">MinionStartingStats instance</param>
            /// <param name="is_starter_minion">Indicate if this is a starting dupe</param>
            /// <param name="guaranteedAptitudeID">Id of aptitude the dupe is guaranteed to have</param>
            public static void Postfix(MinionStartingStats __instance, bool is_starter_minion, string guaranteedAptitudeID = null)
            {
                var newName =  ChooseNameForDupe(__instance.GenderStringKey);
                if(newName == string.Empty)
                {
                    Debug.Log("No name could be found, dupe will not be renamed");
                    return;
                }

                var postfix = ChooseSurnameOrTitle();
                RenameDupe(__instance, $"{newName} {postfix}");
            }

            /// <summary>
            /// Handles the actual renaming of the dupe
            /// </summary>
            /// <param name="instance">MinionStartingStats instance</param>
            /// <param name="newName">Name to give the dupe</param>
            private static void RenameDupe(MinionStartingStats instance, string newName)
            {
                instance.Name = newName;
            }

            /// <summary>
            /// Randomly checks if a dupe should get a title or surname then return the value
            /// </summary>
            /// <returns>Surname or title</returns>
            private static string ChooseSurnameOrTitle()
            {
                var rand = new Random();
                if (TitleChance.Contains(rand.Next(0, 100)) && NameLists.Titles.Count > 0)
                {
                    return NameLists.Titles[rand.Next(0, NameLists.Titles.Count - 1)];
                }

                if(NameLists.Surnames.Count > 0)
                {
                    return NameLists.Surnames[rand.Next(0, NameLists.Surnames.Count - 1)];
                }

                return string.Empty;
            }

            /// <summary>
            /// Handles the renaming of a generated dupe
            /// </summary>
            /// <param name="genderKey">The gender key to use  for picking a dupe name</param>
            private static string ChooseNameForDupe(string genderKey)
            {
                var rand = new Random();
                var name = string.Empty;
                
                switch (genderKey)
                {
                    case "FEMALE":
                        if (NameLists.FemaleNames.Count > 0)
                        {
                            name = NameLists.FemaleNames[rand.Next(0, NameLists.FemaleNames.Count - 1)];
                        }

                        break;
                    case "MALE":
                        if(NameLists.MaleNames.Count > 0)
                        {
                            name = NameLists.MaleNames[rand.Next(0, NameLists.MaleNames.Count - 1)];}
                        break;
                    case "NB":
                        if(NameLists.GenderNeutral.Count > 0)
                        {

                        }
                        name = NameLists.GenderNeutral[rand.Next(0, NameLists.GenderNeutral.Count - 1)];
                        break;
                    default:
                        Debug.Log($"Gender {genderKey} is unknown and will not be handled");
                        break;
                }

                return name;
            }

            /// <summary>
            /// List used to check if dupe should be given a title
            /// </summary>
            private static readonly List<int> TitleChance = new List<int> {0, 25, 75, 100};
        }
    }
}
