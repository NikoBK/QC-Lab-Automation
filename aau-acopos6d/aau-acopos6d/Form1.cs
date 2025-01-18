using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.AxHost;

namespace aau_acopos6d
{
    partial class Form1 : Form
    {
        Highway Highway = new Highway();

        BotHandler BotHandler = new BotHandler();
        public BotHandler TwoBotsHandler { get; private set; }
        public BotHandler StationsHandler { get; private set; }
        public BotHandler JerkHandler { get; private set; }
        public BotHandler CirclingHandler { get; private set; }
        public BotHandler MoonHandler { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool started = Routines.RunStartUpRoutine();
            if (!started)
            {
                MessageBox.Show("Failed to start up, please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Start up successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a bothandler instance and start thread in <see cref="BotHandler"/>
            if (TwoBotsHandler == null)
            {
                TwoBotsHandler = new BotHandler(1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (StationsHandler == null)
            {
                StationsHandler = new BotHandler(2);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (JerkHandler == null)
            {
                JerkHandler = new BotHandler(3);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CirclingHandler == null)
            {
                CirclingHandler = new BotHandler(4);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (MoonHandler == null)
            {
                MoonHandler = new BotHandler(5);
            }
        }

        private void send_bot_to_queue_Click(object sender, EventArgs e)
        {
            List<int> stations = new List<int>();

            char[] send = input.Text.ToCharArray();

            int sendVal;

            stations.Add(1);

            for (int i = 0; i < send.Length; i++)
            {
                sendVal = send[i] - '0';
                if (sendVal == 1)
                {
                    sendVal--;
                }

                stations.Add(sendVal);
            }

            stations.Add(1);

            bool vials = Highway.queue_handler(stations);
            if (!vials)
            {
                MessageBox.Show("No more available vials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void send_queue_to_highway_Click(object sender, EventArgs e)
        {
            Highway.initHighway();
        }

        private void reset_highway_test_Click(object sender, EventArgs e)
        {
            BotHandler.ResetHighway();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Highway.reset_input();
        }

        private void queue_test_Click(object sender, EventArgs e)
        {
            List<int> stations = new List<int>();

            string[] teststations = new string[12];
            teststations[0] = "32123";
            teststations[1] = "23123";
            teststations[2] = "213";
            teststations[3] = "1232312";
            teststations[4] = "32";
            teststations[5] = "1231223";
            teststations[6] = "2321";
            teststations[7] = "2";
            teststations[8] = "12332";
            teststations[9] = "323122";
            teststations[10] = "3";
            teststations[11] = "1";

            for (int i = 0; i < 12; i++)
            {
                char[] send = teststations[i].ToCharArray();

                int sendVal;

                stations.Add(1);

                for (int j = 0; j < send.Length; j++)
                {
                    sendVal = send[j] - '0';
                    if (sendVal == 1)
                    {
                        sendVal--;
                    }

                    stations.Add(sendVal);
                }

                stations.Add(1);

                bool vials = Highway.queue_handler(stations);
                if (!vials)
                {
                    MessageBox.Show("No more available vials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
