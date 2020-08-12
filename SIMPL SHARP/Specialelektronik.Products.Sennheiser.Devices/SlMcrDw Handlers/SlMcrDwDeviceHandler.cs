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
    public class SlMcrDwDeviceHandler : DeviceHandler
    {
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public new event EventHandler<SlMcrDwDeviceEventArgs> Events;

        string _location = "";
        /// <summary>
        /// Sets or gets the location of the device. Max length: 8 characters. Allowed chars: 0-9, A-Z, a-z or 'space'. Must start with a letter. May not start or end with a – or _.
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                Send(value, BaseProperty, "location");
            }
        }
        string _position = "";
        /// <summary>
        /// Sets or gets the position of the device. Intended to be used as the position in the location. Example if location is "Room_1", position might be "Over the table". Max length: 30 chars. Allowed chars: 0-9, A-Z, a-z or 'space'
        /// </summary>
        public string Position
        {
            get { return _position; }
            set
            {
                Send(value, BaseProperty, "position");
            }
        }
        bool _identify = false;
        /// <summary>
        /// Sets or gets if the identify feature of the device is enabled. It blinks the LEDs on the device when true.
        /// </summary>
        public bool Identify
        {
            get { return _identify; }
            set
            {
                Send(value, BaseProperty, "identification", "visual");
            }
        }
        int _ledBrightness;
        /// <summary>
        /// Sets or gets the brightness of the leds on the device in steps of 20%. A value between 0 and 5 where 0 = 0% and 5 = 100%.
        /// </summary>
        public int LedBrightness
        {
            get { return _ledBrightness; }
            set
            {
                Send(value, BaseProperty, "led", "brightness");
            }
        }

        /// <summary>
        /// Contains features, properties and events regarding the device.
        /// </summary>
        public SlMcrDwDeviceHandler(SscCommon common)
            :base (common)
        {
            Handlers.Add("location", HandleLocation);
            Handlers.Add("position", HandlePosition);
            Handlers.Add("identification", HandleIdentify);
            Handlers.Add("led", HandleLed);

            Subscribe(BaseProperty, "location");
            Subscribe(BaseProperty, "position");
            Subscribe(BaseProperty, "identification", "visual");
            Subscribe(BaseProperty, "led", "brightness");
        }

        void HandleLocation(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_location != value)
            {
                _location = value;
                TrigEvent(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Location, value);
            }
        }
        void HandlePosition(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_position != value)
            {
                _position = value;
                TrigEvent(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Position, value);
            }
        }
        void HandleIdentify(JContainer json)
        {
            var obj = (JProperty)json.First.First;

            if (obj.Name == "visual")
            {
                var value = obj.Value.ToObject<bool>();
                if (_identify != value)
                {
                    _identify = value;
                    TrigEvent(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.Identify, value);
                }
            }
        }
        void HandleLed(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "brightness")
            {
                var value = obj.Value.ToObject<int>();
                if (_ledBrightness != value)
                {
                    _ledBrightness = value;
                    var ev = Events;
                    if (ev != null)
                        ev(this, new SlMcrDwDeviceEventArgs(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType.LedBrightness, value));
                }
            }
        }
        /// <summary>
        /// A method to trigger the Events event.
        /// </summary>
        protected override void TrigEvent(DeviceEventArgs.eDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwDeviceEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwDeviceEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwDeviceEventArgs.eSlMcrDwDeviceEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwDeviceEventArgs(type, value));
        }
    }
}
