namespace Iot.Device.St7735.Register;

/// <summary>
/// Register address definitions for ST7735.
/// </summary>
public enum Address : byte {
    /// <summary>
    /// No operation.
    /// </summary>
    NOP = 0x00,

    /// <summary>
    /// Software reset.
    /// </summary>
    SwReset = 0x01,

    /// <summary>
    /// Read display ID.
    /// </summary>
    DisplayId = 0x04,

    /// <summary>
    /// Read display status.
    /// </summary>
    DisplayStatus = 0x09,

    /// <summary>
    /// Sleep in, booster off.
    /// </summary>
    SleepIn = 0x10,

    /// <summary>
    /// Sleep out, booster on.
    /// </summary>
    SleepOut = 0x11,

    /// <summary>
    /// Partial mode on, normal mode off.
    /// </summary>
    PartialModeOn = 0x12,

    /// <summary>
    /// Normal mode on, partial mode off.
    /// </summary>
    NormalModeOn = 0x13,

    /// <summary>
    /// No color inversion.
    /// </summary>
    DisplayInversionOff = 0x20,

    /// <summary>
    /// Colors are inverted.
    /// </summary>
    DisplayInversionOn = 0x21,

    /// <summary>
    /// Turn display off.
    /// </summary>
    DisplayOff = 0x28,

    /// <summary>
    /// Turn display on.
    /// </summary>
    DisplayOn = 0x29,

    /// <summary>
    /// Set column addresses (start and stop).
    /// </summary>
    ColumnAddressSet = 0x2A,

    /// <summary>
    /// Set row addresses (start and stop).
    /// </summary>
    RowAddressSet = 0x2B,

    /// <summary>
    /// Begin writing to graphics memory.
    /// </summary>
    MemoryWrite = 0x2C,

    /// <summary>
    /// Begin reading graphics memory.
    /// </summary>
    MemoryRead = 0x2E,

    /// <summary>
    /// Set partial start/end address.
    /// </summary>
    PartialAddressSet = 0x30,

    /// <summary>
    /// Set directions of how graphics memory is accessed.
    /// </summary>
    MemoryDataAccessControl = 0x36,

    /// <summary>
    /// Pixel color format.
    /// </summary>
    PixelFormat = 0x3A,

    /// <summary>
    /// Frame control (normal mode).
    /// </summary>
    FrameRateControl1 = 0xB1,

    /// <summary>
    /// Frame control (idle mode).
    /// </summary>
    FrameRateControl2 = 0xB2,

    /// <summary>
    /// Frame control (partial mode).
    /// </summary>
    FrameRateControl3 = 0xB3,

    /// <summary>
    /// Display inversion control.
    /// </summary>
    InversionControl = 0xB4,

    /// <summary>
    /// Display function settings.
    /// </summary>
    DisplayFunction = 0xB6,

    /// <summary>
    /// Power control for GVDD.
    /// </summary>
    PowerControl1 = 0xC0,

    /// <summary>
    /// Power control for VGH/VGL.
    /// </summary>
    PowerControl2 = 0xC1,

    /// <summary>
    /// Power control for amplifier and booster (normal mode).
    /// </summary>
    PowerControl3 = 0xC2,

    /// <summary>
    /// Power control for amplifier and booster (idle mode).
    /// </summary>
    PowerControl4 = 0xC3,

    /// <summary>
    /// Power control for amplifier and booster (partial mode).
    /// </summary>
    PowerControl5 = 0xC4,

    /// <summary>
    /// Power control VCOM.
    /// </summary>
    VcomControl = 0xC5,

    /// <summary>
    /// Gamma adjustment (positive polarity).
    /// </summary>
    GammaPositiveCorrection = 0xE0,

    /// <summary>
    /// Gamma adjustment (negative polarity).
    /// </summary>
    GammaNegativeCorrection = 0xE1,
}
