using System;
using System.Collections.Generic;
using MiniJSON;
using System.IO;
using System.Xml;

namespace userInfo
{
    class PixelFit
    {
        public static List<double> parseSleepTime(Dictionary<string, object> dict)
        {
            List<double> result = new List<double>();
            Object value1;
            if (!dict.TryGetValue("sleepActivities", out value1))
            {
                result.Add(0);
                return result;
            }
            var list = (List<Object>)value1;
            foreach (var item in list)
            {
                var activity = (Dictionary<string, object>)item;
                Object value2;

                double sleepDuration;
                if (!activity.TryGetValue("sleepDuration", out value2))
                {
                    sleepDuration = 0;
                }
                else
                {
                    TimeSpan ts = XmlConvert.ToTimeSpan((string)value2);
                    sleepDuration = ts.TotalHours;
                }
                result.Add(sleepDuration);
            }
            return result;
        }
        public static List<int> parseTotalCalories(Dictionary<string, object> dict, string key)
        {
            List<int> result = new List<int>();
            Object value1;
            if (!dict.TryGetValue(key, out value1))
            {
                result.Add(0);
                return result;
            }
            var list = (List<Object>)value1;
            foreach (var item in list)
            {
                var activity = (Dictionary<string, object>)item;
                Object value2, value3;
                int caloriesBurned;
                Dictionary<string, object> caloriesSummary;
                if (!activity.TryGetValue("caloriesBurnedSummary", out value2))
                {
                    caloriesBurned = 0;
                }
                else
                {
                    caloriesSummary = (Dictionary<string, object>)value2;
                    if (!caloriesSummary.TryGetValue("totalCalories", out value3))
                        caloriesBurned = 0;
                    else
                        caloriesBurned = Int32.Parse(value3.ToString());
                }

                result.Add(caloriesBurned);
            }
            return result;
        }
        public static Activities LoadJson(Dictionary<string, object> dict)
        {
            Activities userActivities;
            List<int> list_bike = parseTotalCalories(dict, "bikeActivities");
            List<int> list_free = parseTotalCalories(dict, "freePlayActivities");
            List<int> list_golf = parseTotalCalories(dict, "golfActivities");
            List<int> list_workout = parseTotalCalories(dict, "guidedWorkoutActivities");
            List<int> list_run = parseTotalCalories(dict, "runActivities");
            List<double> list_sleep = parseSleepTime(dict);
            userActivities = new Activities(list_bike, list_free, list_golf, list_workout, list_run, list_sleep);
            return userActivities;
        }
        public struct Activities
        {
            public List<int> bikeActivities;
            public List<int> freePlayActivities;
            public List<int> golfActivities;
            public List<int> guidedWorkoutActivities;
            public List<int> runActivities;
            public List<double> sleepActivities;
            public Activities(List<int> bike, List<int> freePlay, List<int> golf, List<int> guidedWorkout, List<int> run, List<double> sleep)
            {
                bikeActivities = bike;
                freePlayActivities = freePlay;
                golfActivities = golf;
                guidedWorkoutActivities = guidedWorkout;
                runActivities = run;
                sleepActivities = sleep;
            }
        }

        public struct Person
        {
            public string name;
            public double caloriecost;
            public int age;
            public double weight;
            public double height;
            public string gender;
            public double calorieneed;
            public double actfrequency;
            public Activities userActivities;

            public static int GetAge(DateTime reference, DateTime birthday)
            {
                int age = reference.Year - birthday.Year;
                if (reference < birthday.AddYears(age)) age--;
                return age;
            }

            public void parseProfile(Dictionary<string, object> dict)
            {
                object value;
                this.name = (string) dict["firstName"] + " " + (string) dict["lastName"];
                DateTime now = DateTime.Today;
                if (!dict.TryGetValue("birthdate", out value))
                    age = 0;
                else
                    age = GetAge(now, DateTime.Parse(value.ToString()));
                if (!dict.TryGetValue("weight", out value))
                    weight = 0;
                else
                    weight = double.Parse(value.ToString()) / 1000;
                if (!dict.TryGetValue("height", out value))
                    height = 0;
                else
                    height = double.Parse(value.ToString()) / 10;
                this.gender = (string) dict["gender"];
            }

            public Person(string name = "", double caloriecost = 0, int age = 0, double weight = 0, double height = 0, string gender = "Male", double calorieneed = 0, double actfrequency = 0)
            {
                this.name = name;
                this.caloriecost = caloriecost;
                this.age = age;
                this.weight = weight;
                this.height = height;
                this.gender = gender;
                this.calorieneed = calorieneed;
                this.actfrequency = actfrequency;
                this.userActivities = new Activities(null, null, null, null, null, null);
            }
		
		public double BMI()
		{
			return  this.weight / ((this.height/100) * (this.height/100));
		}

            public double BMR()
            {
			//Women BMR = 655 + (9.6 x weight in kilos) + (1.8 x height in cm) - (4.7 x age in years)

			//Men BMR = 66 + (13.7 x weight in kilos) + (5 x height in cm) - (6.8 x age in years)
                double bmr = 0;
                if (this.gender == "Male")
                {
                    bmr = 66+ this.weight * 13.7 + 5 * this.height - 6.8 * this.age;

                }
                else if (this.gender == "Female")
                {
                    bmr = 655 + 9.6*this.weight + 1.8 * this.height - 4.7*this.age;

                }
                return bmr;
            }
            public void setCalorieNeed(double bmr, double frequency)
            {
                if (frequency >= 0 || frequency < 1)
                {
                    this.calorieneed = bmr * 1.2;
                }
                else if (frequency >= 1 || frequency < 3)
                {
                    this.calorieneed = bmr * 1.375;
                }
                else if (frequency >= 3 || frequency < 5)
                {
                    this.calorieneed = bmr * 1.55;
                }
                else if (frequency >= 5 || frequency < 7)
                {
                    this.calorieneed = bmr * 1.725;
                }
            }

            //return true if need > cost, false otherwise. 
            public void setCalorie(Dictionary<string, object> dict)
            {
                setCalorieCost(dict);
                setCalorieNeed(this.BMR(), this.actfrequency);
            }
            public double calorieRatio()
            {
                return caloriecost / calorieneed;
            }

            public double sleepRatio()
            {
                return average(userActivities.sleepActivities) / 7;
            }

            public double runRatio(int start, int end)
            {
                return userActivities.runActivities.Count * 1.0 / (end - start + 1);
            }
            public void setFrequency(int start, int end)
            {
                int sum = userActivities.runActivities.Count + userActivities.freePlayActivities.Count + userActivities.bikeActivities.Count + userActivities.golfActivities.Count + userActivities.guidedWorkoutActivities.Count;
                this.actfrequency = sum * 7.0 / (end - start + 1);
            }
            public static double average(List<double> list)
            {
                double sum = 0;
                foreach (double item in list)
                {
                    sum += item;
                }
                return sum*1.0 / list.Count;
            }
            public static double average(List<int> list)
            {
                int sum = 0;
                foreach (int item in list)
                {
                    sum += item;
                }
                return sum * 1.0 / list.Count;
            }

            public void setCalorieCost(Dictionary<string, object> dict)
            {
                userActivities = LoadJson(dict);
                caloriecost = average(userActivities.bikeActivities) + average(userActivities.freePlayActivities) + average(userActivities.golfActivities) + average(userActivities.guidedWorkoutActivities) + average(userActivities.runActivities);
            }
            
        }
    }
}
   