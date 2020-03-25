using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser;
using Specialelektronik.Products.Sennheiser.SscLib;                          				// For Basic SIMPL# Classes

namespace Sennheiser_Modules_CSharp
{
    public class Tcc2SimplWrapper : SimplWrapperBase<Tcc2>
    {
        public StringDelegate SetLocationFb { get; set; }
        public StringDelegate SetPositionFb { get; set; }
        public UShortDelegate SetIdentifyingFb { get; set; }
        public UShortDelegate SetCustomLedActiveFb { get; set; }
        public UShortDelegate SetCustomLedColorFb { get; set; }
        public UShortDelegate SetMicMuteLedColorFb { get; set; }
        public UShortDelegate SetMicOnLedColorFb { get; set; }
        public UShortDelegate SetLedBrightnessFb { get; set; }
        public UShortDelegate SetMuteFb { get; set; }
        public UShortDelegate SetExclusionZoneActiveFb { get; set; }
        public UShortDelegate SetDanteOutputGainFb { get; set; }
        public UShortDelegate SetSpeakerDetectionThresholdFb { get; set; }
        public StringDelegate SetDanteIpAddressesFb { get; set; }
        public StringDelegate SetDanteMacAddressesFb { get; set; }
        public UShortDelegate SetAzimuthFb { get; set; }
        public UShortDelegate SetElevationFb { get; set; }
        public ShortDelegate SetInputPeakLevelFb { get; set; }

        public Tcc2SimplWrapper()
            : base(new Tcc2())
        {
            Device.DeviceHandler.Events += new EventHandler<Tcc2DeviceEventArgs>(DeviceHandler_Events);
            Device.AudioHandler.Events += new EventHandler<Tcc2AudioEventArgs>(AudioHandler_Events);
            Device.MeterHandler.Events += new EventHandler<Tcc2MeterEventArgs>(MeterHandler_Events);
        }

        public void SetIdentify(ushort state)
        {
            Device.DeviceHandler.Identify = state > 0;
        }
        public void IdentifyToggle()
        {
            Device.DeviceHandler.Identify = !Device.DeviceHandler.Identify;
        }

        public void SetCustomLedActive(ushort state)
        {
            Device.DeviceHandler.CustomLedActive = state > 0;
        }
        public void CustomLedActiveToggle()
        {
            Device.DeviceHandler.CustomLedActive = !Device.DeviceHandler.CustomLedActive;
        }
        public void SetCustomLedColor(ushort value)
        {
            Device.DeviceHandler.CustomLedColor = GetColor(value);
        }
        public void SetMicMuteLedColor(ushort value)
        {
            Device.DeviceHandler.MicMuteLedColor = GetColor(value);
        }
        public void SetMicOnLedColor(ushort value)
        {
            Device.DeviceHandler.MicOnLedColor = GetColor(value);
        }
        public void SetLedBrightness(ushort value)
        {
            Device.DeviceHandler.LedBrightness = value;
        }

        public void SetMute(ushort state)
        {
            Device.AudioHandler.Mute = state > 0;
        }
        public void MuteToggle()
        {
            Device.AudioHandler.Mute = !Device.AudioHandler.Mute;
        }
        public void SetExclusionZoneActive(ushort state)
        {
            Device.AudioHandler.ExclusionZoneActive = state > 0;
        }
        public void ExclusionZoneActiveToggle()
        {
            Device.AudioHandler.ExclusionZoneActive = !Device.AudioHandler.ExclusionZoneActive;
        }

        public void SetDanteOutputGain(ushort value)
        {
            Device.AudioHandler.DanteOutputGain = value;
        }
        public void SetSpeakerDetectionThreshold(ushort value)
        {
            Device.AudioHandler.SpeakerDetectionThreshold = GetSpeakerDetectionThreshold(value);
        }

        public void SetName(string value)
        {
            Device.DeviceHandler.Name = value;
        }
        public void SetLocation(string value)
        {
            Device.DeviceHandler.Location = value;
        }
        public void SetPosition(string value)
        {
            Device.DeviceHandler.Position = value;
        }

        public void EnableAzimuthFeedback(ushort value)
        {
            if (value > 0)
                Device.MeterHandler.EnableAzimuthFeedback();
            else
                Device.MeterHandler.DisableAzimuthFeedback();
        }
        public void EnableElevationFeedback(ushort value)
        {
            if (value > 0)
                Device.MeterHandler.EnableElevationFeedback();
            else
                Device.MeterHandler.DisableElevationFeedback();
        }
        public void EnableInputPeakLevelFeedback(ushort value)
        {
            if (value > 0)
                Device.MeterHandler.EnableInputPeakLevelFeedback();
            else
                Device.MeterHandler.DisableInputPeakLevelFeedback();
        }


