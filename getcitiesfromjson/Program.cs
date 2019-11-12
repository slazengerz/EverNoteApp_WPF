using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;

namespace GetCitiesFromJson
{
    class Program
    {
        private const string API_KEY = "WGysXdajiJBk2n77W7rN0tg4X5edzPRn";
        private const string URL = "http://dataservice.accuweather.com/locations/v1/cities/autocomplete?apikey={0}&q={1}";
        static void Main(string[] args)
        {
            //string filepath = @"C:\Study\WPF\citylist\city.list.json";

            //getCitiesUsingStreams(filepath);
            //getCitiesUsingMemMappedFile(filepath);
            getLocationUsingAPI();
            Console.ReadLine();
        }

        private static async void getLocationUsingAPI()
        {
            
            using(HttpClient client=new HttpClient())
            {
                var url = string.Format(URL, API_KEY, "sydney");
                var response =await client.GetAsync(url);
                var stringfromresponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"stringfromresponse is {stringfromresponse}");
            }
        }

        private static void getCitiesUsingStreams()
        {
            string filepath = @"C:\Study\WPF\citylist\city.list.json";
            DateTime startedtime = DateTime.Now;
            List<string> nameOfCities = new List<string>();
            List<CityDetailsMain.Detail> listOfCities = new List<CityDetailsMain.Detail>();
            List<CityDetailsNonArray.DetailsObject> citiesFromNonArray = new List<CityDetailsNonArray.DetailsObject>();
            StringBuilder sbr = new StringBuilder();
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sbr.Append(line);
                        }
                    }
                }
            }
            Console.WriteLine($"Started at {startedtime}");
            Console.WriteLine($"Ended at {DateTime.Now}");
            Console.WriteLine($"using getCitiesUsingStreams took {DateTime.Now - startedtime}");
            Console.WriteLine($"no of cities are {nameOfCities.Count}");
            if (sbr.Length > 0)
            {
                //deserailze the json whcih is retrevied from file  as stringbuilder

                //listOfCities =(JsonConvert.DeserializeObject<CityDetailsMain.CityDetails>(sbr.ToString())).details;
                
                citiesFromNonArray = (JsonConvert.DeserializeObject<CityDetailsNonArray.GetListOfCities>(sbr.ToString())).detailsObjects;
                Console.WriteLine($"No of records in list are {citiesFromNonArray.Count}");
                var particularcities = from cities in citiesFromNonArray
                                       where cities.name.ToLower().Contains("delhi")
                                       select cities;
                foreach(var city in particularcities)
                {
                    Console.WriteLine($"Id is {city.id} ,Name is {city.name}, country is {city.country}");
                }
            }   
        }

        private static void getCitiesUsingMemMappedFile(string filepath)
        {
            DateTime startedtime = DateTime.Now;
            StringBuilder sbr = new StringBuilder();
            int result;
            using (MemoryMappedFile mmpfile = MemoryMappedFile.CreateFromFile(filepath))
            {
                using (MemoryMappedViewStream mmpstream = mmpfile.CreateViewStream())
                {
                    while ((result = mmpstream.ReadByte()) != -1)
                    {
                        char letter = (char)result;
                        sbr.Append(letter);
                    }
                }
            }
            DateTime finishtime = DateTime.Now;
            Console.WriteLine($"Started at {startedtime}");
            Console.WriteLine($"Ended at {finishtime}");
            Console.WriteLine($"using getCitiesUsingStreams took {finishtime - startedtime}");
            Console.WriteLine($"no of cities are {sbr}");
        }
    }
}
