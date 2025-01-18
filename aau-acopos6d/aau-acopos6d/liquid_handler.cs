using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
/* ACOPOS6D LIBRARY INCLUDE */
using System.Drawing;

namespace aau_acopos6d
{
    internal class liquid_handler
    {
        private static SystemCommands _systemCommand = new SystemCommands();
        private static XBotCommands _xbotCommand = new XBotCommands();
        private static readonly object _lock = new object();
        private static void SafeXBotCommand(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }

        private PointF xbot_exit = new PointF(120, 660);
        private PointF xbot_exit_highway = new PointF(300, 660);

        private PointF handling_point = new PointF(120,360);

        private void goto_handlingpoint(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, handling_point.X / 1000, handling_point.Y / 1000, 0, 0.5, 10);
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

        private void xbot_liquid_handler(int xbot_id)
        {
            //Here the functions correlating to a liquid handler would go, but to simulate something happen we sleep for 4 seconds
            Thread.Sleep(4000);
        }
        public void handle_liquid(int xbot_id)
        {
            goto_handlingpoint(xbot_id);
            xbot_liquid_handler(xbot_id);
            xbot_exiting(xbot_id);
            xbot_exiting_highway(xbot_id);
        }
    }
}
