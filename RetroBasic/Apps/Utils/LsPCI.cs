using System;
using System.Collections.Generic;
using System.Text;
using static Cosmos.HAL.PCIDevice;

namespace RetroBasic.Apps.Utils
{
    class LsPCI
    {
        public static void c_Lspci()
        {
            int count = 0;
            foreach (Cosmos.HAL.PCIDevice device in Cosmos.HAL.PCI.Devices)
            {
                Console.WriteLine(Conversion.D2(device.bus) + ":" + Conversion.D2(device.slot) + ":" + Conversion.D2(device.function) + " - " + "0x" + Conversion.D4(Conversion.DecToHex(device.VendorID)) + ":0x" + Conversion.D4(Conversion.DecToHex(device.DeviceID)) + " : " + DeviceClass.GetTypeString(device) + ": " + DeviceClass.GetDeviceString(device));
                count++;
                if (count == 19)
                {
                    Console.ReadKey();
                    count = 0;
                }
            }
        }
    }
}
