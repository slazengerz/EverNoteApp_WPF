using System;
using System.Collections.Generic;
using System.Text;

namespace GetCitiesFromJson
{
    class CityDetailsNonArray
    {
        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class DetailsObject
        {
            public int id { get; set; }
            public string name { get; set; }
            public string country { get; set; }
            public Coord coord { get; set; }
        }

        public class GetListOfCities
        {
            public List<DetailsObject> detailsObjects { get; set; }
        }

    }
}
