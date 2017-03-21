using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PriceCheckerVGH
{
    public class Core
    {

        public Game gameData;
        public string result;

        //Variables to construct the filename for excel spread
        static DateTime now = DateTime.Now;
        static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        static string fileName1 = now.ToShortDateString();
        static string fileName2 = fileName1.Replace("/", "-");
        static string finalFileName= Path.Combine(filePath, fileName2);
        //TODO: Clean this up...


        public async Task<Game> getGame(string upc)
        {
            HttpClient gamePricer = new HttpClient();
            Dictionary<string, string> jsonKeyValues = new Dictionary<string, string>();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["upc"] = upc;
            var uri = "https://ae.pricecharting.com/api/product?t=7b3a8558f2514753ddabd35a40f711c857c42910&" + queryString;
            using (var content = new StringContent(""))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await gamePricer.GetAsync(uri);

                var jsonString = response.Content.ReadAsStringAsync();
                await jsonString;
                result = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {

                    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString.Result));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Game));
                    gameData = (Game)serializer.ReadObject(ms);
                    ms.Flush();

                    if (gameData.price == null)
                    {
                        Console.WriteLine(gameData.title + " for console " + gameData.console + " found, but no price.");
                        return gameData;
                    }

                    if (gameData.price.Length == 5)
                    {
                        var finalCostFormat = gameData.price.Insert(3, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[4] = '9';
                        gameData.price = "$" + new string(array);
                    }

                    else if (gameData.price.Length == 4)
                    {
                        var finalCostFormat = gameData.price.Insert(2, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[3] = '9';
                        gameData.price = "$" + new string(array);
                    }

                    else if (gameData.price.Length == 3)
                    {
                        var finalCostFormat = gameData.price.Insert(1, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[2] = '9';
                        gameData.price = "$" + new string(array);
                    }

                    

                }
                
                
            }
            return gameData;
        }

        public int writeGame()
        {
            if (gameData.price == null)
            {
                Console.WriteLine("No game found with this upc to be added to database.");
                return -1;
            }
            var csv = new StringBuilder();
            var newLine = string.Format("{0},{1},{2},{3}", gameData.console, gameData.title,gameData.price,gameData.upc);
            csv.AppendLine(newLine);
            using (var streamWriter = new StreamWriter(finalFileName, true))
            {
                streamWriter.WriteLine(csv);
            }

            return 1;
        }
    }
}
