using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beasted
{
    public class Session
    {
        double sessionLength;
        int sessionIndex, numWorkoutsInSession;
        List<Workout> workoutsInSession;
        string sessionName;

        public string SessionName
        {
            get { return sessionName; }
            set { sessionName = value; }
        }
        public int SessionIndex
        {
            get { return sessionIndex; }
            set { sessionIndex = value; }
        }
        public double SessionLength
        {
            get { return sessionLength; }
            set { sessionLength = value; }
        }
        public List<Workout> WorkoutsInSession
        {
            get { return workoutsInSession; }
            set { workoutsInSession = value; }
        }
        [JsonConstructor]
        public Session(string SessionName, int SessionIndex, double SessionLength, List<Workout> WorkoutsInSession)
        {
            sessionName = SessionName;
            sessionIndex = SessionIndex;
            sessionLength = SessionLength;
            workoutsInSession = WorkoutsInSession;
        }
        public Session(int indexOfSession)
        {
            sessionIndex = indexOfSession;
            workoutsInSession = new List<Workout>();
            sessionName = "Session No. " + (sessionIndex + 1);
        }
        public void AddWorkout(Workout newWorkout)
        {
            workoutsInSession.Add(newWorkout);
            numWorkoutsInSession++;
            sessionLength += newWorkout.WorkoutLength;
        }
    }
}
