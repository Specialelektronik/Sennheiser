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
    public class Tcc2DeviceHandler : DeviceHandler
    {
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public new event EventHandler<Tcc2DeviceEventArgs> Events;

        /// <summary>
        /// The available colors for leds
        /// </summary>
        public enum eLedColor
        {
            Unknown,
            LightGreen,
            Green,
            Blue,
            Red,
            Yellow,
            Orange,
            Cyan,
            Pink
        }

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
        /// Sets or gets if the identify feature of the device is enabled. It blinks the LEDs on the mic when true.
        /// </summary>
        public bool Identify
        {
            get { return _identify; }
            set
            {
                Send(value, BaseProperty, "identification", "visual");
            }
        }
        eLedColor _customLedColor = eLedColor.Unknown;
        /// <summary>
        /// Sets or gets the currently selected custom color for the leds.
        /// </summary>
        public eLedColor CustomLedColor
        {
            get { return _customLedColor; }
            set
            {
                string cmd = GetLedColor(value);
                Send(cmd, BaseProperty, "led", "custom", "color");
            }
        }
        bool _customLedActive = false;
        /// <summary>
        /// Sets or gets if the the custom led color is active. The color is set with property CustomLedColor.
        /// </summary>
        public bool CustomLedActive
        {
            get { return _customLedActive; }
            set
            {
                Send(value, BaseProperty, "led", "custom", "active");
            }
        }
        eLedColor _micMuteLedColor = eLedColor.Unknown;
        /// <summary>
        /// Sets or gets the currently selected color for the leds when the mic is muted.
        /// </summary>
        public eLedColor MicMuteLedColor
        {
            get { return _micMuteLedColor; }
            set
            {
                string cmd = GetLedColor(value);
                Send(cmd, BaseProperty, "led", "mic_mute", "color");
            }
        }
        eLedColor _micOnLedColor = eLedColor.Unknown;
        /// <summary>
        /// Sets or gets the currently selected custom color for the leds when the mic is on.
        /// </summary>
        public eLedColor MicOnLedColor
        {
            get { return _micOnLedColor; }
            set
            {
                string cmd = GetLedColor(value);
                Send(cmd, BaseProperty, "led", "mic_on", "color");
            }
        }
        int _ledBrightness;
        /// <summary>
        /// Sets or gets the brightness of the leds on the mic in steps of 20%. A value between 0 and 5 where 0 = 0% and 5 = 100%.
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
        public Tcc2DeviceHandler(SscCommon common)
            :base (common)
        {
            Handlers.Add("location", HandleLocation);
            Handlers.Add("position", HandlePosition);
            Handlers.Add("identification", HandleIdentify);
            Handlers.Add("led", HandleLed);

            Subscribe(BaseProperty, "location");
            Subscribe(BaseProperty, "position");
            Subscribe(BaseProperty, "identification", "visual");
            Subscribe(BaseProperty, "led", "custom", "color");
            Subscribe(BaseProperty, "led", "custom", "active");
            Subscribe(BaseProperty, "led", "mic_mute", "color");
            Subscribe(BaseProperty, "led", "mic_on", "color");
            Subscribe(BaseProperty, "led", "brightness");
        }

        void HandleLocation(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_location != value)
            {
                _location = value;
                TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.Location, value);
            }
        }
        void HandlePosition(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_position != value)
            {
                _position = value;
                TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.Position, value);
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
                    TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.Identify, value);
                }
            }            
        }
        void HandleLed(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "custom")
                HandleLedCustom(obj);
            else if (obj.Name == "mic_mute")
                HandleLedMicMute(obj);
            else if (obj.Name == "mic_on")
                HandleLedMicOn(obj);
            else if (obj.Name == "brightness")
                HandleLedBrightness(obj);
        }
        void HandleLedCustom(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "color")
            {
                var value = GetLedColor(obj.Value.ToString());
                if (_customLedColor != value)
                {
                    _customLedColor = value;
                    TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedColor, value);
                }
            }
            else if (obj.Name == "active")
            {
                var value = obj.Value.ToObject<bool>();
                if (_customLedActive != value)
                {
                    _customLedActive = value;
                    TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedActive, value);
                }
            }
        }
        void HandleLedMicMute(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "color")
            {
                var value = GetLedColor(obj.Value.ToString());
                if (_micMuteLedColor != value)
                {
                    _micMuteLedColor = value;
                    TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicMuteLedColor, value);
                }
            }
        }
        void HandleLedMicOn(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "color")
            {
                var value = GetLedColor(obj.Value.ToString());
                if (_micOnLedColor != value)
                {
                    _micOnLedColor = value;
                    TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicOnLedColor, value);
                }
            }
        }
        void HandleLedBrightness(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>();
            if (_ledBrightness != value)
            {
                _ledBrightness = value;
                var ev = Events;
                if (ev != null)
                    ev(this, new Tcc2DeviceEventArgs(Tcc2DeviceEventArgs.eTcc2DeviceEventType.LedBrightness, value));
            }
        }
        eLedColor GetLedColor(string value)
        {
            if (value == "LIGHT GREEN")
                return eLedColor.LightGreen;
            else if (value == "GREEN")
                return eLedColor.Green;
            else if (value == "BLUE")
                return eLedColor.Blue;
            else if (value == "RED")
                return eLedColor.Red;
            else if (value == "YELLOW")
                return eLedColor.Yellow;
            else if (value == "ORANGE")
                return eLedColor.Orange;
            else if (value == "CYAN")
                return eLedColor.Cyan;
            else if (value == "PINK")
                return eLedColor.Pink;
            else
                return eLedColor.Unknown;
        }
        string GetLedColor(eLedColor value)
        {
            switch (value)
            {
                case eLedColor.LightGreen:
                    return "LIGHT GREEN";
                case eLedColor.Green:
                    return "GREEN";
                case eLedColor.Blue:
                    return "BLUE";
                case eLedColor.Red:
                    return "RED";
                case eLedColor.Yellow:
                    return "YELLOW";
                case eLedColor.Orange:
                    return "ORANGE";
                case eLedColor.Cyan:
                    return "CYAN";
                case eLedColor.Pink:
                    return "PINK";
                default:
                    return null;
            }
        }

        /// <summary>
        /// A method to trigger the Events event.
        /// </summary>
        protected override void TrigEvent(DeviceEventArgs.eDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2DeviceEventArgs(type, value));
        }
        void TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2DeviceEventArgs(type, value));
        }
        void TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2DeviceEventArgs(type, value));
        }
        void TrigEvent(Tcc2DeviceEventArgs.eTcc2DeviceEventType type, eLedColor value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2DeviceEventArgs(type, value));
        }
    }
}
