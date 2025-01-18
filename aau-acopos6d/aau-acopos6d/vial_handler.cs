using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
/* ACOPOS6D LIBRARY INCLUDE */
using System.Drawing;

namespace aau_acopos6d
{
    internal class vial_handler
    {
        private SystemCommands _systemCommand = new SystemCommands();
        private XBotCommands _xbotCommand = new XBotCommands();
        private readonly object _lock = new object();
        private void SafeXBotCommand(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }

        private TcpClient client;
        private NetworkStream stream;

        // Defining dictionaries for shuttle starting positions and pickup and approach points for each vial id
        private Dictionary<int, PointF> shuttlePositions = new Dictionary<int, PointF>()
            {
                {1,new PointF(180,905)}, // Input SBS Rack. Should always have index 1
                {2,new PointF(540, 905)}, // Output SBS Rack. Should always have index 2
            };

        private Dictionary<int, PointF> pickupPoints = new Dictionary<int, PointF>()
            {
                {1, new PointF(315, 857)},
                {2, new PointF(344, 857)},
                {3, new PointF(373, 857)},
                {4, new PointF(402, 857)},
                {5, new PointF(315, 885)},
                {6, new PointF(344, 885)},
                {7, new PointF(373, 885)},
                {8, new PointF(402, 885)},
                {9, new PointF(315, 913)},
                {10, new PointF(344, 913)},
                {11, new PointF(373, 913)},
                {12, new PointF(402, 913)}
            };
        private Dictionary<int, PointF> placePoints = new Dictionary<int, PointF>()
            {
                {1, new PointF(315, 857)},
                {2, new PointF(344, 857)},
                {3, new PointF(373, 857)},
                {4, new PointF(402, 857)},
                {5, new PointF(315, 885)},
                {6, new PointF(344, 885)},
                {7, new PointF(373, 885)},
                {8, new PointF(402, 885)},
                {9, new PointF(315, 913)},
                {10, new PointF(344, 913)},
                {11, new PointF(373, 913)},
                {12, new PointF(402, 913)}
            };

        private Dictionary<int, PointF> approachPoints = new Dictionary<int, PointF>()
            {
                {1, new PointF(315, 780)},
                {2, new PointF(344, 780)},
                {3, new PointF(373, 780)},
                {4, new PointF(402, 780)},
                {5, new PointF(315, 780)},
                {6, new PointF(344, 780)},
                {7, new PointF(373, 780)},
                {8, new PointF(402, 780)},
                {9, new PointF(315, 780)},
                {10, new PointF(344, 780)},
                {11, new PointF(373, 780)},
                {12, new PointF(402, 780)}
            };

        private PointF xbot_parking = new PointF(60,900);
        private PointF xbot_entrance = new PointF(300,780);
        private PointF xbot_exit = new PointF(420,780);

        private PointF xbot_exit_highway = new PointF(420, 660);

        private PointF xbot_approach = new PointF(359,780);
        private PointF xbot_pickup = new PointF(359,885);

        private int input_rack = 1;
        private int output_rack = 2;

        public void startup()
        {
            //float height = 4; //mm
            /*foreach (var shuttle in shuttlePositions)
            {
                int id = shuttle.Key;
                PointF pos = shuttle.Value;
                _xbotCommand.LinearMotionSI(0, id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, pos.X / 1000, pos.Y / 1000, 0, 0.1, 10);
                Routines.WaitSingleXbotIdle(id); // May be needed
                _xbotCommand.ShortAxesMotionSI(0, id, POSITIONMODE.ABSOLUTE, height / 1000, 0, 0, 0, 0.04, 0.06, 0.06, 0.06);
                Routines.WaitSingleXbotIdle(id); // May be needed
            }
            _xbotCommand.ShortAxesMotionSI(0, 3, POSITIONMODE.ABSOLUTE, height / 1000, 0, 0, 0, 0.04, 0.06, 0.06, 0.06); // TEMPORARY
            */
            move_actuator(50);
        }

        
        public void move_actuator(float pos_mm)
        {
            try
            {
                client = new TcpClient("192.168.10.42", 12345); // Connect to Python server
                stream = client.GetStream();

                string message = $"{pos_mm}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                client.Close();
                Thread.Sleep(800);
            }
            catch
            {
            }
        }

