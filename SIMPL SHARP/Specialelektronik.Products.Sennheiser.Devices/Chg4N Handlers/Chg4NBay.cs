using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Contains information about a bay
    /// </summary>
    public class Chg4NBay
    {
        /// <summary>
        /// The possible types of device types that can be inserted into the bay 
        /// </summary>
        public enum eBayDeviceType
        {
            Unknown = 0,
            Handheld = 1,
            Bodypack = 2,
        }

        /// <summary>
        /// Returns true if there is a device inserted into the bay.
        /// </summary>
        public bool Active { get; internal set; }
        /// <summary>
        /// Returns true if the inserted device is charging.
        /// </summary>
        public bool Charging { get; internal set; }
        
        string _serial = "";
        /// <summary>
        /// Serial number of the inserted device.
        /// </summary>
        public string Serial 
        {
            get { return _serial; }
            internal set { _serial = value; } 
        }
        /// <summary>
        /// This is the level of power in the battery.
        /// 1.0 = full. 0.0 = Empty
        /// </summary>
        public double BatteryGauge { get; internal set; }
        /// <summary>
        /// This is the health of the in the battery
        /// 1.0 = Perfect condition. 0.0 = Very bad, or no device inserted.
        /// </summary>
        public double BatteryHealth { get; internal set; }
        /// <summary>
        /// Returns the number of minutes left until device is fully charged.
        /// </summary>
        public int MinutesToFull { get; internal set; }
        /// <summary>
        /// Returns the device type of the device inserted into the bay
        /// </summary>
        public eBayDeviceType DeviceType { get; internal set; }
    }
}
