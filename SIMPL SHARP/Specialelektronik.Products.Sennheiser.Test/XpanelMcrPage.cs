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
    public class XpanelMcrPage
    {
        XpanelForSmartGraphics _xpanel;
        SlMcrDw _mcr;
        SlMcrDwRxHandler _rx;
        SlMcrDwTxHandler _tx;
        SlMcrDwRxAudio _audio;
        int _selectedIndex;

        const int _minDb = -24;

        public XpanelMcrPage(XpanelForSmartGraphics xpanel)
        {
            _xpanel = xpanel;
            _xpanel.SigChange += new SigEventHandler(_xpanel_SigChange);

            _mcr = ControlSystem.Mcr;
            _mcr.Responding += new EventHandler<SscRespondingEventArgs>(_receiver_Responding);
            _mcr.DeviceHandler.Events += new EventHandler<SlMcrDwDeviceEventArgs>(DeviceHandler_Events);
            _mcr.AudioHandler.Events += new EventHandler<SlMcrDwAudioEventArgs>(AudioHandler_Events);
            _mcr.MeterHandler.Events += new EventHandler<SlMcrDwMeterEventArgs>(MeterHandler_Events);
            SelectReceiver(0);
        }

        void SelectReceiver(int index)
        {
            if (_rx != null)
            {
                _rx.Events -= new EventHandler<SlMcrDwRxEventArgs>(RxHandler_Events);
                _tx.Events -= new EventHandler<SlMcrDwTxEventArgs>(TxHandler_Events);    
            }

            _selectedIndex = index;
            _rx = _mcr.RxHandlers[index];
            _tx = _mcr.TxHandlers[index];
            _audio = _mcr.AudioHandler.RxAudio[index];

            _rx.Events += new EventHandler<SlMcrDwRxEventArgs>(RxHandler_Events);
            _tx.Events += new EventHandler<SlMcrDwTxEventArgs>(TxHandler_Events);

            for (int i = 0; i < 4; i++)
                _xpanel.BooleanInput[(uint)(151 + i)].BoolValue = i == index;

            UpdateReceiverValues();
        }

        void UpdateReceiverValues()
        {
            _xpanel.BooleanInput[162].BoolValue = _rx.Identify;
            _xpanel.UShortInput[162].UShortValue = (ushort)(_rx.RfQuality * 65535);
            _xpanel.StringInput[168].StringValue = _rx.Warnings;
            _xpanel.StringInput[169].StringValue = _rx.Rfpi;
            _xpanel.StringInput[170].StringValue = _rx.LastPairedIpei;

            _xpanel.BooleanInput[163].BoolValue = _tx.Active;
            _xpanel.StringInput[173].StringValue = _tx.DeviceType.ToString();
            _xpanel.StringInput[172].StringValue = _tx.BatteryType.ToString();
            _xpanel.BooleanInput[164].BoolValue = _tx.Charging;
            _xpanel.UShortInput[163].UShortValue = (ushort)(_tx.BatteryGauge * 65535);
            _xpanel.UShortInput[164].UShortValue = (ushort)(_tx.BatteryHealth * 65535);
            _xpanel.UShortInput[165].UShortValue = (ushort)_tx.BatteryLifetime;
            _xpanel.StringInput[171].StringValue = _tx.Warnings;

            _xpanel.StringInput[177].StringValue = _audio.OutputGain.ToString();
            for (uint i = 0; i < 7; i++)
                _xpanel.BooleanInput[173 + i].BoolValue = (int)_audio.OutputGain == (_minDb + i * 6);
            _xpanel.StringInput[178].StringValue = _audio.Eq.ToString();
            for (uint i = 0; i < 5; i++)
                _xpanel.BooleanInput[180 + i].BoolValue = (int)_audio.Eq == i;
            _xpanel.BooleanInput[165].BoolValue = _audio.LowCut;

            _xpanel.UShortInput[167].ShortValue = (short)_mcr.MeterHandler.RxInputLevels[_selectedIndex];
        }

        //Event handlers
        void _xpanel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            if (args.Event == eSigEvent.BoolChange && args.Sig.BoolValue)
            {
                if (args.Sig.Number >= 151 && args.Sig.Number <= 154)
                    SelectReceiver((int)args.Sig.Number - 151);
                else if (args.Sig.Number == 162)
                    _rx.Identify = !_rx.Identify;
                else if (args.Sig.Number == 165)
                    _audio.LowCut = !_audio.LowCut;
                else if (args.Sig.Number >= 166 && args.Sig.Number <= 172)
                    _mcr.AudioHandler.DanteOutputGain = (SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain)(_minDb + (args.Sig.Number - 166) * 6);
                else if (args.Sig.Number >= 173 && args.Sig.Number <= 179)
                    _audio.OutputGain = (SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain)(_minDb + (args.Sig.Number - 173) * 6);
                else if (args.Sig.Number >= 180 && args.Sig.Number <= 184)
                    _audio.Eq = (SlMcrDwRxAudio.eSlMcrDwAudioEq)(args.Sig.Number - 180);
                else if (args.Sig.Number == 185)
                    _mcr.DeviceHandler.Identify = !_mcr.DeviceHandler.Identify;
            }
            else if (args.Event == eSigEvent.UShortChange)
            {
                if (args.Sig.Number == 161)
                    _mcr.DeviceHandler.LedBrightness = (int)((args.Sig.UShortValue / 65535.0) * 5);
            }
        }
        void _receiver_Responding(object sender, SscRespondingEventArgs e)
        {
            _xpanel.BooleanInput[161].BoolValue = e.Responding;
        }
        void DeviceHandler_Events(object sender, SlMcrDwDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Name:
                    _xpanel.StringInput[161].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Location:
                    _xpanel.StringInput[162].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Position:
                    _xpanel.StringInput[163].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Version:
                    _xpanel.StringInput[166].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Serial:
                    _xpanel.StringInput[165].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Product:
                    _xpanel.StringInput[164].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.MacAddresses:
                    _xpanel.StringInput[167].StringValue = e.StringValue;
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.LedBrightness:
                    _xpanel.UShortInput[161].UShortValue = (ushort)((e.IntValue / 5.0) * 65535);
                    break;
                case SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Identify:
                    _xpanel.BooleanInput[185].BoolValue = e.BoolValue;
                    break;
            }
        }
        void RxHandler_Events(object sender, SlMcrDwRxEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Identify:
                    _xpanel.BooleanInput[162].BoolValue = e.BoolValue;
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.RfQuality:
                    _xpanel.UShortInput[162].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Warnings:
                    _xpanel.StringInput[168].StringValue = e.StringValue;
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Rfpi:
                    _xpanel.StringInput[169].StringValue = e.StringValue;
                    break;
                case SlMcrDwRxEventArgs.eSlMcrDwRxEventType.LastPairedIpei:
                    _xpanel.StringInput[170].StringValue = e.StringValue;
                    break;
            }
        }
        void TxHandler_Events(object sender, SlMcrDwTxEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Active:
                    _xpanel.BooleanInput[163].BoolValue = e.BoolValue;
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.DeviceType:
                    _xpanel.StringInput[173].StringValue = e.DeviceType.ToString();
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryType:
                    _xpanel.StringInput[172].StringValue = e.BatteryType.ToString();
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Charging:
                    _xpanel.BooleanInput[164].BoolValue = e.BoolValue;
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryGauge:
                    _xpanel.UShortInput[163].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryHealth:
                    _xpanel.UShortInput[164].UShortValue = (ushort)(e.DoubleValue * 65535);
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryLifetime:
                    _xpanel.UShortInput[165].UShortValue = (ushort)e.DoubleValue;
                    break;
                case SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Warnings:
                    _xpanel.StringInput[171].StringValue = e.StringValue;
                    break;
            }
        }
        void AudioHandler_Events(object sender, SlMcrDwAudioEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteMacAddresses:
                    _xpanel.StringInput[174].StringValue = e.StringValue.Replace(",", "<br>"); ;
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteIpAddresses:
                    _xpanel.StringInput[175].StringValue = e.StringValue.Replace(",", "<br>");
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteOutputGain:
                    _xpanel.StringInput[176].StringValue = e.DanteOutputGain.ToString();
                    for (uint i = 0; i < 7; i++)
                        _xpanel.BooleanInput[166 + i].BoolValue = (int)e.DanteOutputGain == (_minDb + i * 6);
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxOutputGain:
                    _xpanel.StringInput[177].StringValue = e.RxAudio.OutputGain.ToString();
                    for (uint i = 0; i < 7; i++)
                        _xpanel.BooleanInput[173 + i].BoolValue = (int)e.RxAudio.OutputGain == (_minDb + i * 6);
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxEq:
                    _xpanel.StringInput[178].StringValue = e.RxAudio.Eq.ToString();
                    for (uint i = 0; i < 5; i++)
                        _xpanel.BooleanInput[180 + i].BoolValue = (int)e.RxAudio.Eq == i;
                    break;
                case SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxLowCut:
                    _xpanel.BooleanInput[165].BoolValue = e.RxAudio.LowCut;
                    break;
            }
        }
        void MeterHandler_Events(object sender, SlMcrDwMeterEventArgs e)
        {
            switch (e.EventType)
            {
                case SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.RxInputLevel:
                    if (e.Index == _selectedIndex)
                        _xpanel.UShortInput[167].ShortValue = (short)e.IntValue;
                    break;
                case SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.MixerLevel:
                    _xpanel.UShortInput[166].ShortValue = (short)e.IntValue;
                    break;
            }
        }
    }
}