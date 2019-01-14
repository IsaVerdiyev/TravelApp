using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAppCore.Entities
{
    public class Weather
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private double temperature;

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        private double pressure;

        public double Pressure
        {
            get { return pressure; }
            set { pressure = value; }
        }

        private double humidity;

        public double Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }

        private double windSpeed;

        public double WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; }
        }

        private string iconPath;

        public string IconPath
        {
            get { return iconPath; }
            set { iconPath = value; }
        }

        private double minTemperature;

        public double MinTemperature
        {
            get { return minTemperature; }
            set { minTemperature = value; }
        }

        private double maxTemperature;

        public double MaxTemperature
        {
            get { return maxTemperature; }
            set { maxTemperature = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public override string ToString()
        {
            return Description;
        }

    }
}
