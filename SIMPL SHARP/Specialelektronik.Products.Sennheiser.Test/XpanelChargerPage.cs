using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using Crestron.SimplSharpPro.UI;

namespace Specialelektronik.Products.Sennheiser.Test
{
    public class XpanelChargerPage
    {
        Chg4N _charger;
        XpanelForSmartGraphics _xpanel;

        public XpanelChargerPage(XpanelForSmartGraphics xpanel)
        {
            _xpanel = xpanel;

            _charger = ControlSystem.Charger;
            _charger.Responding += new EventHandler<SscRespondingEventArgs>(_charger_Responding);
            _charger.BaysHandler.Events += new EventHandler<Chg4NBaysEventArgs>(ChargerBaysHandler_Events);
            _charger.DeviceHandler.Events += new EventHandler<Chg4NDeviceEventArgs>(ChargerDeviceHandler_Events);
        }

        void _charger_Responding(object sender, SscRespondingEventArgs e)
        {
            _xpanel.BooleanInput[1].BoolValue = e.Responding;
        }
        void ChargerDeviceHandler_Events(object sender, Chg4NDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Name:
                    _xpanel.StringInput[1].StringValue = e.StringValue;
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Group:
                    _xpanel.StringInput[2].StringValue = e.StringValue;
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Version:
                    _xpanel.StringInput[5].StringValue = e.StringValue;
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Serial:
                    _xpanel.StringInput[4].StringValue = e.StringValue;
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Product:
                    _xpanel.StringInput[3].StringValue = e.StringValue;
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.MacAddresses:
                    _xpanel.StringInput[6].StringValue = e.StringValue;
                    break;
            }
        }
        void ChargerBaysHandler_Events(object sender, Chg4NBaysEventArgs e)
        {
            switch (e.EventType)
            {
                case Chg4NBaysEventArgs.eChg4NBayEventType.Active:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.BooleanInput[(uint)(101 + i * 10)].BoolValue = e.Bays[i].Active;
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.Charging:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.BooleanInput[(uint)(102 + i * 10)].BoolValue = e.Bays[i].Charging;
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.Serial:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.StringInput[(uint)(102 + i * 10)].StringValue = e.Bays[i].Serial;
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryGauge:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.UShortInput[(uint)(101 + i * 10)].UShortValue = (ushort)(e.Bays[i].BatteryGauge * 65535);
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryHealth:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.UShortInput[(uint)(102 + i * 10)].UShortValue = (ushort)(e.Bays[i].BatteryHealth * 65535);
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.MinutesToFull:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.UShortInput[(uint)(103 + i * 10)].UShortValue = (ushort)(e.Bays[i].MinutesToFull);
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.DeviceType:
                    for (int i = 0; i < _charger.BaysHandler.Bays.Length; i++)
                        _xpanel.StringInput[(uint)(103 + i * 10)].StringValue = e.Bays[i].DeviceType.ToString();
                    break;
            }
        }
    }
}