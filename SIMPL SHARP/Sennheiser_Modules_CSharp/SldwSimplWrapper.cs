using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser;
using Specialelektronik.Products.Sennheiser.SscLib;                          				// For Basic SIMPL# Classes

namespace Sennheiser_Modules_CSharp
{
    public class SldwSimplWrapper : SimplWrapperBase<Sldw>
    {
        public StringDelegate SetGroupFb { get; set; }
        public UShortDelegate SetBrightnessFb { get; set; }

        public UShortDelegate SetIdentifyingFb { get; set; }
        public UShortDelegate SetRfQualityFb { get; set; }
        public UShortDelegate SetMuteSwitchActiveFb { get; set; }
        public StringDelegate SetRxWarningsFb { get; set; }
        public StringDelegate SetRfpiFb { get; set; }
        public StringDelegate SetLastPairedIpeiFb { get; set; }

        public UShortDelegate SetTxActiveFb { get; set; }
        public UShortDelegate SetTxDeviceTypeFb { get; set; }
        public UShortDelegate SetTxBatteryTypeFb { get; set; }
        public UShortDelegate SetTxChargingFb { get; set; }
        public UShortDelegate SetTxBatteryGaugeFb { get; set; }
        public UShortDelegate SetTxBatteryHealthFb { get; set; }
        public UShortDelegate SetTxBatteryLifetimeFb { get; set; }
        public StringDelegate SetTxWarningsFb { get; set; }

        public UShortDelegate SetOutputGainFb { get; set; }
        public UShortDelegate SetEqFb { get; set; }
        public UShortDelegate SetLowCutFb { get; set; }

        public SldwSimplWrapper()
            : base(new Sldw())
        {
            Device.DeviceHandler.Events += new EventHandler<SldwDeviceEventArgs>(DeviceHandler_Events);
            Device.RxHandler.Events += new EventHandler<SldwRxEventArgs>(RxHandler_Events);
            Device.TxHandler.Events += new EventHandler<SldwTxEventArgs>(TxHandler_Events);
            Device.AudioHandler.Events += new EventHandler<SldwAudioEventArgs>(AudioHandler_Events);
        }

        public void SetIdentify(ushort state) 
        {
            Device.RxHandler.Identify = state > 0;
        }
        public void IdentifyToggle()
        {
            Device.RxHandler.Identify = !Device.RxHandler.Identify;
        }
        public void SetMuteSwitchActive(ushort state)
        {
            Device.RxHandler.MuteSwitchActive = state > 0;
        }
        public void MuteSwitchActiveToggle()
        {
            Device.RxHandler.MuteSwitchActive = !Device.RxHandler.MuteSwitchActive;
        }
        public void SetLowCut(ushort state)
        {
            Device.AudioHandler.LowCut = state > 0;
        }
        public void LowCutToggle()
        {
            Device.AudioHandler.LowCut = !Device.AudioHandler.LowCut;
        }
        public void SetOutputGain(ushort value)
        {
            Device.AudioHandler.OutputGain = (SldwAudioHandler.eSldwAudioOutputGain)value;
        }
        public void SetEq(ushort value)
        {
            Device.AudioHandler.Eq = (SldwAudioHandler.eSldwAudioEq)value;
        }
        public void SetBrightness(ushort value)
        {
            Device.DeviceHandler.Brightness = value / 65535.0;
        }

        public void SetName(string value)
        {
            Device.DeviceHandler.Name = value;
        }
        public void SetGroup(string value)
        {
            Device.DeviceHandler.Group = value;
        }

        public void EnableRfQualityFeedback(ushort state)
        {
            if (state > 0)
                Device.RxHandler.EnableRfQualityFeedback();
            else
                Device.RxHandler.DisableRfQualityFeedback();
        }

        //Event handlers
        void DeviceHandler_Events(object sender, SldwDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwDeviceEventArgs.eSldwDeviceEventType.Name:
                    SetNameFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Group:
                    SetGroupFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Version:
                    SetVersionFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Serial:
                    SetSerialFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Product:
                    SetProductFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.MacAddresses:
                    SetMacAddressesFb(e.StringValue);
                    break;
                case SldwDeviceEventArgs.eSldwDeviceEventType.Brightness:
                    SetBrightnessFb((ushort)(e.Brightness * 65535));
                    break;
            }
        }
        void RxHandler_Events(object sender, SldwRxEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwRxEventArgs.eSldwRxEventType.Identify:
                    SetIdentifyingFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case SldwRxEventArgs.eSldwRxEventType.RfQuality:
                    SetRfQualityFb((ushort)(e.DoubleValue * 65535));
                    break;
                case SldwRxEventArgs.eSldwRxEventType.MuteSwitchActive:
                    SetMuteSwitchActiveFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case SldwRxEventArgs.eSldwRxEventType.Warnings:
                    SetRxWarningsFb(e.StringValue);
                    break;
                case SldwRxEventArgs.eSldwRxEventType.Rfpi:
                    SetRfpiFb(e.StringValue);
                    break;
                case SldwRxEventArgs.eSldwRxEventType.LastPairedIpei:
                    SetLastPairedIpeiFb(e.StringValue);
                    break;
            }
        }
        void TxHandler_Events(object sender, SldwTxEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwTxEventArgs.eSldwTxEventType.Active:
                    SetTxActiveFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case SldwTxEventArgs.eSldwTxEventType.DeviceType:
                    SetTxDeviceTypeFb((ushort)e.DeviceType);
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryType:
                    SetTxBatteryTypeFb((ushort)e.BatteryType);
                    break;
                case SldwTxEventArgs.eSldwTxEventType.Charging:
                    SetTxChargingFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryGauge:
                    SetTxBatteryGaugeFb((ushort)(e.DoubleValue * 65535));
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryHealth:
                    SetTxBatteryHealthFb((ushort)(e.DoubleValue * 65535));
                    break;
                case SldwTxEventArgs.eSldwTxEventType.BatteryLifetime:
                    SetTxBatteryLifetimeFb((ushort)(e.DoubleValue));
                    break;
                case SldwTxEventArgs.eSldwTxEventType.Warnings:
                    SetTxWarningsFb(e.StringValue);
                    break;
            }
        }
        void AudioHandler_Events(object sender, SldwAudioEventArgs e)
        {
            switch (e.EventType)
            {
                case SldwAudioEventArgs.eSldwAudioEventType.OutputGain:
                    SetOutputGainFb((ushort)e.OutputGain);
                    break;
                case SldwAudioEventArgs.eSldwAudioEventType.Eq:
                    SetEqFb((ushort)e.Eq);
                    break;
                case SldwAudioEventArgs.eSldwAudioEventType.LowCut:
                    SetLowCutFb(Convert.ToUInt16(e.LowCut));
                    break;
            }
        }
    }
}
