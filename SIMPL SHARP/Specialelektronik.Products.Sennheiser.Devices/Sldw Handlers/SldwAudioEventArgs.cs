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
    public class SldwAudioEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSldwAudioEventType
        {
            OutputGain,
            Eq,
            LowCut,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSldwAudioEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type LowCut.
        /// </summary>
        public bool LowCut { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type OutputGain.
        /// </summary>
        public SldwAudioHandler.eSldwAudioOutputGain OutputGain { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type Eq.
        /// </summary>
        public SldwAudioHandler.eSldwAudioEq Eq { get; private set; }
        //public SldwRxHandler.eSldwRxMuteMode MuteMode { get; private set; }

        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SldwAudioEventArgs(eSldwAudioEventType type, bool value)
        {
            EventType = type;
            LowCut = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SldwAudioEventArgs(eSldwAudioEventType type, SldwAudioHandler.eSldwAudioOutputGain value)
        {
            EventType = type;
            OutputGain = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SldwAudioEventArgs(eSldwAudioEventType type, SldwAudioHandler.eSldwAudioEq value)
        {
            EventType = type;
            Eq = value;
        }

        /// <summary>
        /// Returns the event printed as "SldwAudioEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSldwAudioEventType.OutputGain:
                    return String.Format("SldwAudioEventArgs - {0}: {1}", EventType, OutputGain);
                case eSldwAudioEventType.Eq:
                    return String.Format("SldwAudioEventArgs - {0}: {1}", EventType, Eq);
                case eSldwAudioEventType.LowCut:
                    return String.Format("SldwAudioEventArgs - {0}: {1}", EventType, LowCut);
                default:
                    return "SldwAudioEventArgs - Unknown Event Type";
            }
        }
    }
}