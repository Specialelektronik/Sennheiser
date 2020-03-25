using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the MeterHandler class
    /// </summary>
    public class Tcc2MeterEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eTcc2MeterEventType
        {
            Elevation,
            Azimuth,
            InputPeakLevel,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eTcc2MeterEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed
        /// </summary>
        public int IntValue { get; private set; }

        /// <summary>
        /// Event args for the MeterHandler class
        /// </summary>
        public Tcc2MeterEventArgs(eTcc2MeterEventType type, int value)
        {
            EventType = type;
            IntValue = value;
        }

        /// <summary>
        /// Returns the event printed as "DeviceEventArgs - {EventType}: {IntValue}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eTcc2MeterEventType.Elevation:
                case eTcc2MeterEventType.Azimuth:
                case eTcc2MeterEventType.InputPeakLevel:
                    return String.Format("Tcc2MeterEventArgs - {0}: {1}", EventType, IntValue);
                default:
                    return String.Format("Tcc2MeterEventArgs - Unknown EventType");
            }
            
        }
    }
}
