using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Contains features, properties and events regarding the device.
    /// </summary>
    public class Chg4NDeviceHandler : DeviceHandler
    {
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public new event EventHandler<Chg4NDeviceEventArgs> Events;

        string _group = "";
        /// <summary>
        /// Sets or gets the group (location) of the device.
        /// </summary>
        public string Group
        {
            get { return _group; }
            set
            {
                Send(value, BaseProperty, "group");
            }
        }

        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public Chg4NDeviceHandler(SscCommon common)
            :base (common)
        {
            Handlers.Add("group", HandleGroup);

            Subscribe(BaseProperty, "group");
        }

        void HandleGroup(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_group != value)
            {
                _group = obj.Value.ToString();
                TrigEvent(Chg4NDeviceEventArgs.eChg4NDeviceEventType.Group, value);
            }
        }

        /// <summary>
        /// A method to trigger the Events event.
        /// </summary>
        protected override void TrigEvent(DeviceEventArgs.eDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Chg4NDeviceEventArgs(type, value));
        }
        void TrigEvent(Chg4NDeviceEventArgs.eChg4NDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Chg4NDeviceEventArgs(type, value));
        }
    }
}
