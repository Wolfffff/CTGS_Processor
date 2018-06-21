using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CTGS_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            string regexPattern = "";
            Directory.CreateDirectory("OUTPUT");
            Console.Write("Entergy Contingency Comparison and Analysis Tool \n \n");
            Console.Write("Enter the leading entries of all entry ids(comma separated): ");
            var patternSelectors = Console.ReadLine().Split(',');


            for (int i = 0; i < patternSelectors.Length; i++)
            {
                if (i == patternSelectors.Length - 1)
                {
                    regexPattern += patternSelectors[i];

                }else{
                    regexPattern += patternSelectors[i] + "|";
                }
            }

            regexPattern = "^(" + regexPattern + ")+";

            Console.Write("\n \nEnter exact location of old CSV(e.g. \"C:/User/Name/Desktop/CTGS_Dec2017_Final.csv\"): \n");
            var oldLocation = Console.ReadLine();
            //oldLocation = @"/Users/Wolf/Desktop/CTGS_Dec2017_Final.csv";
            var oldList = Helpers.CSVHelper.LoadAndConvert(oldLocation, regexPattern);

            Console.Write("\nEnter exact location of new CSV: \n");
            var newLocation = Console.ReadLine();
            //newLocation = @"/Users/Wolf/Desktop/Post_CTGS_March2018.csv";
            var newList = Helpers.CSVHelper.LoadAndConvert(newLocation, regexPattern);
            var results = Helpers.ProcessingHelper.CompareLists(oldList.Item3, newList.Item3);


            var compareIdLists = Helpers.ProcessingHelper.CompareRemovedandAdded(oldList.Item2, newList.Item2);//item 3 = removed

            var modifiedList = Helpers.ProcessingHelper.CheckForModification(oldList.Item1, newList.Item1, compareIdLists.Item1);




            var removedAndAddedFromExistingContingency = Helpers.ProcessingHelper.GetDeletedAndAdded(oldList.Item1, newList.Item1, compareIdLists.Item2);

            Helpers.CSVHelper.Single(removedAndAddedFromExistingContingency.Item1, "Added.csv");
            Helpers.CSVHelper.Single(removedAndAddedFromExistingContingency.Item2, "Removed.csv");

            Console.WriteLine("\nAdded and removed entries were exported to Added.csv and Removed.csv respectively\n");

            List<string> totalModifications = new List<string>();
            totalModifications.AddRange(modifiedList.Item1);
            totalModifications.AddRange(modifiedList.Item2);
            totalModifications.AddRange(modifiedList.Item3);
            totalModifications.AddRange(modifiedList.Item4);
            totalModifications.AddRange(modifiedList.Item5);
            totalModifications.AddRange(modifiedList.Item6);
            totalModifications.AddRange(modifiedList.Item7);
            totalModifications = totalModifications.OrderBy(i => i.Split(',')[0]).ToList(); //Add all and sort accordingly. This is done so that if we ever need to use the lists outside, we have access to the individually.

            Helpers.CSVHelper.Single(totalModifications, "Modifications.csv");

            Console.WriteLine("\nModified entries were exported to Modifications.csv\n");





            //modifiedList = Helpers.PostProceessModifications(modifiedList.Item1, modifiedList.Item2, modifiedList.Item3, modifiedList.Item4,modifiedList.Item5,modifiedList.Item6, modifiedList.Item7);

            Helpers.CSVHelper.CreateEntityGroupCsv(modifiedList.Rest.Item1, compareIdLists.Item3);

            var descriptionChanges = Helpers.ProcessingHelper.CompareDescriptions(newList.Item1, oldList.Item1);

            Helpers.CSVHelper.Single(descriptionChanges, "Descriptions.csv");
            Console.WriteLine("\nContingensies with modified descriptions output to Descriptions.csv\n\n\n");

            Console.WriteLine("Press any key to exit...");


            Console.ReadKey();







        }
    }
}
