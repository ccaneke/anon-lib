using System;
using System.Collections; // collections created from classes in this namespace are untyped which is what I need for an Address object
using System.Collections.Generic;

namespace AddressClasses

// I need to use a hashtable instead of a collection because a hashtable is a collection created from the System.Collections
// class which do not store objects as objects of a specific type but as objects of the type Object

{
    public class Address
    {
        public Hashtable address { get; set; }

        public Address(Line1 line1, Line2 line2, Line3 line3, City city, Postcode postcode, County county, Country country)
        {
            this.address = new Hashtable() { {"line1", line1 }, {"line2", line2}, {"line3", line3}, {"city", city}, {"postcode", postcode},
                                                {"county", county}, {"country", country} };
        }
    }

    public class Line1
    {
        public int houseNumber { get; set; }
        public string road { get; set; }
        public string suffix { get; set; }
        readonly private Random random;

        public Line1(List<int> houseNumbers, List<string> roads, List<string> suffixes)
        {
            this.random = new Random();
            randomHouseNumber(houseNumbers);
            randomRoad(roads);
            randomSuffix(suffixes);
        }

        public void randomHouseNumber(List<int> houseNumbers)
        {
            this.houseNumber = houseNumbers[this.random.Next(houseNumbers.Count)];
        }

        public void randomRoad(List<string> roads)
        {
            this.road = roads[this.random.Next(roads.Count)];
        }

        public void randomSuffix(List<string> suffixes) // parameter is a wire to a static List object in Program.cs
        {
            this.suffix = suffixes[this.random.Next(suffixes.Count)];
        }

        public string toString()
        {
            return "" + this.houseNumber + " " + this.road + " " + this.suffix;
        }
    }

    public class Line2
    {
        public string road { get; set; }
        private readonly Random random;

        public Line2(List<string> roads)
        {
            random = new Random();
            road = roads[random.Next(roads.Count)];
        }

        public string toString()
        {
            return road;
        }

    }

    public class Line3
    {
        public string road { get; set; }
        private readonly Random random;

        public Line3(List<string> roads)
        {
            this.random = new Random();
            this.road = roads[this.random.Next(roads.Count)];
        }

        public string toString()
        {
            return this.road;
        }
    }

    public class City
    {
        public string name { get; set; }
        private readonly Random random;

        public City(List<string> cities)
        {
            this.random = new Random();
            randomizeCity(cities);
        }

        public void randomizeCity(List<string> cities)
        {
            this.name = cities[this.random.Next(cities.Count)];
        }

        public string toString()
        {
            return this.name;
        }
    }

    public class Postcode // represents a random postcode
    {
        public char letter1 { get; set; }
        public char letter2 { get; set; }
        public int number1 { get; set; }
        public int number2 { get; set; }
        public char letter3 { get; set; }
        public char letter4 { get; set; }
        private readonly Random random;
        private readonly string alphabets;

        public Postcode()
        {
            this.random = new Random();
            this.alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.letter1 = randomizeLetter(alphabets, random);
            this.letter2 = randomizeLetter(alphabets, random);
            this.number1 = this.random.Next(10);
            this.number2 = this.random.Next(10);
            this.letter3 = Postcode.randomizeLetter(this.alphabets, this.random);
            this.letter4 = Postcode.randomizeLetter(this.alphabets, this.random);
        }

        public string toString()
        {
            // Update: Note that you cannot concatenate characters, instead of concatenating characters, using the + binary operator for
            // characters adds their unsigned intergers so 'Y' + 'V' + 1 gives (i.e. results in) 176 because the unsigned integer (i.e.
            // positive integer) of the character 'Y' is 89 and the unsigned integer of the character 'V' is 86. Thanks to the debugger!
            // So I prepend an empty string to the first character and then all values that follow are converted to string
            // can concatenate characters, not just strings
            return "" + this.letter1 + this.letter2 + this.number1 + " " + this.number2 + this.letter3 + this.letter4;
        }

        private static char randomizeLetter(string alphabets, Random rnd)
        {
            return alphabets[rnd.Next(alphabets.Length)];
        }

    }

    public class County
    {
        public string name { get; set; } // gets and sets a string to a hidden field that is created by this accessor
        private readonly Random random;

        public County(List<string> counties)
        {
            this.random = new Random();
            name = counties[this.random.Next(counties.Count)];
        }

        public string toString()
        {
            return this.name;
        }

    }

    public class Country
    {
        public string name { get; set; }
        private readonly Random random;

        public Country(List<string> countriesList)
        {
            this.random = new Random();
            name = countriesList[this.random.Next(countriesList.Count)];
        }

        public Country(string country)
        {
            name = country;
        }

        public string toString()
        {
            return name;
        }

    }
}
