using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using Newtonsoft.Json.Linq;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Contains features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
    /// </summary>
    public class SlMcrDwTxHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "mates".
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        /// <summary>
        /// The available device types
        /// </summary>
        public enum eSlMcrDwTxDeviceType
        {
            Unknown = -1,
            Handheld = 0,
            Bodypack = 1,
            Tablestand = 2,
            Boundary = 3,
        }
        /// <summary>
        /// The available battery types
        /// </summary>
        public enum eSlMcrDwTxBatteryType
        {
            Unknown = -1,
            Battery = 0,
            Rechargable = 1,
        }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SlMcrDwTxEventArgs> Events;

        /// <summary>
        /// The currently connected transmitter type.
        /// </summary>
        public eSlMcrDwTxDeviceType DeviceType { get; private set; }
        /// <summary>
        /// The currently connected transmitters battery type.
        /// </summary>
        public eSlMcrDwTxBatteryType BatteryType { get; private set; }
        /// <summary>
        /// This is true when a transmitter is charging while it is on and connected. This will not work when charging a handmic or bodypack in the CHG-4N, as it will then disconnect from the device.
        /// </summary>
        public bool Charging { get; private set; }
        /// <summary>
        /// The currently connected transmitters battery level. 
        /// 1.0 = Full. 0.0 = Empty.
        /// </summary>
        public double BatteryGauge { get; private set; }
        /// <summary>
        /// The currently connected transmitters battery health level. 
        /// 1.0 = Perfect condition. 0.0 = Very bad
        /// </summary>
        public double BatteryHealth { get; private set; }
        /// <summary>
        /// The currently connected transmitters battery lifetime in minutes. Lifetime means before you have to replace the rechargable battery with a new one, not until the current charge is depleted. This only works if you have a rechargable battery.
        /// </summary>
        public int BatteryLifetime { get; private set; }
        /// <summary>
        /// The warning message shown on the frontpanel of the transmitter.
        /// Example: "Low Bat".
        /// </summary>
        public string Warnings { get; private set; }

        string _txBaseName;

        /// <summary>
        /// This is true when a transmitter (such as the handmic or bodypack) is turned on and connected to the device.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Contains features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
        /// </summary>
        public SlMcrDwTxHandler(SscCommon common, string txBaseName)
            : base(common, "mates")
        {
            _txBaseName = txBaseName;

            Warnings = "";
            DeviceType = eSlMcrDwTxDeviceType.Unknown;
            BatteryType = eSlMcrDwTxBatteryType.Unknown;

            Handlers.Add(txBaseName, HandleTx);

            Subscribe(BaseProperty, _txBaseName, "active");
        }

        void SubscribeTx()
        {
            Subscribe(BaseProperty, _txBaseName, "device_type");
            Subscribe(BaseProperty, _txBaseName, "bat_type");
            Subscribe(BaseProperty, _txBaseName, "bat_charging");
            Subscribe(BaseProperty, _txBaseName, "bat_gauge");
            Subscribe(BaseProperty, _txBaseName, "bat_health");
            Subscribe(BaseProperty, _txBaseName, "bat_state");
        }
        void UnsubscribeTx()
        {
            Unsubscribe(false, BaseProperty, _txBaseName, "device_type");
            Unsubscribe(false, BaseProperty, _txBaseName, "bat_type");
            Unsubscribe(false, BaseProperty, _txBaseName, "bat_charging");
            Unsubscribe(false, BaseProperty, _txBaseName, "bat_gauge");
            Unsubscribe(false, BaseProperty, _txBaseName, "bat_health");
            Unsubscribe(false, BaseProperty, _txBaseName, "bat_state");
        }

        void ClearValues()
        {
            DeviceType = eSlMcrDwTxDeviceType.Unknown;
            BatteryType = eSlMcrDwTxBatteryType.Unknown;
            Charging = false;
            BatteryGauge = 0;
            BatteryHealth = 0;
            BatteryLifetime = 0;

            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.DeviceType, DeviceType);
            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryType, BatteryType);
            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Charging, Charging);
            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryGauge, BatteryGauge);
            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryHealth, BatteryHealth);
            TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryLifetime, BatteryLifetime);
        }

        void HandleTx(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "active")
                HandleActive(obj);
            else if (obj.Name == "device_type")
                HandleDeviceType(obj);
            else if (obj.Name == "bat_type")
                HandleBatteryType(obj);
            else if (obj.Name == "bat_charging")
                HandleBatteryCharging(obj);
            else if (obj.Name == "bat_gauge")
                HandleBatteryGauge(obj);
            else if (obj.Name == "bat_health")
                HandleBatteryHealth(obj);
            else if (obj.Name == "bat_lifetime") 
                HandleBatteryLifetime(obj);
            //"bat_lifetime" should be the response of subscribing to "bat_state" when using a rechargable battery, according to the manual.
            //But that is not true. It actually returns a "bat_state"...
            else if (obj.Name == "bat_state")
                HandleBatteryLifetime(obj);
            else if (obj.Name == "warnings")
                HandleWarnings(obj);
        }

        void HandleActive(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (Active != value)
            {
                Active = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Active, value);
                if (Active)
                {
                    SubscribeTx();
                }
                else
                {
                    UnsubscribeTx();
                    ClearValues();
                }
            }
        }
        void HandleDeviceType(JContainer json)
        {
            var obj = (JProperty)json;
            var strValue = obj.Value.ToString();

            eSlMcrDwTxDeviceType value = eSlMcrDwTxDeviceType.Unknown;
            if (strValue == "HANDHELD")
                value = eSlMcrDwTxDeviceType.Handheld;
            else if (strValue == "BODYPACK")
                value = eSlMcrDwTxDeviceType.Bodypack;
            else if (strValue == "TABLE-STAND")
                value = eSlMcrDwTxDeviceType.Tablestand;
            else if (strValue == "BOUNDARY")
                value = eSlMcrDwTxDeviceType.Boundary;

            if (DeviceType != value)
            {
                DeviceType = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.DeviceType, value);
            }
        }
        void HandleBatteryType(JContainer json)
        {
            var obj = (JProperty)json;
            var strValue = obj.Value.ToString(); 

            eSlMcrDwTxBatteryType value = eSlMcrDwTxBatteryType.Unknown;
            if (strValue == "BATTERY")
                value = eSlMcrDwTxBatteryType.Battery;
            else if (strValue == "RECHARGEABLE")
                value = eSlMcrDwTxBatteryType.Rechargable;
            
            if (BatteryType != value)
            {
                BatteryType = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryType, value);
            }
        }
        void HandleBatteryCharging(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (Charging != value)
            {
                Charging = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Charging, value);
            }
        }
        void HandleBatteryGauge(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (BatteryGauge != value)
            {
                BatteryGauge = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryGauge, value);
            }
        }
        void HandleBatteryHealth(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (BatteryHealth != value)
            {
                BatteryHealth = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryHealth, value);
            }
        }
        void HandleBatteryLifetime(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>();
            if (BatteryLifetime != value)
            {
                BatteryLifetime = value;
                TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.BatteryLifetime, value);
            }
        }
        void HandleWarnings(JContainer json)
        {
            var obj = (JArray)json.First;
            var value = String.Join(", ", obj.Select(e => e.Value<string>()).ToArray());
            if (Warnings != value)
            {
                Warnings = value;
                var ev = Events;
                if (ev != null)
                    ev(this, new SlMcrDwTxEventArgs(SlMcrDwTxEventArgs.eSlMcrDwTxEventType.Warnings, value));
            }
        }

        void TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwTxEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType type, double value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwTxEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType type, eSlMcrDwTxDeviceType value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwTxEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwTxEventArgs.eSlMcrDwTxEventType type, eSlMcrDwTxBatteryType value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwTxEventArgs(type, value));
        }

    }
}