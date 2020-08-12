using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the base DeviceHandler class
    /// </summary>
    public class SlMcrDwDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSlMcrDwDeviceEventType
        {
            Name,
            Location,
            Position,
            Version,
            Serial,
            Product,
            MacAddresses,
            Identify,
            LedBrightness,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSlMcrDwDeviceEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed (except brightness and identify)
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type LedBrightness.
        /// </summary>
        public int IntValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type Identify.
        /// </summary>
        public bool BoolValue { get; private set; }

        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SlMcrDwDeviceEventArgs(eSlMcrDwDeviceEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the DeviceHandler class
        /// </summary>
        public SlMcrDwDeviceEventArgs(eSlMcrDwDeviceEventType type, int value)
        {
            EventType = type;
            IntValue = value;
        }
        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SlMcrDwDeviceEventArgs(eSlMcrDwDeviceEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SlMcrDwDeviceEventArgs(DeviceEventArgs.eDeviceEventType type, string value)
        {
            StringValue = value;
            switch (type)
            {
                case DeviceEventArgs.eDeviceEventType.Name:
                    EventType = eSlMcrDwDeviceEventType.Name;
                    break;
                case DeviceEventArgs.eDeviceEventType.Version:
                    EventType = eSlMcrDwDeviceEventType.Version;
                    break;
                case DeviceEventArgs.eDeviceEventType.Serial:
                    EventType = eSlMcrDwDeviceEventType.Serial;
                    break;
                case DeviceEventArgs.eDeviceEventType.Product:
                    EventType = eSlMcrDwDeviceEventType.Product;
                    break;
                case DeviceEventArgs.eDeviceEventType.MacAddresses:
                    EventType = eSlMcrDwDeviceEventType.MacAddresses;
                    break;
            }
        }
        /// <summary>
        /// Returns the event printed as "SlMcrDwDeviceEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSlMcrDwDeviceEventType.Identify:
                    return String.Format("SlMcrDwDeviceEventArgs - {0}: {1}", EventType, BoolValue);
                case eSlMcrDwDeviceEventType.LedBrightness:
                    return String.Format("SlMcrDwDeviceEventArgs - {0}: {1}", EventType, IntValue);
                default:
                    return String.Format("SlMcrDwDeviceEventArgs - {0}: {1}", EventType, StringValue);
            }
            
        }
    }
}
