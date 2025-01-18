using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
/* ACOPOS6D LIBRARY INCLUDE */
using System.Drawing;

namespace aau_acopos6d
{
    internal class mixing_handler
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

        private PointF xbot_entrance = new PointF(540, 660);
        private PointF xbot_exit = new PointF(540, 300);

        private PointF xbot_exit_highway = new PointF(420, 300);

        private PointF mixing_coords = new PointF(600, 480);

        private void xbot_entering(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.DIRECT, xbot_entrance.X / 1000, xbot_entrance.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void goto_mixing(int xbot_id)
        {
            SafeXBotCommand(() =>
            {
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.XTHENY, mixing_coords.X / 1000, mixing_coords.Y / 1000, 0, 0.5, 10);
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
                _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.ABSOLUTE, LINEARPATHTYPE.YTHENX, xbot_exit_highway.X / 1000, xbot_exit_highway.Y / 1000, 0, 0.5, 10);
                Routines.WaitSingleXbotIdle(xbot_id);
            });
        }

        private void mix(int xbot_id, int method, double shakeMag, double duration)
        {
            double motionTime = 0;
            MotionRtn motionRtn;

            if (method == 0)
            {
                _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.BLOCKBUFFER);
                while (motionTime < duration)
                {
                    motionRtn = _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.RELATIVE, LINEARPATHTYPE.DIRECT, shakeMag / 1000, shakeMag / 1000, 0, 5, 10);
                    motionTime += motionRtn.TravelTimeSecs;
                    motionRtn = _xbotCommand.LinearMotionSI(0, xbot_id, POSITIONMODE.RELATIVE, LINEARPATHTYPE.DIRECT, -(shakeMag / 1000), -(shakeMag / 1000), 0, 5, 10);
                    motionTime += motionRtn.TravelTimeSecs;
                }
                _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.RELEASEBUFFER);
            }

            if (method == 1)
            {
                SafeXBotCommand(() =>
                {
                    _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.BLOCKBUFFER);
                });
                while (motionTime < duration)
                {
                    SafeXBotCommand(() =>
                    {
                        motionRtn = _xbotCommand.ArcMotionMetersRadians(0, xbot_id, ARCMODE.TARGETRADIUS, ARCTYPE.MINORARC, ARCDIRECTION.CLOCKWISE, POSITIONMODE.RELATIVE, shakeMag / 1000, shakeMag / 1000, 0, 5, 10, shakeMag / 1000, 0);
                        motionTime += motionRtn.TravelTimeSecs;
                    });

                    SafeXBotCommand(() =>
                    {
                        motionRtn = _xbotCommand.ArcMotionMetersRadians(0, xbot_id, ARCMODE.TARGETRADIUS, ARCTYPE.MINORARC, ARCDIRECTION.CLOCKWISE, POSITIONMODE.RELATIVE, -(shakeMag / 1000), -(shakeMag / 1000), 0, 5, 10, shakeMag / 1000, 0);
                        motionTime += motionRtn.TravelTimeSecs;
                    });
                }
                SafeXBotCommand(() =>
                {
                    _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.RELEASEBUFFER);
                });
            }

            if (method == 2)
            {
                _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.BLOCKBUFFER);
                while (motionTime < duration)
                {
                    motionRtn = _xbotCommand.ArcMotionMetersRadians(0, xbot_id, ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, ARCDIRECTION.CLOCKWISE, POSITIONMODE.RELATIVE, shakeMag / 1000, 0, 0, 5, 10, shakeMag / 2000, 0);
                    motionTime += motionRtn.TravelTimeSecs;
                    motionRtn = _xbotCommand.ArcMotionMetersRadians(0, xbot_id, ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, ARCDIRECTION.COUNTERCLOCKWISE, POSITIONMODE.RELATIVE, -(shakeMag / 1000), 0, 0, 5, 10, shakeMag / 2000, 0);
                    motionTime += motionRtn.TravelTimeSecs;
                }
                _xbotCommand.MotionBufferControl(xbot_id, MOTIONBUFFEROPTIONS.RELEASEBUFFER);
            }
        }
        public void handle_mixing(int xbot_id, int method, int magnitude, double duration)
        {
            // Remove xbot from highway
            xbot_entering(xbot_id);

            // Take xbot to mixing area
            goto_mixing(xbot_id);

            // Run mixing sequence
            mix(xbot_id, method, magnitude, duration);
            
            // Take xbot to exit
            xbot_exiting(xbot_id);

            // Exit xbot to highway
            xbot_exiting_highway(xbot_id);
        }
    }
}
