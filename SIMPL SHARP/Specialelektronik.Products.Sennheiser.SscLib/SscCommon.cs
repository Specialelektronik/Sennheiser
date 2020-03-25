using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// This is the base class that all devices derive from.
    /// </summary>
    public class SscCommon : IDisposable
    {
        /// <summary>
        /// Will trig when there are error messages from the device
        /// </summary>
        public event EventHandler<SscErrorEventArgs> Errors;
        /// <summary>
        /// Will trig when the device starts or stops respoding.
        /// </summary>
        public event EventHandler<SscRespondingEventArgs> Responding;
        /// <summary>
        /// This will trig whenever there is incoming data from the device. The use case for this would be to extend the functionality of the library.
        /// </summary>
        public event EventHandler<SscIncomingCommandEventArgs> IncomingCommand;

        /// <summary>
        /// Returns true as long as the device is responding. As the protocol uses UDP there is no connection state, 
        /// so it might take up to a minute before responding goes low after the device has stopped responding. Listen to the Responding event to know when this changes.
        /// </summary>
        public bool IsResponding { get; private set; }

        bool _debug;
        /// <summary>
        /// Enables debug messages to be printed to the text console while set to true. Make sure this is not left on when not used.
        /// </summary>
        public bool Debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                _transport.Debug = value;
            }
        }

        int _subscriptionTimeSeconds = 60;
        int _subscriptionTimerMs = 29000;
        List<string[]> _subscriptions = new List<string[]>();
        CTimer _subscriptionTimer;
        CTimer _responseTimer;
        
        UdpTransport _transport;

        internal Dictionary<string, ABaseHandler> Handlers = new Dictionary<string, ABaseHandler>();

        /// <summary>
        /// This is the base class that all devices derive from.
        /// </summary>
        /// <param name="delimiter">Different Sennheiser use different delimiters. This should be the delimiter that the particular product uses.</param>
        public SscCommon(byte delimiter)
        {
            _responseTimer = new CTimer(ResponseTimerExpired, Timeout.Infinite);
            _transport = new UdpTransport(CommandReceivedCallback, delimiter);

            new DeviceHandler(this);
        }

        /// <summary>
        /// Opens up the connection to the device.
        /// </summary>
        /// <param name="ip">Ip number of the device.</param>
        /// <param name="port">Default is 45</param>
        public void Connect(string ip, int port)
        {
            _transport.Connect(ip, port);
            ResubscribeAll();
        }
        /// <summary>
        /// Opens up the connection to the device. Uses default port 45.
        /// </summary>
        public void Connect(string ip)
        {
            Connect(ip, 45);
        }
        /// <summary>
        /// Stops the connection to the device.
        /// </summary>
        public void Disconnect()
        {
            _transport.Disconnect();
        }

        internal void AddHandler(string property, ABaseHandler handler)
        {
            Handlers[property] = handler;
        }
        
        void CommandReceivedCallback(string data)
        {
            _responseTimer.Reset(_subscriptionTimerMs * 2);
            if (IsResponding != true)
            {                
                IsResponding = true;
                TrigRespondingEvent(true);
            }

            var evIc = IncomingCommand;
            if (evIc != null)
            {
                var args = new SscIncomingCommandEventArgs(data);
                evIc(this, args);
                if (args.Handled)
                    return;
            }
            // There is a bug in some of the error responses from Sennheiser that genereates an invalid json
            // This is a workaround to not crash the json parser.
            if (data.Contains("\"error\":"))
            {
                var ev = Errors;
                if (ev != null)
                    ev(this, new SscErrorEventArgs(data.Trim('\0', '\r', '\n')));

                return;
            }

            var json = JObject.Parse(data);
            var property = ((JProperty)json.First).Name;

            ABaseHandler handler = null;
            if (Handlers.TryGetValue(property, out handler))
            {
                if (json.First.First.HasValues)
                    handler.HandleResponse((JObject)json.First.Children().First());
                else
                    handler.HandleResponse(json);
            }
        }
        void ResponseTimerExpired(object o)
        {
            if (IsResponding != false)
            {
                IsResponding = false;
                TrigRespondingEvent(false);
            }
        }
        void TrigRespondingEvent(bool responding)
        {
            var ev = Responding;
            if (ev != null)
                ev(this, new SscRespondingEventArgs(responding));
        }

        /// <summary>
        /// Used to send commands to the device.
        /// Example command: "{\"device\":{\"reset\":true}}"
        /// </summary>
        public void Send(string data)
        {
            _transport.Send(data);
        }
        /// <summary>
        /// Used to send commands to the device.
        /// Example: Send("Room_1", "device", "location")
        /// </summary>
        public void Send(object value, params string[] path)
        {
            var root = GetJsonObjectFromPath(value, path);
            _transport.Send(root.ToString(Formatting.None));
        }


        internal void Subscribe(params string[] path)
        {
            var lifetime = new JObject();
            lifetime["lifetime"] = _subscriptionTimeSeconds;

            JObject subRoot = GetJsonObjectFromPath(null, lifetime, path);

            var arr = new JArray();
            arr.Add(subRoot);

            if (!_subscriptions.Any(s => s.SequenceEqual(path)))
                _subscriptions.Add(path);

            Send(arr, "osc", "state", "subscribe");

            if (_subscriptionTimer == null)
                _subscriptionTimer = new CTimer(SubscriptionTimerExpired, _subscriptionTimerMs);
        }
        internal void Unsubscribe(bool sendCancel, params string[] path)
        {
            var cancel = new JObject();
            cancel["cancel"] = true;

            JObject subRoot = GetJsonObjectFromPath(null, cancel, path);

            var arr = new JArray();
            arr.Add(subRoot);

            _subscriptions.RemoveAll(s => s.SequenceEqual(path));

            if (sendCancel)
                Send(arr, "osc", "state", "subscribe");
        }
        void ResubscribeAll()
        {
            foreach (var sub in _subscriptions)
                Subscribe(sub);
        }
        void SubscriptionTimerExpired(object o)
        {
            try
            {
                ResubscribeAll();
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Sennheiser - Exception in SscCommon.SubscriptionTimerExpired()", ex);
            }
            if (_subscriptionTimer != null)
                _subscriptionTimer.Reset(_subscriptionTimerMs);
        }
        JObject GetJsonObjectFromPath(object value, params string[] path)
        {
            return GetJsonObjectFromPath(value, null, path);
        }
        JObject GetJsonObjectFromPath(object value, object hashObject, params string[] path)
        {
            // The hash object (#) (mainly used for subscribe/unsubscribe) has to come as the first parameter of the object, 
            // otherwise it will not work

            JObject root = new JObject();
            JObject current = root;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var newObj = new JObject();
                current[path[i]] = newObj;
                current = newObj;
            }
            if (hashObject != null)
                current["#"] = (JToken)hashObject;

            JToken jtokenValue = null;
            if (value != null)
                jtokenValue = JToken.FromObject(value);
            
            current[path[path.Length - 1]] = jtokenValue;

            return root;
        }

        /// <summary>
        /// Used to clean up timers and connections. This must be called when your program stops.
        /// </summary>
        public void Dispose()
        {
            _transport.Dispose();
            if (_subscriptionTimer != null)
            {
                _subscriptionTimer.Dispose();
                _subscriptionTimer = null;
            }
        }
    }
}
