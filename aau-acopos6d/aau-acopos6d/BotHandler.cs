/* ACOPOS6D LIBRARY INCLUDE */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Threading;
using static aau_acopos6d.MoveBots;
using static aau_acopos6d.Routines;

namespace aau_acopos6d
{
    class BotHandler
    {
        private SystemCommands _systemCommand = new SystemCommands();
        private XBotCommands _xbotCommand = new XBotCommands();

        //private MoveBots _moveBots = new MoveBots();

        private PointF[] stationPos = new PointF[6];
        private PointF[] twoBots = new PointF[2];

        public PointF[] highwayStart = new PointF[7];

        public BotHandler(int function = 0)
        {
            switch(function)
            {
                case 0:
                    break;

                case 1:
                    Thread twoBots = new Thread(MoveTwoBots);
                    twoBots.Start();
                    break;

                case 2:
                    Thread toStations = new Thread(MoveToStations);
                    toStations.Start();
                    break;

                case 3:
                    Thread jerk = new Thread(Jerking);
                    jerk.Start();
                    break;

                case 4:
                    Thread circle = new Thread(Circling);
                    circle.Start();
                    break;

                case 5:
                    Thread moon = new Thread(Mooning);
                    moon.Start();
                    break;
            }
        }

        public void ResetHighway()
        {
            int[] xBotIDs = GetIds();

            highwayStart[0] = new PointF(0.180f, 0.905f);
            highwayStart[1] = new PointF(0.540f, 0.905f);
            highwayStart[2] = new PointF(0.300f, 0.060f);
            highwayStart[3] = new PointF(0.420f, 0.060f);
            highwayStart[4] = new PointF(0.540f, 0.060f);
            highwayStart[5] = new PointF(0.660f, 0.060f);
            highwayStart[6] = new PointF(0.780f, 0.060f);

            for (int i = 0; i < 2; i++)
            {
                MoveSingleBotZ(xBotIDs[i], 0.004f);
                WaitSingleXbotIdle(xBotIDs[i]);
                MoveSingleBotToPosYX(xBotIDs[i], highwayStart[i]);
            }

            for (int i = 2; i < xBotIDs.Length; i++)
            {
                MoveSingleBotZ(xBotIDs[i], 0.004f);
                WaitSingleXbotIdle(xBotIDs[i]);
                MoveSingleBotToPosXY(xBotIDs[i], highwayStart[i]);
            }
        }

        public void MoveTwoBots()
        {
            RunStartUpRoutine();
            int[] xBotIDs = GetIds();

            twoBots[0] = new PointF(0.300f, 0.300f);
            twoBots[1] = new PointF(0.100f, 0.200f);

            WaitSingleXbotIdle(xBotIDs[0]);
            //MoveSingleBotToPos(xBotIDs[0], twoBots[0]);
        }

        public void MoveToStations()
        {
            RunStartUpRoutine();
            int[] xBotIDs = GetIds();

            stationPos[0] = new PointF(0.060f, 0.060f);
            stationPos[1] = new PointF(0.660f, 0.060f);
            stationPos[2] = new PointF(0.060f, 0.450f);
            stationPos[3] = new PointF(0.660f, 0.450f);
            stationPos[4] = new PointF(0.060f, 0.900f);
            stationPos[5] = new PointF(0.660f, 0.900f);

            WaitSingleXbotIdle(xBotIDs[0]);
            //MoveSingleBotToPos(xBotIDs[0], stationPos[0]);
        }

        public void Jerking()
        {
            /*RunStartUpRoutine();
            int[] xBotIDs = GetIds();

            double totalTime = 0;

            WaitSingleXbotIdle(xBotIDs[0]);
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.BLOCKBUFFER);
            while (totalTime < 5)
            {
                MotionRtn time = MoveSingleBotRelative(xBotIDs[0], 0.002f, 0);
                totalTime = totalTime + time.TravelTimeSecs;
                time = MoveSingleBotRelative(xBotIDs[0], -0.002f, 0);
                totalTime = totalTime + time.TravelTimeSecs;
            }
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.RELEASEBUFFER);
            */
        }

        public void Circling()
        {
            RunStartUpRoutine();
            int[] xBotIDs = GetIds();

            double totalTime = 0;

            WaitSingleXbotIdle(xBotIDs[0]);
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.BLOCKBUFFER);
            while (totalTime < 5)
            {
                MotionRtn time = _xbotCommand.ArcMotionMetersRadians(0, xBotIDs[0], ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, 0, POSITIONMODE.RELATIVE, 0.1, 0, 2, 2, 10, 0.05, 0);
                totalTime = totalTime + time.TravelTimeSecs;
                time = _xbotCommand.ArcMotionMetersRadians(0, xBotIDs[0], ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, 0, POSITIONMODE.RELATIVE, -0.1, 0, 2, 2, 10, 0.05, 0);
                totalTime = totalTime + time.TravelTimeSecs;
            }
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.RELEASEBUFFER);
        }

        public void Mooning()
        {
            RunStartUpRoutine();
            int[] xBotIDs = GetIds();

            double totalTime = 0;

            WaitSingleXbotIdle(xBotIDs[0]);
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.BLOCKBUFFER);
            while (totalTime < 5)
            {
                MotionRtn time = _xbotCommand.ArcMotionMetersRadians(0, xBotIDs[0], ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, ARCDIRECTION.CLOCKWISE, POSITIONMODE.RELATIVE, 0.1, 0, 0, 2, 10, 0.05, 0);
                totalTime = totalTime + time.TravelTimeSecs;
                time = _xbotCommand.ArcMotionMetersRadians(0, xBotIDs[0], ARCMODE.TARGETRADIUS, ARCTYPE.MAJORARC, ARCDIRECTION.COUNTERCLOCKWISE, POSITIONMODE.RELATIVE, -0.1, 0, 0, 2, 10, 0.05, 0);
                totalTime = totalTime + time.TravelTimeSecs;
            }
            _xbotCommand.MotionBufferControl(xBotIDs[0], MOTIONBUFFEROPTIONS.RELEASEBUFFER);
        }
    }
}
