using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Event args for the base DeviceHandler class
    /// </summary>
    public class Chg4NBaysEventArgs : EventArgs
    {
        /// <summary>
        /// The available property changes that can be trigged with this event
        /// </summary>
        public enum eChg4NBayEventType
        {
            Active,
            Charging,
            Serial,
            BatteryGauge,
            BatteryHealth,
            MinutesToFull,
            DeviceType,
        }
        /// <summary>
        /// The property that changed. It might have changed on any or all of the bays.
        /// </summary>
        public eChg4NBayEventType EventType { get; private set; }
        /// <summary>
        /// An array with the available charging bays. The changed property might have changed on any or all of the bays.
        /// </summary>
        public Chg4NBay[] Bays { get; private set; }

        /// <summary>
        /// Event args for the base DeviceHandler class
        /// </summary>
        public Chg4NBaysEventArgs(eChg4NBayEventType type, Chg4NBay[] bays)
        {
            EventType = type;
            Bays = bays;
        }

        /// <summary>
        /// Will print the changed value for all of the bays.
        /// Example: "Chg4NBaysEventArgs - Bays Active, 1: true, 2: false, 3: false, 4: false"
        /// </summary>
        public override string ToString()
        {
            switch (EventType)
            {
                case eChg4NBayEventType.Active:
                    return String.Format("Chg4NBaysEventArgs - Bays Active, 1: {0}, 2: {1}, 3: {2}, 4: {3}", Bays[0].Active, Bays[1].Active, Bays[2].Active, Bays[3].Active);
                case eChg4NBayEventType.Charging:
                    return String.Format("Chg4NBaysEventArgs - Bays Charging, 1: {0}, 2: {1}, 3: {2}, 4: {3}", Bays[0].Charging, Bays[1].Charging, Bays[2].Charging, Bays[3].Charging);
                case eChg4NBayEventType.Serial:
                    return String.Format("Chg4NBaysEventArgs - Bays Serial, 1: {0}, 2: {1}, 3: {2}, 4: {3}", Bays[0].Serial, Bays[1].Serial, Bays[2].Serial, Bays[3].Serial);
                case eChg4NBayEventType.BatteryGauge:
                    return String.Format("Chg4NBaysEventArgs - Bays Battery Gauge, 1: {0}%, 2: {1}%, 3: {2}%, 4: {3}%", Bays[0].BatteryGauge * 100, Bays[1].BatteryGauge * 100, Bays[2].BatteryGauge * 100, Bays[3].BatteryGauge * 100);
                case eChg4NBayEventType.BatteryHealth:
                    return String.Format("Chg4NBaysEventArgs - Bays Battery Health, 1: {0}%, 2: {1}%, 3: {2}%, 4: {3}%", Bays[0].BatteryHealth * 100, Bays[1].BatteryHealth * 100, Bays[2].BatteryHealth * 100, Bays[3].BatteryHealth * 100);
                case eChg4NBayEventType.MinutesToFull:
                    return String.Format("Chg4NBaysEventArgs - Bays Minutes to full, 1: {0} min, 2: {1} min, 3: {2} min, 4: {3} min", Bays[0].MinutesToFull, Bays[1].MinutesToFull, Bays[2].MinutesToFull, Bays[3].MinutesToFull);
                case eChg4NBayEventType.DeviceType:
                    return String.Format("Chg4NBaysEventArgs - Bays Device type, 1: {0}, 2: {1}, 3: {2}, 4: {3}", Bays[0].DeviceType, Bays[1].DeviceType, Bays[2].DeviceType, Bays[3].DeviceType);
                default:
                    return "Chg4NBaysEventArgs - Unknown Event Type";
            }
        }
    }
}
