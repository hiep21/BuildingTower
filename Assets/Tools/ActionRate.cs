using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tool
{
    public class ActionRate
    {

        public static int GetRandomIndexByRate(List<int> rates, int totalRate)
        {
            List<List<int>> listRates = new List<List<int>>();
            List<int> listRated = new List<int>();
            foreach (var item in rates)
            {
                List<int> listRate = new List<int>();
                listRate = ConvertNumberToRate(item, totalRate, listRated);

                listRated.AddRange(listRate);
                listRates.Add(listRate);
            }

            int randomIndex = RandomNumber(0, totalRate);
            foreach (var item in listRates)
            {
                if (item.Any(f => f == randomIndex))
                {
                    return listRates.FindIndex(f => f == item);
                }
            }
            return -1;

        }


        public static List<int> ConvertNumberToRate(int numberRate, int totalRate, List<int> listRated)
        {
            List<int> resultConvert = new List<int>();
            for (int i = 0; i < numberRate; i++)
            {
                int randomNumber = RandomNumber(0, totalRate);
                if (!resultConvert.Any(x => x == randomNumber) && !listRated.Any(x => x == randomNumber))
                {
                    resultConvert.Add(randomNumber);
                }
                else
                {
                    i--;
                }
            }
            return resultConvert;

        }
        static int RandomNumber(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
}
