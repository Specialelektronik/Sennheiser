using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// Event args for the IncomingCommand event
    /// </summary>
    public class SscIncomingCommandEventArgs : EventArgs
    {
        /// <summary>
        /// The JSON data that was received.
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// If you set this to `true` then this command will not be handled by the library.
        /// </summary>
        public bool Handled { get; set; }
        /// <summary>
        /// Event args for the IncomingCommand event
        /// </summary>
        public SscIncomingCommandEventArgs(string command)
        {
            Command = command;
        }
    }
}