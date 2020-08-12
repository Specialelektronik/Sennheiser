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
    public class SldwRxEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSldwRxEventType
        {
            Identify,
            RfQuality,
            MuteSwitchActive,
            Warnings,
            Rfpi,
            LastPairedIpei,
            //MuteMode,
            //MuteState,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSldwRxEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types Identify and MuteSwitchActive.
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
        public SldwRxEventArgs(eSldwRxEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base RxHandler class
        /// </summary>
        public SldwRxEventArgs(eSldwRxEventType type, double value)
        {
            EventType = type;
            DoubleValue = value;
        }
        /// <summary>
        /// Event args for the base RxHandler class
        /// </summary>
        public SldwRxEventArgs(eSldwRxEventType type, string value)
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
        /// Returns the event printed as "SldwRxEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSldwRxEventType.Identify:
                //case eSldwRxEventType.MuteState:
                case eSldwRxEventType.MuteSwitchActive:
                    return String.Format("SldwRxEventArgs - {0}: {1}", EventType, BoolValue);
                //case eSldwRxEventType.MuteMode:
                //    return String.Format("SldwRxEventArgs - {0}: {1}", EventType, MuteMode);
                case eSldwRxEventType.RfQuality:
                    return String.Format("SldwRxEventArgs - {0}: {1}", EventType, DoubleValue);
                case eSldwRxEventType.Warnings:
                case eSldwRxEventType.Rfpi:
                case eSldwRxEventType.LastPairedIpei:
                    return String.Format("SldwRxEventArgs - {0}: {1}", EventType, StringValue);
                default:
                    return "SldwRxEventArgs - Unknown Event Type";
            }
        }
    }
}