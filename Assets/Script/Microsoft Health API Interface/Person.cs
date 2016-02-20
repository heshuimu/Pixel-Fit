using System;
using System.Collections.Generic;
using MiniJSON;
using System.IO;

namespace testMiniJson
{
    class PixelFit
    {
        public static List<int> parse(Dictionary<string, object> dict, string key)
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
        public static Activities LoadJson()
        {
            Activities userActivities;
            using (StreamReader r = new StreamReader("test.json"))
            {
                string json = r.ReadToEnd();
                var dict = Json.Deserialize(json) as Dictionary<string, object>;
                List<int> list_bike = parse(dict, "bikeActivities");
                List<int> list_free = parse(dict, "freePlayActivities");
                List<int> list_golf = parse(dict, "golfActivities");
                List<int> list_workout = parse(dict, "guidedWorkoutActivities");
                List<int> list_run = parse(dict, "runActivities");
                List<int> list_sleep = parse(dict, "sleepActivities");
                userActivities = new Activities(list_bike, list_free, list_golf, list_workout, list_run, list_sleep);
            }
            return userActivities;
        }
        public struct Activities
        {
            public List<int> bikeActivities;
            public List<int> freePlayActivities;
            public List<int> golfActivities;
            public List<int> guidedWorkoutActivities;
            public List<int> runActivities;
            public List<int> sleepActivities;
            public Activities(List<int> bike, List<int> freePlay, List<int> golf, List<int> guidedWorkout, List<int> run, List<int> sleep)
            {
                bikeActivities = bike;
                freePlayActivities = freePlay;
                golfActivities = golf;
                guidedWorkoutActivities = guidedWorkout;
                runActivities = run;
                sleepActivities = sleep;
            }
        }
        public static int average(List<int> list)
        {
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            return sum / list.Count;
        }

        public struct Person
        {
            public string name;
            public double caloriecost;
            public int age;
            public double weight;
            public double height;
            public char gender;
            public double calorieneed;
            public int actfrequency;
            public Activities userActivities;

            public Person(string name = "", double caloriecost = 0, int age = 0, double weight = 0, double height = 0, char gender = 'M', double calorieneed = 0, int actfrequency = 0)
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

            public double BMR()
            {
                double bmr = 0;
                if (this.gender == 'm' || this.gender == 'M')
                {
                    bmr = 10 * this.weight + 6.25 * this.height - 5 * this.age + 5;

                }
                else if (this.gender == 'f' || this.gender == 'F')
                {
                    bmr = 10 * this.weight + 6.25 * this.height - 5 * this.age - 161;

                }
                return bmr;
            }
            public void setCalorieNeed(double bmr, int frequency)
            {
                if (frequency == 0)
                {
                    this.calorieneed = bmr * 1.2;
                }
                else if (frequency == 2)
                {
                    this.calorieneed = bmr * 1.375;
                }
                else if (frequency == 4)
                {
                    this.calorieneed = bmr * 1.55;
                }
                else if (frequency == 6)
                {
                    this.calorieneed = bmr * 1.725;
                }
            }

            //return true if need > cost, false otherwise. 
            public bool compareCalorie()
            {
                return caloriecost / calorieneed > 0.2;
            }

            public static double average(List<int> list)
            {
                int sum = 0;
                foreach (int item in list)
                {
                    sum += item;
                }
                return sum*1.0 / list.Count;
            }

            public void setCalorieCost()
            {
                userActivities = LoadJson();
                caloriecost = average(userActivities.bikeActivities) + average(userActivities.freePlayActivities) + average(userActivities.golfActivities) + average(userActivities.guidedWorkoutActivities) + average(userActivities.runActivities) + average(userActivities.sleepActivities);
            }
        }
    }
}
   