using System;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.St7735.Register;

namespace Iot.Device.St7735;

/// <summary>
/// Provides very basic access to ST7735 LCD controller.
/// </summary>
public class ST7735 {
    private const byte _nativeNumberOfColumns = 132;
    private const byte _nativeNumberOfRows = 162;

    private readonly SpiDevice _spi;
    private readonly GpioController _gpio = new();
    private readonly int _controlPin;
    private readonly int _backlightPin = 12;

    private readonly byte[] _buffer1 = new byte[1];

    private byte _offsetLeft;
    private byte _offsetTop;

    /// <summary>
    /// Native width of the LCD display.This depends on the LCD matrix that is connected to ST7735 controller.
    /// </summary>
    public int NativeWidth { get; protected set; } = 80;

    /// <summary>
    /// Native height of the LCD display.This depends on the LCD matrix that is connected to ST7735 controller.
    /// </summary>
    public int NativeHeight { get; protected set; } = 160;

    /// <summary>
    /// Actual width of the usable LCD area. This may differ from <see cref="NativeWidth"/> because screen may be rotated, for example.
    /// </summary>
    public int ActualWidth { get; protected set; }

    /// <summary>
    /// Actual height of the usable LCD area. This may differ from <see cref="NativeHeight"/> because screen may be rotated, for example.
    /// </summary>
    public int ActualHeight { get; protected set; }

    /// <summary>
    /// Rotation of the LCD screen.
    /// </summary>
    public Orientation Orientation { get; protected set; }

    /// <summary>
    /// TODO
    /// </summary>
    public ST7735(SpiDevice spiDevice, int controlPin, int nativeWidth, int nativeHeight) {
        _spi = spiDevice;

        _controlPin = controlPin;

        NativeWidth = nativeWidth;
        NativeHeight = nativeHeight;

        _gpio.OpenPin(_controlPin, PinMode.Output);
        _gpio.OpenPin(_backlightPin, PinMode.Output);

        Initialize();
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="orientation">TODO</param>
    public void SetOrientation(Orientation orientation) {
        Orientation = orientation;

        switch (Orientation) {
            case Orientation.Normal:
                ActualWidth = NativeWidth;
                ActualHeight = NativeHeight;

                SetRegister(Address.MemoryDataAccessControl, 0x08 /* RGB mode */);

                _offsetLeft = (byte)((_nativeNumberOfColumns - ActualWidth) / 2);
                _offsetTop = (byte)((_nativeNumberOfRows - ActualHeight) / 2);

                SetRegion(0, 0, ActualWidth, ActualHeight);
                break;

            case Orientation.Rotated180:
                ActualWidth = NativeHeight;
                ActualHeight = NativeWidth;

                SetRegister(Address.MemoryDataAccessControl, 0x80 /* Row order reversed */ + 0x20 /* Row/column switch */ + 0x08 /* RGB mode */);

                _offsetLeft = (byte)((_nativeNumberOfRows - ActualWidth) / 2);
                _offsetTop = (byte)((_nativeNumberOfColumns - ActualHeight) / 2);

                SetRegion(0, 0, ActualWidth, ActualHeight);
                break;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public virtual void Initialize() {
    }

    /// <summary>
    /// Set the register of the LCD controller (no payload).
    /// </summary>
    /// <param name="register">Address of the register.</param>
    public void SetRegister(Address register) {
        _buffer1[0] = (byte)register;
        _gpio.Write(_controlPin, PinValue.Low);
        _spi.Write(_buffer1);
    }

    /// <summary>
    /// Set the register of the LCD controller (single-byte payload).
    /// </summary>
    /// <param name="register">Address of the register.</param>
    /// <param name="value">Value of the register.</param>
    public void SetRegister(Address register, byte value) {
        _buffer1[0] = (byte)register;
        _gpio.Write(_controlPin, PinValue.Low);
        _spi.Write(_buffer1);

        _buffer1[0] = value;
        _gpio.Write(_controlPin, PinValue.High);
        _spi.Write(_buffer1);
    }

    /// <summary>
    /// Set the register of the LCD controller (multi-byte payload).
    /// </summary>
    /// <param name="register">Address of the register.</param>
    /// <param name="value">Value of the register.</param>
    public void SetRegister(Address register, byte[] value) {
        var valueSpan = new Span<byte>(value);

        var bytesSent = 0;

        _buffer1[0] = (byte)register;

        _gpio.Write(_controlPin, PinValue.Low);
        _spi.Write(_buffer1);

        _gpio.Write(_controlPin, PinValue.High);
        while (bytesSent < value.Length) {
            if (value.Length - bytesSent > 4096) {
                _spi.Write(valueSpan.Slice(bytesSent, 4096));
                bytesSent += 4096;
            }
            else {
                _spi.Write(valueSpan.Slice(bytesSent, value.Length - bytesSent));
                bytesSent += value.Length - bytesSent;
            }
        }
    }

    /// <summary>
    /// Turns on the display.
    /// </summary>
    public void TurnOn() {
        _gpio.Write(_backlightPin, PinValue.High);
    }

    /// <summary>
    /// Turns off the display.
    /// </summary>
    public void TurnOff() {
        _gpio.Write(_backlightPin, PinValue.Low);
    }

    /// <summary>
    /// Sets clipping region of the display area. It will only be possible to change pixels in that region.
    /// </summary>
    /// <param name="x">Origin X of the clip region.</param>
    /// <param name="y">Origin Y of the clip region.</param>
    /// <param name="width">Width of the clip region.</param>
    /// <param name="height">Height of the clip region.</param>
    public void SetRegion(int x, int y, int width, int height) {
        var x0 = x + _offsetLeft;
        var x1 = x + width - 1 + _offsetLeft;
        SetRegister(Address.ColumnAddressSet, new byte[] { (byte)(x0 >> 8), (byte)x0, (byte)(x1 >> 8), (byte)x1 });

        var y0 = y + _offsetTop;
        var y1 = y + height - 1 + _offsetTop;
        SetRegister(Address.RowAddressSet, new byte[] { (byte)(y0 >> 8), (byte)y0, (byte)(y1 >> 8), (byte)y1 });
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="data">TODO</param>
    public void SendBitmap(byte[] data) {
        if (data == null) {
            throw new ArgumentNullException(nameof(data));
        }

        SetRegister(Address.MemoryWrite, data);
    }
}
