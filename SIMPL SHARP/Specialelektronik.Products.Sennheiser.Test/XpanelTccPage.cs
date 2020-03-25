using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Specialelektronik.Products.Sennheiser.Test
{
    public class XpanelTccPage
    {
        Tcc2 _tcc;
        XpanelForSmartGraphics _xpanel;

        public XpanelTccPage(XpanelForSmartGraphics xpanel)
        {
            _xpanel = xpanel;

            _tcc = ControlSystem.Tcc;
            _tcc.Responding += new EventHandler<SscRespondingEventArgs>(Device_Responding);
            _tcc.DeviceHandler.Events += new EventHandler<Tcc2DeviceEventArgs>(DeviceHandler_Events);
            _tcc.MeterHandler.Events += new EventHandler<Tcc2MeterEventArgs>(MeterHandler_Events);
            _tcc.AudioHandler.Events += new EventHandler<Tcc2AudioEventArgs>(AudioHandler_Events);
            _xpanel.SigChange += new SigEventHandler(_xpanel_SigChange);
        }

        //Event handlers
        void _xpanel_SigChange(BasicTriList currentDevice, Crestron.SimplSharpPro.SigEventArgs args)
        {
            if (args.Event == eSigEvent.BoolChange && args.Sig.BoolValue)
            {
                if (args.Sig.Number == 42)
                    _tcc.DeviceHandler.CustomLedActive = !_tcc.DeviceHandler.CustomLedActive;
                else if (args.Sig.Number >= 43 && args.Sig.Number <= 50)
                    _tcc.DeviceHandler.CustomLedColor = (Tcc2DeviceHandler.eLedColor)((args.Sig.Number - 43) + 1);
                else if (args.Sig.Number >= 51 && args.Sig.Number <= 58)
                    _tcc.DeviceHandler.MicMuteLedColor = (Tcc2DeviceHandler.eLedColor)((args.Sig.Number - 51) + 1);
                else if (args.Sig.Number >= 59 && args.Sig.Number <= 66)
                    _tcc.DeviceHandler.MicOnLedColor = (Tcc2DeviceHandler.eLedColor)((args.Sig.Number - 59) + 1);
                else if (args.Sig.Number == 67)
                    _tcc.DeviceHandler.Identify = !_tcc.DeviceHandler.Identify;
                else if (args.Sig.Number == 68)
                    _tcc.AudioHandler.Mute = !_tcc.AudioHandler.Mute;
                else if (args.Sig.Number == 69)
                    _tcc.AudioHandler.ExclusionZoneActive = !_tcc.AudioHandler.ExclusionZoneActive;
                else if (args.Sig.Number >= 70 && args.Sig.Number <= 72)
                    _tcc.AudioHandler.SpeakerDetectionThreshold = (Tcc2AudioHandler.eSpeakerDetectionThreshold)((args.Sig.Number - 70) + 1);
            }
            else if (args.Event == eSigEvent.UShortChange)
            {
                if (args.Sig.Number == 41)
                    _tcc.DeviceHandler.LedBrightness = (int)((args.Sig.UShortValue / 65535.0) * 5);
                if (args.Sig.Number == 45)
                    _tcc.AudioHandler.DanteOutputGain = args.Sig.UShortValue;
            }
        }
        void Device_Responding(object sender, SscRespondingEventArgs e)
        {
            _xpanel.BooleanInput[41].BoolValue = e.Responding;
        }
        void DeviceHandler_Events(object sender, Tcc2DeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Name:
                    _xpanel.StringInput[41].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Location:
                    _xpanel.StringInput[42].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Position:
                    _xpanel.StringInput[50].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Version:
                    _xpanel.StringInput[45].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Serial:
                    _xpanel.StringInput[44].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Product:
                    _xpanel.StringInput[43].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MacAddresses:
                    _xpanel.StringInput[46].StringValue = e.StringValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Identify:
                    _xpanel.BooleanInput[67].BoolValue = e.BoolValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedColor:
                    _xpanel.StringInput[47].StringValue = e.LedColor.ToString();
                    for (uint i = 0; i < 8; i++)
                        _xpanel.BooleanInput[43 + i].BoolValue = (int)e.LedColor == i + 1;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedActive:
                    _xpanel.BooleanInput[42].BoolValue = e.BoolValue;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicMuteLedColor:
                    _xpanel.StringInput[48].StringValue = e.LedColor.ToString();
                    for (uint i = 0; i < 8; i++)
                        _xpanel.BooleanInput[51 + i].BoolValue = (int)e.LedColor == i + 1;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicOnLedColor:
                    _xpanel.StringInput[49].StringValue = e.LedColor.ToString();
                    for (uint i = 0; i < 8; i++)
                        _xpanel.BooleanInput[59 + i].BoolValue = (int)e.LedColor == i + 1;
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.LedBrightness:
                    _xpanel.UShortInput[41].UShortValue = (ushort)((e.IntValue / 5.0) * 65535);
                    break;
            }
        }
        void MeterHandler_Events(object sender, Tcc2MeterEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2MeterEventArgs.eTcc2MeterEventType.Elevation:
                    _xpanel.UShortInput[43].UShortValue = (ushort)e.IntValue;
                    break;
                case Tcc2MeterEventArgs.eTcc2MeterEventType.Azimuth:
                    _xpanel.UShortInput[42].UShortValue = (ushort)e.IntValue;
                    break;
                case Tcc2MeterEventArgs.eTcc2MeterEventType.InputPeakLevel:
                    _xpanel.UShortInput[44].ShortValue = (short)e.IntValue;
                    break;
            }
        }
        void AudioHandler_Events(object sender, Tcc2AudioEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2AudioEventArgs.eTcc2AudioEventType.ExclusionZoneActive:
                    _xpanel.BooleanInput[69].BoolValue = e.BoolValue;
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteMacAddresses:
                    _xpanel.StringInput[51].StringValue = e.StringValue.Replace(",", "<br>"); ;
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteIpAddresses:
                    _xpanel.StringInput[52].StringValue = e.StringValue.Replace(",","<br>");
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteOutputGain:
                    _xpanel.UShortInput[45].UShortValue = (ushort)e.IntValue;
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.SpeakerDetectionThreshold:
                    _xpanel.StringInput[53].StringValue = e.SpeakerDetectionThreshold.ToString();
                    for (uint i = 0; i < 3; i++)
                        _xpanel.BooleanInput[70 + i].BoolValue = (int)e.SpeakerDetectionThreshold == i + 1;
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.Mute:
                    _xpanel.BooleanInput[68].BoolValue = e.BoolValue;
                    break;
            }
        }
    }
}