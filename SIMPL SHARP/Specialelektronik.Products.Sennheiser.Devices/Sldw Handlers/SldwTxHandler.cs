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
    public class SldwTxHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "mates".
        /// </summary>
        protected override string BaseProperty { get { return "mates"; } }

        /// <summary>
        /// The available device types
        /// </summary>
        public enum eSldwTxDeviceType
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
        public enum eSldwTxBatteryType
        {
            Unknown = -1,
            Battery = 0,
            Rechargable = 1,
        }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SldwTxEventArgs> Events;

        /// <summary>
        /// This is true when a transmitter (such as the handmic or bodypack) is turned on and connected to the device.
        /// </summary>
        public bool Active { get; private set; }
        /// <summary>
        /// The currently connected transmitter type.
        /// </summary>
        public eSldwTxDeviceType DeviceType { get; private set; }
        /// <summary>
        /// The currently connected transmitters battery type.
        /// </summary>
        public eSldwTxBatteryType BatteryType { get; private set; }
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

        /// <summary>
        /// Contains features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
        /// </summary>
        public SldwTxHandler(SscCommon common)
            : base(common)
        {
            Warnings = "";
            DeviceType = eSldwTxDeviceType.Unknown;
            BatteryType = eSldwTxBatteryType.Unknown;

            Handlers.Add("active", HandleActive);
            Handlers.Add("tx1", HandleTx);

            Subscribe(BaseProperty, "active");
        }

        void SubscribeTx1()
        {
            Subscribe(BaseProperty, "tx1", "device_type");
            Subscribe(BaseProperty, "tx1", "bat_type");
            Subscribe(BaseProperty, "tx1", "bat_charging");
            Subscribe(BaseProperty, "tx1", "bat_gauge");
            Subscribe(BaseProperty, "tx1", "bat_health");
            Subscribe(BaseProperty, "tx1", "bat_state");
        }
        void UnsubscribeTx1()
        {
            Unsubscribe(false, BaseProperty, "tx1", "device_type");
            Unsubscribe(false, BaseProperty, "tx1", "bat_type");
            Unsubscribe(false, BaseProperty, "tx1", "bat_charging");
            Unsubscribe(false, BaseProperty, "tx1", "bat_gauge");
            Unsubscribe(false, BaseProperty, "tx1", "bat_health");
            Unsubscribe(false, BaseProperty, "tx1", "bat_state");
        }
        void ClearValues()
        {
            DeviceType = eSldwTxDeviceType.Unknown;
            BatteryType = eSldwTxBatteryType.Unknown;
            Charging = false;
            BatteryGauge = 0;
            BatteryHealth = 0;
            BatteryLifetime = 0;

            TrigEvent(SldwTxEventArgs.eSldwTxEventType.DeviceType, DeviceType);
            TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryType, BatteryType);
            TrigEvent(SldwTxEventArgs.eSldwTxEventType.Charging, Charging);
            TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryGauge, BatteryGauge);
            TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryHealth, BatteryHealth);
            TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryLifetime, BatteryLifetime);
        }


        void HandleActive(JContainer json)
        {
            var obj = (JArray)json.First;
            var value = obj.Count > 0;
            if (Active != value)
            {
                Active = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.Active, value);
                if (Active)
                {
                    SubscribeTx1();
                }
                else
                {
                    UnsubscribeTx1();
                    ClearValues();
                }
            }
        }
        void HandleTx(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            
            if (obj.Name == "device_type")
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
            else if (obj.Name == "warnings")
                HandleWarnings(obj);
            
        }
        void HandleDeviceType(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<eSldwTxDeviceType>();
            if (DeviceType != value)
            {
                DeviceType = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.DeviceType, value);
            }
        }
        void HandleBatteryType(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<eSldwTxBatteryType>();
            if (BatteryType != value)
            {
                BatteryType = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryType, value);
            }
        }
        void HandleBatteryCharging(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (Charging != value)
            {
                Charging = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.Charging, value);
            }
        }
        void HandleBatteryGauge(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (BatteryGauge != value)
            {
                BatteryGauge = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryGauge, value);
            }
        }
        void HandleBatteryHealth(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (BatteryHealth != value)
            {
                BatteryHealth = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryHealth, value);
            }
        }
        void HandleBatteryLifetime(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>();
            if (BatteryLifetime != value)
            {
                BatteryLifetime = value;
                TrigEvent(SldwTxEventArgs.eSldwTxEventType.BatteryLifetime, value);
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
                    ev(this, new SldwTxEventArgs(SldwTxEventArgs.eSldwTxEventType.Warnings, value));
            }
        }

        void TrigEvent(SldwTxEventArgs.eSldwTxEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwTxEventArgs(type, value));
        }
        void TrigEvent(SldwTxEventArgs.eSldwTxEventType type, double value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwTxEventArgs(type, value));
        }
        void TrigEvent(SldwTxEventArgs.eSldwTxEventType type, eSldwTxDeviceType value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwTxEventArgs(type, value));
        }
        void TrigEvent(SldwTxEventArgs.eSldwTxEventType type, eSldwTxBatteryType value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SldwTxEventArgs(type, value));
        }
    }
}