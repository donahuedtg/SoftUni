using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
		
namespace SomeWorkDictionary
{
    class Program
    {		
		static void Main(string[] args)
        {
            Dictionary<int, int>  resultList = GetStatusOfAccrProfGroup();
            int[] arr = { 100, 500, 1000, 1200, 300, 200 };

            foreach (var arrItem in arr)
            {
                int profGroupId = arrItem;
                int result = StatusAccr(resultList, profGroupId);

                string message = MessageAccr(result);


                Console.WriteLine("{0} - |{1}|", profGroupId, message);
            }

        }


        public static Dictionary<int, int> GetStatusOfAccrProfGroup()
        {
            int[] arr = { 100, 200, 100, 300, 400, 500 };
            int[] arrStatus = { 0, 1, 2, 3 };
            Dictionary<int, int> profGroupStatusAccr = new Dictionary<int, int>();

            foreach (var item in arr)
            {
                if (!profGroupStatusAccr.ContainsKey(item))
                {
                    if (item == 100 || item == 300)
                    {
                        profGroupStatusAccr.Add(item, arrStatus[1]);
                    }
                    else
                    {
                        profGroupStatusAccr.Add(item, arrStatus[0]);
                    }

                }
            }

            return profGroupStatusAccr;
        }


        public static int StatusAccr(Dictionary<int, int> profGroupStatusAccr, int profGroupId)
        {
            int result = -1;
            if (profGroupStatusAccr.ContainsKey(profGroupId))
            {
                try
                {
                    profGroupStatusAccr.TryGetValue(profGroupId, out result);
                    return result;
                    
                }
                catch (Exception)
                {

                    return result = -1;
                }

            }

            return - 1;
        }

        public static string MessageAccr(int statusAccr)
        {
            if (statusAccr == -1)
            {
                return "Няма информация";
            }
            else if (statusAccr == 0)
            {
                return "Изтекла информация";
            }
            else if(statusAccr == 1)
            {
                return "";
            }
            else
            {
                return "Няма акредитация";
            }

        }
	}
}