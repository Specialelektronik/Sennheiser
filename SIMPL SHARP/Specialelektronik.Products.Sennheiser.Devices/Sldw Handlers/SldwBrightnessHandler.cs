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
    /// This class is for internal use only.
    /// It handles the brightness change, but that is found in the DeviceHandler
    /// </summary>
    public class SldwBrightnessHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "brightness".
        /// </summary>
        protected override string BaseProperty { get { return "brightness"; } }

        /// <summary>
        /// This will be trigged when the brightness changed.
        /// </summary>
        public event EventHandler BrightnessChanged;

        double _brightness;
        /// <summary>
        /// Sets or gets the current brightness of the frontpanel display.
        /// </summary>
        public double Brightness
        {
            get { return _brightness; }
            set
            {
                Send((int)(value * 100), BaseProperty);
            }
        }
        /// <summary>
        /// This class is for internal use only.
        /// </summary>
        public SldwBrightnessHandler(SscCommon common)
            : base(common)
        {
            Handlers.Add("brightness", HandleBrightness);

            Subscribe(BaseProperty);
        }

        void HandleBrightness(JContainer json)
        {
            var obj = (JProperty)json;
            var value = obj.Value.ToObject<int>() / 100.0;
            if (_brightness != value)
            {
                _brightness = value;
                var ev = BrightnessChanged;
                if (ev != null)
                    ev(this, EventArgs.Empty);
            }
        }
    }
}