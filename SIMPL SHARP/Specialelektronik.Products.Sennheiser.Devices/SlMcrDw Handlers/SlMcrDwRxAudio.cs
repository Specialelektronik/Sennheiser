using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace Specialelektronik.Products.Sennheiser
{
    public class SlMcrDwRxAudio
    {
        /// <summary>
        /// The available EQ settings.
        /// </summary>
        public enum eSlMcrDwAudioEq
        {
            Unknown = -1,
            Off = 0,
            FemaleSpeech = 1,
            MaleSpeech = 2,
            Media = 3,
            Custom = 4,
        }

        /// <summary>
        /// The index of the Rx device as found in SlMcrDw.AudioHandler.RxAudio or SlMcrDw.RxHandlers
        /// </summary>
        public int Index { get; private set; }

        SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain _outputGain;
        /// <summary>
        /// Sets or gets the output.
        /// </summary>
        public SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain OutputGain
        {
            get { return _outputGain; }
            set
            {
                _handler.RxSend((int)value, _rxBaseProperty, "gain");
            }
        }

        eSlMcrDwAudioEq _eq;
        /// <summary>
        /// Sets or gets the currently selected eq.
        /// </summary>
        public eSlMcrDwAudioEq Eq
        {
            get { return _eq; }
            set
            {
                _handler.RxSend(GetAudioEq(value), _rxBaseProperty, "equalizer", "preset");
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
                _handler.RxSend(value, _rxBaseProperty, "low_cut");
            }
        }

        SlMcrDwAudioHandler _handler;
        string _rxBaseProperty;

        internal SlMcrDwRxAudio(SlMcrDwAudioHandler handler, int rxIndex)
        {
            _handler = handler;
            _rxBaseProperty = "rx" + (rxIndex + 1);

            _handler.RxSubscribe(_rxBaseProperty, "gain");
            _handler.RxSubscribe(_rxBaseProperty, "equalizer", "preset");
            _handler.RxSubscribe(_rxBaseProperty, "low_cut");
        }

        internal void HandleRx(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "gain")
                HandleOutput(obj);
            else if (obj.Name == "equalizer")
                HandleEq(obj);
            else if (obj.Name == "low_cut")
                HandleLowCut(obj);
        }

        void HandleOutput(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<SlMcrDwAudioHandler.eSlMcrDwAudioOutputGain>();
            if (_outputGain != value)
            {
                _outputGain = value;
                _handler.TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxOutputGain, this);
            }
        }
        void HandleEq(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name != "preset")
                return;

            var value = GetAudioEq(obj.Value.ToString());
            if (_eq != value)
            {
                _eq = value;
                _handler.TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxEq, this);
            }
        }
        void HandleLowCut(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (_lowCut != value)
            {
                _lowCut = value;
                _handler.TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.RxLowCut, this);
            }
        }

        eSlMcrDwAudioEq GetAudioEq(string value)
        {
            if (value == "OFF")
                return eSlMcrDwAudioEq.Off;
            else if (value == "FEMALE_SPEECH")
                return eSlMcrDwAudioEq.FemaleSpeech;
            else if (value == "MALE_SPEECH")
                return eSlMcrDwAudioEq.MaleSpeech;
            else if (value == "MEDIA")
                return eSlMcrDwAudioEq.Media;
            else if (value == "CUSTOM")
                return eSlMcrDwAudioEq.Custom;
            else
                return eSlMcrDwAudioEq.Unknown;

        }
        string GetAudioEq(eSlMcrDwAudioEq value)
        {
            switch (value)
            {
                case eSlMcrDwAudioEq.Off:
                    return "OFF";
                case eSlMcrDwAudioEq.FemaleSpeech:
                    return "FEMALE_SPEECH";
                case eSlMcrDwAudioEq.MaleSpeech:
                    return "MALE_SPEECH";
                case eSlMcrDwAudioEq.Media:
                    return "MEDIA";
                case eSlMcrDwAudioEq.Custom:
                    return "CUSTOM";
                default:
                    return null;
            }
        }
    }
}