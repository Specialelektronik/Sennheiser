using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the base AudioHandler class
    /// </summary>
    public class Tcc2AudioEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eTcc2AudioEventType
        {
            ExclusionZoneActive,
            DanteMacAddresses,
            DanteIpAddresses,
            DanteOutputGain,
            SpeakerDetectionThreshold,
            Mute,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eTcc2AudioEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types ExclusionZoneActive and Mute.
        /// </summary>
        public bool BoolValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type DanteOutputGain.
        /// </summary>
        public int IntValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types DanteMacAddresses and DanteIpAddresses.
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type SpeakerDetectionThreshold.
        /// </summary>
        public Tcc2AudioHandler.eSpeakerDetectionThreshold SpeakerDetectionThreshold { get; private set; }

        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public Tcc2AudioEventArgs(eTcc2AudioEventType type, bool value)
        {
            EventType = type;
            BoolValue = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public Tcc2AudioEventArgs(eTcc2AudioEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public Tcc2AudioEventArgs(eTcc2AudioEventType type, int value)
        {
            EventType = type;
            IntValue = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public Tcc2AudioEventArgs(eTcc2AudioEventType type, Tcc2AudioHandler.eSpeakerDetectionThreshold value)
        {
            EventType = type;
            SpeakerDetectionThreshold = value;
        }

        /// <summary>
        /// Returns the event printed as "Tcc2AudioEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eTcc2AudioEventType.ExclusionZoneActive:
                case eTcc2AudioEventType.Mute:
                    return String.Format("Tcc2AudioEventArgs - {0}: {1}", EventType, BoolValue);
                case eTcc2AudioEventType.DanteMacAddresses:
                case eTcc2AudioEventType.DanteIpAddresses:
                    return String.Format("Tcc2AudioEventArgs - {0}: {1}", EventType, StringValue);
                case eTcc2AudioEventType.DanteOutputGain:
                    return String.Format("Tcc2AudioEventArgs - {0}: {1}", EventType, IntValue);
                case eTcc2AudioEventType.SpeakerDetectionThreshold:
                    return String.Format("Tcc2AudioEventArgs - {0}: {1}", EventType, SpeakerDetectionThreshold);
                default:
                    return "Tcc2AudioEventArgs - Unknown Event Type";
            }
        }
    }
}