using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2018.Day04
{
    /*
     * Covering the walls, someone has spent an hour starting every midnight for the past few months
     * secretly observing this guard post! They've been writing down the ID of the one guard on duty
     * that night - the Elves seem to have decided that one guard was enough for the overnight shift -
     * as well as when they fall asleep or wake up while at their post (your puzzle input). Find the
     * guard that has the most minutes asleep. What minute does that guard spend asleep the most?
     *
     * What is the ID of the guard you chose multiplied by the minute you chose?
     *
     * https://adventofcode.com/2018/day/4
     */

    public static class Part01
    {
        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day04);
            Array.Sort(input, StringComparer.InvariantCulture);
            ProcessInput(input, out string[] timestamps, out string[] events);
            CreateTimetable(timestamps, events, out List<bool[]> timetable, out List<int> ids);
            int id = FindGuardWithMostMinutesAsleep(timetable, ids);
            int minuteMostSlept = FindGuardsMostasleepMinute(timetable, ids, id);

            Console.WriteLine("Guard with id: {0}", id);
            Console.WriteLine("Minute most slept: {0}", minuteMostSlept);

            int result = minuteMostSlept * id;

            General.PrintResult("The answer is", result);
        }

        /// <summary>
        /// Splits the input into separate arrays.
        /// </summary>
        /// <param name="timestamps">The time of the event.</param>
        /// <param name="events">The logged event.</param>
        private static void ProcessInput(string[] input, out string[] timestamps, out string[] events)
        {
            timestamps = new string[input.Length];
            events = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];

                // Timestamp is 18 chars long.
                timestamps[i] = line.Substring(0, 18);
                events[i] = line.Substring(18).Trim();
            }
        }

        /// <summary>
        /// Creates a timetable containing entries of every night, with an
        /// accompanying list of guard ids.
        /// </summary>
        private static void CreateTimetable(string[] timestamps, string[] events, out List<bool[]> timetable, out List<int> ids)
        {
            timetable = new List<bool[]>();
            ids = new List<int>();

            bool[] tableEntry = Array.Empty<bool>();
            for (int i = 0; i < timestamps.Length; i++)
            {
                string @event = events[i];
                switch (@event)
                {
                    case "falls asleep":
                        break;

                    case "wakes up":
                        continue;

                    default:
                        tableEntry = new bool[60];
                        int id = GetGuardId(@event);

                        timetable.Add(tableEntry);
                        ids.Add(id);
                        continue;
                }

                string asleepTimestamp = timestamps[i];
                string awakeTimestamp = timestamps[i + 1];
                int asleepMinute = GetMinutes(asleepTimestamp);
                int awakeMinute = GetMinutes(awakeTimestamp);

                for (int j = asleepMinute; j < awakeMinute; j++)
                    tableEntry[j] = true;
            }
        }

        /// <summary>
        /// Finds the guard who sleeps the most.
        /// </summary>
        private static int FindGuardWithMostMinutesAsleep(List<bool[]> timetable, List<int> ids)
        {
            List<int> guardIds = new List<int>();
            List<int> sleepMinutes = new List<int>();
            int guardIndex;

            for (int i = 0; i < timetable.Count; i++)
            {
                bool[] tableEntry = timetable[i];
                int id = ids[i];

                if (!guardIds.Contains(id))
                {
                    guardIds.Add(id);
                    sleepMinutes.Add(0);
                    guardIndex = guardIds.Count - 1;
                }
                else
                    guardIndex = guardIds.IndexOf(id);

                for (int j = 0; j < tableEntry.Length; j++)
                    if (tableEntry[j])
                        sleepMinutes[guardIndex]++;
            }

            int maxValue = sleepMinutes.Max();
            int maxIndex = sleepMinutes.IndexOf(maxValue);
            return guardIds[maxIndex];
        }

        /// <summary>
        /// Returns the minute that the given guard is the most like to sleep.
        /// </summary>
        private static int FindGuardsMostasleepMinute(List<bool[]> timetable, List<int> ids, int guardID)
        {
            int[] totalMinutesAsleep = new int[60];
            for (int i = 0; i < timetable.Count; i++)
            {
                bool[] tableEntry = timetable[i];
                int id = ids[i];
                if (id != guardID)
                    continue;

                for (int j = 0; j < tableEntry.Length; j++)
                {
                    bool isAsleep = tableEntry[j];
                    if (isAsleep)
                        totalMinutesAsleep[j]++;
                }
            }

            int maxValue = totalMinutesAsleep.Max();
            for (int i = 0; i < totalMinutesAsleep.Length; i++)
                if (totalMinutesAsleep[i] == maxValue)
                    return i;

            throw new Exception("No valid value");
        }

        /// <summary>
        /// Returns the id of the guard referenced in the given event.
        /// </summary>
        private static int GetGuardId(string @event)
        {
            string[] rawEntry = @event.Split(' ');
            string rawId = rawEntry[1].Substring(1);
            return int.Parse(rawId);
        }

        /// <summary>
        /// Gets minutes from the timestamp.
        /// </summary>
        private static int GetMinutes(string timestamp)
        {
            // Super elegant way to get minutes.
            string[] rawTime = timestamp.Split(' ')[1].Substring(0, 5).Split(':');

            // If before midnight, skip to midnight.
            int hours = int.Parse(rawTime[0]);
            if (hours != 0)
                return 0;

            // Else, return minutes.
            return int.Parse(rawTime[1]);
        }
    }
}