using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// All handlers must derive from this class.
    /// </summary>
    public abstract class ABaseHandler
    {
        /// <summary>
        /// This is the root level name of the SSC command path to this handler.
        /// Example: The DeviceHandler handles commands where the part starts with "device". The BaseProperty of DeviceHandler is "device".
        /// {"device":{"reset":true}}
        /// </summary>
        protected abstract string BaseProperty { get; }
        /// <summary>
        /// Contains all sub level handlers, where the key is the second level name of the SSC command path.
        /// Example: We have the following command: {"device":{"reset":true}}. We would then add a handler for "reset" that parses and takes care of that.
        /// </summary>
        protected Dictionary<string, Action<JContainer>> Handlers = new Dictionary<string, Action<JContainer>>();

        /// <summary>
        /// This is the reference to the SscCommon that you passed into the constructor.
        /// </summary>
        protected SscCommon Common;

        /// <summary>
        /// All handlers must derive from this class.
        /// </summary>
        public ABaseHandler(SscCommon common)
        {
            Common = common;
            Common.Responding += new EventHandler<SscRespondingEventArgs>(Common_Responding);
            common.AddHandler(BaseProperty, this);
        }
        /// <summary>
        /// This method will be called as soon as the device responds after being offline.
        /// </summary>
        protected virtual void PollOnResponse() { }
        
        /// <summary>
        /// Subscribes to changes for the specified path. The library will automatically resubscribe when needed.
        /// </summary>
        /// <param name="path">The path to the property you want to subscribe to. Example: Device name would be Subscribe("device", "name")</param>
        public void Subscribe(params string[] path)
        {
            Common.Subscribe(path);
        }
        /// <summary>
        /// Removes the subscription for the specified path.
        /// </summary>
        /// <param name="sendCancel">Set to true if you want a unsubscribe command to be sent to the device. If this is false, this will eventually be unsubscribed as we dont resubscribe.</param>
        /// <param name="path">The path to the property you want to subscribe to. Example: Device name would be Subscribe("device", "name")</param>
        public void Unsubscribe(bool sendCancel, params string[] path)
        {
            Common.Unsubscribe(sendCancel, path);
        }

        /// <summary>
        /// Used to send commands to the device.
        /// Example command: "{\"device\":{\"reset\":true}}"
        /// </summary>
        protected void Send(string data)
        {
            Common.Send(data);
        }
        /// <summary>
        /// Used to send commands to the device.
        /// Example: Send("Room_1", "device", "location")
        /// </summary>
        protected void Send(object value, params string[] path)
        {
            Common.Send(value, path);
        }

        internal void HandleResponse(JObject json)
        {
            var property = ((JProperty)json.First).Name;

            Action<JContainer> handler = null;
            if (Handlers.TryGetValue(property, out handler))
            {
                handler((JContainer)json.First);
            }
        }

        //Event handlers
        void Common_Responding(object sender, SscRespondingEventArgs e)
        {
            PollOnResponse();
        }
    }
}
