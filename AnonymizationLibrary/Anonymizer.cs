using System;
using System.Collections.Generic;
using System.IO;

namespace AnonymizationLibrary
{
    public class Anonymizer
    {
            static List<int> houseNumbers = houseNumbersList();

        static List<string> streets = CreateStreets();

            private static List<string> CreateStreets()
        {
            List<string> streets = null;
            try
            {
            streets = ArrayToList(File.ReadAllLines(@"/Users/christopheraneke/Projects/street.txt"));

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return streets;
        }

        static List<string> streetSuffixes = CreateStreetSuffixes();

        private static List<string> CreateStreetSuffixes()
        {
            List<string> streetSuffixes = null;
            try
            {
                streetSuffixes = ArrayToList(File.ReadAllLines(@"/Users/christopheraneke/Projects/street-suffix.txt"));

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return streetSuffixes;
        }

            static List<string> cities = new List<string>() { "Bath", "Birmingham", "Bradford", "Brighton and Hove", "Bristol", "Cambridge", "Canterbury", "Carlisle", "Chester", "Chichester", "Coventry", "Derby", "Durham", "Ely", "Exeter", "Gloucester", "Hereford",
            "Kingston upon Hull", "Lancaster", "Leeds", "Leicester", "Lichfield", "Lincoln", "Liverpool", "City of London", "Manchester",
            "Newcastle upon Tyne", "Norwich", "Nottingham", "Oxford", "Peterborough", "Plymouth", "Portsmouth", "Preston", "Ripon",
            "Salford", "Salisbury", "Sheffield", "Southampton", "St Albans", "Stoke - on - Trent", "Sunderland", "Truro", "Wakefield",
            "Wells", "Westminster", "Winchester", "Wolverhampton", "Worcester", "York"};
            static List<string> counties = ArrayToList(System.IO.File.ReadAllLines(@"/Users/christopheraneke/Projects/counties.txt"));


        readonly static string[] familyNames;

        private static string[] CreateFamilyNames()
        {
            string[] familyNames = null;
            try
            {
                familyNames = File.ReadAllLines(@"/Users/christopheraneke/Projects/family_names.txt");
                
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return familyNames;
        }

            readonly static Random randomNumGenerator = new Random();

        public static List<dynamic> AnonymizePatients(string[] jsonArray)
        {
        }

        private static List<string> ArrayToList(string[] array)
        {
            List<string> list = new List<string>();
            foreach (string element in array)
            {
                list.Add(element);
            }

            return list;
        }

        private static List<int> houseNumbersList()
        {
            List<int> numbers = new List<int>();

            for (int number = 1; number < 1000; number++)
            {
                numbers.Add(number);
            }

            return numbers;
        }
    }
}
