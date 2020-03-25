using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the DeviceHandler class
    /// </summary>
    public class Tcc2DeviceEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eTcc2DeviceEventType
        {
            Name,
            Location,
            Position,
            Version,
            Serial,
            Product,
            MacAddresses,
            Identify,
            CustomLedColor,
            CustomLedActive,
            MicMuteLedColor,
            MicOnLedColor,
            LedBrightness,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eTcc2DeviceEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types Name, Location, Position, Version, Serial, Product and MacAddresses.
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types Identify and CustomLedActive.
        /// </summary>
        public bool BoolValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type LedBrightness.
        /// </summary>
        public int IntValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types CustomLedColor, MicMuteLedColor and MicOnLedColor.
        /// </summary>
        public Tcc2DeviceHandler.eLedColor LedColor { get; private set; }

        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public Tcc2DeviceEventArgs(eTcc2DeviceEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public Tcc2DeviceEventArgs(eTcc2DeviceEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public Tcc2DeviceEventArgs(eTcc2DeviceEventType type, int value)
        {
            EventType = type;
            IntValue = value;
        }
        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public Tcc2DeviceEventArgs(eTcc2DeviceEventType type, Tcc2DeviceHandler.eLedColor value)
        {
            EventType = type;
            LedColor = value;
        }
        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public Tcc2DeviceEventArgs(DeviceEventArgs.eDeviceEventType type, string value)
        {
            StringValue = value;
            switch (type)
            {
                case DeviceEventArgs.eDeviceEventType.Name:
                    EventType = eTcc2DeviceEventType.Name;
                    break;
                case DeviceEventArgs.eDeviceEventType.Version:
                    EventType = eTcc2DeviceEventType.Version;
                    break;
                case DeviceEventArgs.eDeviceEventType.Serial:
                    EventType = eTcc2DeviceEventType.Serial;
                    break;
                case DeviceEventArgs.eDeviceEventType.Product:
                    EventType = eTcc2DeviceEventType.Product;
                    break;
                case DeviceEventArgs.eDeviceEventType.MacAddresses:
                    EventType = eTcc2DeviceEventType.MacAddresses;
                    break;
            }
        }

        /// <summary>
        /// Returns the event printed as "Tcc2DeviceEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eTcc2DeviceEventType.Name:
                case eTcc2DeviceEventType.Location:
                case eTcc2DeviceEventType.Position:
                case eTcc2DeviceEventType.Version:
                case eTcc2DeviceEventType.Serial:
                case eTcc2DeviceEventType.Product:
                case eTcc2DeviceEventType.MacAddresses:
                    return String.Format("Tcc2DeviceEventArgs - {0}: {1}", EventType, StringValue);
                case eTcc2DeviceEventType.Identify:
                case eTcc2DeviceEventType.CustomLedActive:
                    return String.Format("Tcc2DeviceEventArgs - {0}: {1}", EventType, BoolValue);
                case eTcc2DeviceEventType.CustomLedColor:
                case eTcc2DeviceEventType.MicMuteLedColor:
                case eTcc2DeviceEventType.MicOnLedColor:
                    return String.Format("Tcc2DeviceEventArgs - {0}: {1}", EventType, LedColor);
                case eTcc2DeviceEventType.LedBrightness:
                    return String.Format("Tcc2DeviceEventArgs - {0}: {1}", EventType, IntValue);
                default:
                    return String.Format("Tcc2DeviceEventArgs - Unknown EventType");
            }
            
        }
    }
}
