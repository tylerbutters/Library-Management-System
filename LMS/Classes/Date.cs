using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Globalization;

namespace LMS
{
    public class Date
    {
        private static Date reservationDate;

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public Date(int day, int month, int year)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static Date GetReservationDate()
        {
            if (reservationDate == null)
            {
                // Set the default reservation date
                reservationDate = new Date(22, 6, 2023); // Modify the values as desired
            }
            return reservationDate;
        }

        public static void SetReservationDate(Date date)
        {
            reservationDate = date;
        }

        public override string ToString()
        {
            return $"{Day}/{Month}/{Year}";
        }
    }
}
