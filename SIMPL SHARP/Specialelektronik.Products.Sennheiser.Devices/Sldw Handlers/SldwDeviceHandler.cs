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
    public class SldwDeviceHandler : DeviceHandler
    {
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public new event EventHandler<SldwDeviceEventArgs> Events;

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
        /// Sets or gets the brightness of the frontpanel. 
        /// 1.0 = 100%. 0.0 = 0%
        /// </summary>
        public double Brightness
        {
            get { return _brightnessHandler.Brightness; }
            set { _brightnessHandler.Brightness = value; }
        }
        SldwBrightnessHandler _brightnessHandler;

        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public SldwDeviceHandler(SscCommon common, SldwBrightnessHandler brightnessHandler)
            :base (common)
        {
            _brightnessHandler = brightnessHandler;
            _brightnessHandler.BrightnessChanged += new EventHandler(_brightnessHandler_BrightnessChanged);

            Handlers.Add("group", HandleGroup);

            Subscribe(BaseProperty, "group");
        }

        void HandleGroup(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_group != value)
            {
                _group = value;
                TrigEvent(SldwDeviceEventArgs.eSldwDeviceEventType.Group, value);
            }
        }

        /// <summary>
        /// A method to trigger the Events event.
        /// </summary>
        protected override void TrigEvent(DeviceEventArgs.eDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwDeviceEventArgs(type, value));
        }
        void TrigEvent(SldwDeviceEventArgs.eSldwDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwDeviceEventArgs(type, value));
        }

        void _brightnessHandler_BrightnessChanged(object sender, EventArgs e)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwDeviceEventArgs(SldwDeviceEventArgs.eSldwDeviceEventType.Brightness, _brightnessHandler.Brightness));
        }
    }
}
