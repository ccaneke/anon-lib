using System;
namespace AnonymizationOFPatientData
{
    public class DateOfBirth
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }

        public int age { get; set; }

        private string _monthname;

        public string monthname
        {
            get
            {
                return this._monthname;
            }

            set
            {
                _monthname = value;   
            }
        }
        

        public DateOfBirth(int day, int month, int year)
        {
            this.day = day;
            this.month = month;
            this.year = year;
            this._monthname = monthNumberToMonthName(month);
        }

        // This function takes in two dates and returns the number of years
        // 12/04/2016
        public int differenceInDates()
        {
            DateTime now = DateTime.Now; // Note: that a property acts like a static method since users of the class can use a (public)
                                        // property

            int currentYear = now.Year; // endYear
            int currentMonth = now.Month; // endMonth
            int currentDay = now.Day; // endDay

            int years = currentYear - this.year; // year is brith year

            if (currentMonth < this.month) // no need to check day when start month is bigger than end montth
            {
                years--;
            } else if (currentMonth == this.month && this.day > currentDay) // when startMonth is less than or equal to endMonth
                                                                            // we only subtract a year if endMonth is equal to startMonth
                                                                            // and startDay is greater than endDay
            {
                years--;
            }

            //this.age = years; prefer to return age
            return years;

        }

        public/*private*/ static string monthNumberToMonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1:
                    {
                        return "Jan";
                    }

                case 2:
                    {
                        return "Feb";
                    }

                case 3:
                    {
                        return "Mar";
                    }

                case 4:
                    {
                        return "Apr";
                    }

                case 5:
                    {
                        return "May";
                    }

                case 6:
                    {
                        return "Jun";
                    }

                case 7:
                    {
                        return "Jul";
                    }

                case 8:
                    {
                        return "Aug";
                    }

                case 9:
                    {
                        return "Sep";
                    }
                case 10:
                    {
                        return "Oct";
                    }
                case 11:
                    {
                        return "Nov";
                    }

                case 12:
                    {
                        return "Dec";
                    }

                default:
                    {
                        return "Not a Month";
                    }
            }
        }

        override
        public string ToString()
        {
            return this.day + " " + this._monthname + " " + this.year; 
        }

        public bool compareAge(DateOfBirth otherBirthDate)
        {
            return this.differenceInDates() == otherBirthDate.differenceInDates();
        }

        public DateOfBirth randomizeDateOfBirth(DateOfBirth otherBirthDate, Random random)
        {

            if (this.compareAge(otherBirthDate)) // if only ages (i.e. differenceInDates) are the same
            {
                // update the monthname after randomly changing the monthnumber because monthname is only set at the beginning when
                // the object's state is formatted
                otherBirthDate.monthname = monthNumberToMonthName(otherBirthDate.month);
                
                return otherBirthDate;
            }
            otherBirthDate.day = random.Next(1, 31);
            otherBirthDate.month = random.Next(1, 13);

            return randomizeDateOfBirth(otherBirthDate, random); // returns only the value in the bottom function stack back up through
        }

        public DateOfBirth clone()
        {
            DateOfBirth birthDate = new DateOfBirth(this.day, this.month, this.year);

            return birthDate;
        }
    }
}
