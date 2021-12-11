namespace Iot.Device.St7735;

/// <summary>
/// Orientation of the display.
/// </summary>
public enum Orientation {
    /// <summary>
    /// Native orientation, as designed by the manufacturer.
    /// </summary>
    Normal,

    /// <summary>
    /// Rotated CW by 90 degrees.
    /// </summary>
    Rotated90,

    /// <summary>
    /// Rotated CW by 180 degrees.
    /// </summary>
    Rotated180,

    /// <summary>
    /// Rotated CW by 270 degrees.
    /// </summary>
    Rotated270
}
