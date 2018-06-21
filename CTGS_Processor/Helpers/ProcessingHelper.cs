using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTGS_Processor.Helpers
{
    //If we want to optimize it, we can put all of this into a single function but because the low complexity, there's no reason to worry about processing speedcat 
    public class ProcessingHelper
    {


        public static Tuple<int, int, int, List<string>> CompareLists(List<string> oldList, List<string> newList)
        {
            var csv = new StringBuilder();
            List<string> total = new List<string>();
            var added = newList.Except(oldList).ToList();
            var inter = newList.Intersect(oldList).ToList();
            var rem = oldList.Except(newList).ToList();

            string temp = "";

            for (int i = 0; i < added.Count; i++)
            {
                if (added[i].Split(',').Length == 10)
                {
                    temp = " ,";
                }
                else
                {
                    temp = "";
                }
                added[i] = string.Format("{0},{2}{1}", added[i], "Added", temp);
                total.Add(added[i]);
            }
            for (int i = 0; i < rem.Count; i++)
            {
                if (rem[i].Split(',').Length == 10)
                {
                    temp = " ,";
                }
                else
                {
                    temp = "";
                }
                rem[i] = string.Format("{0},{2}{1}", rem[i], "Removed", temp);
                total.Add(rem[i]);
            }




            total = total.OrderBy(i => i.Split(',')[1]).ToList();
            foreach (var e in total)
            {
                csv.AppendLine(e);
            }


            File.WriteAllText(@"OUTPUT/Output.csv", csv.ToString());
            return new Tuple<int, int, int, List<string>>(added.Count(), inter.Count(), rem.Count(), total);

        }

        public static Tuple<List<string>, List<string>, List<string>> CompareRemovedandAdded(List<string> oldList, List<string> newList)
        {

            var added = newList.Except(oldList).ToList();
            var inter = newList.Intersect(oldList).ToList();
            var rem = oldList.Except(newList).ToList();


            return new Tuple<List<string>, List<string>, List<string>>(added, inter, rem);
        }

        public static List<string> CompareDescriptions(string[,] oldArray, string[,] newArray){

            var names = new List<string>();

            for (int n = 0; n < newArray.GetLength(0); n++)
            {
                for (int i = 0; i < oldArray.GetLength(0); i++)
                {
                    if (oldArray[i, 1] == newArray[n, 1])
                    {
                        if (oldArray[i, 2] != newArray[n, 2] && !names.Contains(newArray[n, 1] + "," + newArray[n, 2]))
                        {
                            names.Add(newArray[n, 1] + "," + newArray[n, 2]);
                        }
                    }

                }

            }
            return names;
        }

        public static Tuple<List<string>, List<string>> GetDeletedAndAdded(string[,] oldArray, string[,] newArray, List<string> inter){
           
            var isRemoved = false;
            var isNew = false;
            string temp = "";
            List<string> newList = new List<string>();
            List<string> removedList = new List<string>();

            for (int n = 0; n < newArray.GetLength(0); n++)
            {
                temp = "";
                isNew = true;
                for (int i = 0; i < oldArray.GetLength(0); i++)
                {
                    switch (newArray[n, 5])
                    {
                        case "LN":
                        case "ZBR":
                        case "LD":
                        case "UN":
                        case "CP":

                            if(oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7])
                            {
                                isNew = false;
                            }
                            break;


                        case "CB":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 8] == newArray[n, 8])
                            {
                                isNew = false;
                            }

                            break;

                        case "XF":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && oldArray[i, 8] == newArray[n, 8])
                            { //Adjust to find required matches
                                isNew = false;
                            }

                            break;
                    }
                }
                if(isNew){
                    for (int q = 0; q < newArray.GetLength(1); q++)
                    {
                        if (q != newArray.GetLength(1) - 1)
                        {
                            temp += newArray[n, q] + ',';
                        }
                        else
                        {
                            temp += newArray[n, q] + ',';
                            switch (newArray[n, 5]) //To add key
                            {
                                case "LN":
                                case "ZBR":
                                case "LD":
                                case "UN":
                                case "CP":
                                    temp += newArray[n, 1];
                                    temp += newArray[n, 5];
                                    temp += newArray[n, 6];
                                    temp += newArray[n, 7];
                                    break;
                                case "CB":
                                    temp += newArray[n, 1];
                                    temp += newArray[n, 5];
                                    temp += newArray[n, 6];
                                    temp += newArray[n, 8];
                                    break;
                                case "XF":
                                    temp += newArray[n, 1];
                                    temp += newArray[n, 5];
                                    temp += newArray[n, 6];
                                    temp += newArray[n, 7];
                                    temp += newArray[n, 8];

                                    break;
                            }

                        }
                    }
                    newList.Add(temp);
                }
            }

            for (int i = 0; i < oldArray.GetLength(0); i++)
            {
                temp = "";
                isRemoved = true;
                for (int n = 0; n < newArray.GetLength(0); n++)
                {
                    switch (oldArray[i, 5])
                    {
                        case "LN":
                        case "ZBR":
                        case "LD":
                        case "UN":
                        case "CP":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7])
                            {
                                isRemoved = false;
                            }
                            break;


                        case "CB":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 8] == newArray[n, 8])
                            {
                                isRemoved = false;
                            }

                            break;

                        case "XF":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && oldArray[i, 8] == newArray[n, 8])
                            { //Adjust to find required matches
                                isRemoved = false;
                            }

                            break;
                    }
                }
                if (isRemoved)
                {
                    for (int q = 0; q < oldArray.GetLength(1); q++)
                    {
                        if (q != oldArray.GetLength(1) - 1)
                        {
                            temp += oldArray[i, q] + ',';
                        }
                        else
                        {

                            temp += oldArray[i, q] +',';
                            switch (oldArray[i, 5]) //To add key
                            {
                                case "LN":
                                case "ZBR":
                                case "LD":
                                case "UN":
                                case "CP":
                                    temp += oldArray[i, 1];
                                    temp += oldArray[i, 5];
                                    temp += oldArray[i, 6];
                                    temp += oldArray[i, 7];
                                    break;
                                case "CB":
                                    temp += oldArray[i, 1];
                                    temp += oldArray[i, 5];
                                    temp += oldArray[i, 6];
                                    temp += oldArray[i, 8];
                                    break;
                                case "XF":
                                    temp += oldArray[i, 1];
                                    temp += oldArray[i, 5];
                                    temp += oldArray[i, 6];
                                    temp += oldArray[i, 7];
                                    temp += oldArray[i, 8];
                                    
                                    break;
                            }
                        }
                    }
                    removedList.Add(temp);
                }
            }
            return Tuple.Create(newList, removedList);

        }



        public static Tuple<List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, Tuple<List<string>>> CheckForModification(string[,] oldArray, string[,] newArray, List<string> newContingencyNames) //Use this method to implement requirements and craft outputs
        {
            
            //New function to find all values in entirely new list. Processing 2.

            //OldComparison
            List<string> ln = new List<string>();
            List<string> zbr = new List<string>();
            List<string> ld = new List<string>();
            List<string> un = new List<string>();
            List<string> cb = new List<string>();
            List<string> cp = new List<string>();
            List<string> xf = new List<string>();

            List<string> newContingencyList = new List<string>();

            string temp = "";
            temp = oldArray[0, 0];
            for (int n = 0; n < newArray.GetLength(0); n++)
            {
                temp = "";
                if (newContingencyNames.Contains(newArray[n,1]))
                {
                    for (int q = 0; q < newArray.GetLength(1); q++)
                    {
                        if (q != newArray.GetLength(1) - 1)
                        {
                            temp += newArray[n, q] + ',';
                        }
                        else
                        {
                            temp += newArray[n, q];
                        }
                    }
                    newContingencyList.Add(temp);
                }

            for (int i = 0; i < oldArray.GetLength(0); i++)

            {



                    switch (oldArray[i, 5])
                    {
                        case "LN":


                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 8] != newArray[n, 8] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                ln.Add(temp);
                            }
                            break;


                        case "ZBR":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 8] != newArray[n, 8] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                zbr.Add(temp);
                            }


                            break;

                        case "LD":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 8] != newArray[n, 8] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                ld.Add(temp);
                            }


                            break;

                        case "UN":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 8] != newArray[n, 8] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                un.Add(temp);
                            }

                            break;

                        case "CB":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 8] == newArray[n, 8] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 7] != newArray[n, 7] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                cb.Add(temp);
                            }

                            break;

                        case "CP":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 8] != newArray[n, 8] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                cp.Add(temp);
                            }

                            break;

                        case "XF":

                            if (oldArray[i, 1] == newArray[n, 1] && oldArray[i, 5] == newArray[n, 5] && oldArray[i, 6] == newArray[n, 6] && oldArray[i, 7] == newArray[n, 7] && oldArray[i, 8] == newArray[n, 8] && (oldArray[i, 2] != newArray[n, 2] || oldArray[i, 3] != newArray[n, 3] || oldArray[i, 4] != newArray[n, 4] || oldArray[i, 9] != newArray[n, 9] || oldArray[i, 10] != newArray[n, 10]))
                            { //Adjust to find required matches
                                for (int q = 0; q < oldArray.GetLength(1); q++)
                                {
                                    if (q != oldArray.GetLength(1) - 1)
                                    {
                                        temp += oldArray[i, q] + ',';
                                    }
                                    else
                                    {
                                        temp += oldArray[i, q];
                                    }
                                }
                                un.Add(temp);
                            }

                            break;
                    }


                }

            }

            return new Tuple<List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, List<string>, Tuple<List<string>>>(ln, zbr, ld, un, cb, cp, xf, Tuple.Create(newContingencyList));
        }
        //public static List<string> PostProcessModifications(List<string> ln, List<string> zbr, List<string> ld, List<string> un, List<string>cb, List<string>cp, List<string> xf){
        //    return new 
        //}
    }
}
