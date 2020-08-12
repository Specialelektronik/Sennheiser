using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Sennheiser_Modules_CSharp
{
    public delegate void ShortDelegate(short value);
    public delegate void ShortDelegateIndex(ushort index, short value);
    public delegate void UShortDelegate(ushort value);
    public delegate void UShortDelegateIndex(ushort index, ushort value);
    public delegate void StringDelegate(SimplSharpString value);
    public delegate void StringDelegateIndex(ushort index, SimplSharpString value);

    public class SimplWrapperBase<T> where T : SscCommon
    {
        public UShortDelegate SetRespondingFb { get; set; }
        public StringDelegate SetNameFb { get; set; }
        public StringDelegate SetVersionFb { get; set; }
        public StringDelegate SetSerialFb { get; set; }
        public StringDelegate SetProductFb { get; set; }
        public StringDelegate SetMacAddressesFb { get; set; }
        public StringDelegate SetIncomingCommandFb { get; set; }

        public string Ip { get; set; }
        public ushort Port { get; set; }

        public ushort Debug
        {
            set
            {
                Device.Debug = value > 0;
            }
        }
        ushort _incomingCommandEnabled = 0;
        public ushort IncomingCommandEnabled
        {
            set
            {
                if (_incomingCommandEnabled != value)
                {
                    _incomingCommandEnabled = value;
                    if (value > 0)
                        Device.IncomingCommand += new EventHandler<SscIncomingCommandEventArgs>(_device_IncomingCommand);
                    else
                        Device.IncomingCommand -= new EventHandler<SscIncomingCommandEventArgs>(_device_IncomingCommand);
                }
            }
        }

        protected T Device;

        public SimplWrapperBase()
        {

        }
        public SimplWrapperBase(T device)
        {
            Init(device);
        }
        protected void Init(T device)
        {
            Device = device;
            Device.Responding += new EventHandler<SscRespondingEventArgs>(_device_Responding);
        }

        public void Connect()
        {
            Device.Connect(Ip, Port);
        }
        public void Disconnect()
        {
            Device.Disconnect();
        }

        public void Send(string data)
        {
            Device.Send(data);
        }

        //Event handlers
        void CrestronEnvironment_ProgramStatusEventHandler(eProgramStatusEventType programEventType)
        {
            if (programEventType == eProgramStatusEventType.Stopping && Device != null)
                Device.Dispose();
        }
        void _device_Responding(object sender, SscRespondingEventArgs e)
        {
            SetRespondingFb(Convert.ToUInt16(e.Responding));
        }
        void _device_IncomingCommand(object sender, SscIncomingCommandEventArgs e)
        {
            SetIncomingCommandFb(e.Command);
        }
    }
}