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
    /// Contains features, properties and events regarding meters, such as azimuth, elevation and input peak level.
    /// </summary>
    public class Tcc2MeterHandler : ABaseHandler
    {
        /// <summary>
        /// Returns the base property of this handler, which is "m".
        /// </summary>
        protected override string BaseProperty { get { return "m"; } }

        /// <summary>
        /// This will be trigged when any of the properties of this handler changes.
        /// </summary>
        public event EventHandler<Tcc2MeterEventArgs> Events;

        /// <summary>
        /// If EnableElevationFeedback() has been called, this will contain the current the elevation of the speaker in degrees. A value between 0 and 90.
        /// </summary>
        public int ElevationDegrees { get; private set; }
        /// <summary>
        /// If EnableAzimuthFeedback() has been called, this will contain the current the direction of the speaker in degrees. A value between 0 and 359.
        /// </summary>
        public int AzimuthDegrees { get; private set; }
        /// <summary>
        /// If EnableInputPeakLevelFeedback() has been called, this will contain the current input peak level in dB of the mic. A value between -90 and 0.
        /// </summary>
        public int InputPeakLevelDb { get; private set; }

        /// <summary>
        /// Contains features, properties and events regarding meters, such as azimuth, elevation and input peak level.
        /// </summary>
        public Tcc2MeterHandler(SscCommon common)
            : base(common)
        {
            Handlers.Add("beam", HandleBeam);
            Handlers.Add("in1", HandleIn);
        }

        /// <summary>
        /// Enables subscription to Azimuth degrees feedback. When enabled the Tcc2.MeterHandler.AzimuthDegrees property will be continuosly updated. This will also cause the Tcc2.MeterHandler.Events event to trig whenever there's a change.
        /// </summary>
        public void EnableAzimuthFeedback()
        {
            Subscribe(BaseProperty, "beam", "azimuth");
        }
        /// <summary>
        /// Disables the Azimuth degrees feedback.
        /// </summary>
        public void DisableAzimuthFeedback()
        {
            Unsubscribe(true, BaseProperty, "beam", "azimuth");
        }
        /// <summary>
        /// Enables subscription to Elevation degrees feedback. When enabled the Tcc2.MeterHandler.ElevationDegrees property will be continuosly updated. This will also cause the Tcc2.MeterHandler.Events event to trig whenever there's a change.  
        /// </summary>
        public void EnableElevationFeedback()
        {
            Subscribe(BaseProperty, "beam", "elevation");
        }
        /// <summary>
        /// Disables the Elevation degrees feedback.
        /// </summary>
        public void DisableElevationFeedback()
        {
            Unsubscribe(true, BaseProperty, "beam", "elevation");
        }
        /// <summary>
        /// Enables subscription to input peak level feedback. When enabled the Tcc2.MeterHandler.InputPeakLevelDb property will be continuosly updated. This will also cause the Tcc2.MeterHandler.Events event to trig whenever there's a change.
        /// </summary>
        public void EnableInputPeakLevelFeedback()
        {
            Subscribe(BaseProperty, "in1", "peak");              
        }
        /// <summary>
        ///  Disables the input peak level feedback.
        /// </summary>
        public void DisableInputPeakLevelFeedback()
        {
            Unsubscribe(true, BaseProperty, "in1", "peak");
        }

        void HandleBeam(JContainer json)
        {
            var obj = (JProperty)json.First.First;

            if (obj.Name == "elevation")
            {
                var value = obj.Value.ToObject<int>();
                if (ElevationDegrees != value)
                {
                    ElevationDegrees = value;
                    TrigEvent(Tcc2MeterEventArgs.eTcc2MeterEventType.Elevation, value);
                }
            }
            else if (obj.Name == "azimuth")
            {
                var value = obj.Value.ToObject<int>();
                if (ElevationDegrees != value)
                {
                    ElevationDegrees = value;
                    TrigEvent(Tcc2MeterEventArgs.eTcc2MeterEventType.Azimuth, value);
                }
            }
        }
        void HandleIn(JContainer json)
        {
            var obj = (JProperty)json.First.First;

            if (obj.Name == "peak")
            {
                var value = obj.Value.ToObject<int>();
                if (InputPeakLevelDb != value)
                {
                    InputPeakLevelDb = value;
                    TrigEvent(Tcc2MeterEventArgs.eTcc2MeterEventType.InputPeakLevel, value);
                }
            }
        }

        void TrigEvent(Tcc2MeterEventArgs.eTcc2MeterEventType type, int value)
        {
            var ev = Events;
            if (ev != null)
                ev(this, new Tcc2MeterEventArgs(type, value));
        }
    }
}