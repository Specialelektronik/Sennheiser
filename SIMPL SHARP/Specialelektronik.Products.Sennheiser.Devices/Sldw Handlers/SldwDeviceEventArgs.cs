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
    public class SldwDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSldwDeviceEventType
        {
            Name,
            Group,
            Version,
            Serial,
            Product,
            MacAddresses,
            Brightness
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSldwDeviceEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed (except brightness)
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type Brightness.
        /// 1.0 = 100%. 0.0 = 0%
        /// </summary>
        public double Brightness { get; private set ;}

        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SldwDeviceEventArgs(eSldwDeviceEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SldwDeviceEventArgs(eSldwDeviceEventType type, double value)
        {
            EventType = type;
            Brightness = value;
        }
        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public SldwDeviceEventArgs(DeviceEventArgs.eDeviceEventType type, string value)
        {
            StringValue = value;
            switch (type)
            {
                case DeviceEventArgs.eDeviceEventType.Name:
                    EventType = eSldwDeviceEventType.Name;
                    break;
                case DeviceEventArgs.eDeviceEventType.Version:
                    EventType = eSldwDeviceEventType.Version;
                    break;
                case DeviceEventArgs.eDeviceEventType.Serial:
                    EventType = eSldwDeviceEventType.Serial;
                    break;
                case DeviceEventArgs.eDeviceEventType.Product:
                    EventType = eSldwDeviceEventType.Product;
                    break;
                case DeviceEventArgs.eDeviceEventType.MacAddresses:
                    EventType = eSldwDeviceEventType.MacAddresses;
                    break;
            }
        }
        /// <summary>
        /// Returns the event printed as "SldwDeviceEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSldwDeviceEventType.Brightness:
                    return String.Format("SldwDeviceEventArgs - {0}: {1}", EventType, Brightness);
                default:
                    return String.Format("SldwDeviceEventArgs - {0}: {1}", EventType, StringValue);
            }
            
        }
    }
}
