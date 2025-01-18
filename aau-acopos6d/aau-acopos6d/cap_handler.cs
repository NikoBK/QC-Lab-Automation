using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
/* ACOPOS6D LIBRARY INCLUDE */
using System.Drawing;

namespace aau_acopos6d
{
    internal class cap_handler
    {
        private SystemCommands _systemCommand = new SystemCommands();
        private XBotCommands _xbotCommand = new XBotCommands();
        private TcpClient client;
        private NetworkStream stream;
        private readonly object _lock = new object();
        private void SafeXBotCommand(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }

        private PointF xbot_entrance = new PointF(420, 180);
        private PointF xbot_exit = new PointF(300, 180);
        private PointF xbot_exit_highway = new PointF(300, 300);

        private PointF xbot_approach = new PointF(360,180);
        private PointF xbot_cappoint = new PointF(360,60);

        public void cap()
        {
            try
            {
                client = new TcpClient("192.168.10.42", 54321); // Connect to Python server
                stream = client.GetStream();

                string message = "cap";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                client.Close();
                Thread.Sleep(2000);
            }
            catch
            {
            }
        }

        private void xbot_enter(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_entrance.X / 1000, xbot_entrance.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void xbot_goto_approach(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_approach.X / 1000, xbot_approach.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void xbot_goto_cappoint(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_cappoint.X / 1000, xbot_cappoint.Y / 1000, 0, 0.5, 10);
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
        }

        private void xbot_exiting_highway(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, xbot_exit_highway.X / 1000, xbot_exit_highway.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        public void handle_capping(int xbot_id)
        {
            xbot_enter(xbot_id);
            xbot_goto_approach(xbot_id);
            Thread.Sleep(2000); // Sleep to allow cap to be put on
            xbot_goto_cappoint(xbot_id);
            cap();
            //Thread.Sleep(1500); // Sleep to allow cap to be put on
            xbot_exiting(xbot_id);
            xbot_exiting_highway(xbot_id);
        }
    }
}
