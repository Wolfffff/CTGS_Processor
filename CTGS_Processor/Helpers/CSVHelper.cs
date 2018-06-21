using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CTGS_Processor.Helpers
{
    public class CSVHelper
    {

        public static Tuple<string[,], List<string>, List<string>> LoadAndConvert(string location, string regexPattern)
        {
            try
            {
                var ids = new List<string>();
                //Soft from oldest to newest
                var list = new List<string>();
                var outputLines = new List<string[]>();
                var temp = "";

                //Skip header
                List<string> lines = File.ReadAllLines(location).Skip(1).ToList();


                foreach (string line in lines)
                {
                    outputLines.Add(line.Split(",").ToArray());
                }
                var ticker = 0;

                var R = outputLines.Count;
                var C = outputLines[0].Length;
                var arr = new string[R, C];
                for (int r = 0; r != R; r++)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(outputLines[r][1], regexPattern))
                    {
                        for (int c = 0; c != C; c++)
                        {
                                
                            

                            temp += outputLines[r][c] + ",";
                            if (!ids.Contains(outputLines[r][1])) //Get list of ids
                            {
                                ids.Add(outputLines[r][1]);

                            };
                            if (c == C - 1)
                            {
                                list.Add(temp.TrimEnd(','));
                                temp = "";
                              
                            }

                        }
                        ticker += 1;
                    }
                }
                var finalArr = new string[ticker, C];
                int ticker2 = 0;
                for (int r = 0; r != R; r++) //Craft correct size array
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(outputLines[r][1], regexPattern))
                    {
                        
                        for (int c = 0; c != C; c++)
                        {
                            finalArr[ticker2, c] = outputLines[r][c];
                        }
                        ticker2 += 1;
                    }
                }

                return new Tuple<string[,], List<string>, List<string>>(finalArr, ids, list);
            }
            catch (Exception e)
            {
                throw new Exception("Error in finding or preprocessing \n", e);
            }

        }



        public static void Single(List<string> list, string fileName){
            using (var file = File.CreateText("OUTPUT/" + fileName))
            {
                foreach (var e in list)
                {
                    file.WriteLine(e);
                }


            }
        }

        public static void CreateEntityGroupCsv(List<string> addedList, List<string> removalList){
            using (var file = File.CreateText("OUTPUT/AddedAndRemovedContingencies.csv"))
            {
                file.WriteLine("Added");
                foreach (var e in addedList)
                {
                    file.WriteLine(e);
                }
                file.WriteLine(" ");
                file.WriteLine("Removed");
                foreach (var e in removalList)
                {
                    file.WriteLine(e);
                }


            }
        }
    }
}
