using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser;
using Specialelektronik.Products.Sennheiser.SscLib;                          				// For Basic SIMPL# Classes

namespace Sennheiser_Modules_CSharp
{
    public class Chg4NSimplWrapper : SimplWrapperBase<Chg4N>
    {
        public StringDelegate SetGroupFb { get; set; }

        public UShortDelegateIndex SetBayActiveFb { get; set; }
        public UShortDelegateIndex SetBayChargingFb { get; set; }
        public StringDelegateIndex SetBaySerialFb { get; set; }
        public UShortDelegateIndex SetBayBatteryGauge { get; set; }
        public UShortDelegateIndex SetBayBatteryHealth { get; set; }
        public UShortDelegateIndex SetBayMinutesToFull { get; set; }
        public UShortDelegateIndex SetBayDeviceType { get; set; }

        public Chg4NSimplWrapper()
            : base(new Chg4N())
        {
            Device.DeviceHandler.Events += new EventHandler<Chg4NDeviceEventArgs>(DeviceHandler_Events);
            Device.BaysHandler.Events += new EventHandler<Chg4NBaysEventArgs>(BaysHandler_Events);
        }

        public void SetName(string value)
        {
            Device.DeviceHandler.Name = value;
        }
        public void SetGroup(string value)
        {
            Device.DeviceHandler.Group = value;
        }

        //Event handlers
        void DeviceHandler_Events(object sender, Chg4NDeviceEventArgs e)
        {
            switch (e.EventType)
            {
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Name:
                    SetNameFb(e.StringValue);
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Group:
                    SetGroupFb(e.StringValue);
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Version:
                    SetVersionFb(e.StringValue);
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Serial:
                    SetSerialFb(e.StringValue);
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Product:
                    SetProductFb(e.StringValue);
                    break;
                case Chg4NDeviceEventArgs.eChg4NDeviceEventType.MacAddresses:
                    SetMacAddressesFb(e.StringValue);
                    break;
            }
        }
        void BaysHandler_Events(object sender, Chg4NBaysEventArgs e)
        {
            switch (e.EventType)
            {
                case Chg4NBaysEventArgs.eChg4NBayEventType.Active:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayActiveFb(i, Convert.ToUInt16(e.Bays[i].Active));
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.Charging:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayChargingFb(i, Convert.ToUInt16(e.Bays[i].Charging));
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.Serial:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBaySerialFb(i, e.Bays[i].Serial);
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryGauge:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayBatteryGauge(i, (ushort)(e.Bays[i].BatteryGauge * 65535));
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryHealth:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayBatteryHealth(i, (ushort)(e.Bays[i].BatteryHealth * 65535));
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.MinutesToFull:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayMinutesToFull(i, (ushort)e.Bays[i].MinutesToFull);
                    break;
                case Chg4NBaysEventArgs.eChg4NBayEventType.DeviceType:
                    for (ushort i = 0; i < e.Bays.Length; i++)
                        SetBayDeviceType(i, (ushort)e.Bays[i].DeviceType);
                    break;
            }
        }
        
    }
}
