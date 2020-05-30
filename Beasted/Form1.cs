using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace Beasted
{
    public partial class beastedMainForm : Form
    {
        //For Dragging-------------------------------------------------------------------------------
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //-------------------------------------------------------------------------------------------
        public string currentSaveLocation = null;
        List<Session> AllSessions = new List<Session>();
        Session currentSession;
        public static string[] weightWorkouts = {"Curls","Bench Presses"};
        public static string[] bodyWeightWorkouts = {"Pushups", "Crunches", "Burpees", "Pull-Ups"};
        public static string[] cardio = { "Running" };
        public beastedMainForm()
        {
            InitializeComponent();
            changePossibleWorkouts();
            menuStrip1.BackColor = Color.FromArgb(25, 25, 25);
            menuStrip1.ForeColor = Color.FromArgb(230, 232, 230);
            AddSession();
        }

        public void changePossibleWorkouts()
        {
            Color enabled = Color.FromArgb(230, 232, 230);
            Color disabled = Color.FromArgb(173, 173, 173);
            lbWorkouts.Items.Clear();
            string[] allPossibleWorkouts;
            allPossibleWorkouts = weightWorkouts;

            if (rbBodyWeight.Checked)
            {
                allPossibleWorkouts = bodyWeightWorkouts;
                numReps.Enabled = true;
                numSets.Enabled = true;
                numWeight.Enabled = false;
                tbKilometers.Enabled = false;
                numReps.BackColor = enabled;
                numSets.BackColor = enabled;
                numWeight.BackColor = disabled;
                tbKilometers.BackColor = disabled;
            }
            else
            {
                if (rbCardio.Checked)
                {
                    allPossibleWorkouts = cardio;
                    numReps.Enabled = false;
                    numSets.Enabled = false;
                    numWeight.Enabled = false;
                    tbKilometers.Enabled = true;
                    numReps.BackColor = disabled;
                    numSets.BackColor = disabled;
                    numWeight.BackColor = disabled;
                    tbKilometers.BackColor = enabled;
                }
                else
                {
                    if (rbWeight.Checked)
                    {
                        allPossibleWorkouts = weightWorkouts;
                        numReps.Enabled = true;
                        numSets.Enabled = true;
                        numWeight.Enabled = true;
                        tbKilometers.Enabled = false;
                        numReps.BackColor = enabled;
                        numSets.BackColor = enabled;
                        numWeight.BackColor = enabled;
                        tbKilometers.BackColor = disabled;
                    }
                }
            }
            for (int i = 0; i < allPossibleWorkouts.Length; i++)
            {
                lbWorkouts.Items.Add(allPossibleWorkouts[i]);
            }
        }

        public void updateAllDisplayedWorkouts()
        {
            lbDisplayWorkouts.Items.Clear();
            if (currentSession != null && currentSession.WorkoutsInSession.Count != 0) {
                for (int i = 0; i < currentSession.WorkoutsInSession.Count; i++)
                {
                    string toDisplay = null;
                    if (currentSession.WorkoutsInSession.ElementAt(i).WorkoutType == "Weight")
                    {
                        toDisplay = currentSession.WorkoutsInSession.ElementAt(i).WorkoutType + " | " + currentSession.WorkoutsInSession.ElementAt(i).WorkoutName + " | For: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutLength) + " Hours | Sets: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutSets) + " | Reps: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutReps) + " | Weight: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutWeight) + "kg";
                    }
                    else
                    {
                        if (currentSession.WorkoutsInSession.ElementAt(i).WorkoutType == "Body Weight")
                        {
                            toDisplay = currentSession.WorkoutsInSession.ElementAt(i).WorkoutType + " | " + currentSession.WorkoutsInSession.ElementAt(i).WorkoutName + " | For: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutLength) + " Hours | Sets: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutSets) + " | Reps: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutReps) + " |";
                        }
                        else
                        {
                            toDisplay = currentSession.WorkoutsInSession.ElementAt(i).WorkoutType + " | " + currentSession.WorkoutsInSession.ElementAt(i).WorkoutName + " | For: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).WorkoutLength) + " Hours | Distance Ran: " + Convert.ToString(currentSession.WorkoutsInSession.ElementAt(i).RunDistance) + "km";
                        }
                    }
                    lbDisplayWorkouts.Items.Add(toDisplay);
                }
            }
        }

        public void changeCurrentSession()
        {
            if (cbSessions.Items.Count != 0)
            {
                currentSession = AllSessions.ElementAt(cbSessions.SelectedIndex);
                updateAllDisplayedWorkouts();
            }
        }

        private void rbBodyWeight_CheckedChanged(object sender, EventArgs e)
        {
            changePossibleWorkouts();
        }

        private void rbWeight_CheckedChanged(object sender, EventArgs e)
        {
            changePossibleWorkouts();
        }

        private void rbCardio_CheckedChanged(object sender, EventArgs e)
        {
            changePossibleWorkouts();
        }

        private void lbWorkouts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Color selectedColor = Color.FromArgb(241, 80, 37);
            //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(selectedColor);
            //System.Drawing.Graphics formGraphics;
            //formGraphics = this.CreateGraphics();
            //formGraphics.FillEllipse(myBrush, new Rectangle(0, 0, 200, 300));
            //myBrush.Dispose();
            //formGraphics.Dispose();
        }

        private void lbWorkouts_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color selectedColor = Color.FromArgb(241, 80, 37);
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,e.Font,e.Bounds,e.Index,e.State ^ DrawItemState.Selected,e.ForeColor,selectedColor);//Choose the color
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(lbWorkouts.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void btnAddWorkout_MouseClick(object sender, MouseEventArgs e)
        {
            if (lbWorkouts.SelectedItem == null || tbLength.Text == null)
            {
                MessageBox.Show("Not all required values have been filled out. Please fill out every value coresponding to the workout.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string name = lbWorkouts.SelectedItem.ToString();
                double length = Convert.ToDouble(tbLength.Text);

                if (currentSession != null)
                {

                    if (rbBodyWeight.Checked)
                    {
                        if (numReps.Value <= 0 || numSets.Value <= 0)
                        {
                            MessageBox.Show("Reps or Sets can not be set to 0. Do some work first, then fill these in champ.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            int sets = Convert.ToInt32(numSets.Value);
                            int reps = Convert.ToInt32(numReps.Value);
                            Workout newWorkout = new Workout("Body Weight", name, length, sets, reps);
                            currentSession.AddWorkout(newWorkout);
                        }
                    }
                    else
                    {
                        if (rbCardio.Checked)
                        {
                            if (tbKilometers==null || Convert.ToDouble(tbKilometers.Text)<=0)
                            {
                                MessageBox.Show("Your distance ran can not be set to 0. Do some work first, then fill these in champ.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                double distance = Convert.ToDouble(tbKilometers.Text);
                                Workout newWorkout = new Workout("Cardio", name, length,distance);
                                currentSession.AddWorkout(newWorkout);
                            }
                        }
                        else
                        {
                            if (rbWeight.Checked)
                            {
                                if (numReps.Value <= 0 || numSets.Value <= 0)
                                {
                                    MessageBox.Show("Reps, Sets or the Weight can not be set to 0. Do some work first, then fill these in champ.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    int sets = Convert.ToInt32(numSets.Value);
                                    int reps = Convert.ToInt32(numReps.Value);
                                    int weight = Convert.ToInt32(numWeight.Value);
                                    Workout newWorkout = new Workout("Weight", name, length, sets, reps, weight);
                                    currentSession.AddWorkout(newWorkout);
                                }
                            }
                        }
                    }
                } 
            }
            updateAllDisplayedWorkouts();
        }

        private void btnNewSession_MouseClick(object sender, MouseEventArgs e)
        {
            AddSession();
        }
        public void AddSession()
        {
            int sessionIndex;
            if (AllSessions == null)
            {
                sessionIndex = 0;
            }
            else
            {
                sessionIndex = AllSessions.Count;
            }
            Session newSession = new Session(sessionIndex);
            AllSessions.Add(newSession);
            currentSession = newSession;
            cbSessions.Items.Add(currentSession.SessionName);
            cbSessions.SelectedIndex = AllSessions.Count - 1;
        }
        private void lbDisplayWorkouts_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void cbSessions_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeCurrentSession();
        }

        private void btnQuit_MouseClick(object sender, MouseEventArgs e)
        {
            string jsonString = JsonConvert.SerializeObject(AllSessions, Formatting.Indented);
            
            DirectoryInfo savingDir = new DirectoryInfo(@"C:\Users\Luka\Desktop\FIlestream");
            string jsonFilePath = @"C:\Users\Luka\Desktop\FIlestream\test.json";
            File.WriteAllText(jsonFilePath, jsonString);
        }

        private void btnGraphs_MouseClick(object sender, MouseEventArgs e)
        {
            Form2 GraphForm = new Form2(AllSessions);
            GraphForm.Show();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string jsonString = JsonConvert.SerializeObject(AllSessions, Formatting.Indented);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json File|*.json";
            saveFileDialog.Title = "Save Your Workout Details";
            saveFileDialog.DefaultExt = "json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, jsonString);
                    currentSaveLocation = saveFileDialog.FileName;
                }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string jsonString = JsonConvert.SerializeObject(AllSessions, Formatting.Indented);
            if (currentSaveLocation != null)
            {
                System.IO.File.WriteAllText(currentSaveLocation, jsonString);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllSessions = new List<Session>();
            cbSessions.Items.Clear();
            lbDisplayWorkouts.Items.Clear();
            AddSession();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json File|*.json";
            openFileDialog.Title = "Open Your Workout Details";
            string serializedJsonString;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                serializedJsonString = System.IO.File.ReadAllText(openFileDialog.FileName);
                AllSessions = new List<Session>();
                cbSessions.Items.Clear();
                lbDisplayWorkouts.Items.Clear();
                AllSessions = JsonConvert.DeserializeObject<List<Session>>(serializedJsonString);
                currentSaveLocation = openFileDialog.FileName;
            }
            for(int i = 0; i < AllSessions.Count; i++)
            {
                cbSessions.Items.Add(AllSessions.ElementAt(i).SessionName);
            }
            cbSessions.SelectedIndex = AllSessions.Count - 1;
        }

        private void buttonExitProgram_MouseEnter(object sender, EventArgs e)
        {
            buttonExitProgram.BackColor = Color.Red;
            buttonExitProgram.ForeColor = Color.FromArgb(25, 25, 25);
        }

        private void buttonExitProgram_MouseLeave(object sender, EventArgs e)
        {
            buttonExitProgram.ForeColor = Color.Red;
            buttonExitProgram.BackColor = Color.FromArgb(25, 25, 25);
        }

        private void buttonExitProgram_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.Yellow;
            button1.ForeColor = Color.FromArgb(25, 25, 25);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Yellow;
            button1.BackColor = Color.FromArgb(25, 25, 25);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
