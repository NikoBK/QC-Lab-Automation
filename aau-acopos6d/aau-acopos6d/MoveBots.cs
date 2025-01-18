using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* ACOPOS6D LIBRARY INCLUDE */
using static aau_acopos6d.Routines;

namespace aau_acopos6d
{
    public class MoveBots
    {
        private SystemCommands _systemCommand = new SystemCommands();
        private static XBotCommands _xbotCommand = new XBotCommands();
        private static readonly object _lock = new object();
        private static void SafeXBotCommand(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }

        public static void MoveSingleBotToPos(int xbot, PointF Pos)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot, 0, 0, Pos.X, Pos.Y, 0, 0.5, 2);
                //WaitSingleXbotIdle(xbot);
            });
        }

        public static void MoveSingleBotToPosYX(int xbot, PointF Pos)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot, 0, LINEARPATHTYPE.YTHENX, Pos.X, Pos.Y, 0, 0.5, 2);
                //WaitSingleXbotIdle(xbot);
            });
        }
        public static void MoveSingleBotToPosXY(int xbot, PointF Pos)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot, 0, LINEARPATHTYPE.XTHENY, Pos.X, Pos.Y, 0, 0.5, 2);
                //WaitSingleXbotIdle(xbot);
            });
        }

        public static void MoveSingleBotZ(int xbot, float z)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.ShortAxesMotionSI(0, xbot, 0, z, 0, 0, 0, 2, 2, 2, 2);
                //WaitSingleXbotIdle(xbot);
            });
        }

        public static void MoveSingleBotRelative(int xbot, float x, float y)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot, POSITIONMODE.RELATIVE, 0, x, y, 0, 0.5, 2);
                //WaitSingleXbotIdle(xbot);
            });
        }
    }
}
