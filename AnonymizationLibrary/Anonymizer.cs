using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using AddressClasses;

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


        readonly static string[] familyNames = CreateFamilyNames();


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

            static DateOfBirth globalDateOfBirth = null;

        public static List<dynamic> AnonymizePatients(string jsons)
        {
            // Handles deserialization for caller
            List<PersonModel> listOfDeserializedJSON = JsonConvert.DeserializeObject<List<PersonModel>>(jsons);

            List<dynamic> newPersons = new List<dynamic>();

            foreach (PersonModel person in listOfDeserializedJSON)
            {
                // dynamic type for ExpandoObject in order to enable late binding of variables
                dynamic dynObject = new ExpandoObject();

                copyProperties(person, dynObject);

                newPersons.Add(dynObject);
            }

            foreach (dynamic newPerson in newPersons)
            {
                if (streets == null || streetSuffixes == null || familyNames == null)
                {
                    return null;
                }

                Line1 line1 = new Line1(houseNumbers, streets, streetSuffixes);

                Address address = new Address(line1, new Line2(streets), new Line3(streets),
                    new City(cities), new Postcode(), new County(counties), new Country("United Kingdom"));

                IDictionary<String, Object> intermediateDictionary = (IDictionary<String, Object>)newPerson;

                KeyValuePair<String, Object>[] DictionaryCopy = new KeyValuePair<String, Object>[intermediateDictionary.Count];

                intermediateDictionary.CopyTo(DictionaryCopy, 0);

                // enumerate copy to prevent Exception
                foreach (KeyValuePair<String, Object> property in DictionaryCopy)
                {
                    switch (property.Key)
                    {
                        case "person_id":
                            {
                                if (newPerson.person_id == null)
                                {
                                    break;
                                }

                                Guid guid = Guid.NewGuid();
                                newPerson.person_id = guid.ToString();
                                break;
                            }
                        case "nhsnumber":
                            {
                                if (newPerson.nhsnumber == null)
                                {
                                    break;
                                }

                                Randomizer rand = new Randomizer(newPerson.nhsnumber);
                                rand.shuffleCharacters();
                                string randomString = rand.randomizedNumberString;

                                newPerson.nhsnumber = randomString;
                                break;
                            }

                        case "mrn":
                            {
                                if (property.Value == null)
                                {
                                    break;
                                }
                                Randomizer randomizer = new Randomizer(newPerson.mrn);
                                randomizer.shuffleCharacters();
                                newPerson.mrn = randomizer.randomizedMrn;
                                break;
                            }

                        case "firstname":
                            {
                                if (newPerson.firstname == null)
                                {
                                    break;
                                }

                                newPerson.firstname = "Patient";
                                break;

                            }

                        case "middlename":
                            {
                                if (newPerson.middlename == null)
                                {
                                    break;
                                }
                                Randomizer rand = new Randomizer(newPerson.middlename);
                                rand.shuffleCharacters();

                                string initial = "" + rand.randomizedInitial;

                                newPerson.middlename = initial.ToUpper();
                                break;
                            }

                        case "familyname":
                            {
                                if (newPerson.familyname == null)
                                {
                                    break;
                                }

                                newPerson.familyname = familyNames[randomNumGenerator.Next(familyNames.Length)];
                                break;
                            }

                        case "fullname":
                            {
                                if (newPerson.fullname == null)
                                {
                                    break;
                                }

                                // Todo: include initial
                                newPerson.fullname = newPerson.firstname + " " + newPerson.familyname;
                                break;
                            }

                        case "preferredname":
                            {
                                if (newPerson.preferredname == null)
                                {
                                    break;
                                }

                                newPerson.preferredname = null;
                                break;
                            }

                        case "dateofbirth":
                            {
                                if (newPerson.dateofbirth == null)
                                {
                                    break;
                                }

                                // Transform the dateOfBirth from 2016-04-12T00:00:00 to 12 Apr 2016 for decomposeDateIntoInts
                                int[] dd_mm_yy = decomposeDateIntoInts(transformDateFormat(newPerson.dateofbirth));

                                DateOfBirth myBirthDate = new DateOfBirth(dd_mm_yy[0], dd_mm_yy[1], dd_mm_yy[2]);
                                DateOfBirth dummyBirthDate = myBirthDate.clone();

                                Random random = new Random();

                                dummyBirthDate.day = random.Next(1, 31);
                                dummyBirthDate.month = random.Next(1, 13);

                                DateOfBirth randomlyGeneratedBirthDate = myBirthDate.randomizeDateOfBirth(dummyBirthDate, random);

                                newPerson.dateofbirth = randomlyGeneratedBirthDate.ToString();

                                globalDateOfBirth = randomlyGeneratedBirthDate;

                                break;
                            }

                        case "dateofbirthformatted":
                            {
                                if (newPerson.dateofbirthformatted == null)
                                {
                                    break;
                                }

                                if (globalDateOfBirth.day < 10 && globalDateOfBirth.month < 10)
                                {

                                    newPerson.dateofbirthformatted = "0" + globalDateOfBirth.day + "/" + "0" + globalDateOfBirth.month + "/" +
                                                                                                        +globalDateOfBirth.year;
                                }
                                else if (globalDateOfBirth.day < 10)
                                {

                                    newPerson.dateofbirthformatted = "0" + globalDateOfBirth.day + "/" + globalDateOfBirth.month + "/" +
                                                                                                        globalDateOfBirth.year;
                                }
                                else if (globalDateOfBirth.month < 10)
                                {

                                    newPerson.dateofbirthformatted = globalDateOfBirth.day + "/" + "0" + globalDateOfBirth.month + '/' +
                                                                                                        globalDateOfBirth.year;
                                }
                                else // when both day and month are greater than 9 do not prepend with a 0
                                {

                                    newPerson.dateofbirthformatted = "" + globalDateOfBirth.day + "/" + globalDateOfBirth.month + "/" +
                                                                                                    globalDateOfBirth.year;
                                }
                                break;
                            }

                        case "agetext":
                            {
                                if (newPerson.agetext == null)
                                {
                                    break;
                                }

                                newPerson.agetext = "";
                                break;
                            }

                        case "dateofdeath":
                            {
                                if (newPerson.dateofdeath == null)
                                {
                                    break;
                                }
                                
                                int[] ddmmyy = decomposeDateIntoInts(transformDateFormat(newPerson.dateofdeath));
                                DateOfBirth dateOfDeath = new DateOfBirth(ddmmyy[0], ddmmyy[1], ddmmyy[2]);
                                DateOfBirth otherDateOfDeath = dateOfDeath.clone();
                                otherDateOfDeath.day = randomNumGenerator.Next(1, 31);
                                otherDateOfDeath.month = randomNumGenerator.Next(1, 13);

                                DateOfBirth anonymizedDateOFDeath = dateOfDeath.randomizeDateOfBirth(otherDateOfDeath, randomNumGenerator);

                                newPerson.dateofdeath = anonymizedDateOFDeath.ToString();
                                break;
                            }

                        case "line1":
                            {
                                if (newPerson.line1 == null)
                                {
                                    break;
                                }

                                Line1 firstLineOfAddress = (Line1)address.address["line1"];

                                newPerson.line1 = firstLineOfAddress.toString();
                                break;
                            }

                        case "line2":
                            {
                                if (newPerson.line2 == null)
                                {
                                    break;
                                }

                                Line2 secondLineOfAddress = (Line2)address.address[property.Key];

                                newPerson.line2 = secondLineOfAddress.toString();
                                break;
                            }

                        case "line3":
                            {
                                if (newPerson.line3 == null)
                                {
                                    break;
                                }

                                Line3 thirdLineOfAddress = (Line3)address.address[property.Key];

                                newPerson.line3 = thirdLineOfAddress.toString();
                                break;
                            }

                        case "city":
                            {
                                if (newPerson.city == null)
                                {
                                    break;
                                }

                                City cityOFAddress = (City)address.address[property.Key];

                                newPerson.city = cityOFAddress.toString();
                                break;
                            }

                        case "postcodezip":
                            {
                                if (newPerson.postcodezip == null)
                                {
                                    break;
                                }
                                Postcode postCodeOfAddress = (Postcode)address.address["postcode"];

                                newPerson.postcodezip = postCodeOfAddress.toString();
                                break;
                            }

                        case "countystateprovince":
                            {
                                if (newPerson.countystateprovince == null)
                                {
                                    break;
                                }
                                County countyOfAddress = (County)address.address["county"];

                                newPerson.countystateprovince = countyOfAddress.toString();
                                break;
                            }

                        case "country":
                            {
                                if (newPerson.country == null)
                                {
                                    break;
                                }
                                Country countryOfAddress = (Country)address.address[property.Key];

                                newPerson.country = countryOfAddress.toString();
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }

            }

            foreach (dynamic dyn in newPersons)
            {
                IDictionary<String, Object> afterRenamingProperty = (IDictionary<String, Object>)dyn;
                RenameKeys(afterRenamingProperty, "person_id", "anonymizedperson_id");
            }

            return newPersons;
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

        private static void copyProperties(PersonModel from, dynamic to)
        {
            to.person_id = from.person_id;
            to.nhsnumber = from.nhsnumber;
            to.mrn = from.mrn;
            to.titlecode = from.titlecode;
            to.titletext = from.titletext;
            to.firstname = from.firstname;
            to.middlename = from.middlename;
            to.familyname = from.familyname;
            to.fullname = from.fullname;
            to.preferredname = from.preferredname;
            to.dateofbirth = from.dateofbirth;
            to.dateofbirthformatted = from.dateofbirthformatted;
            to.ageinyears = from.ageinyears;
            to.agetext = from.agetext;
            to.dateofdeath = from.dateofdeath;
            to.gender = from.gender;
            to.ethnicity = from.ethnicity;
            to.maritalstatus = from.maritalstatus;
            to.religion = from.religion;
            to.deathindicator = from.deathindicator;
            to.line1 = from.line1;
            to.line2 = from.line2;
            to.line3 = from.line3;
            to.city = from.city;
            to.postcodezip = from.postcodezip;
            to.countystateprovince = from.countystateprovince;
            to.country = from.country;
        }

        private static int[] decomposeDateIntoInts(string dateOfBirth)
        {
            string[] dd_monthName_yy = dateOfBirth.Split(' ');

            int[] dd_mm_yy = null;
            try
            {
                dd_mm_yy = new int[] {int.Parse(dd_monthName_yy[0]), monthNameToMonthNumber(dd_monthName_yy[1]),
                                                                                                            int.Parse(dd_monthName_yy[2])};
            }
            catch (FormatException exception)
            {
                Console.WriteLine($"The date of birth: {dateOfBirth} throws the exception: {exception}");
            }
            return dd_mm_yy; 
        }

        private static int monthNameToMonthNumber(string monthString)
        {
            switch (monthString)
            {
                case "Jan":
                    {
                        return 1;
                    }

                case "Feb":
                    {
                        return 2;
                    }

                case "Mar":
                    {
                        return 3;
                    }

                case "Apr":
                    {
                        return 4;
                    }

                case "May":
                    {
                        return 5;
                    }
                case "June":
                    {
                        return 6;
                    }

                case "Jul":
                    {
                        return 7;
                    }

                case "Aug":
                    {
                        return 8;
                    }

                case "Sep":
                    {
                        return 9;
                    }

                case "Oct":
                    {
                        return 10;
                    }

                case "Nov":
                    {
                        return 11;
                    }

                case "Dec":
                    {
                        return 12;
                    }

                default:
                    {
                        return -1;
                    }

            }
        }

        // transforms a date format from 2018-04-16T00:00:00 to 16 Apr 2018
        private static string transformDateFormat(string dateOfDeath)
        {
            string[] Tsplit = dateOfDeath.Split('T');

            string[] withoutTimeComponent = new string[] { Tsplit[0] };

            string[] hyphenDelimited = withoutTimeComponent[0].Split('-');

            string year = hyphenDelimited[0];
            string month = monthNumberStringtoMonthName(hyphenDelimited[1]);
            string day = hyphenDelimited[2];

            return day + " " + month + " " + year;

        }

        private static string monthNumberStringtoMonthName(string monthNumberString)
        {
            switch (monthNumberString)
            {
                case "01":
                    {
                        return "Jan";
                    }

                case "02":
                    {
                        return "Feb";
                    }

                case "03":
                    {
                        return "Mar";
                    }

                case "04":
                    {
                        return "Apr";
                    }

                case "05":
                    {
                        return "May";
                    }

                case "06":
                    {
                        return "Jun";
                    }

                case "07":
                    {
                        return "Jul";
                    }

                case "08":
                    {
                        return "Aug";
                    }

                case "09":
                    {
                        return "Sep";
                    }

                case "10":
                    {
                        return "Oct";
                    }

                case "11":
                    {
                        return "Nov";
                    }

                case "12":
                    {
                        return "Dec";
                    }

                default:
                    {
                        break;
                    }

            }
            return "Not a valid month number string";
        }

        private static void RenameKeys(IDictionary<String, Object> dict, string from, string to)
        {
            object tempValue = dict[from];
            dict.Remove(from);
            dict.Add(to, tempValue);
        }
    }
}