        Tcc2DeviceHandler.eLedColor GetColor(ushort value)
        {
            switch (value)
            {
                case 0:
                    return Tcc2DeviceHandler.eLedColor.LightGreen;
                case 1:
                    return Tcc2DeviceHandler.eLedColor.Green;
                case 2:
                    return Tcc2DeviceHandler.eLedColor.Blue;
                case 3:
                    return Tcc2DeviceHandler.eLedColor.Red;
                case 4:
                    return Tcc2DeviceHandler.eLedColor.Yellow;
                case 5:
                    return Tcc2DeviceHandler.eLedColor.Orange;
                case 6:
                    return Tcc2DeviceHandler.eLedColor.Cyan;
                case 7:
                    return Tcc2DeviceHandler.eLedColor.Pink;
                default:
                    return Tcc2DeviceHandler.eLedColor.Unknown;
            }
        }
        ushort GetColor(Tcc2DeviceHandler.eLedColor value)
        {
            switch (value)
            {
                case Tcc2DeviceHandler.eLedColor.LightGreen:
                    return 0;
                case Tcc2DeviceHandler.eLedColor.Green:
                    return 1;
                case Tcc2DeviceHandler.eLedColor.Blue:
                    return 2;
                case Tcc2DeviceHandler.eLedColor.Red:
                    return 3;
                case Tcc2DeviceHandler.eLedColor.Yellow:
                    return 4;
                case Tcc2DeviceHandler.eLedColor.Orange:
                    return 5;
                case Tcc2DeviceHandler.eLedColor.Cyan:
                    return 6;
                case Tcc2DeviceHandler.eLedColor.Pink:
                    return 7;
                default:
                    return 0;
            }
        }
        Tcc2AudioHandler.eSpeakerDetectionThreshold GetSpeakerDetectionThreshold(ushort value)
        {
            switch (value)
            {
                case 0:
                    return Tcc2AudioHandler.eSpeakerDetectionThreshold.QuietRoom;
                case 1:
                    return Tcc2AudioHandler.eSpeakerDetectionThreshold.NormalRoom;
                case 2:
                    return Tcc2AudioHandler.eSpeakerDetectionThreshold.LoudRoom;
                default:
                    return Tcc2AudioHandler.eSpeakerDetectionThreshold.Unknown;
            }
        }
        ushort GetSpeakerDetectionThreshold(Tcc2AudioHandler.eSpeakerDetectionThreshold value)
        {
            switch (value)
            {
                case Tcc2AudioHandler.eSpeakerDetectionThreshold.QuietRoom:
                    return 0;
                case Tcc2AudioHandler.eSpeakerDetectionThreshold.NormalRoom:
                    return 1;
                case Tcc2AudioHandler.eSpeakerDetectionThreshold.LoudRoom:
                    return 2;
                default:
                    return 0;
            }
        }

        //Event handlers
        void DeviceHandler_Events(object sender, Tcc2DeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Name:
                    SetNameFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Location:
                    SetLocationFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Position:
                    SetPositionFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Version:
                    SetVersionFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Serial:
                    SetSerialFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Product:
                    SetProductFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MacAddresses:
                    SetMacAddressesFb(e.StringValue);
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Identify:
                    SetIdentifyingFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedColor:
                    SetCustomLedColorFb(GetColor(e.LedColor));
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedActive:
                    SetCustomLedActiveFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicMuteLedColor:
                    SetMicMuteLedColorFb(GetColor(e.LedColor));
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicOnLedColor:
                    SetMicOnLedColorFb(GetColor(e.LedColor));
                    break;
                case Tcc2DeviceEventArgs.eTcc2DeviceEventType.LedBrightness:
                    SetLedBrightnessFb((ushort)e.IntValue);
                    break;
            }
        }
        void AudioHandler_Events(object sender, Tcc2AudioEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2AudioEventArgs.eTcc2AudioEventType.ExclusionZoneActive:
                    SetExclusionZoneActiveFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteMacAddresses:
                    SetDanteMacAddressesFb(e.StringValue);
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteIpAddresses:
                    SetDanteIpAddressesFb(e.StringValue);
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteOutputGain:
                    SetDanteOutputGainFb((ushort)e.IntValue);
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.SpeakerDetectionThreshold:
                    SetSpeakerDetectionThresholdFb(GetSpeakerDetectionThreshold(e.SpeakerDetectionThreshold));
                    break;
                case Tcc2AudioEventArgs.eTcc2AudioEventType.Mute:
                    SetMuteFb(Convert.ToUInt16(e.BoolValue));
                    break;
            }
        }
        void MeterHandler_Events(object sender, Tcc2MeterEventArgs e)
        {
            switch (e.EventType)
            {
                case Tcc2MeterEventArgs.eTcc2MeterEventType.Elevation:
                    SetElevationFb((ushort)e.IntValue);
                    break;
                case Tcc2MeterEventArgs.eTcc2MeterEventType.Azimuth:
                    SetAzimuthFb((ushort)e.IntValue);
                    break;
                case Tcc2MeterEventArgs.eTcc2MeterEventType.InputPeakLevel:
                    SetInputPeakLevelFb((short)e.IntValue);
                    break;
            }
        }
    }
}
