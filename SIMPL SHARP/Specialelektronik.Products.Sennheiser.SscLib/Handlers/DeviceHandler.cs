using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// The base DeviceHandler. All other DeviceHandlers derive from this class.
    /// </summary>
    public class DeviceHandler : ABaseHandler
    {
        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<DeviceEventArgs> Events;

        /// <summary>
        /// Returns the base property of this handler, which is "device".
        /// </summary>
        protected override string BaseProperty { get { return base.BaseProperty; } }

        string _name = "";
        /// <summary>
        /// Sets or gets the name of the device. Max 8 chars.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                Send(value, BaseProperty, "name");
            }
        }
        /// <summary>
        /// Gets the firmware version of the device. Example: 1.1.0.
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        /// Gets the serial number of the device. Example: 1234567890.
        /// </summary>
        public string Serial { get; private set; }
        /// <summary>
        /// Gets the product name of the device. Example: CHG4N
        /// </summary>
        public string Product { get; private set; }
        /// <summary>
        /// Gets the mac adresses of the device. Example: 00:1B:66:11:22:33 or 00:1B:66:11:22:33,00:1B:66:22:33:44.
        /// </summary>
        public string MacAddresses { get; private set; }

        /// <summary>
        /// The base DeviceHandler. All other DeviceHandlers derive from this class.
        /// </summary>
        public DeviceHandler(SscCommon common)
            : base(common, "device")
        {
            Handlers.Add("name", HandleName);
            Handlers.Add("identity", HandleIdentity);
            Handlers.Add("network", HandleNetwork);

            Subscribe(BaseProperty, "name");
        }

        /// <summary>
        /// Asks the device about Version, Serial, Product and MacAddresses. 
        /// This is automatically done on connection, so your should not need to do this manually.
        /// </summary>
        public virtual void PollInfo()
        {
            Send(null, BaseProperty, "identity", "version");
            Send(null, BaseProperty, "identity", "serial");
            Send(null, BaseProperty, "identity", "product");
            Send(null, BaseProperty, "network", "ether", "macs");
        }

        /// <summary>
        /// This method will be called as soon as the device responds after being offline.
        /// </summary>
        protected override void PollOnResponse()
        {
            base.PollOnResponse();
            PollInfo();
        }
        /// <summary>
        /// This method will be called when an incoming command with the path device/identity arrives.
        /// </summary>
        protected virtual void HandleIdentity(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "version")
            {
                var value = obj.Value.ToString();
                if (Version != value)
                {
                    Version = value;
                    TrigEvent(DeviceEventArgs.eDeviceEventType.Version, value);
                }
            }
            else if (obj.Name == "serial")
            {
                var value = obj.Value.ToString();
                if (Serial != value)
                {
                    Serial = value;
                    TrigEvent(DeviceEventArgs.eDeviceEventType.Serial, value);
                }
            }
            else if (obj.Name == "product") 
            {
                var value = obj.Value.ToString();
                if (Product != value)
                {
                    Product = value;
                    TrigEvent(DeviceEventArgs.eDeviceEventType.Product, value);
                }
            }
        }
        /// <summary>
        /// This method will be called when an incoming command with the path device/network arrives.
        /// </summary>
        protected virtual void HandleNetwork(JContainer json)
        {
            var obj = (JProperty)json.First.First;
            if (obj.Name == "ether")
            {
                var etherJson = (JObject)obj.First;
                if (((JProperty)etherJson.First).Name == "macs")
                {
                    var addresses = String.Join(",", etherJson["macs"].ToObject<string[]>());
                    if (MacAddresses != addresses)
                    {
                        MacAddresses = addresses;
                        TrigEvent(DeviceEventArgs.eDeviceEventType.MacAddresses, MacAddresses);
                    }
                }
            }
        }
        /// <summary>
        /// This method will be called when an incoming command with the path device/name arrives.
        /// </summary>
        protected virtual void HandleName(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToString();
            if (_name != value)
            {
                _name = value;
                TrigEvent(DeviceEventArgs.eDeviceEventType.Name, value);
            }
        }

        /// <summary>
        /// A method to trigger the Events event.
        /// </summary>
        protected virtual void TrigEvent(DeviceEventArgs.eDeviceEventType type, string value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new DeviceEventArgs(type, value));
        }
    }
}
