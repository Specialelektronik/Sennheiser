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
    public class SlMcrDwMeterEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSlMcrDwMeterEventType
        {
            RxInputLevel,
            MixerLevel
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSlMcrDwMeterEventType EventType { get; private set; }
        /// <summary>
        /// The value of the property that changed
        /// </summary>
        public int IntValue { get; private set; }
        /// <summary>
        /// The index of the receiver that changed
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Event args for the MeterHandler class
        /// </summary>
        public SlMcrDwMeterEventArgs(eSlMcrDwMeterEventType type, int value)
        {
            EventType = type;
            IntValue = value;
        }
        public SlMcrDwMeterEventArgs(eSlMcrDwMeterEventType type, int index, int value)
        {
            EventType = type;
            IntValue = value;
            Index = index;
        }

        /// <summary>
        /// Returns the event printed as "DeviceEventArgs - {EventType}: {IntValue}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSlMcrDwMeterEventType.RxInputLevel:
                    return String.Format("SlMcrDwMeterEventArgs - {0}: Receiver index: {1}, Value: {2}", EventType, Index, IntValue);
                case eSlMcrDwMeterEventType.MixerLevel:
                    return String.Format("SlMcrDwMeterEventArgs - {0}: {1}", EventType, IntValue);
                default:
                    return String.Format("SlMcrDwMeterEventArgs - Unknown EventType");
            }
            
        }
    }
}
