using System;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    	// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using Specialelektronik.Products.Sennheiser;
using Specialelektronik.Products.Sennheiser.SscLib;


namespace Specialelektronik.Products.Sennheiser.Test
{
    public class ControlSystem : CrestronControlSystem
    {
        public static ControlSystem Instance { get; private set; }

        Xpanel _xpanel;

        public static Chg4N Charger;
        public static Sldw Receiver;
        public static SlMcrDw Mcr;
        public static Tcc2 Tcc;

        public ControlSystem()
            : base()
        {
            try
            {
                Instance = this;

                Thread.MaxNumberOfUserThreads = 20;
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControlSystem_ControllerProgramEventHandler);
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Exception in the constructor: {0}", ex);
            }
        }
        public override void InitializeSystem()
        {
            try
            {
                Charger = new Chg4N();
                Charger.Errors += new EventHandler<SscErrorEventArgs>(Errors);
                Charger.BaysHandler.Events += new EventHandler<Chg4NBaysEventArgs>(ChargerBaysHandler_Events);
                Charger.DeviceHandler.Events += new EventHandler<Chg4NDeviceEventArgs>(ChargerDeviceHandler_Events);

                Receiver = new Sldw();
                Receiver.Errors += new EventHandler<SscErrorEventArgs>(Errors);
                Receiver.DeviceHandler.Events += new EventHandler<SldwDeviceEventArgs>(ReceiverDeviceHandler_Events);
                Receiver.RxHandler.Events += new EventHandler<SldwRxEventArgs>(ReceiverRxHandler_Events);
                Receiver.TxHandler.Events += new EventHandler<SldwTxEventArgs>(ReceiverTxHandler_Events);
                Receiver.AudioHandler.Events += new EventHandler<SldwAudioEventArgs>(ReceiverAudioHandler_Events);
                Receiver.RxHandler.EnableRfQualityFeedback();

                Tcc = new Tcc2();
                Tcc.Errors += new EventHandler<SscErrorEventArgs>(Errors);
                Tcc.DeviceHandler.Events += new EventHandler<Tcc2DeviceEventArgs>(TccDeviceHandler_Events);
                Tcc.MeterHandler.Events += new EventHandler<Tcc2MeterEventArgs>(TccMeterHandler_Events);
                Tcc.AudioHandler.Events += new EventHandler<Tcc2AudioEventArgs>(TccAudioHandler_Events);
                Tcc.MeterHandler.EnableAzimuthFeedback();
                Tcc.MeterHandler.EnableElevationFeedback();
                Tcc.MeterHandler.EnableInputPeakLevelFeedback();

                Mcr = new SlMcrDw(4);
                Mcr.Errors += new EventHandler<SscErrorEventArgs>(Errors);
                Mcr.DeviceHandler.Events += new EventHandler<SlMcrDwDeviceEventArgs>(McrDeviceHandler_Events);
                Mcr.AudioHandler.Events += new EventHandler<SlMcrDwAudioEventArgs>(McrAudioHandler_Events);
                Mcr.MeterHandler.Events += new EventHandler<SlMcrDwMeterEventArgs>(MeterHandler_Events);
                Mcr.MeterHandler.EnableMixerLevelFeedback();
                Mcr.MeterHandler.EnableRxInputLevelFeedbacks();
                foreach (var handler in Mcr.RxHandlers)
                {
                    handler.EnableRfQualityFeedback();
                    handler.Events += new EventHandler<SlMcrDwRxEventArgs>(McrRxHandler_Events);
                }
                foreach (var handler in Mcr.TxHandlers)
                    handler.Events += new EventHandler<SlMcrDwTxEventArgs>(McrTxHandler_Events);

                _xpanel = new Xpanel(0x03);

                Charger.Connect("192.168.9.27");
                Receiver.Connect("192.168.9.29");
                Tcc.Connect("192.168.10.133");
                Mcr.Connect("10.54.20.236");

                CrestronConsole.AddNewConsoleCommand(cmd =>
                {
                    if (cmd.ToLower() == "on")
                    {
                        //Charger.Debug = true;
                        //Receiver.Debug = true;
                        //Tcc.Debug = true;
                        Mcr.Debug = true;
                    }
                    else if (cmd.ToLower() == "off")
                    {
                        //Charger.Debug = false;
                        //Receiver.Debug = false;
                        //Tcc.Debug = false;
                        Mcr.Debug = false;
                    }
                }, "sennheiserdebug", "usage: sennheiserdebug <on/off>", ConsoleAccessLevelEnum.AccessOperator);
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Exception in InitializeSystem: {0}", ex);
            }
        }

        //Event handlers
        void Errors(object sender, SscErrorEventArgs e)
        {
            CrestronConsole.PrintLine(sender.ToString() + " Error: " + e.Json);
        }
        void ChargerDeviceHandler_Events(object sender, Chg4NDeviceEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void ChargerBaysHandler_Events(object sender, Chg4NBaysEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }

        void ReceiverDeviceHandler_Events(object sender, SldwDeviceEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void ReceiverRxHandler_Events(object sender, SldwRxEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void ReceiverTxHandler_Events(object sender, SldwTxEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void ReceiverAudioHandler_Events(object sender, SldwAudioEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }

        void TccDeviceHandler_Events(object sender, Tcc2DeviceEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void TccMeterHandler_Events(object sender, Tcc2MeterEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void TccAudioHandler_Events(object sender, Tcc2AudioEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }

        void McrDeviceHandler_Events(object sender, SlMcrDwDeviceEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void McrTxHandler_Events(object sender, SlMcrDwTxEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void McrRxHandler_Events(object sender, SlMcrDwRxEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void McrAudioHandler_Events(object sender, SlMcrDwAudioEventArgs e)
        {
            CrestronConsole.PrintLine(e.ToString());
        }
        void MeterHandler_Events(object sender, SlMcrDwMeterEventArgs e)
        {
            //CrestronConsole.PrintLine(e.ToString());
        }

        void ControlSystem_ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Stopping):
                    if (Charger != null)
                        Charger.Dispose();
                    if (Receiver != null)
                        Receiver.Dispose();
                    if (Tcc != null)
                        Tcc.Dispose();
                    if (Mcr != null)
                        Mcr.Dispose();
                    break;
            }

        }
    }
}