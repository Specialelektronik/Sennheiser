using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// This class integrates with Sennheiser TeamConnect Ceiling 2, a microphone mounted in the ceiling of the room.
    /// </summary>
    public class Tcc2 : SscCommon
    {
        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public Tcc2DeviceHandler DeviceHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding meters, such as azimuth, elevation and input peak level.
        /// </summary>
        public Tcc2MeterHandler MeterHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the audio output, such as Dante output gain and Mute.
        /// </summary>
        public Tcc2AudioHandler AudioHandler { get; private set; }

        /// <summary>
        /// This class integrates with Sennheiser TeamConnect Ceiling 2, a microphone mounted in the ceiling of the room.
        /// </summary>
        public Tcc2()
            : base (0x0A)
        {
            DeviceHandler = new Tcc2DeviceHandler(this);
            MeterHandler = new Tcc2MeterHandler(this);
            AudioHandler = new Tcc2AudioHandler(this);
        }

    }
}