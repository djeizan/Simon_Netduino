using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Simon
{
    public static class GPIO
    {
        public static OutputPort Led = new OutputPort(Pins.ONBOARD_LED, false);
        public static InputPort Botao = new InputPort(Pins.ONBOARD_BTN, true, Port.ResistorMode.Disabled);

    }
}