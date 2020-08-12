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
    public class SlMcrDwAudioEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eSlMcrDwAudioEventType
        {
            DanteMacAddresses,
            DanteIpAddresses,
            DanteOutputGain,
            RxOutputGain,
            RxEq,
            RxLowCut,
        }
        /// <summary>
        /// The property that changed
        /// </summary>
        public eSlMcrDwAudioEventType EventType { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event types DanteMacAddresses and DanteIpAddresses.
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// Contains the new value of the property for event type DanteOutputGain.
        /// </summary>
        public SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain DanteOutputGain { get; private set; }

        /// <summary>
        /// Contains the properties for event types RxOutputGain, RxEq and RxLowCut.
        /// </summary>
        public SlMcrDwRxAudio RxAudio { get; private set; }


        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SlMcrDwAudioEventArgs(eSlMcrDwAudioEventType type, SlMcrDwRxAudio value)
        {
            EventType = type;
            RxAudio = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SlMcrDwAudioEventArgs(eSlMcrDwAudioEventType type, string value)
        {
            EventType = type;
            StringValue = value;
        }
        /// <summary>
        /// Event args for the base AudioHandler class
        /// </summary>
        public SlMcrDwAudioEventArgs(eSlMcrDwAudioEventType type, SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain value)
        {
            EventType = type;
            DanteOutputGain = value;
        }

        
        /// <summary>
        /// Returns the event printed as "SlMcrDwAudioEventArgs - {EventType}: {Value}"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eSlMcrDwAudioEventType.DanteMacAddresses:
                case eSlMcrDwAudioEventType.DanteIpAddresses:
                    return String.Format("SlMcrDwAudioEventArgs - {0}: {1}", EventType, StringValue);
                case eSlMcrDwAudioEventType.DanteOutputGain:
                    return String.Format("SlMcrDwAudioEventArgs - {0}: {1}", EventType, DanteOutputGain);
                case eSlMcrDwAudioEventType.RxOutputGain:
                    return String.Format("SlMcrDwAudioEventArgs - {0}: RxAudio.Index: {1}, OutputGain: {2}", EventType, RxAudio.Index, RxAudio.OutputGain);
                case eSlMcrDwAudioEventType.RxEq:
                    return String.Format("SlMcrDwAudioEventArgs - {0}: RxAudio.Index: {1}, Eq: {2}", EventType, RxAudio.Index, RxAudio.Eq);
                case eSlMcrDwAudioEventType.RxLowCut:
                    return String.Format("SlMcrDwAudioEventArgs - {0}: RxAudio.Index: {1}, LowCut: {2}", EventType, RxAudio.Index, RxAudio.LowCut);
                default:
                    return "SlMcrDwAudioEventArgs - Unknown Event Type";
            }
        }
    }
}