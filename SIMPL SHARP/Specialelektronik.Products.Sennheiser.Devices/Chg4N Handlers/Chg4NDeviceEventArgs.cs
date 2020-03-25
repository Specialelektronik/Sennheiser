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
    public class Chg4NDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eChg4NDeviceEventType
        {
            Name,
            Group,
            Version,
            Serial,
            Product,
            MacAddresses
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eChg4NDeviceEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed
        /// </summary>
        public string StringValue { get; private set; }

        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public Chg4NDeviceEventArgs(eChg4NDeviceEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public Chg4NDeviceEventArgs(DeviceEventArgs.eDeviceEventType type, string value)
        {
            StringValue = value;
            switch (type)
            {
                case DeviceEventArgs.eDeviceEventType.Name:
                    EventType = eChg4NDeviceEventType.Name;
                    break;
                case DeviceEventArgs.eDeviceEventType.Version:
                    EventType = eChg4NDeviceEventType.Version;
                    break;
                case DeviceEventArgs.eDeviceEventType.Serial:
                    EventType = eChg4NDeviceEventType.Serial;
                    break;
                case DeviceEventArgs.eDeviceEventType.Product:
                    EventType = eChg4NDeviceEventType.Product;
                    break;
                case DeviceEventArgs.eDeviceEventType.MacAddresses:
                    EventType = eChg4NDeviceEventType.MacAddresses;
                    break;
            }
        }
        /// <summary>
        /// Returns the event printed as "Chg4NDeviceEventArgs - {EventType}: {StringValue}"
        /// </summary>
        public override string ToString()
        {
            return String.Format("Chg4NDeviceEventArgs - {0}: {1}", EventType, StringValue);
        }
    }
}
