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
    public class SscRespondingEventArgs : EventArgs
    {
        /// <summary>
        /// Is true if the device is responding.
        /// </summary>
        public bool Responding { get; private set; }
        /// <summary>
        /// Event args for the IncomingCommand event
        /// </summary>
        public SscRespondingEventArgs(bool responding)
        {
            Responding = responding;
        }
    }
}