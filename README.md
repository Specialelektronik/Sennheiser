Unofficial
# Sennheiser SIMPL+ Modules and Simpl Sharp Pro libraries
Developed by Niklas Olsson - JaDeVa AB

- [Sennheiser SIMPL+ Modules and Simpl Sharp Pro libraries](#sennheiser-simpl--modules-and-simpl-sharp-pro-libraries)
  * [How to download the SIMPL+ modules and SIMPL Demo program](#how-to-download-the-simpl--modules-and-simpl-demo-program)
  * [Summary](#summary)
  * [Dependencies](#dependencies)
  * [Important notes and Known issues](#important-notes-and-known-issues)
  * [Getting started](#getting-started)
  * [CHG 4N](#chg-4n)
    + [Quickstart](#quickstart)
    + [Handlers](#handlers)
    + [Methods](#methods)
      - [DeviceHandler methods](#devicehandler-methods)
    + [Properties](#properties)
      - [DeviceHandler properties](#devicehandler-properties)
      - [BayHandler properties](#bayhandler-properties)
    + [Events](#events)
      - [DeviceHandler events](#devicehandler-events)
      - [BaysHandler events](#bayshandler-events)
  * [SL Rack Receiver DW](#sl-rack-receiver-dw)
    + [Quickstart](#quickstart-1)
    + [Handlers](#handlers-1)
    + [Methods](#methods-1)
      - [DeviceHandler methods](#devicehandler-methods-1)
      - [RxHandler methods](#rxhandler-methods)
    + [Properties](#properties-1)
      - [DeviceHandler properties](#devicehandler-properties-1)
      - [RxHandler properties](#rxhandler-properties)
      - [TxHandler properties](#txhandler-properties)
      - [AudioHandler properties](#audiohandler-properties)
    + [Events](#events-1)
      - [DeviceHandler events](#devicehandler-events-1)
      - [RxHandler events](#rxhandler-events)
      - [TxHandler events](#txhandler-events)
      - [AudioHandler events](#audiohandler-events)
  * [TeamConnect Ceiling 2](#teamconnect-ceiling-2)
    + [Quickstart](#quickstart-2)
    + [Handlers](#handlers-2)
    + [Methods](#methods-2)
      - [DeviceHandler methods](#devicehandler-methods-2)
      - [MeterHandler methods](#meterhandler-methods)
    + [Properties](#properties-2)
      - [DeviceHandler properties](#devicehandler-properties-2)
      - [MeterHandler properties](#meterhandler-properties)
      - [AudioHandler properties](#audiohandler-properties-1)
    + [Events](#events-2)
      - [DeviceHandler events](#devicehandler-events-2)
      - [MeterHandler events](#meterhandler-events)
      - [AudioHandler events](#audiohandler-events-1)
  * [Release notes](#release-notes)
    + [1.0.0 (Initial version)](#100--initial-version-)


## How to I download the SIMPL+ modules and SIMPL Demo program
1. Press the green **Clone or download** button in the top right, and select **Download ZIP**
2. Open up the file and open the **SIMPL** directory.
3. Use the `SennheiserDemo_CP3_compiled.zip`

## Summary
This repository contains

* SIMPL+ modules
    * `Sennheiser_CHG4N_x.x.x_SE` - Module to control CHG 4N
    * `Sennheiser_SLDW_x.x.x_SE` - Module to control SL Rack Receiver DW
    * `Sennheiser_TCC2_x.x.x_SE` - Module to control TeamConnect Ceiling 2
* SimplSharp (C#) solution containing projects for both SIMPL+ modules and Simpl Sharp Pro
    * `Specialelektronik.Products.Sennheiser.Devices`
        * This project contains the library for controlling the following products
            * [CHG 4N](#chg-4n)
            * [SL Rack Receiver DW](#sl-rack-receiver-dw)
            * [TeamConnect Ceiling 2](#teamconnect-ceiling-2)
    * `Specialelektronik.Products.Sennheiser.SscLib`
        * This project contains the base library for all Sennheiser modules. This is a good starting point to make a driver for future products that utilizes the Sennheiser SSC protocol. It handles the connections and provides some initial parsing.
    * `Specialelektronik.Products.Sennheiser.Test`
        * This is a S#Pro Demo program. Load this program to your processor and use the Xpanel  `Sennheiser_Xpanel.vtp` to test the libraries.
    * `Sennheiser_Modules_CSharp`
        * This project contains wrapper classes that is used for the SIMPL+ modules.
* SIMPL Windows Demo program
* VTPro-E project to be used with both Simpl Sharp Pro and SIMPL Windows demo programs.
* Documentation for all SIMPL+ modules, as well as this README as a PDF

This readme is mainly focused on information on how to use the classes in `Specialelektronik.Products.Sennheiser.Devices`.

## Dependencies
You need to add the following reference to you project
```
SimplSharpNewtonsoft
```

## Important notes and Known issues

* When Sennheiser sends error messages, they sometimes have malformed JSON, so they can't be parsed as JSON. Because of that all errors from the devices are return to you as a string with the JSON in it. If you want to act based on an error you have to subscribe to the `Errors` event of the device and parse the data yourself.

## Getting started
Start up your S#Pro project and do the following:

* Add references to the dependencies shown in the [Dependencies](#dependencies) section of this readme.
* Add a reference to `Specialelektronik.Products.Sennheiser.Devices.dll`.

## CHG 4N
Class name: `Chg4N`

This class integrates with Sennheiser CHG-4N, a battery charger for Sennheiser Handmics and Bodypacks.  

### Quickstart
```csharp
// Instantiate the device
var device = new Chg4N();

// Subscribe to events
device.BaysHandler.Events += new EventHandler<Chg4NBaysEventArgs>(BaysHandler_Events);
device.DeviceHandler.Events += new EventHandler<Chg4NDeviceEventArgs>(DeviceHandler_Events);

// Connect to the device
device.Connect("192.168.10.135");

// DeviceHandler event handler
void DeviceHandler_Events(object sender, Chg4NDeviceEventArgs e)
{
    switch (e.EventType)
    {
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Name:
            string name = e.StringValue;
            break;
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Group:
            string group = e.StringValue;
            break;
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Version:
            string version = e.StringValue;
            break;
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Serial:
            string serial = e.StringValue;
            break;
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.Product:
            string product = e.StringValue;
            break;
        case Chg4NDeviceEventArgs.eChg4NDeviceEventType.MacAddresses:
            string macAddresses = e.StringValue;
            break;
    }
}

// BaysHandler event handler
void BaysHandler_Events(object sender, Chg4NBaysEventArgs e)
{
    //First bay is index 0, second is 1 etc.
    switch (e.EventType)
    {
        case Chg4NBaysEventArgs.eChg4NBayEventType.Active:
            bool active = e.Bays[0].Active; 
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.Charging:
            bool charging = e.Bays[0].Charging; 
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.Serial:
            string serial = e.Bays[0].Serial; 
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryGauge:
            double gauge = e.Bays[0].BatteryGauge; // A value between 0.0 and 1.0
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.BatteryHealth:
            double health = e.Bays[0].BatteryHealth; // A value between 0.0 and 1.0
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.MinutesToFull:
            int minutes = e.Bays[0].MinutesToFull; 
            break;
        case Chg4NBaysEventArgs.eChg4NBayEventType.DeviceType:
            Chg4NBay.eBayDeviceType type = e.Bays[0].DeviceType;
            break;
    }
}
```

### Handlers
The device's properties, methods and events are located in a few different locations. This is completely based on how Sennheiser has decided to do in their protocol.
On the root class `Chg4N` you will find some (see below) but you also the following handlers where you will find more.

* `DeviceHandler` - Contains things regarding the device. This is found in `Chg4N.DeviceHandler`.
* `BaysHandler` - Contains things regarding the charging bays and the devices inserted into the bays. This is found in `Chg4N.BaysHandler`.

### Methods

* `Connect(string ip)` - Opens up the connection to the device. Uses default port 45.
* `Connect(string ip, int port)` - Opens up the connection to the device.
* `Disconnect` - Stops the connection to the device.
* `Send(string data)` - Used as a way to send your own commands. Refer to the Sennheiser Sound Control Protocol (SSC). Example command: `{"device":{"reset":true}}`. 
* `Send(object value, params string[] path)` - Used as a way to send your own commands. An example would be `Send("name", "device", "location")`.
* `Dispose()` - Used to clean up timers and connections. This must be called when your program stops.

#### DeviceHandler methods
Located in `Chg4N.DeviceHandler`.

* `PollInfo()` - Asks the device about `Version`, `Serial`, `Product` and `MacAddresses`. This is automatically done on connection, so your should not need to do this manually.

### Properties

* `IsResponding` - Returns `true` as long as the device is responding. As the protocol uses UDP there is no connection state, so it might take up to a minute before responding goes low after the device has stopped responding. Listen to the `Responding` event to know when this changes.
* `Debug` - Enables debug messages to be printed to the text console while set to `true`. Make sure this is not left on when not used.
* `DeviceHandler` - Returns the device handler, where you have features, properties and events regarding the device.
* `BaysHandler` - Returns the bays handler, where you have features, properties and events regarding the charging bays and the devices inserted into the bays.

#### DeviceHandler properties
Located in `Chg4N.DeviceHandler`.

* `Name` - Sets or gets the name of the device. Max 8 chars.
* `Version` - The firmware version of the device. Example: 1.1.0.
* `Serial` - The serial number of the device. Example: 1234567890.
* `Product` - The product name of the device. Example: CHG4N.
* `MacAddresses` - The mac adresses of the device. Example: 00:1B:66:11:22:33.
* `Group` - Sets or gets the group (location) of the device.

#### BayHandler properties
Located in `Chg4N.BaysHandler`.

* `Bays` - An array of the available charging bays. Each bay has a set of properties.
    * `Active` - Returns `true` if there is a device inserted into the bay.
    * `Charging` - Returns `true` if the inserted device is charging.
    * `Serial` - Serial number of the inserted device.
    * `BatteryGauge` - This is the level of power in the battery. `1.0` = Full. `0.0` = Empty.
    * `BatteryHealth` - This is the health of the in the battery. `1.0` = Perfect condition. `0.0` = Very bad, or no device inserted.
    * `MinutesToFull` - Returns the number of minutes left until device is fully charged.
    * `DeviceType` - Returns the device type. Possible types are `Handheld` or `Bodypack`.

### Events

* `Errors` - Will trig when there are error messages from the device. The event args contains:
    * `Json` - The raw JSON data returned from the device. Warning! See [Important notes and known issues](#important-notes-and-known-issues)
* `Responding` - Will trig when the device starts or stops respoding. The event args contains:
    * `Responding` - Is `true` if the device is responding.
* `IncomingCommand` - This will trig whenever there is incoming data from the device. The use case for this would be to extend the functionality of the library. The event args contains:
    * `Command` - The JSON data that was received.
    * `Handled` - If you set this to `true` then this command will not be handled by the library.

#### DeviceHandler events
Located in `Chg4N.DeviceHandler`.

* `Events` - This will trig when any of the [DeviceHandler properties](#devicehandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `StringValue` - The new value of the changed property.

#### BaysHandler events
Located in `Chg4N.BaysHandler`.

* `Events` - This will trig when any of the [BaysHandler bays properties](#bayshandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `Bays` - An array with the available charging bays. The changed property might have changed on any or all of the bays.

## SL Rack Receiver DW
Class name: `Sldw`

This class integrates with Sennheiser SpeechLine Digital Wireless (SLDW), a wireless microphone system.  

### Quickstart
```csharp
// Instantiate the device
var device = new Sldw();

// Subscribe to events
device.Errors += new EventHandler<SscErrorEventArgs>(Errors);
device.DeviceHandler.Events += new EventHandler<SldwDeviceEventArgs>(DeviceHandler_Events);
device.RxHandler.Events += new EventHandler<SldwRxEventArgs>(RxHandler_Events);
device.TxHandler.Events += new EventHandler<SldwTxEventArgs>(TxHandler_Events);
device.AudioHandler.Events += new EventHandler<SldwAudioEventArgs>(AudioHandler_Events);

// Enable RfQuality feedback reporting
device.RxHandler.EnableRfQualityFeedback();

// Connect to the device
device.Connect("192.168.10.135");

// DeviceHandler event handler
void DeviceHandler_Events(object sender, SldwDeviceEventArgs e)
{
    switch (e.EventType)
    {
        case SldwDeviceEventArgs.eSldwDeviceEventType.Name:
            string name = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.Group:
            string group = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.Version:
            string version = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.Serial:
            string serial = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.Product:
            string product = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.MacAddresses:
            string macAddresses = e.StringValue;
            break;
        case SldwDeviceEventArgs.eSldwDeviceEventType.Brightness:
            double brightness = e.Brightness;
            break;
    }
}

// RxHandler event handler
void RxHandler_Events(object sender, SldwRxEventArgs e)
{
    switch (e.EventType)
    {
        case SldwRxEventArgs.eSldwRxEventType.Identify:
            bool identifying = e.BoolValue;
            break;
        case SldwRxEventArgs.eSldwRxEventType.RfQuality:
            double rfQuality = e.DoubleValue; // A value between 0.0 and 1.0
            break;
        case SldwRxEventArgs.eSldwRxEventType.MuteSwitchActive:
            bool muteSwitchActive = e.BoolValue;
            break;
        case SldwRxEventArgs.eSldwRxEventType.Warnings:
            string warning = e.StringValue;
            break;
    }
}

void TxHandler_Events(object sender, SldwTxEventArgs e)
{
    switch (e.EventType)
    {
        case SldwTxEventArgs.eSldwTxEventType.Active:
            var active = e.BoolValue;
            break;
        case SldwTxEventArgs.eSldwTxEventType.DeviceType:
            SldwTxHandler.eSldwTxDeviceType deviceType = e.DeviceType;
            break;
        case SldwTxEventArgs.eSldwTxEventType.BatteryType:
            SldwTxHandler.eSldwTxBatteryType batteryType = e.BatteryType;
            break;
        case SldwTxEventArgs.eSldwTxEventType.Charging:
            bool charging = e.BoolValue;
            break;
        case SldwTxEventArgs.eSldwTxEventType.BatteryGauge:
            double batteryGauge = e.DoubleValue; // A value between 0.0 and 1.0
            break;
        case SldwTxEventArgs.eSldwTxEventType.BatteryHealth:
            double batteryHealth = e.DoubleValue; // A value between 0.0 and 1.0
            break;
        case SldwTxEventArgs.eSldwTxEventType.BatteryLifetime:
            double batteryLifetimeInMinutes = e.DoubleValue;
            break;
        case SldwTxEventArgs.eSldwTxEventType.Warnings:
            string warning = e.StringValue;
            break;
    }
}

void AudioHandler_Events(object sender, SldwAudioEventArgs e)
{
    switch (e.EventType)
    {
        case SldwAudioEventArgs.eSldwAudioEventType.OutputGain:
            SldwAudioHandler.eSldwAudioOutputGain outputGain = e.OutputGain;
            break;
        case SldwAudioEventArgs.eSldwAudioEventType.Eq:
            SldwAudioHandler.eSldwAudioEq eq = e.Eq;
            break;
        case SldwAudioEventArgs.eSldwAudioEventType.LowCut:
            bool lowCut = e.LowCut;
            break;
    }
}
```

### Handlers
The device's properties, methods and events are located in a few different locations. This is completely based on how Sennheiser has decided to do in their protocol.
On the root class `Sldw` you will find some (see below) but you also the following handlers where you will find more.

* `DeviceHandler` - Contains things regarding the device. This is found in `Sldw.DeviceHandler`.
* `RxHandler` - Contains things regarding the receiving end, such as RF quality. This is found in `Sldw.RxHandler`.
* `TxHandler` - Contains things regarding the transmitting end (the microphone or bodypack), such as battery level. This is found in `Sldw.TxHandler`.
* `AudioHandler` - Contains things regarding the receiving end, such as Output gain and EQ. This is found in `Sldw.AudioHandler`.

### Methods

* `Connect(string ip)` - Opens up the connection to the device. Uses default port 45.
* `Connect(string ip, int port)` - Opens up the connection to the device.
* `Disconnect` - Stops the connection to the device.
* `Send(string data)` - Used as a way to send your own commands. Refer to the Sennheiser Sound Control Protocol (SSC). Example command: `{"device":{"reset":true}}`. 
* `Send(object value, params string[] path)` - Used as a way to send your own commands. An example would be `Send("name", "device", "location")`.
* `Dispose()` - Used to clean up timers and connections. This must be called when your program stops.

#### DeviceHandler methods
Located in `Sldw.DeviceHandler`.

* `PollInfo()` - Asks the device about `Version`, `Serial`, `Product` and `MacAddresses`. This is automatically done on connection, so your should not need to do this manually.

#### RxHandler methods
Located in `Sldw.RxHandler`.

* `EnableRfQualityFeedback()` - Enables subscription to RF quality feedback. When enabled the `Sldw.RxHandler.RfQuality` property will be continuosly updated. This will also cause the `Sldw.RxHandler.Events` event to trig whenever there's a change. The reason you have to manually enable this is because the device is quite ”chatty” so if you don't use this feature all that traffic is unneccesary. 
* `DisableRfQualityFeedback()` - Disables the RF quality feedback (see `EnableRfQualityFeedback()`).

### Properties

* `IsResponding` - Returns `true` as long as the device is responding. As the protocol uses UDP there is no connection state, so it might take up to a minute before responding goes low after the device has stopped responding. Listen to the `Responding` event to know when this changes.
* `Debug` - Enables debug messages to be printed to the text console while set to `true`. Make sure this is not left on when not used.
* `DeviceHandler` - Returns the device handler, where you have features, properties and events regarding the device.
* `RxHandler` - Returns the RX handler, where you have features, properties and events regarding the receiving end, such as RF quality.
* `TxHandler` - Returns the TX handler, where you have features, properties and events regarding the transmitting end (the microphone or bodypack), such as battery level.
* `AudioHandler` - Returns the audio handler, where you have features, properties and events regarding the receiving end, such as Output gain and EQ.

#### DeviceHandler properties
Located in `Sldw.DeviceHandler`.

* `Name` - Sets or gets the name of the device. Max 8 chars.
* `Version` - The firmware version of the device. Example: 1.1.0.
* `Serial` - The serial number of the device. Example: 1234567890.
* `Product` - The product name of the device. Example: CHG4N.
* `MacAddresses` - The mac adresses of the device. Example: 00:1B:66:11:22:33.
* `Group` - Sets or gets the group (location) of the device.
* `Brightness` - Sets or gets the brightness of the frontpanel. `1.0` = 100%. `0.0` = 0%

#### RxHandler properties
Located in `Sldw.RxHandler`.

* `Identify` - Sets or gets if the identify feature of the device is enabled. It blinks a LED on the frontpanel when `true`.
* `MuteSwitchActive` - Sets or gets if the possibility to use the mute button on the transmitting device (such as the handmic or bodypack) should be possible. If `true`, the mute button on the transmitting device will be functional. 
* `RfQuality` - If `EnableRfQualityFeedback()` has been called, this will contain the current RF connection quality with the transmitter. `1.0` = 100%. `0.0` = 0%
* `Warnings` - The warning message shown on the frontpanel of the device.  Example: `"Bad Link"`.

#### TxHandler properties
Located in `Sldw.TxHandler`.

* `Active` - This is `true` when a transmitter (such as the handmic or bodypack) is turned on and connected to the device.
* `DeviceType` - The currently connected transmitter type. Possible values are `Handheld`, `Bodypack`, `Tablestand` and `Boundary`.
* `BatteryType` - The currently connected transmitters battery type. Possible values are `Battery` and `Rechargable`.
* `Charging` - This is `true` when a transmitter is charging while it is on and connected. This will not work when charging a handmic or bodypack in the CHG-4N, as it will then disconnect from the device.
* `BatteryGauge` - The currently connected transmitters battery level. `1.0` = Full. `0.0` = Empty.
* `BatteryHealth` - The currently connected transmitters battery health level. `1.0` = Perfect condition. `0.0` = Very bad
* `BatteryLifetime` - The currently connected transmitters battery lifetime in minutes. Lifetime means before you have to replace the rechargable battery with a new one, not until the current charge is depleted. This only works if you have a rechargable battery.
* `Warnings` - The warning message shown on the frontpanel of the transmitter.  Example: `"Low Bat"`.

#### AudioHandler properties
Located in `Sldw.AudioHandler`.

* `OutputGain` - Sets or gets the output gain as an enum. The values range from -24dB to 12dB in steps of 6dB.
* `Eq` - Sets or gets the currently selected eq. Possible valus are `Off`, `FemaleSpeech`, `MaleSpeech`, `Media` and `Custom`.
* `LowCut` - Sets or gets if the Low Cut equalizer feature is enabled. It removes the bass frequencies in the audio. Low cut is enabled if `true`.

### Events

* `Errors` - Will trig when there are error messages from the device. The event args contains:
    * `Json` - The raw JSON data returned from the device. Warning! See [Important notes and known issues](#important-notes-and-known-issues)
* `Responding` - Will trig when the device starts or stops respoding. The event args contains:
    * `Responding` - Is `true` if the device is responding.
* `IncomingCommand` - This will trig whenever there is incoming data from the device. The use case for this would be to extend the functionality of the library. The event args contains:
    * `Command` - The JSON data that was received.
    * `Handled` - If you set this to `true` then this command will not be handled by the library.

#### DeviceHandler events
Located in `Sldw.DeviceHandler`.

* `Events` - This will trig when any of the [DeviceHandler properties](#devicehandler-properties-1) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `StringValue` - The new value of the changed property.
    * `Brightness` - The new value of brightness. `1.0` = 100%. `0.0` = 0%

#### RxHandler events
Located in `Sldw.RxHandler`.

* `Events` - This will trig when any of the [RxHandler properties](#rxhandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `BoolValue` - Contains the new value of the property for event types `Identify` and `MuteSwitchActive`.
    * `DoubleValue` - Contains the new value of the property for event type `RfQuality`.
    * `StringValue` - Contains the new value of the property for event type `Warnings`.

#### TxHandler events
Located in `Sldw.TxHandler`.

* `Events` - This will trig when any of the [TxHandler properties](#txhandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `BoolValue` - Contains the new value of the property for event types `Active` and `Charging`.
    * `DoubleValue` - Contains the new value of the property for event types `BatteryGauge`, `BatteryHealth` and `BatteryLifetime`.
    * `StringValue` - Contains the new value of the property for event type `Warnings`.
    * `DeviceType` - Contains the new value of the property for event type `DeviceType`.
    * `BatteryType` - Contains the new value of the property for event type `BatteryType`.

#### AudioHandler events
Located in `Sldw.AudioHandler`.

* `Events` - This will trig when any of the [AudioHandler properties](#audiohandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `LowCut` -  - Contains the new value of the property for event type `LowCut`.
    * `OutputGain` -  - Contains the new value of the property for event type `OutputGain`.
    * `Eq` -  - Contains the new value of the property for event type `Eq`.  

## TeamConnect Ceiling 2
Class name: `Tcc2`

This class integrates with Sennheiser TeamConnect Ceiling 2, a microphone mounted in the ceiling of the room. 

### Quickstart
```csharp
// Instantiate te device
var device = new Tcc2();

// Subscribe to events
device.DeviceHandler.Events += new EventHandler<Tcc2DeviceEventArgs>(ChargerDeviceHandler_Events);
device.MeterHandler.Events += new EventHandler<Tcc2MeterEventArgs>(MeterHandler_Events);
device.AudioHandler.Events += new EventHandler<Tcc2AudioEventArgs>(AudioHandler_Events);

// Enable feedback reporting
device.MeterHandler.EnableAzimuthFeedback();
device.MeterHandler.EnableElevationFeedback();
device.MeterHandler.EnableInputPeakLevelFeedback();

// Connect to the device
device.Connect("192.168.10.135");

// DeviceHandler event handler
void DeviceHandler_Events(object sender, Tcc2DeviceEventArgs e)
{
    switch (e.EventType)
    {
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Name:
            string name = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Location:
            string group = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Version:
            string version = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Serial:
            string serial = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Product:
            string product = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MacAddresses:
            string macAddresses = e.StringValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.Identify:
            bool identifying = e.BoolValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedColor:
            Tcc2DeviceHandler.eLedColor color = e.LedColor;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.CustomLedActive:
            bool customLedActive = e.BoolValue;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicMuteLedColor:
            Tcc2DeviceHandler.eLedColor color = e.LedColor;
            break;  
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.MicOnLedColor:
            Tcc2DeviceHandler.eLedColor color = e.LedColor;
            break;
        case Tcc2DeviceEventArgs.eTcc2DeviceEventType.LedBrightness:
            int ledBrightness = e.IntValue; // A value between 0 and 5
            break;
    }
}

// MeterHandler event handler
void MeterHandler_Events(object sender, Tcc2MeterEventArgs e)
{
    switch (e.EventType)
    {
        case Tcc2MeterEventArgs.eTcc2MeterEventType.Elevation:
            var elevationDegrees = e.IntValue; // A value between 0 and 90
            break;
        case Tcc2MeterEventArgs.eTcc2MeterEventType.Azimuth:
            var azimuthDegrees = e.IntValue; // A value between 0 and 359
            break;
        case Tcc2MeterEventArgs.eTcc2MeterEventType.InputPeakLevel:
            var inputPeakLevelDb = e.IntValue; // A value between -90 and 0
            break;
    }
}

// AudioHandler event handler
void AudioHandler_Events(object sender, Tcc2AudioEventArgs e)
{
    switch (e.EventType)
    {
        case Tcc2AudioEventArgs.eTcc2AudioEventType.ExclusionZoneActive:
            bool exclusionZoneActive = e.BoolValue;
            break;
        case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteMacAddresses:
            string danteMacAddresses = e.StringValue; // Two comma separated addresses
            break;
        case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteIpAddresses:
            string danteIpAddresses = e.StringValue; // Two comma separated addresses
            break;
        case Tcc2AudioEventArgs.eTcc2AudioEventType.DanteOutputGain:
            int danteOutputGainDb = e.IntValue; // A value between 0 and 24
            break;
        case Tcc2AudioEventArgs.eTcc2AudioEventType.SpeakerDetectionThreshold:
            Tcc2AudioHandler.eSpeakerDetectionThreshold threshold = e.SpeakerDetectionThreshold;
            break;
        case Tcc2AudioEventArgs.eTcc2AudioEventType.Mute:
            bool muted = e.BoolValue;
            break;
    }
}
```

### Handlers
The device's properties, methods and events are located in a few different locations. This is completely based on how Sennheiser has decided to do in their protocol.
On the root class `Tcc2` you will find some (see below) but you also the following handlers where you will find more.

* `DeviceHandler` - Contains things regarding the device. This is found in `Tcc2.DeviceHandler`.
* `MeterHandler` - Contains things regarding meters, such as azimuth, elevation and input peak level. This is found in `Tcc2.MeterHandler`.
* `AudioHandler` - Contains things regarding the audio output, such as Dante Output gain and Mute. This is found in `Tcc2.AudioHandler`.

### Methods

* `Connect(string ip)` - Opens up the connection to the device. Uses default port 45.
* `Connect(string ip, int port)` - Opens up the connection to the device.
* `Disconnect` - Stops the connection to the device.
* `Send(string data)` - Used as a way to send your own commands. Refer to the Sennheiser Sound Control Protocol (SSC). Example command: `{"device":{"reset":true}}`. 
* `Send(object value, params string[] path)` - Used as a way to send your own commands. An example would be `Send("name", "device", "location")`.
* `Dispose()` - Used to clean up timers and connections. This must be called when your program stops.

#### DeviceHandler methods
Located in `Tcc2.DeviceHandler`.

* `PollInfo()` - Asks the device about `Version`, `Serial`, `Product` and `MacAddresses`. This is automatically done on connection, so your should not need to do this manually.

#### MeterHandler methods
Located in `Tcc2.MeterHandler`.

The reason you have to manually enable some feedback is because the device is quite ”chatty” so if you don't use those features all that traffic is unneccesary.

* `EnableAzimuthFeedback()` - Enables subscription to Azimuth degrees feedback. When enabled the `Tcc2.MeterHandler.AzimuthDegrees` property will be continuosly updated. This will also cause the `Tcc2.MeterHandler.Events` event to trig whenever there's a change.  
* `DisableAzimuthFeedback()` - Disables the Azimuth degrees feedback.
* `EnableElevationFeedback()` - Enables subscription to Elevation degrees feedback. When enabled the `Tcc2.MeterHandler.ElevationDegrees` property will be continuosly updated. This will also cause the `Tcc2.MeterHandler.Events` event to trig whenever there's a change.  
* `DisableElevationFeedback()` - Disables the Elevation degrees feedback.
* `EnableInputPeakLevelFeedback()` - Enables subscription to input peak level feedback. When enabled the `Tcc2.MeterHandler.InputPeakLevelDb` property will be continuosly updated. This will also cause the `Tcc2.MeterHandler.Events` event to trig whenever there's a change.  
* `DisableInputPeakLevelFeedback()` - Disables the input peak level feedback.

### Properties

* `IsResponding` - Returns `true` as long as the device is responding. As the protocol uses UDP there is no connection state, so it might take up to a minute before responding goes low after the device has stopped responding. Listen to the `Responding` event to know when this changes.
* `Debug` - Enables debug messages to be printed to the text console while set to `true`. Make sure this is not left on when not used.
* `DeviceHandler` - Returns the device handler, where you have features, properties and events regarding the device.
* `MeterHandler` - Returns the meter handler, where you have features, properties and events regarding meters, such as azimuth, elevation and input peak level.
* `AudioHandler` - Returns the audio handler, where you have features, properties and events regarding the audio output, such as Dante output gain and Mute.

#### DeviceHandler properties
Located in `Tcc2.DeviceHandler`.

* `Name` - Sets or gets the name of the device. Max 8 chars.
* `Version` - The firmware version of the device. Example: 1.1.0.
* `Serial` - The serial number of the device. Example: 1234567890.
* `Product` - The product name of the device. Example: CHG4N.
* `MacAddresses` - The mac adresses of the device. Example: 00:1B:66:11:22:33.
* `Location` - Sets or gets the location of the device. Max length: 8 characters. Allowed chars: 0-9, A-Z, a-z or 'space'. Must start with a letter. May not start or end with a – or _.
* `Position` - Sets or gets the position of the device. Intended to be used as the position in the location. Example if location is `"Room_1"`, position might be `"Over the table"`. Max length: 30 chars. Allowed chars: 0-9, A-Z, a-z or 'space'
* `Identify` - Sets or gets if the identify feature of the device is enabled. It blinks the LEDs on the mic when `true`.
* `CustomLedColor` - Sets or gets the currently selected custom color for the leds. Available colors are `LightGreen`, `Green`, `Blue`, `Red`, `Yellow`, `Orange`, `Cyan` and `Pink`.
* `CustomLedActive` - Sets or gets if the the custom led color is active. The color is set with property `CustomLedColor`.
* `MicMuteLedColor` - Sets or gets the currently selected color for the leds when the mic is muted. Available colors are `LightGreen`, `Green`, `Blue`, `Red`, `Yellow`, `Orange`, `Cyan` and `Pink`.
* `MicOnLedColor` - Sets or gets the currently selected custom color for the leds when the mic is on. Available colors are `LightGreen`, `Green`, `Blue`, `Red`, `Yellow`, `Orange`, `Cyan` and `Pink`.
* `LedBrightness` - Sets or gets the brightness of the leds on the mic in steps of 20%. A value between 0 and 5 where `0` = 0% and `5` = 100%.

#### MeterHandler properties
Located in `Tcc2.MeterHandler`.

* `AzimuthDegrees` - If `EnableAzimuthFeedback()` has been called, this will contain the current the direction of the speaker in degrees. A value between `0` and `359`.
* `ElevationDegrees` - If `EnableElevationFeedback()` has been called, this will contain the current the elevation of the speaker in degrees. A value between `0` and `90`.
* `InputPeakLevelDb` - If `EnableInputPeakLevelFeedback()` has been called, this will contain the current input peak level in dB of the mic. A value between `-90` and `0`.

#### AudioHandler properties
Located in `Tcc2.AudioHandler`.

* `ExclusionZoneActive` - Sets or gets if exclusion zones are active in the device. Exclusion zones are areas where the microphone should not be listening. These are configured in the device settings.
* `DanteMacAddresses` - The mac addresses of the Dante outputs. This returns both addresses separated with a comma. Example: 00:1B:66:44:55:66,00:1B:66:77:88:99.
* `DanteIpAddresses` - The ip addresses of the Dante outputs. This returns both addresses separated with a comma. If there is no network cable connected or no addresses set, this might return a string only containing a comma.  Example: 192.168.10.2,192.168.10.3.
* `DanteOutputGain` - Sets or gets the current Dante output gain in dB. A value between `0` and `24`.
* `SpeakerDetectionThreshold` - Sets or gets the currently selected sensitivity of the speaker detection. Available values are `QuietRoom`, `NormalRoom` and `LoudRoom`.
* `Mute` - Sets or gets if the audio output is muted. `true` is muted.

### Events

* `Errors` - Will trig when there are error messages from the device. The event args contains:
    * `Json` - The raw JSON data returned from the device. Warning! See [Important notes and known issues](#important-notes-and-known-issues)
* `Responding` - Will trig when the device starts or stops respoding. The event args contains:
    * `Responding` - Is `true` if the device is responding.
* `IncomingCommand` - This will trig whenever there is incoming data from the device. The use case for this would be to extend the functionality of the library. The event args contains:
    * `Command` - The JSON data that was received.
    * `Handled` - If you set this to `true` then this command will not be handled by the library.

#### DeviceHandler events
Located in `Tcc2.DeviceHandler`.

* `Events` - This will trig when any of the [DeviceHandler properties](#devicehandler-properties-2) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `StringValue` - Contains the new value of the property for event types `Name`, `Location`, `Position`, `Version`, `Serial`, `Product` and `MacAddresses`.
    * `BoolValue` - Contains the new value of the property for event types `Identify` and `CustomLedActive`.
    * `IntValue` - Contains the new value of the property for event type `LedBrightness`.
    * `LedColor` - Contains the new value of the property for event types `CustomLedColor`, `MicMuteLedColor` and `MicOnLedColor`.

#### MeterHandler events
Located in `Tcc2.MeterHandler`.

* `Events` - This will trig when any of the [MeterHandler properties](#meterhandler-properties) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `IntValue` - The new value of the changed property. 

#### AudioHandler events
Located in `Tcc2.AudioHandler`.

* `Events` - This will trig when any of the [AudioHandler properties](#audiohandler-properties-1) change. The event args contains:
    * `EventType` - An enum telling you which property changed.
    * `BoolValue` - Contains the new value of the property for event types `ExclusionZoneActive` and `Mute`.
    * `IntValue` - Contains the new value of the property for event type `DanteOutputGain`.
    * `StringValue` - Contains the new value of the property for event types `DanteMacAddresses` and `DanteIpAddresses`.
    * `SpeakerDetectionThreshold` - Contains the new value of the property for event type `SpeakerDetectionThreshold`.

## Release notes

### 1.0.0 (Initial version)

* Supports CHG 4N (`Chg4N`)
* Supports SL Rack Receiver DW (`Sldw`)
* Supports TeamConnect Ceiling 2 (`Tcc2`)

