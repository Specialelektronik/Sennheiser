using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Specialelektronik.Products.Sennheiser
{
    /// <summary>
    /// Contains features, properties and events regarding the audio output, such as Dante output gain.
    /// </summary>
    public class SlMcrDwAudioHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "audio".
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        /// <summary>
        /// The available audio output gain levels.
        /// </summary>
        public enum eSlMcrDwAudioOutputGain
        {
            Unknown = -1,
            Minus24Db = -24,
            Minus18Db = -18,
            Minus12Db = -12,
            Minus6Db = -6,
            ZeroDb = 0,
            Plus6Db = 6,
            Plus12Db = 12,
        }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<SlMcrDwAudioEventArgs> Events;

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

        eSlMcrDwAudioOutputGain _danteOutputGain;
        /// <summary>
        /// Sets or gets the output.
        /// </summary>
        public eSlMcrDwAudioOutputGain DanteOutputGain
        {
            get { return _danteOutputGain; }
            set
            {
                Send((int)value, BaseProperty, "dante", "mixer", "gain");
            }
        }

        List<SlMcrDwRxAudio> _rxAudio = new List<SlMcrDwRxAudio>();
        public ReadOnlyCollection<SlMcrDwRxAudio> RxAudio { get; private set; }

        /// <summary>
        /// Contains features, properties and events regarding the audio output, such as Dante output gain.
        /// </summary>
        public SlMcrDwAudioHandler(SscCommon common, int numberOfChannels)
            : base(common, "audio")
        {
            Handlers.Add("out2", HandleOut2);
            Handlers.Add("dante", HandleDante);

            for (int i = 0; i < numberOfChannels; i++)
            {
                var rxBaseProperty = "rx" + (i + 1);
                var rxHandler = new SlMcrDwRxAudio(this, i);
                Handlers.Add(rxBaseProperty, rxHandler.HandleRx);
                _rxAudio.Add(rxHandler);
            }

            RxAudio = _rxAudio.AsReadOnly();

            Subscribe(BaseProperty, "dante", "mixer", "gain");
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

        void HandleOut2(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "network")
                HandleDanteNetwork(obj);
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
                        TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteMacAddresses, addresses);
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
                        TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteIpAddresses, addresses);
                    }
                }
            }
        }
        void HandleDante(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "mixer")
            {
                obj = (JProperty)obj.First.First;
                if (obj.Name == "gain")
                {
                    var value = obj.Value.ToObject<eSlMcrDwAudioOutputGain>();
                    if (_danteOutputGain != value)
                    {
                        _danteOutputGain = value;
                        TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType.DanteOutputGain, value);
                    }
                }
            }
        }

        internal void RxSend(object value, params string[] path)
        {
            var completePath = new List<string>() { BaseProperty };
            completePath.AddRange(path);
            Send(value, completePath.ToArray());
        }
        internal void RxSubscribe(params string[] path)
        {
            var completePath = new List<string>() { BaseProperty };
            completePath.AddRange(path);
            Subscribe(completePath.ToArray());
        }

        void TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwAudioEventArgs(type, value));
        }
        internal void TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType type, SlMcrDwRxAudio value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwAudioEventArgs(type, value));
        }
        void TrigEvent(SlMcrDwAudioEventArgs.eSlMcrDwAudioEventType type, eSlMcrDwAudioOutputGain value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new SlMcrDwAudioEventArgs(type, value));
        }
    }
}