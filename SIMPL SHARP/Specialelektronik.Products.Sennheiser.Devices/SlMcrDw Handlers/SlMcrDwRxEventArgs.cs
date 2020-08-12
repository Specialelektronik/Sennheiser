using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the base RxHandler class
    /// </summary>
    public class SlMcrDwRxEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSlMcrDwRxEventType
        {
            Identify,
            RfQuality,
            Warnings,
            Rfpi,
            LastPairedIpei,
            //MuteMode,
            //MuteState,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSlMcrDwRxEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types Identify.
        /// </summary>
        public bool BoolValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type RfQuality.
        /// </summary>
        public double DoubleValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type Warnings, Rfpi and LastPairedIpei.
        /// </summary>
        public string StringValue { get; private set; }
        //public SldwRxHandler.eSldwRxMuteMode MuteMode { get; private set; }

        /// <summary>
        /// Event args for the base RxHandler class
        /// </summary>
        public SlMcrDwRxEventArgs(eSlMcrDwRxEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base RxHandler class
        /// </summary>
        public SlMcrDwRxEventArgs(eSlMcrDwRxEventType type, double value)
        {
            EventType = type;
            DoubleValue = value;
        }
        /// <summary>
        /// Event args for the base RxHandler class
        /// </summary>
        public SlMcrDwRxEventArgs(eSlMcrDwRxEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /*
        public SldwRxEventArgs(eSldwRxEventType type, SldwRxHandler.eSldwRxMuteMode value)
        {
            EventType = type;
            MuteMode = value;
        }
        */

        /// <summary>
        /// Returns the event printed as "SlMcrDwRxEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSlMcrDwRxEventType.Identify:
                //case eSldwRxEventType.MuteState:
                    return String.Format("SlMcrDwRxEventArgs - {0}: {1}", EventType, BoolValue);
                //case eSldwRxEventType.MuteMode:
                //    return String.Format("SldwRxEventArgs - {0}: {1}", EventType, MuteMode);
                case eSlMcrDwRxEventType.RfQuality:
                    return String.Format("SlMcrDwRxEventArgs - {0}: {1}", EventType, DoubleValue);
                case eSlMcrDwRxEventType.Warnings:
                case eSlMcrDwRxEventType.Rfpi:
                case eSlMcrDwRxEventType.LastPairedIpei:
                    return String.Format("SlMcrDwRxEventArgs - {0}: {1}", EventType, StringValue);
                default:
                    return "SlMcrDwRxEventArgs - Unknown Event Type";
            }
        }
    }
}