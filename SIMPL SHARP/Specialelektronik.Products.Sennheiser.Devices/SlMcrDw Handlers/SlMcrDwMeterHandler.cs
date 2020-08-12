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
    /// Contains features, properties and events regarding meters, such as receiver input level.
    /// </summary>
    public class SlMcrDwMeterHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "m".
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SlMcrDwMeterEventArgs> Events;

        /// <summary>
        /// If EnableRxInputLevelFeedbacks() has been called, this will contain the incoming audio levels on the different channels. A value between -60 and 0.
        /// </summary>
        public int[] RxInputLevels { get; private set; }
        /// <summary>
        /// If EnableMixerLevelFeedback() has been called, this will contain the mixed audio level. A value between -60 and 0.
        /// </summary>
        public int MixerLevel { get; private set; }


        /// <summary>
        /// Contains features, properties and events regarding meters, such as receiver input level.
        /// </summary>
        public SlMcrDwMeterHandler(SscCommon common, int numberOfChannels)
            : base(common, "m")
        {
            RxInputLevels = new int[numberOfChannels];

            Handlers.Add("mixer", HandleMixer);
            for (int i = 0; i < numberOfChannels; i++)
                Handlers.Add("rx" + (i + 1), HandleRx);
        }

        /// <summary>
        /// Enables subscription to mixed audio level feedback. When enabled the SlMcrDw.MeterHandler.MixerLevel property will be continuosly updated. This will also cause the SlMcrDw.MeterHandler.Events event to trig whenever there's a change.
        /// </summary>
        public void EnableMixerLevelFeedback()
        {
            Subscribe(BaseProperty, "mixer", "level");
        }
        /// <summary>
        /// Disables the mixed audio level feedback.
        /// </summary>
        public void DisableMixerLevelFeedback()
        {
            Unsubscribe(true, BaseProperty, "mixer", "level");
        }
        /// <summary>
        /// Enables subscription to incoming audio levels feedback. When enabled the SlMcrDw.MeterHandler.RxInputLevels property will be continuosly updated. This will also cause the SlMcrDw.MeterHandler.Events event to trig whenever there's a change.
        /// </summary>
        public void EnableRxInputLevelFeedbacks()
        {
            for (int i = 0; i < RxInputLevels.Length; i++)
                Subscribe(BaseProperty, "rx" + (i + 1), "level");
        }
        /// <summary>
        /// Disables the mixed audio level feedback.
        /// </summary>
        public void DisableRxInputLevelFeedbacks()
        {
            for (int i = 0; i < RxInputLevels.Length; i++)
                Unsubscribe(true, BaseProperty, "rx" + (i + 1), "level");
        }

        void HandleMixer(JContainer json)
        {
            var obj = (JProperty)json.First.First;

            if (obj.Name == "level")
            {
                var value = obj.Value.ToObject<int>();
                if (MixerLevel != value)
                {
                    MixerLevel = value;
                    TrigEvent(SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.MixerLevel, value);
                }
            }
        }
        void HandleRx(JContainer json)
        {
            var prop = (JProperty)json;
            var index = int.Parse(prop.Name.Substring(2)) - 1;

            var obj = (JProperty)prop.First.First;

            if (obj.Name == "level")
            {
                var value = obj.Value.ToObject<int>();
                if (RxInputLevels[index] != value)
                {
                    RxInputLevels[index] = value;
                    TrigEvent(SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType.RxInputLevel, index, value);
                }
            }
        }

        void TrigEvent(SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType type, int value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwMeterEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwMeterEventArgs.eSlMcrDwMeterEventType type, int index, int value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwMeterEventArgs(type, index, value));
        }
    }
}