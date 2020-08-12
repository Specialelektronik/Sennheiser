using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the base TxHandler class
    /// </summary>
    public class SlMcrDwTxEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSlMcrDwTxEventType
        {
            Active,
            DeviceType,
            BatteryType,
            Charging,
            BatteryGauge,
            BatteryHealth,
            BatteryLifetime,
            Warnings,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSlMcrDwTxEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types Active and Charging.
        /// </summary>
        public bool BoolValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types BatteryGauge, BatteryHealth and BatteryLifetime.
        /// </summary>
        public double DoubleValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type Warnings.
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type DeviceType.
        /// </summary>
        public SlMcrDwTxHandler.eSlMcrDwTxDeviceType DeviceType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type BatteryType.
        /// </summary>
        public SlMcrDwTxHandler.eSlMcrDwTxBatteryType BatteryType { get; private set; }

        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SlMcrDwTxEventArgs(eSlMcrDwTxEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SlMcrDwTxEventArgs(eSlMcrDwTxEventType type, double value)
        {
            EventType = type;
            DoubleValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SlMcrDwTxEventArgs(eSlMcrDwTxEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SlMcrDwTxEventArgs(eSlMcrDwTxEventType type, SlMcrDwTxHandler.eSlMcrDwTxDeviceType value)
        {
            EventType = type;
            DeviceType = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SlMcrDwTxEventArgs(eSlMcrDwTxEventType type, SlMcrDwTxHandler.eSlMcrDwTxBatteryType value)
        {
            EventType = type;
            BatteryType = value;
        }

        /// <summary>
        /// Returns the event printed as "DeviceEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSlMcrDwTxEventType.Active:
                case eSlMcrDwTxEventType.Charging:
                    return String.Format("SlMcrDwTxEventArgs - {0}: {1}", EventType, BoolValue);
                case eSlMcrDwTxEventType.DeviceType:
                    return String.Format("SlMcrDwTxEventArgs - {0}: {1}", EventType, DeviceType);
                case eSlMcrDwTxEventType.BatteryType:
                    return String.Format("SlMcrDwTxEventArgs - {0}: {1}", EventType, BatteryType);
                case eSlMcrDwTxEventType.BatteryGauge:
                case eSlMcrDwTxEventType.BatteryHealth:
                case eSlMcrDwTxEventType.BatteryLifetime:
                    return String.Format("SlMcrDwTxEventArgs - {0}: {1}", EventType, DoubleValue);
                case eSlMcrDwTxEventType.Warnings:
                    return String.Format("SlMcrDwTxEventArgs - {0}: {1}", EventType, StringValue);
                default:
                    return "SlMcrDwTxEventArgs - Unknown Event Type";
            }
        }
    }
}