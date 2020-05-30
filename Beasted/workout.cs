using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beasted
{
    public class Workout
    {
        private string workoutType, workoutName;
        private double workoutLength, runDistance;
        private int workoutSets, workoutReps, workoutWeight;

        public string WorkoutType
        {
            get { return workoutType; }
            set { workoutType = value; }
        }
        public string WorkoutName
        {
            get { return workoutName; }
            set { workoutName = value; }
        }
        public double WorkoutLength
        {
            get { return workoutLength; }
            set { workoutLength = value; }
        }
        public double RunDistance
        {
            get { return runDistance; }
            set { runDistance = value; }
        }
        public int WorkoutSets
        {
            get { return workoutSets; }
            set { workoutSets = value; }
        }
        public int WorkoutReps
        {
            get { return workoutReps; }
            set { workoutReps = value; }
        }
        public int WorkoutWeight
        {
            get { return workoutWeight; }
            set { workoutWeight = value; }
        }
        [JsonConstructor]
        public Workout(string WorkoutType, string WorkoutName, double WorkoutLength, double RunDistance, int WorkoutSets, int WorkoutReps, int WorkoutWeight)
        {
            workoutType = WorkoutType;
            workoutName = WorkoutName;
            workoutLength = WorkoutLength;
            runDistance = RunDistance;
            workoutSets = WorkoutSets;
            workoutReps = WorkoutReps;
            workoutWeight = WorkoutWeight;
        }
        public Workout(string type, string name, double length, double distance)
        {
            workoutType = type;
            workoutName = name;
            workoutLength = length;
            runDistance = distance;
        }
        public Workout(string type, string name, double length, int sets, int reps)
        {
            workoutType = type;
            workoutName = name;
            workoutLength = length;
            workoutSets = sets;
            workoutReps = reps;
        }
        public Workout(string type, string name, double length, int sets, int reps, int weight)
        {
            workoutType = type;
            workoutName = name;
            workoutLength = length;
            workoutSets = sets;
            workoutReps = reps;
            workoutWeight = weight;
        }
    }
}
