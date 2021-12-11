using System.Device.Gpio;
using System.Device.Spi;
using System.Threading;
using Tevux.Device.St7735.Register;

namespace Tevux.Device.St7735;

public static class PimoroniExtensions {
    public static void InitializeAsPimoroniEnviro(this ST7735 baseClass, SpiDevice spiDevice, GpioController gpioDriver, int controlPin = 9, int backlightPin = 12) {
        baseClass.Initialize(spiDevice, gpioDriver, controlPin, backlightPin, 80, 160);

        // Initialization sequence taken from Pimoroni python scripts.
        baseClass.SetRegister(Address.SwReset);
        Thread.Sleep(200);

        baseClass.SetRegister(Address.SleepOut);
        Thread.Sleep(200);

        baseClass.SetRegister(Address.FrameRateControl1, new byte[] { 0x01, 0x2C, 0x2D });
        baseClass.SetRegister(Address.FrameRateControl2, new byte[] { 0x01, 0x2C, 0x2D });
        baseClass.SetRegister(Address.FrameRateControl3, new byte[] { 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D });

        baseClass.SetRegister(Address.InversionControl, 0x07);

        baseClass.SetRegister(Address.PowerControl1, new byte[] { 0xA2, 0x02, 0x84 });
        baseClass.SetRegister(Address.PowerControl2, new byte[] { 0x0A, 0x00 });
        baseClass.SetRegister(Address.PowerControl4, new byte[] { 0x8A, 0x2A });
        baseClass.SetRegister(Address.PowerControl5, new byte[] { 0x8A, 0xEE });
        baseClass.SetRegister(Address.VcomControl, 0x0E);

        baseClass.SetRegister(Address.DisplayInversionOn);
        baseClass.SetRegister(Address.PixelFormat, 0x05);

        baseClass.SetRegister(Address.GammaPositiveCorrection, new byte[] { 0x02, 0x1c, 0x07, 0x12, 0x37, 0x32, 0x29, 0x2d, 0x29, 0x25, 0x2e, 0x39, 0x00, 0x01, 0x03, 0x10 });
        baseClass.SetRegister(Address.GammaNegativeCorrection, new byte[] { 0x03, 0x1d, 0x07, 0x06, 0x2e, 0x2c, 0x29, 0x2d, 0x2e, 0x2e, 0x37, 0x3f, 0x00, 0x00, 0x02, 0x10 });

        baseClass.SetRegister(Address.NormalModeOn);
        Thread.Sleep(10);

        baseClass.SetRegister(Address.DisplayOn);
        Thread.Sleep(100);

        baseClass.SetOrientation(Orientation.Normal);

        baseClass.TurnOn();
    }

}
