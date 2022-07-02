/* Copyright (C) Interneuron, Inc - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Chukwuemezie Aneke <ccanekedev@gmail.com>, May 2020
 */

using System;
using Newtonsoft.Json;

namespace AnonymizationLibrary
{
    // model of JSON object
    public class PersonModel
    {
        //[JsonProperty(PropertyName = "anonymizedperson_id")] // this is very dangerous as it changed the name even in deserialization
        public string person_id { get; set; }
        public string nhsnumber { get; set; }
        public string mrn { get; set; }
        public string titlecode { get; set; }
        public string titletext { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        /* does not work probably because the backing field is hidden and can only be accessed by the auto-implemented property middlename
        public string middlename { get;
            set {
                string upper = "" + value
                    middlename = upper; }*/
        public string familyname { get; set; }
        public string fullname { get; set; }
        public string preferredname { get; set; }
        public string dateofbirth { get; set; }
        public string dateofbirthformatted { get; set; }
        public string ageinyears { get; set; }
        public string agetext { get; set; }
        public string dateofdeath { get; set; }
        public string gender { get; set; }
        public string ethnicity { get; set; }
        public string maritalstatus { get; set; }
        public string religion { get; set; }
        public string deathindicator { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string city { get; set; }
        public string postcodezip { get; set; }
        public string countystateprovince { get; set; }
        public string country { get; set; }
    }
}
