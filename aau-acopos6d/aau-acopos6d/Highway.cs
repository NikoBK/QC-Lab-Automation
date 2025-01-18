/* ACOPOS6D LIBRARY INCLUDE */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Concurrent;
using static aau_acopos6d.MoveBots;
using static aau_acopos6d.Routines;

namespace aau_acopos6d
{
    class Highway
    {
        //this class contains a collection of system commands such as connecting to the PMC, gain mastership, etc.
        //private static SystemCommands _systemCommand = new SystemCommands();
        //this class contains a collection of xbot commands, such as activate xbots, levitation control, linear motion, etc.
        //private static XBotCommands _xbotCommand = new XBotCommands();

        private cap_handler cap_handler = new cap_handler();
        private liquid_handler liquid_handler = new liquid_handler();
        private mixing_handler mixing_handler = new mixing_handler();
        private vial_handler vial_handler = new vial_handler();

        public ConcurrentDictionary<int, PointF> C { get; private set; }
        public ConcurrentDictionary<int, PointF> D { get; private set; }
        public ConcurrentDictionary<int, PointF> Q { get; private set; }
        public ConcurrentDictionary<int, int> bots { get; private set; }
        public ConcurrentDictionary<int, int[]> botPath { get; private set; }
        public ConcurrentQueue<int[]> paths { get; private set; }

        public ConcurrentQueue<int> vialInput { get; private set; }
        public ConcurrentQueue<int> vialOutput { get; private set; }

        int vialAmmount = 12;

        int inUse0 = 0;
        int inUse1 = 0;
        int inUse2 = 0;
        int inUse3 = 0;

        public void reset_input()
        {
            Interlocked.Exchange(ref vialAmmount, 12);
        }
        public bool queue_handler(List<int> stations)
        {
            if (vialAmmount == 0)
            {
                return false;
            }
            Interlocked.Decrement(ref vialAmmount);

            int[] stationsarr = stations.ToArray();

            if (paths == null)
            {
                paths = new ConcurrentQueue<int[]>();
            }

            paths.Enqueue(stationsarr);

            return true;

            /*
            List<int> path = new List<int>();

            path.Add(xBotIDs[bot]);

            for (int i = 0; i < stationsarr.Length; i++)
            {
                path.Add(stationsarr[i]);
            }

            if (botPath == null)
            {
                botPath = new ConcurrentDictionary<int, List<int>>();
            }

            botPath.AddOrUpdate(bot, path, (key, oldValue) => path);
            */
        }
        public void initHighway()
        {
            vial_handler.startup();

            C = new ConcurrentDictionary<int, PointF>();

            C.TryAdd(0, new PointF(0.300f, 0.300f));
            C.TryAdd(1, new PointF(0.300f, 0.660f));
            C.TryAdd(2, new PointF(0.420f, 0.660f));
            C.TryAdd(3, new PointF(0.420f, 0.300f));

            D = new ConcurrentDictionary<int, PointF>();

            D.TryAdd(0, new PointF(0.300f, 0.300f));
            D.TryAdd(1, new PointF(0.300f, 0.660f));
            D.TryAdd(2, new PointF(0.420f, 0.660f));
            D.TryAdd(3, new PointF(0.420f, 0.300f));

            Q = new ConcurrentDictionary<int, PointF>();

            Q.TryAdd(0, new PointF(0.180f, 0.060f));
            Q.TryAdd(1, new PointF(0.300f, 0.060f));
            Q.TryAdd(2, new PointF(0.420f, 0.060f));
            Q.TryAdd(3, new PointF(0.540f, 0.060f));
            Q.TryAdd(4, new PointF(0.660f, 0.060f));
            Q.TryAdd(5, new PointF(0.660f, 0.180f));

            int[] xBotIDs = GetIds();

            if (bots == null)
            {
                bots = new ConcurrentDictionary<int, int>();
            }

            for (int i = 2; i < xBotIDs.Length; i++)
            {
                bots.TryAdd(i-2, xBotIDs[i]);
            }

            if (botPath == null)
            {
                botPath = new ConcurrentDictionary<int, int[]>();
            }

            if (paths == null)
            {
                paths = new ConcurrentQueue<int[]>();
            }

            if (vialInput == null)
            {
                vialInput = new ConcurrentQueue<int>();
            }

            if (vialOutput == null)
            {
                vialOutput = new ConcurrentQueue<int>();
            }

            for (int i = 0; i < 12; i++)
            {
                vialInput.Enqueue(i + 1);
                vialOutput.Enqueue(i + 1);
            }

            Thread assignments = new Thread(new ThreadStart(assignment_handler));
            assignments.Name = String.Format($"assigment handler");
            assignments.IsBackground = true;
            assignments.Start();

            for (int i = 2; i < xBotIDs.Length; i++)
            {
                Thread botTask = new Thread(new ThreadStart(path_handler));
                botTask.Name = String.Format($"{i - 2}");
                botTask.IsBackground = true;
                botTask.Start();
            }
        }

