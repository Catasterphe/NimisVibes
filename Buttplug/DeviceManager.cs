using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Core;
using UnityEngine;

namespace NimisVibes;

public class DeviceManager
{
    private List<ButtplugClientDevice> ConnectedDevices { get; set; }
    private ButtplugClient ButtplugClient { get; set; }

    public DeviceManager(string clientName)
    {
        ConnectedDevices = [];
        ButtplugClient = new ButtplugClient(clientName);
        Plugin.Log.LogInfo("BP client created for " + clientName);
        ButtplugClient.DeviceAdded += HandleDeviceAdded;
        ButtplugClient.DeviceRemoved += HandleDeviceRemoved;
    }

    public bool IsConnected() => ButtplugClient.Connected;

    public async void ConnectDevices()
    {
        if (ButtplugClient.Connected) { return; }

        try
        {
            Plugin.Log.LogInfo($"Attempting to connect to Intiface server at ws://localhost:12345");
            await ButtplugClient.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://localhost:12345")));
            Plugin.Log.LogInfo("Connection successful. Beginning scan for devices");
            await ButtplugClient.StartScanningAsync();
        }
        catch (ButtplugException exception)
        {
            Plugin.Log.LogError($"Attempt to connect to devices failed. Ensure Intiface is running and attempt to reconnect from the 'Devices' section in the mod's in-game settings.");
            Plugin.Log.LogDebug($"ButtplugIO error occured while connecting devices: {exception}");
        }
    }

    public void VibrateWithTime(double intensity, float time)
    {
        Task[] tasks = ConnectedDevices
            .Where((device) => device.VibrateAttributes.Count > 0)
            .Select(async (device) => {
                await device.VibrateAsync(Mathf.Clamp((float)intensity, 0f, 1.0f));
                await Task.Delay((int)(time*1000f));
                await device.Stop();
            })
            .ToArray();

        Task.WhenAll(tasks);
    }

    private void HandleDeviceAdded(object sender, DeviceAddedEventArgs args)
    {
        Plugin.Log.LogInfo($"{args.Device.Name} connected to client {ButtplugClient.Name}");
        ConnectedDevices.Add(args.Device);
    }

    private void HandleDeviceRemoved(object sender, DeviceRemovedEventArgs args)
    {
        Plugin.Log.LogInfo($"{args.Device.Name} disconnected from client {ButtplugClient.Name}");
        ConnectedDevices.Remove(args.Device);
    }

    private bool CanVibrate(ButtplugClientDevice device)
    {
        return device.VibrateAttributes.Count > 0;
    }
}
