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
    /// Contains features, properties and events regarding the audio output, such as Dante output gain and Mute.
    /// </summary>
    public class Tcc2AudioHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "audio".
        /// </summary>
        protected override string BaseProperty { get { return "audio"; } }
        
        /// <summary>
        /// The available speaker detection threshold levels.
        /// </summary>
        public enum eSpeakerDetectionThreshold
        {
            Unknown,
            QuietRoom,
            NormalRoom,
            LoudRoom,
        }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<Tcc2AudioEventArgs> Events;

        bool _exclusionZoneActive = false;

        /// <summary>
        /// Sets or gets if exclusion zones are active in the device. Exclusion zones are areas where the microphone should not be listening. These are configured in the device settings.
        /// </summary>
        public bool ExclusionZoneActive
        {
            get { return _exclusionZoneActive; }
            set
            {
                Send(value, BaseProperty, "exclusion_zone", "active");
            }
        }
        /// <summary>
        /// The mac addresses of the Dante outputs. This returns both addresses separated with a comma.
        /// Example: 00:1B:66:44:55:66,00:1B:66:77:88:99.
        /// </summary>
        public string DanteMacAddresses { get; private set; }
        /// <summary>
        /// The ip addresses of the Dante outputs. This returns both addresses separated with a comma. If there is no network cable connected or no addresses set, this might return a string only containing a comma. 
        /// Example: 192.168.10.2,192.168.10.3.
        /// </summary>
        public string DanteIpAddresses { get; private set; }
        int _danteOutputGain;
        /// <summary>
        /// Sets or gets the current Dante output gain in dB.
        /// Value between 0 - 24. Values are dB
        /// </summary>
        public int DanteOutputGain
        {
            get { return _danteOutputGain; }
            set
            {
                Send(value, BaseProperty, "out2", "gain");
            }
        }
        eSpeakerDetectionThreshold _speakerDetectionThreshold;
        /// <summary>
        /// Sets or gets the currently selected sensitivity of the speaker detection.
        /// </summary>
        public eSpeakerDetectionThreshold SpeakerDetectionThreshold
        {
            get { return _speakerDetectionThreshold; }
            set
            {
                Send(GetSpeakerDetectionThreshold(value), BaseProperty, "source_detection", "threshold");
            }
        }
        bool _mute;
        /// <summary>
        /// Sets or gets if the audio output is muted. true is muted.
        /// </summary>
        public bool Mute
        {
            get { return _mute; }
            set
            {
                Send(value, BaseProperty, "mute");
            }
        }

        /// <summary>
        /// Contains features, properties and events regarding the audio output, such as Dante output gain and Mute.
        /// </summary>
        public Tcc2AudioHandler(SscCommon common)
            : base(common)
        {
            Handlers.Add("exclusion_zone", HandleExclusionZone);
            Handlers.Add("out2", HandleDante);
            Handlers.Add("source_detection", HandleSourceDetection);
            Handlers.Add("mute", HandleMute);

            Subscribe(BaseProperty, "exclusion_zone", "active");
            Subscribe(BaseProperty, "out2", "gain");
            Subscribe(BaseProperty, "source_detection", "threshold");
            Subscribe(BaseProperty, "mute");
        }

        /// <summary>
        /// This method will be called as soon as the device responds after being offline.
        /// </summary>
        protected override void PollOnResponse()
        {
            base.PollOnResponse();
            Send(null, BaseProperty, "out2", "network", "ether", "macs");
            Send(null, BaseProperty, "out2", "network", "ipv4", "ipaddr");
        }

        void HandleExclusionZone(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "active")
            {
                var value = obj.Value.ToObject<bool>();
                if (_exclusionZoneActive != value)
                {
                    _exclusionZoneActive = value;
                    TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType.ExclusionZoneActive, value);
                }
            }
        }
        void HandleDante(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "network")
                HandleDanteNetwork(obj);
            else if (obj.Name == "gain")
                HandleDanteGain(obj);

            
        }
        void HandleDanteNetwork(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "ether")
            {
                obj = (JProperty)obj.First.First;
                if (obj.Name == "macs")
                {
                    var addresses = String.Join(",", obj.Value.ToObject<string[]>());
                    if (DanteMacAddresses != addresses)
                    {
                        DanteMacAddresses = addresses;
                        TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType.DanteMacAddresses, addresses);
                    }
                }
            }
            else if (obj.Name == "ipv4")
            {
                obj = (JProperty)obj.First.First;
                if (obj.Name == "ipaddr")
                {
                    var addresses = String.Join(",", obj.Value.ToObject<string[]>());
                    if (DanteIpAddresses != addresses)
                    {
                        DanteIpAddresses = addresses;
                        TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType.DanteIpAddresses, addresses);
                    }
                }
            }
        }
        void HandleDanteGain(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>();
            if (_danteOutputGain != value)
            {
                _danteOutputGain = value;
                TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType.DanteOutputGain, value);
            }
        }
        void HandleSourceDetection(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "threshold")
            {
                var value = GetSpeakerDetectionThreshold(obj.Value.ToObject<string>());
                if (_speakerDetectionThreshold != value)
                {
                    _speakerDetectionThreshold = value;
                    var ev = Events;
                    if (ev != null)
                        ev(this, new Tcc2AudioEventArgs(Tcc2AudioEventArgs.eTcc2AudioEventType.SpeakerDetectionThreshold, value));
                }
            }
        }
        void HandleMute(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<bool>();
            if (_mute != value)
            {
                _mute = value;
                TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType.Mute, value);
            }
        }
        eSpeakerDetectionThreshold GetSpeakerDetectionThreshold(string value)
        {
            if (value == "quiet_room")
                return eSpeakerDetectionThreshold.QuietRoom;
            else if (value == "normal_room")
                return eSpeakerDetectionThreshold.NormalRoom;
            else if (value == "loud_room")
                return eSpeakerDetectionThreshold.LoudRoom;
            else
                return eSpeakerDetectionThreshold.Unknown;
        }
        string GetSpeakerDetectionThreshold(eSpeakerDetectionThreshold value)
        {
            switch (value)
            {
                case eSpeakerDetectionThreshold.QuietRoom:
                    return "quiet_room";
                case eSpeakerDetectionThreshold.NormalRoom:
                    return "normal_room";
                case eSpeakerDetectionThreshold.LoudRoom:
                    return "loud_room";
                default:
                    return null;
            }
        }

        void TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2AudioEventArgs(type, value));
        }
        void TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType type, int value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2AudioEventArgs(type, value));
        }
        void TrigEvent(Tcc2AudioEventArgs.eTcc2AudioEventType type, bool value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2AudioEventArgs(type, value));
        }
    }
}