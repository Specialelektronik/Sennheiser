using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;                          				// For Basic SIMPL# Classes

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// This class integrates with Sennheiser SpeechLine Digital Wireless (SLDW), a wireless microphone system.
    /// </summary>
    public class Sldw : SscCommon
    {
        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public SldwDeviceHandler DeviceHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the receiving end, such as RF quality.
        /// </summary>
        public SldwRxHandler RxHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
        /// </summary>
        public SldwTxHandler TxHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the receiving end, such as Output gain and EQ.
        /// </summary>
        public SldwAudioHandler AudioHandler { get; private set; }

        /// <summary>
        /// This class integrates with Sennheiser SpeechLine Digital Wireless (SLDW), a wireless microphone system.
        /// </summary>
        public Sldw()
            : base (0x00)
        {
            DeviceHandler = new SldwDeviceHandler(this, new SldwBrightnessHandler(this));
            RxHandler = new SldwRxHandler(this);
            TxHandler = new SldwTxHandler(this);
            AudioHandler = new SldwAudioHandler(this);
        }
    }
}
