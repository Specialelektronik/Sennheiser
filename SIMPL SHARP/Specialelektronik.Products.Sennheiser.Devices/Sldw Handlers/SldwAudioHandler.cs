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
    /// Contains features, properties and events regarding the receiving end, such as Output gain and EQ.
    /// </summary>
    public class SldwAudioHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "audio".
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        /// <summary>
        /// The available audio output gain levels.
        /// </summary>
        public enum eSldwAudioOutputGain
        {
            Unknown = -1,
            Minus24Db = 0,
            Minus18Db = 1,
            Minus12Db = 2,
            Minus6Db = 3,
            ZeroDb = 4,
            Plus6Db = 5,
            Plus12Db = 6,
        }
        /// <summary>
        /// The available EQ settings.
        /// </summary>
        public enum eSldwAudioEq
        {
            Unknown = -1,
            Off = 0,
            FemaleSpeech = 1,
            MaleSpeech = 2,
            Media = 3,
            Custom = 4,
        }
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SldwAudioEventArgs> Events;

        eSldwAudioOutputGain _outputGain;
        /// <summary>
        /// Sets or gets the output.
        /// </summary>
        public eSldwAudioOutputGain OutputGain
        {
            get { return _outputGain; }
            set
            {
                Send((int)value, BaseProperty, "out1", "gain_db");
            }
        }
        eSldwAudioEq _eq;
        /// <summary>
        /// Sets or gets the currently selected eq.
        /// </summary>
        public eSldwAudioEq Eq
        {
            get { return _eq; }
            set
            {
                Send((int)value, BaseProperty, "equalizer", "preset");
            }
        }
        bool _lowCut = false;
        /// <summary>
        /// Sets or gets if the Low Cut equalizer feature is enabled. It removes the bass frequencies in the audio. Low cut is enabled if true.
        /// </summary>
        public bool LowCut
        {
            get { return _lowCut; }
            set
            {
                Send(value, BaseProperty, "low_cut");
            }
        }

        /// <summary>
        /// Contains features, properties and events regarding the receiving end, such as Output gain and EQ.
        /// </summary>
        public SldwAudioHandler(SscCommon common)
            : base(common, "audio")
        {
            Handlers.Add("out1", HandleOutput);
            Handlers.Add("equalizer", HandleEq);
            Handlers.Add("low_cut", HandleLowCut);

            Subscribe(BaseProperty, "out1", "gain_db");
            Subscribe(BaseProperty, "equalizer", "preset");
            Subscribe(BaseProperty, "low_cut");
        }

        void HandleOutput(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name != "gain_db")
                return;

            var value = obj.Value.ToObject<eSldwAudioOutputGain>();
            if (_outputGain != value)
            {
                _outputGain = value;
                var ev = Events;
                if (ev != null)
                    ev(this, new SldwAudioEventArgs(SldwAudioEventArgs.eSldwAudioEventType.OutputGain, value));
            }
        }
        void HandleEq(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name != "preset")
                return;

            var value = obj.Value.ToObject<eSldwAudioEq>();
            if (_eq != value)
            {
                _eq = value;
                var ev = Events;
                if (ev != null)
                    ev(this, new SldwAudioEventArgs(SldwAudioEventArgs.eSldwAudioEventType.Eq, value));
            }
        }
        void HandleLowCut(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (_lowCut != value)
            {
                _lowCut = value;
                var ev = Events;
                if (ev != null)
                    ev(this, new SldwAudioEventArgs(SldwAudioEventArgs.eSldwAudioEventType.LowCut, value));
            }
        }
    }
}