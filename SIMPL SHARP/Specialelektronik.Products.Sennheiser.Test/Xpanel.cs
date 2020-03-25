using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;
using Specialelektronik.Products.Sennheiser.SscLib;

namespace Specialelektronik.Products.Sennheiser.Test
{
    public class Xpanel
    {
        XpanelForSmartGraphics _xpanel;
        XpanelChargerPage _chargerPage;
        XpanelReceiverPage _receiverPage;
        XpanelTccPage _tccPage;

        public Xpanel(uint ipId)
        {
            _xpanel = new XpanelForSmartGraphics(ipId, ControlSystem.Instance);
            
            if (_xpanel.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                ErrorLog.Error("Xpanel could not register: " + _xpanel.RegistrationFailureReason);

            _chargerPage = new XpanelChargerPage(_xpanel);
            _receiverPage = new XpanelReceiverPage(_xpanel);
            _tccPage = new XpanelTccPage(_xpanel);
        }

    }
}