        public void assignment_handler()
        {
            //int[] xBotIDs = GetIds();
            int[] next;
            while (true)
            {
                for (int i = 0; i < 4/*xBotIDs.Length - 2*/; i++)
                {
                    if (!botPath.ContainsKey(i))
                    {
                        if (paths.TryDequeue(out next))
                        {
                            botPath.TryAdd(i, next);
                        }
                    }
                }
            }
        }

        public void path_handler()
        {
            Thread trd = Thread.CurrentThread;
            string name = trd.Name;
            int Num = int.Parse(name);

            bool input = true;

            bool stationFinished = false;

            int lastStation = -1;

            int bot = bots[Num];

            MoveSingleBotToPosXY(bot, C[0]);

            if (!botPath.ContainsKey(Num))
            {
                MoveSingleBotToPosYX(bot, C[0]);
                bot_idle(Num, bot,lastStation);
            }

            int[] station = botPath[Num];

            /*
            int[] station = new int[path.Length];

            for (int i = 0; i < station.Length; i++)
            {
                station[i] = path[i + 1];
            }
            /*
            station[0] = path[1];
            station[1] = path[2];
            station[2] = path[3];
            */
            MoveSingleBotToPosXY(bot, C[0]);

            foreach (int i in station)
            {
                stationFinished = false;
                if (i < lastStation)
                {
                    if (lastStation < 1)
                    {
                        WaitSingleXbotIdle(bot);
                        MoveSingleBotToPosYX(bot, C[2]);
                    }
                    WaitSingleXbotIdle(bot);
                    MoveSingleBotToPosYX(bot, C[0]);
                    lastStation = -1;
                }

                while (!stationFinished)
                {
                    if (lastStation < 3)
                    {
                        WaitSingleXbotIdle(bot);
                        switch (i)
                        {
                            case 0:
                                if (lastStation == 0)
                                {
                                    loop(bot, i);
                                }
                                MoveSingleBotToPos(bot, C[0]);

                                if (0 == Interlocked.Exchange(ref inUse0, 1))
                                {
                                    station_handler(bot, 0, input);
                                    Interlocked.Exchange(ref inUse0, 0);
                                    stationFinished = true;
                                }
                                else
                                {
                                    loop(bot, i);
                                }
                                break;

                            case 1:
                                if (lastStation == 1)
                                {
                                    loop(bot, lastStation);
                                }
                                MoveSingleBotToPos(bot, C[1]);

                                if (0 == Interlocked.Exchange(ref inUse1, 1))
                                {
                                    station_handler(bot, 1, input);
                                    Interlocked.Exchange(ref inUse1, 0);
                                    if (input == true)
                                    {
                                        input = false;
                                    }
                                    else
                                    {
                                        input = true;
                                    }
                                    stationFinished = true;
                                }
                                else
                                {
                                    loop(bot, i);
                                }
                                break;

                            default:
                                if (lastStation == 2)
                                {
                                    MoveSingleBotToPosYX(bot, C[0]);
                                }
                                MoveSingleBotToPosYX(bot, C[2]);
                                break;
                        }
                    }

                    WaitSingleXbotIdle(bot);
                    switch (i)
                    {
                        case 2:
                            MoveSingleBotToPosYX(bot, C[2]);

                            if (0 == Interlocked.Exchange(ref inUse2, 1))
                            {
                                station_handler(bot, 2, input);
                                Interlocked.Exchange(ref inUse2, 0);
                                stationFinished = true;
                            }
                            else
                            {
                                loop(bot, i);
                            }
                            break;

                        case 3:
                            if (lastStation == 3)
                            {
                                MoveSingleBotToPosYX(bot, C[2]);
                            }
                            MoveSingleBotToPos(bot, C[3]);

                            if (0 == Interlocked.Exchange(ref inUse3, 1))
                            {
                                station_handler(bot, 3, input);
                                Interlocked.Exchange(ref inUse3, 0);
                                stationFinished = true;
                            }
                            else
                            {
                                loop(bot, i);
                            }
                            break;
                    }
                }
                lastStation = i;
            }

            int[] removed;
            botPath.TryRemove(Num, out removed);
            bot_idle(Num, bot, lastStation);

            if (lastStation < 1)
            {
                WaitSingleXbotIdle(bot);
                MoveSingleBotToPosYX(bot, C[2]);
            }

            if (lastStation != 3)
            {
                WaitSingleXbotIdle(bot);
                MoveSingleBotToPosXY(bot, C[3]);
                WaitSingleXbotIdle(bot);
            }


            /*
            int spot = (int)Interlocked.Read(ref fillQueue);
            Interlocked.Increment(ref fillQueue);
            MoveSingleBotToPosYX(bot, Q[spot]);
            */
        }

