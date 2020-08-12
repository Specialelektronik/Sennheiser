using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.UI;
using Specialelektronik.Products.Sennheiser.SscLib;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace Specialelektronik.Products.Sennheiser.Test
{
    public class XpanelReceiverPage
    {
        XpanelForSmartGraphics _xpanel;
        Sldw _receiver;

        public XpanelReceiverPage(XpanelForSmartGraphics xpanel)
        {
            _xpanel = xpanel;
            _xpanel.SigChange += new SigEventHandler(_xpanel_SigChange);

            _receiver = ControlSystem.Receiver;
            _receiver.Responding += new EventHandler<SscRespondingEventArgs>(_receiver_Responding);
            _receiver.DeviceHandler.Events += new EventHandler<SldwDeviceEventArgs>(DeviceHandler_Events);
            _receiver.RxHandler.Events += new EventHandler<SldwRxEventArgs>(RxHandler_Events);
            _receiver.TxHandler.Events += new EventHandler<SldwTxEventArgs>(TxHandler_Events);
            _receiver.AudioHandler.Events += new EventHandler<SldwAudioEventArgs>(AudioHandler_Events);
        }

        

        //Event handlers
        void _xpanel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            if (args.Event == eSigEvent.BoolChange && args.Sig.BoolValue)
            {
                if (args.Sig.Number == 12)
                    _receiver.RxHandler.Identify = !_receiver.RxHandler.Identify;
                else if (args.Sig.Number == 13)
                    _receiver.RxHandler.MuteSwitchActive = !_receiver.RxHandler.MuteSwitchActive;
                else if (args.Sig.Number == 22)
                    _receiver.AudioHandler.LowCut = !_receiver.AudioHandler.LowCut;
                else if (args.Sig.Number >= 23 && args.Sig.Number <= 29)
                    _receiver.AudioHandler.OutputGain = (SldwAudioHandler.eSldwAudioOutputGain)(args.Sig.Number - 23);
                else if (args.Sig.Number >= 30 && args.Sig.Number <= 34)
                    _receiver.AudioHandler.Eq = (SldwAudioHandler.eSldwAudioEq)(args.Sig.Number - 30);
            }
            else if (args.Event == eSigEvent.UShortChange)
            {
                if (args.Sig.Number == 15)
                    _receiver.DeviceHandler.Brightness = args.Sig.UShortValue / 65535.0;
            }
        }
        void _receiver_Responding(object sender, SscRespondingEventArgs e)
        {
            _xpanel.BooleanInput[11].BoolValue = e.Responding;
        }
        void DeviceHandler_Events(object sender, SldwDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwDeviceEventArgs.eSldwDeviceEventType.Name:
                    _xpanel.StringInput[11].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Group:
                    _xpanel.StringInput[12].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Version:
                    _xpanel.StringInput[15].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Serial:
                    _xpanel.StringInput[14].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Product:
                    _xpanel.StringInput[13].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.MacAddresses:
                    _xpanel.StringInput[16].StringValue = e.StringValue;
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Brightness:
                    _xpanel.UShortInput[15].UShortValue = (ushort)(e.Brightness * 65535);
                    break;
            }
        }
        void RxHandler_Events(object sender, SldwRxEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwRxEventArgs.eSldwRxEventType.Identify:
                    _xpanel.BooleanInput[12].BoolValue = e.BoolValue;
                    break;
                case SldwRxEventArgs.eSldwRxEventType.RfQuality:
                    _xpanel.UShortInput[11].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SldwRxEventArgs.eSldwRxEventType.MuteSwitchActive:
                    _xpanel.BooleanInput[13].BoolValue = e.BoolValue;
                    break;
                case SldwRxEventArgs.eSldwRxEventType.Warnings:
                    _xpanel.StringInput[20].StringValue = e.StringValue;
                    break;
                case SldwRxEventArgs.eSldwRxEventType.Rfpi:
                    _xpanel.StringInput[24].StringValue = e.StringValue;
                    break;
                case SldwRxEventArgs.eSldwRxEventType.LastPairedIpei:
                    _xpanel.StringInput[25].StringValue = e.StringValue;
                    break;
            }
        }
        void TxHandler_Events(object sender, SldwTxEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwTxEventArgs.eSldwTxEventType.Active:
                    _xpanel.BooleanInput[20].BoolValue = e.BoolValue;
                    break;
                case SldwTxEventArgs.eSldwTxEventType.DeviceType:
                    _xpanel.StringInput[19].StringValue = e.DeviceType.ToString();
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryType:
                    _xpanel.StringInput[18].StringValue = e.BatteryType.ToString();
                    break;
                case SldwTxEventArgs.eSldwTxEventType.Charging:
                    _xpanel.BooleanInput[21].BoolValue = e.BoolValue;
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryGauge:
                    _xpanel.UShortInput[12].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryHealth:
                    _xpanel.UShortInput[13].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryLifetime:
                    _xpanel.UShortInput[14].UShortValue = (ushort)e.DoubleValue;
                    break;
                case SldwTxEventArgs.eSldwTxEventType.Warnings:
                    _xpanel.StringInput[21].StringValue = e.StringValue;
                    break;
            }
        }
        void AudioHandler_Events(object sender, SldwAudioEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwAudioEventArgs.eSldwAudioEventType.OutputGain:
                    _xpanel.StringInput[22].StringValue = e.OutputGain.ToString();
                    for (uint i = 0; i < 7; i++)
                        _xpanel.BooleanInput[23 + i].BoolValue = (int)e.OutputGain == i;
                    break;
                case SldwAudioEventArgs.eSldwAudioEventType.Eq:
                    _xpanel.StringInput[23].StringValue = e.Eq.ToString();
                    for (uint i = 0; i < 5; i++)
                        _xpanel.BooleanInput[30 + i].BoolValue = (int)e.Eq == i;
                    break;
                case SldwAudioEventArgs.eSldwAudioEventType.LowCut:
                    _xpanel.BooleanInput[22].BoolValue = e.LowCut;
                    break;
            }
        }
    }
}