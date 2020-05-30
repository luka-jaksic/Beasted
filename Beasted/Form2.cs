using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beasted
{
    public partial class Form2 : Form
    {
        public List<Session> allSessions;

        public Form2( List<Session> parentSessions)
        {
            InitializeComponent();
            allSessions = parentSessions;
            drawProgressionOfWeight();
        }

        public void drawProgressionOfWeight()
        {
            int PlotY = 0;
            int PlotX = 1;
            for(int i = 0; i < allSessions.Count; i++)
            {
                for(int j = 0; j < allSessions.ElementAt(i).WorkoutsInSession.Count; j++)
                {
                    if (allSessions.ElementAt(i).WorkoutsInSession.ElementAt(j).WorkoutType == "Weight")
                    {
                        PlotY = allSessions.ElementAt(i).WorkoutsInSession.ElementAt(j).WorkoutWeight;
                        chart1.Series["Weight"].Points.AddXY(PlotX, PlotY);
                        PlotY = allSessions.ElementAt(i).WorkoutsInSession.ElementAt(j).WorkoutReps;
                        chart1.Series["Reps"].Points.AddXY(PlotX, PlotY);
                        PlotX++;
                    }
                }
            }
        }


    }
}
