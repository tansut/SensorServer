using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Kalitte.Sensors.Configuration
{
[Serializable, StructLayout(LayoutKind.Sequential)]
public struct ProviderCapability : IEquatable<ProviderCapability>
{
    private const int UninitializedValue = 0;
    //private const int DiscoveryValue = 1;
    //private const int TriggeredDiscoveryValue = 2;
    //private const int ProviderDefunctEventValue = 3;
    private const int TcpTransportValue = 4;
    private const int SerialTransportValue = 5;
    private const int HttpTransportValue = 6;
    private const int BluetoothTransportValue = 7;
    private const int VendorDefinedTransportValue = 8;
    private const int UsernamePasswordAuthenticationValue = 9;
    private const int UsbTransportValue = 10;
    private const int LastValue = 10;
    public static readonly ProviderCapability Uninitialized;
    //public static readonly ProviderCapability Discovery;
    //public static readonly ProviderCapability TriggeredDiscovery;
    //public static readonly ProviderCapability ProviderDefunctEvent;
    public static readonly ProviderCapability TcpTransport;
    public static readonly ProviderCapability SerialTransport;
    public static readonly ProviderCapability HttpTransport;
    public static readonly ProviderCapability BluetoothTransport;
    public static readonly ProviderCapability UsbTransport;
    public static readonly ProviderCapability VendorDefinedTransport;
    public static readonly ProviderCapability UserNamePasswordAuthentication;
    private static Dictionary<int, string> standardDescriptions;
    private readonly int enumValue;
    private readonly string description;
    public int Value
    {
        get
        {
            return this.enumValue;
        }
    }
    public string Description
    {
        get
        {
            return this.description;
        }
    }
    private ProviderCapability(int value)
    {
        if (standardDescriptions == null)
        {
            Init();
        }
        if (0 > value)
        {
            throw new ArgumentException("InvalidEnumValue");
        }
        this.enumValue = value;
        this.description = standardDescriptions[value];
    }

    public ProviderCapability(int value, string description)
    {
        if (0 >= value)
        {
            throw new ArgumentException("InvalidEnumValue");
        }
        if (value <= 10)
        {
            throw new InvalidOperationException("UseStandardCons");
        }
        this.enumValue = value;
        this.description = description;
    }

    private static void Init()
    {
        standardDescriptions = new Dictionary<int, string>();
        standardDescriptions[1] = "Discovery";
        standardDescriptions[2] = "TriggeredDiscovery";
        standardDescriptions[3] = "ProviderDefunctEvent";
        standardDescriptions[4] = "TcpTransport";
        standardDescriptions[5] = "SerialTransport";
        standardDescriptions[6] = "HttpTransport";
        standardDescriptions[7] = "BluetoothTransport";
        standardDescriptions[8] = "VendorDefinedTransport";
        standardDescriptions[10] = "UsbTransport";
        standardDescriptions[9] = "UserNamePasswordAuthentication";
        standardDescriptions[10] = "UsbTransport";
        standardDescriptions[0] = "Uninitialized";
    }

    public static explicit operator ProviderCapability(int value)
    {
        if (0 > value)
        {
            throw new ArgumentException("InvalidEnumValue");
        }
        switch (value)
        {
            case 0:
                return Uninitialized;

            //case 1:
            //    return Discovery;

            //case 2:
            //    return TriggeredDiscovery;

            //case 3:
            //    return ProviderDefunctEvent;

            case 4:
                return TcpTransport;

            case 5:
                return SerialTransport;

            case 6:
                return HttpTransport;

            case 7:
                return BluetoothTransport;

            case 8:
                return VendorDefinedTransport;

            case 10:
                return UsbTransport;
        }
        throw new ArgumentException("NonstandardValue");
    }

    public bool IsDiscoveryRelated
    {
        get
        {
            if (this.enumValue != 1)
            {
                return (this.enumValue == 2);
            }
            return true;
        }
    }
    public bool IsEventRelated
    {
        get
        {
            return (this.enumValue == 3);
        }
    }
    public bool IsTransportRelated
    {
        get
        {
            if ((((this.enumValue != 4) && (this.enumValue != 5)) && ((this.enumValue != 6) && (this.enumValue != 7))) && (this.enumValue != 8))
            {
                return (this.enumValue == 10);
            }
            return true;
        }
    }
    public bool Equals(ProviderCapability other)
    {
        return (this == other);
    }

    public override bool Equals(object obj)
    {
        return ((obj is ProviderCapability) && (this == ((ProviderCapability) obj)));
    }

    public override int GetHashCode()
    {
        return this.enumValue;
    }

    public static bool operator ==(ProviderCapability providerCapability1, ProviderCapability providerCapability2)
    {
        return (providerCapability1.enumValue == providerCapability2.enumValue);
    }

    public static bool operator !=(ProviderCapability providerCapability1, ProviderCapability providerCapability2)
    {
        return (providerCapability1.enumValue != providerCapability2.enumValue);
    }

    public override string ToString()
    {
        return this.Description;
    }

    static ProviderCapability()
    {
        Uninitialized = new ProviderCapability(0);
        //Discovery = new ProviderCapability(1);
        //TriggeredDiscovery = new ProviderCapability(2);
        //ProviderDefunctEvent = new ProviderCapability(3);
        TcpTransport = new ProviderCapability(4);
        SerialTransport = new ProviderCapability(5);
        HttpTransport = new ProviderCapability(6);
        BluetoothTransport = new ProviderCapability(7);
        UsbTransport = new ProviderCapability(10);
        VendorDefinedTransport = new ProviderCapability(8);
        UserNamePasswordAuthentication = new ProviderCapability(9);
    }
}

 

 

}
