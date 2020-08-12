using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser;

namespace Sennheiser_Modules_CSharp
{
    public class SlMcrDwSimplWrapper : SimplWrapperBase<SlMcrDw>
    {
        public StringDelegate SetLocationFb { get; set; }
        public StringDelegate SetPositionFb { get; set; }
        public UShortDelegate SetLedBrightnessFb { get; set; }
        public UShortDelegate SetIdentifyingFb { get; set; }
        public UShortDelegate SetDanteOutputGainFb { get; set; }
        public StringDelegate SetDanteIpAddressesFb { get; set; }
        public StringDelegate SetDanteMacAddressesFb { get; set; }
        public StringDelegateIndex SetRxWarningsFb { get; set; }
        public StringDelegateIndex SetRfpiFb { get; set; }
        public StringDelegateIndex SetLastPairedIpeiFb { get; set; }
        public UShortDelegateIndex SetRfQualityFb { get; set; }
        public UShortDelegateIndex SetRxIdentifyingFb { get; set; }
        public UShortDelegateIndex SetTxActiveFb { get; set; }
        public UShortDelegateIndex SetTxDeviceTypeFb { get; set; }
        public UShortDelegateIndex SetTxBatteryTypeFb { get; set; }
        public UShortDelegateIndex SetTxChargingFb { get; set; }
        public UShortDelegateIndex SetTxBatteryGaugeFb { get; set; }
        public UShortDelegateIndex SetTxBatteryHealthFb { get; set; }
        public UShortDelegateIndex SetTxBatteryLifetimeFb { get; set; }
        public StringDelegateIndex SetTxWarningsFb { get; set; }
        public UShortDelegateIndex SetRxOutputGainFb { get; set; }
        public UShortDelegateIndex SetEqFb { get; set; }
        public UShortDelegateIndex SetLowCutFb { get; set; }
        public ShortDelegate SetMixerLevelFb { get; set; }
        public ShortDelegateIndex SetRxInputLevelFb { get; set; }

        public SlMcrDwSimplWrapper()
        {

        }

        public void SimplInit(ushort numberOfChannels)
        {
            Init(new SlMcrDw(numberOfChannels));
            Device.DeviceHandler.Events += new EventHandler<SlMcrDwDeviceEventArgs>(DeviceHandler_Events);
            Device.AudioHandler.Events += new EventHandler<SlMcrDwAudioEventArgs>(AudioHandler_Events);
            Device.MeterHandler.Events += new EventHandler<SlMcrDwMeterEventArgs>(MeterHandler_Events);
            foreach (var handler in Device.RxHandlers)
                handler.Events += new EventHandler<SlMcrDwRxEventArgs>(rxHandler_Events);
            foreach (var handler in Device.TxHandlers)
                handler.Events += new EventHandler<SlMcrDwTxEventArgs>(txHandler_Events);
        }

        public void SetIdentify(ushort state)
        {
            Device.DeviceHandler.Identify = state > 0;
        }
        public void IdentifyToggle()
        {
            Device.DeviceHandler.Identify = !Device.DeviceHandler.Identify;
        }

        public void SetLedBrightness(ushort value)
        {
            Device.DeviceHandler.LedBrightness = value;
        }

        public void SetRxIdentify(ushort index, ushort state)
        {
            if (index < Device.RxHandlers.Count)
                Device.RxHandlers[index].Identify = state > 0;
        }
        public void RxIdentifyToggle(ushort index)
        {
            if (index < Device.RxHandlers.Count)
                Device.RxHandlers[index].Identify = !Device.RxHandlers[index].Identify;
        }
        public void SetLowCut(ushort index, ushort state)
        {
            if (index < Device.AudioHandler.RxAudio.Count)
                Device.AudioHandler.RxAudio[index].LowCut = state > 0;
        }
        public void LowCutToggle(ushort index)
        {
            if (index < Device.AudioHandler.RxAudio.Count)
                Device.AudioHandler.RxAudio[index].LowCut = !Device.AudioHandler.RxAudio[index].LowCut;
        }
        public void SetOutputGain(ushort index, ushort value)
        {
            if (index < Device.AudioHandler.RxAudio.Count)
                Device.AudioHandler.RxAudio[index].OutputGain = GetOutputGain(value);
        }
        public void SetEq(ushort index, ushort value)
        {
            if (index < Device.AudioHandler.RxAudio.Count)
                Device.AudioHandler.RxAudio[index].Eq = (SlMcrDwRxAudio.eSlMcrDwAudioEq)value;
        }

