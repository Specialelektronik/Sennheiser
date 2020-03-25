using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Contains features, properties and events regarding the charging bays and the devices inserted into the bays.
    /// </summary>
    public class Chg4NBaysHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "bays".
        /// </summary>
        protected override string BaseProperty { get { return "bays"; } }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<Chg4NBaysEventArgs> Events;

        Chg4NBay[] _bays = new Chg4NBay[4];
        /// <summary>
        /// The available charging bays.
        /// </summary>
        public Chg4NBay[] Bays { get { return _bays; } }

        /// <summary>
        /// Contains features, properties and events regarding the charging bays and the devices inserted into the bays.
        /// </summary>
        public Chg4NBaysHandler(SscCommon common)
            : base(common)
        {
            for (int i = 0; i < _bays.Length; i++)
                _bays[i] = new Chg4NBay();

            Handlers.Add("active", HandleActive);
            Handlers.Add("serial", HandleSerial);
            Handlers.Add("charging", HandleCharging);
            Handlers.Add("bat_gauge", HandleBatteryGauge);
            Handlers.Add("bat_health", HandleBatteryHealth);
            Handlers.Add("bat_timetofull", HandleBatteryTimeToFull);
            Handlers.Add("device_type", HandleDeviceType);

            Subscribe(BaseProperty, "active");
            Subscribe(BaseProperty, "serial");
            Subscribe(BaseProperty, "charging");
            Subscribe(BaseProperty, "bat_gauge");
            Subscribe(BaseProperty, "bat_timetofull");
            Subscribe(BaseProperty, "bat_health");
            Subscribe(BaseProperty, "device_type");
        }

        void HandleActive(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<bool>();
                if (val != _bays[i].Active)
                {
                    changed = true;
                    _bays[i].Active = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.Active);
        }
        void HandleSerial(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<string>();
                if (val != _bays[i].Serial)
                {
                    changed = true;
                    _bays[i].Serial = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.Serial);
        }
        void HandleCharging(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<bool>();
                if (val != _bays[i].Charging)
                {
                    changed = true;
                    _bays[i].Charging = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.Charging);
        }
        void HandleBatteryGauge(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<int>() / 100.0;
                if (val != _bays[i].BatteryGauge)
                {
                    changed = true;
                    _bays[i].BatteryGauge = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.BatteryGauge);
        }
        void HandleBatteryHealth(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<int>() / 100.0;
                if (val != _bays[i].BatteryHealth)
                {
                    changed = true;
                    _bays[i].BatteryHealth = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.BatteryHealth);
        }
        void HandleBatteryTimeToFull(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<int>();
                if (val != _bays[i].MinutesToFull)
                {
                    changed = true;
                    _bays[i].MinutesToFull = val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.MinutesToFull);
        }
        void HandleDeviceType(JContainer json)
        {
            var obj = (JArray)json.First;
            var changed = false;
            for (int i = 0; i < obj.Count; i++)
            {
                var val = obj[i].Value<int>();
                if (val != (int)_bays[i].DeviceType)
                {
                    changed = true;
                    _bays[i].DeviceType = (Chg4NBay.eBayDeviceType)val;
                }
            }
            if (changed)
                TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType.DeviceType);
        }

        void TrigEvent(Chg4NBaysEventArgs.eChg4NBayEventType type)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Chg4NBaysEventArgs(type, _bays));
        }
    }
}
