using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;                          				// For Basic SIMPL# Classes

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// This class integrates with Sennheiser SpeechLine Digital Wireless Multi-Channel Receiver (SL MCR DW), a wireless microphone system.
    /// </summary>
    public class SlMcrDw : SscCommon
    {
        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public SlMcrDwDeviceHandler DeviceHandler { get; private set; }
        
        List<SlMcrDwRxHandler> _rxHandlers = new List<SlMcrDwRxHandler>();
        /// <summary>
        /// Contains features, properties and events regarding the receiving end, such as RF quality.
        /// </summary>
        public ReadOnlyCollection<SlMcrDwRxHandler> RxHandlers { get; private set; }
        List<SlMcrDwTxHandler> _txHandlers = new List<SlMcrDwTxHandler>();
        /// <summary>
        /// Contains features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
        /// </summary>
        public ReadOnlyCollection<SlMcrDwTxHandler> TxHandlers { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the receiving end, such as Output gain and EQ.
        /// </summary>
        public SlMcrDwAudioHandler AudioHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding meters, such as receiver input level.
        /// </summary>
        public SlMcrDwMeterHandler MeterHandler { get; private set; }
        
        /// <summary>
        /// This class integrates with Sennheiser SpeechLine Digital Wireless Multi-Channel Receiver (SL MCR DW), a wireless microphone system.
        /// </summary>
        public SlMcrDw(int numberOfChannels)
            : base (0x00)
        {
            DeviceHandler = new SlMcrDwDeviceHandler(this);
            AudioHandler = new SlMcrDwAudioHandler(this, numberOfChannels);
            MeterHandler = new SlMcrDwMeterHandler(this, numberOfChannels);

            for (int i = 0; i < numberOfChannels; i++)
                _rxHandlers.Add(new SlMcrDwRxHandler(this, "rx" + (i + 1)));
            for (int i = 0; i < numberOfChannels; i++)
                _txHandlers.Add(new SlMcrDwTxHandler(this, "tx" + (i + 1)));

            RxHandlers = _rxHandlers.AsReadOnly();
            TxHandlers = _txHandlers.AsReadOnly();
        }
    }
}