        public void SetDanteOutputGain(ushort value)
        {
            Device.AudioHandler.DanteOutputGain = GetOutputGain(value);
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

        public void EnableRfQualityFeedback(ushort state)
        {
            if (state > 0)
                foreach (var handler in Device.RxHandlers)
                    handler.EnableRfQualityFeedback();
            else
                foreach (var handler in Device.RxHandlers)
                    handler.DisableRfQualityFeedback();
        }
        public void EnableRxInputLevelFeedback(ushort state)
        {
            if (state > 0)
                Device.MeterHandler.EnableRxInputLevelFeedbacks();
            else
                Device.MeterHandler.DisableRxInputLevelFeedbacks();
        }
        public void EnableMixerLevelFeedback(ushort state)
        {
            if (state > 0)
                Device.MeterHandler.EnableMixerLevelFeedback();
            else
                Device.MeterHandler.DisableMixerLevelFeedback();
        }

        ushort GetOutputGain(SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain gain)
        {
            switch (gain)
            {
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus24Db:
                    return 0;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus18Db:
                    return 1;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus12Db:
                    return 2;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus6Db:
                    return 3;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.ZeroDb:
                    return 4;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Plus6Db:
                    return 5;
                case SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Plus12Db:
                    return 6;
                default:
                    return 0;
            }
        }
        SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain GetOutputGain(ushort gain) 
        {
            switch (gain)
            {
                case 0:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus24Db;
                case 1:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus18Db;
                case 2:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus12Db;
                case 3:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Minus6Db;
                case 4:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.ZeroDb;
                case 5:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Plus6Db;
                case 6:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Plus12Db;
                default:
                    return SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain.Unknown;
            }
        }

        void DeviceHandler_Events(object sender, SlMcrDwDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Name:
                    SetNameFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Location:
                    SetLocationFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Position:
                    SetPositionFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Version:
                    SetVersionFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Serial:
                    SetSerialFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Product:
                    SetProductFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.MacAddresses:
                    SetMacAddressesFb(e.StringValue);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Identify:
                    SetIdentifyingFb(Convert.ToUInt16(e.BoolValue));
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.LedBrightness:
                    SetLedBrightnessFb((ushort)e.IntValue);
                    break;
            }
        }
        void txHandler_Events(object sender, SlMcrDwTxEventArgs e)
        {
            var index = (ushort)Device.TxHandlers.IndexOf((SlMcrDwTxHandler)sender);
            switch (e.EventType)
            {
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Active:
                    SetTxActiveFb(index, Convert.ToUInt16(e.BoolValue));
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.DeviceType:
                    SetTxDeviceTypeFb(index, (ushort)e.DeviceType);
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryType:
                    SetTxBatteryTypeFb(index, (ushort)e.BatteryType);
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Charging:
                    SetTxChargingFb(index, Convert.ToUInt16(e.BoolValue));
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryGauge:
                    SetTxBatteryGaugeFb(index, (ushort)(e.DoubleValue * 65535));
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryHealth:
                    SetTxBatteryHealthFb(index, (ushort)(e.DoubleValue * 65535));
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryLifetime:
                    SetTxBatteryLifetimeFb(index, (ushort)(e.DoubleValue));
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Warnings:
                    SetTxWarningsFb(index, e.StringValue);
                    break;
            }
        }
        void rxHandler_Events(object sender, SlMcrDwRxEventArgs e)
        {
            var index = (ushort)Device.RxHandlers.IndexOf((SlMcrDwRxHandler)sender);
            switch (e.EventType)
            {
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Identify:
                    SetRxIdentifyingFb(index, Convert.ToUInt16(e.BoolValue));
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.RfQuality:
                    SetRfQualityFb(index, (ushort)(e.DoubleValue * 65535));
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Warnings:
                    SetRxWarningsFb(index, e.StringValue);
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Rfpi:
                    SetRfpiFb(index, e.StringValue);
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.LastPairedIpei:
                    SetLastPairedIpeiFb(index, e.StringValue);
                    break;
            }
        }
        void AudioHandler_Events(object sender, SlMcrDwAudioEventArgs e)
        {
            ushort index = 0;
            if (Device.AudioHandler.RxAudio != null)
                index = (ushort)Device.AudioHandler.RxAudio.IndexOf((SlMcrDwRxAudio)e.RxAudio);

            switch (e.EventType)
            {
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteMacAddresses:
                    SetDanteMacAddressesFb(e.StringValue);
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteIpAddresses:
                    SetDanteIpAddressesFb(e.StringValue);
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteOutputGain:
                    SetDanteOutputGainFb(GetOutputGain(e.DanteOutputGain));
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxOutputGain:
                    SetRxOutputGainFb(index, GetOutputGain(e.RxAudio.OutputGain));
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxEq:
                    SetEqFb(index, (ushort)e.RxAudio.Eq);
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxLowCut:
                    SetLowCutFb(index, Convert.ToUInt16(e.RxAudio.LowCut));
                    break;
            }
        }
        void MeterHandler_Events(object sender, SlMcrDwMeterEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.RxInputLevel:
                    SetRxInputLevelFb((ushort)e.Index, (short)e.IntValue);
                    break;
                case SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.MixerLevel:
                    SetMixerLevelFb((short)e.IntValue);
                    break;
            }
        }
    }
}