        public void loop(int bot, int station)
        {
            WaitSingleXbotIdle(bot);
            if (station < 2)
            {
                MoveSingleBotToPosYX(bot, C[2]);
                WaitSingleXbotIdle(bot);
            }
            MoveSingleBotToPosYX(bot, C[0]);
            WaitSingleXbotIdle(bot);
        }

        public void bot_idle(int Num, int bot, int station)
        {
            WaitSingleXbotIdle(bot);
            if (station < 2)
            {
                MoveSingleBotToPosYX(bot, C[2]);
                WaitSingleXbotIdle(bot);
            }
            MoveSingleBotToPosYX(bot, C[0]);
            WaitSingleXbotIdle(bot);

            while (!botPath.ContainsKey(Num))
            {
                MoveSingleBotToPosXY(bot, C[1]);
                WaitSingleXbotIdle(bot);
                MoveSingleBotToPosXY(bot, C[3]);
                WaitSingleXbotIdle(bot);
            }
            path_handler();
        }

        public void station_handler(int bot, int station, bool input)
        {
            WaitSingleXbotIdle(bot);
            switch (station)
            {
                //Station 1
                case 0:
                    liquid_handler.handle_liquid(bot);
                    WaitSingleXbotIdle(bot);
                    break;
                
                //Vial handler
                case 1:
                    int vial;

                    if (input)
                    {
                        vialInput.TryDequeue(out vial);
                        vial_handler.handle_vial(vial, bot, input);
                    }
                    else
                    {
                        vialOutput.TryDequeue(out vial);
                        vial_handler.handle_vial(vial, bot, input);
                    }
                    WaitSingleXbotIdle(bot);
                    break;

                //Station 2
                case 2:
                    mixing_handler.handle_mixing(bot, 1, 3, 8);
                    WaitSingleXbotIdle(bot);
                    break;

                //Station 3
                case 3:
                    cap_handler.handle_capping(bot);
                    WaitSingleXbotIdle(bot);
                    break;
            }
        }
    }
}