        private void pickup_vial(int vial_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, input_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, approachPoints[vial_id].X / 1000, approachPoints[vial_id].Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(input_rack);
            });

            move_actuator(0);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, input_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, pickupPoints[vial_id].X / 1000, pickupPoints[vial_id].Y / 1000, 0, 0.1, 10);
                Routines.WaitSingleXbotIdle(input_rack);
            });

            move_actuator(50);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, input_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, approachPoints[vial_id].X / 1000, approachPoints[vial_id].Y / 1000, 0, 0.1, 10);
                Routines.WaitSingleXbotIdle(input_rack);
            });

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, input_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, shuttlePositions[input_rack].X / 1000, shuttlePositions[input_rack].Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(input_rack);
            });
        }

        private void place_vial(int vial_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, output_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, approachPoints[13 - vial_id].X / 1000, approachPoints[13 - vial_id].Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(output_rack);
            });

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, output_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, pickupPoints[13 - vial_id].X / 1000, pickupPoints[13 - vial_id].Y / 1000, 0, 0.1, 10);
                Routines.WaitSingleXbotIdle(output_rack);
            });

            move_actuator(0);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, output_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, approachPoints[13 - vial_id].X / 1000, approachPoints[13 - vial_id].Y / 1000, 0, 0.1, 10);
                Routines.WaitSingleXbotIdle(output_rack);
            });

            move_actuator(50);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, output_rack, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, shuttlePositions[output_rack].X / 1000, shuttlePositions[output_rack].Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(output_rack);
            });
        }

        private void pickup_xbot(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_approach.X / 1000, xbot_approach.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            move_actuator(0);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_pickup.X / 1000, xbot_pickup.Y / 1000, 0, 0.15, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            move_actuator(50);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_approach.X / 1000, xbot_approach.Y / 1000, 0, 0.15, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void place_xbot(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_approach.X / 1000, xbot_approach.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, xbot_pickup.X / 1000, xbot_pickup.Y / 1000, 0, 0.15, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            move_actuator(0);

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_approach.X / 1000, xbot_approach.Y / 1000, 0, 0.15, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            move_actuator(50);
        }

        private void park_xbot(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, xbot_parking.X / 1000, xbot_parking.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void xbot_exiting(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_exit.X / 1000, xbot_exit.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });

            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, xbot_exit_highway.X / 1000, xbot_exit_highway.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void xbot_entering(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, xbot_entrance.X / 1000, xbot_entrance.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }
        
        public void handle_vial(int vial_id, int xbot_id,bool input)
        {
            // Remove single holder xbot from highway
            xbot_entering(xbot_id);


            if (input)
            {
                int rack_id = 1;
                // Park single xbot
                park_xbot(xbot_id);

                // remove specified vial from input rack
                pickup_vial(vial_id);

                // pickup vial with single xbot
                place_xbot(xbot_id);

                //return xbot to highway
                xbot_exiting(xbot_id);
            }
            
            else
            {
                int rack_id = 2;
                //pickup vial with single xbot
                pickup_xbot(xbot_id);

                //park single xbot
                park_xbot(xbot_id);

                //place single vial in output
                place_vial(vial_id);

                //return xbot to highway
                xbot_exiting(xbot_id);
            }

            //PointF initialPos = new PointF(300,660);
            //_xbotCommand.LinearMotionSI(0, 3, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, initialPos.X / 1000, initialPos.Y / 1000, 0, 0.5, 10); // TEMPORARY
        }
    }
}
