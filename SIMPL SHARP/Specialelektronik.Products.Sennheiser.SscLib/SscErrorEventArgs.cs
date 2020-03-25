using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specialelektronik.Products.Sennheiser.SscLib
{
    /// <summary>
    /// Event args for the Error event
    /// </summary>
    public class SscErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The raw JSON data returned from the device.
        /// WARNING: When Sennheiser sends error messages, they sometimes have malformed JSON, so they can't be parsed as JSON.
        /// </summary>
        public string Json { get; private set; }

        /// <summary>
        /// Event args for the Error event
        /// </summary>
        public SscErrorEventArgs(string json)
        {
            Json = json;
        }
    }
}
