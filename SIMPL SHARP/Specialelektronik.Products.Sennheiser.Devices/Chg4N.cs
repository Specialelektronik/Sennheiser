using System;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// This class integrates with Sennheiser CHG-4N, a battery charger for Sennheiser Handmics and Bodypacks.
    /// </summary>
    public class Chg4N : SscCommon
    {
        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public Chg4NDeviceHandler DeviceHandler { get; private set; }
        /// <summary>
        /// Contains features, properties and events regarding the charging bays and the devices inserted into the bays.
        /// </summary>
        public Chg4NBaysHandler BaysHandler { get; private set; }

        /// <summary>
        /// This class integrates with Sennheiser CHG-4N, a battery charger for Sennheiser Handmics and Bodypacks.
        /// </summary>
        public Chg4N()
            : base(0)
        {
            DeviceHandler = new Chg4NDeviceHandler(this);
            BaysHandler = new Chg4NBaysHandler(this);
        }
    }
}