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
    public class SldwTxEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSldwTxEventType
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
        public eSldwTxEventType EventType { get; private set; }
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
        public SldwTxHandler.eSldwTxDeviceType DeviceType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type BatteryType.
        /// </summary>
        public SldwTxHandler.eSldwTxBatteryType BatteryType { get; private set; }

        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SldwTxEventArgs(eSldwTxEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SldwTxEventArgs(eSldwTxEventType type, double value)
        {
            EventType = type;
            DoubleValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SldwTxEventArgs(eSldwTxEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SldwTxEventArgs(eSldwTxEventType type, SldwTxHandler.eSldwTxDeviceType value)
        {
            EventType = type;
            DeviceType = value;
        }
        /// <summary>
        /// Event args for the base TxHandler class
        /// </summary>
        public SldwTxEventArgs(eSldwTxEventType type, SldwTxHandler.eSldwTxBatteryType value)
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
                case eSldwTxEventType.Active:
                case eSldwTxEventType.Charging:
                    return String.Format("SldwTxEventArgs - {0}: {1}", EventType, BoolValue);
                case eSldwTxEventType.DeviceType:
                    return String.Format("SldwTxEventArgs - {0}: {1}", EventType, DeviceType);
                case eSldwTxEventType.BatteryType:
                    return String.Format("SldwTxEventArgs - {0}: {1}", EventType, BatteryType);
                case eSldwTxEventType.BatteryGauge:
                case eSldwTxEventType.BatteryHealth:
                case eSldwTxEventType.BatteryLifetime:
                    return String.Format("SldwTxEventArgs - {0}: {1}", EventType, DoubleValue);
                case eSldwTxEventType.Warnings:
                    return String.Format("SldwTxEventArgs - {0}: {1}", EventType, StringValue);
                default:
                    return "SldwTxEventArgs - Unknown Event Type";
            }
        }
    }
}