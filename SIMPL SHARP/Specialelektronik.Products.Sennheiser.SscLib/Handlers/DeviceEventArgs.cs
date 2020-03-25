using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// Event args for the base DeviceHandler class
    /// </summary>
    public class DeviceEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eDeviceEventType
        {
            Name,
            Version,
            Serial,
            Product,
            MacAddresses,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eDeviceEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed
        /// </summary>
        public string StringValue { get; private set; }

        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        /// <param name="type">The property that changed</param>
        /// <param name="value">The value of the property that changed</param>
        public DeviceEventArgs(eDeviceEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Returns the event printed as "DeviceEventArgs - {EventType}: {StringValue}"
        /// </summary>
        public override string ToString()
        {
            return String.Format("DeviceEventArgs - {0}: {1}", EventType, StringValue);
        }
    }
}
