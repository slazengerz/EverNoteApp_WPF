using System;
using System.Collections.Generic;
using System.Text;

namespace GetCitiesFromJson
{
    class CityDetailsMain
    {
        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Detail
        {
            public int id { get; set; }
            public string name { get; set; }
            public string country { get; set; }
            public Coord coord { get; set; }
        }

        public class CityDetails
        {
            public List<Detail> details { get; set; }
        }

    }
}
