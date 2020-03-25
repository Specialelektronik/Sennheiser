using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;


namespace Specialelektronik.Products.Sennheiser.SscLib
{
    class UdpTransport
    {
        public bool Debug { get; set; }

        UDPServer _client;
        Encoding _encoding = Encoding.GetEncoding("latin1");

        Action<string> _commandReceivedAction;
        byte _delimiter;

        bool _connected;

        string _ip;

        CrestronQueue<string> _queue = new CrestronQueue<string>(25);
        object _workHandle;
        
        public UdpTransport(Action<string> commandReceivedAction, byte delimiter)
        {
            _commandReceivedAction = commandReceivedAction;
            _delimiter = delimiter;

            _workHandle = CrestronInvoke.BeginInvoke(HandleQueue);
        }
        public void Connect(string ip, int port)
        {
            if (_client != null)
                _client.Dispose();

            _ip = ip;
            _client = new UDPServer();

            var result = _client.EnableUDPServer(ip, 0, port);
            if (result != SocketErrorCodes.SOCKET_OK)
                ErrorLog.Error("Sennheiser - Error when enabling udp listener for ip {0}, port: {1} :: {2}", ip, port, result);
            else if (Debug)
                CrestronConsole.PrintLine("Sennheiser - UDP Client enabled for ip {0}, remotePort: {1}", ip, port);

            _connected = true;
            _client.ReceiveDataAsync(DataReceivedCallback);
        }
        public void Disconnect()
        {
            _connected = false;
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
        void DataReceivedCallback(UDPServer client, int numberOfBytesReceived)
        {
            if (numberOfBytesReceived == 0)
                return;

            try
            {
                if (client.IPAddressLastMessageReceivedFrom == _ip)
                {
                    byte[] bytes = new byte[numberOfBytesReceived];
                    Array.Copy(_client.IncomingDataBuffer, bytes, numberOfBytesReceived);

                    if (Debug)
                        CrestronConsole.PrintLine("Sennheiser - {0} - Received command: {1}",
                                                  client.IPAddressLastMessageReceivedFrom,
                                                  new string(bytes.Select(b => (char)b).ToArray()).TrimEnd('\0'));

                    var index = -1;
                    while ((index = Array.IndexOf(bytes, _delimiter, index + 1)) >= 0)
                    {
                        var cmdBytes = bytes.Take(index + 1);
                        var cmd = new string(cmdBytes.Select(b => (char)b).ToArray());

                        _commandReceivedAction(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Sennheiser - " + client.IPAddressLastMessageReceivedFrom + " - Exception when receiving data.", ex);
            }
            if (_client != null)
                _client.ReceiveDataAsync(DataReceivedCallback);
        }

        public void Send(string data)
        {
            if (_connected)
                _queue.TryToEnqueue(data);
        }

        public void Dispose()
        {
            Disconnect();
            _queue.TryToEnqueue(null);
        }

        void HandleQueue(object o)
        {
            while (true)
            {
                try
                {
                    var data = _queue.Dequeue();
                    if (data == null)
                        return;

                    if (Debug)
                        CrestronConsole.PrintLine("Sennheiser - {0} - Sending data: {1}", _client.AddressToAcceptConnectionFrom, data);

                    if (_connected)
                    {
                        var bytes = _encoding.GetBytes(data);
                        _client.SendData(bytes, bytes.Length);
                        CrestronEnvironment.Sleep(25);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Error("Sennheiser - Exception in UdpTransport.HandleQueue: " + ex.Message);
                }
            }
        }
    }
}
