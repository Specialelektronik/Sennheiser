using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using Newtonsoft.Json.Linq;

namespace Specialelektronik.Products.Sennheiser
{
    public class SlMcrDwRxHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "rxY" where Y is the current handler index + 1.
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SlMcrDwRxEventArgs> Events;

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

        /// <summary>
        /// If EnableRfQualityFeedback() has been called, this will contain the current RF connection quality with the transmitter. 1.0 = 100%. 0.0 = 0%
        /// </summary>
        public double RfQuality { get; private set; }

        /// <summary>
        /// The warning message shown on the frontpanel of the device. 
        /// Example: "Bad Link".
        /// </summary>
        public string Warnings { get; private set; }

        /// <summary>
        /// The RFPI of this channel
        /// Example: "028112cf28"
        /// </summary>
        public string Rfpi { get; private set; }

        /// <summary>
        ///  The IPEI of the (last) connected portable device
        ///  Example: "028110a938"
        /// </summary>
        public string LastPairedIpei { get; private set; }
        /*
        eSldwRxMuteMode _muteMode;
        public eSldwRxMuteMode MuteMode
        {
            get { return _muteMode; }
            set
            {
                Send((int)value, BaseProperty, "mute_mode");
            }
        }
        bool _muteState = false;
        public bool MuteState
        {
            get { return _muteState; }
            set
            {
                Send(value, BaseProperty, "mute_state");
            }
        }
        */

        public SlMcrDwRxHandler(SscCommon common, string baseProperty)
            : base(common, baseProperty)
        {
            Handlers.Add("identification", HandleIdentify);
            Handlers.Add("rf_quality", HandleRfQuality);
            Handlers.Add("warnings", HandleWarnings);
            Handlers.Add("rfpi", HandleRfpi);
            Handlers.Add("last_paired_ipei", HandleLastPairedIpei);
            //Handlers.Add("mute_mode", HandleMuteMode);
            //Handlers.Add("mute_state", HandleMuteState);

            Subscribe(BaseProperty, "identification", "visual");
            //Subscribe(BaseProperty, "rfpi");                      //This should be subscribable when looking at the manual, but it throws an error.
            Subscribe(BaseProperty, "last_paired_ipei");
            //Subscribe(BaseProperty, "mute_mode");
            //Subscribe(BaseProperty, "mute_state");
        }

        /// <summary>
        /// Enables subscription to RF quality feedback. When enabled the Sldw.RxHandler.RfQuality property will be continuosly updated. This will also cause the Sldw.RxHandler.Events event to trig whenever there's a change. The reason you have to manually enable this is because the device is quite ”chatty” so if you don't use this feature all that traffic is unneccesary.
        /// </summary>
        public void EnableRfQualityFeedback()
        {
            Subscribe(BaseProperty, "rf_quality");
        }
        /// <summary>
        /// Disables the RF quality feedback
        /// </summary>
        public void DisableRfQualityFeedback()
        {
            Unsubscribe(true, BaseProperty, "rf_quality");
        }

        void PollRfpi()
        {
            Send(null, BaseProperty, "rfpi");
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
                    TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Identify, value);
                }
            }
        }
        void HandleRfQuality(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (RfQuality != value)
            {
                RfQuality = value;
                TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType.RfQuality, RfQuality);
            }
        }
        void HandleWarnings(JContainer json)
        {
            var obj = (JArray)json.First;
            var value = String.Join(", ", obj.Select(e => e.Value<string>()).ToArray());
            if (Warnings != value)
            {
                Warnings = value;
                TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Warnings, value);
            }
        }
        void HandleRfpi(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (Rfpi != value)
            {
                Rfpi = value;
                TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType.Rfpi, value);
            }
        }
        void HandleLastPairedIpei(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (LastPairedIpei != value)
            {
                LastPairedIpei = value;
                TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType.LastPairedIpei, value);
                
                PollRfpi(); //Workaround because subscribing to Rfpi is not working.
            }
        }
        /*
        void HandleMuteMode(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<eSldwRxMuteMode>();
            if (_muteMode != value)
            {
                _muteMode = value;
                TrigEvent(SldwRxEventArgs.eSldwRxEventType.MuteMode, _muteMode);
            }
        }
        void HandleMuteState(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (_muteState != value)
            {
                _muteState = value;
                TrigEvent(SldwRxEventArgs.eSldwRxEventType.MuteState, _muteState);
            }
        }
        */

        void TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwRxEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType type, double value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwRxEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwRxEventArgs.eSlMcrDwRxEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwRxEventArgs(type, value));
        }
    }
}