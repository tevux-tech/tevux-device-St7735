using System;
using System.Device.Spi;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using Tevux.Device.St7735;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Runtime.InteropServices;

// Preparing SPI bus.
var spiSettings = new SpiConnectionSettings(0, 1);
spiSettings.ClockFrequency = 12_000_000;
var spiBus = SpiDevice.Create(spiSettings); // Ft4222Spi(new SpiConnectionSettings(0, 1) { ClockFrequency = 1_000_000, Mode = SpiMode.Mode0 });

// Preparing GPIO controller (nothing to prepare, actually).
var gpioController = new System.Device.Gpio.GpioController();

// Pimoroni is using 0.96" 80x160 LCD displays for their Automation and Enviro pHats.
var lcd = new ST7735();
lcd.InitializeAsPimoroniEnviro(spiBus, gpioController);

// Hardware is now initialized.
Console.WriteLine("Let's go!");

// Clearing the screen.
lcd.Clear();

// If you want, you can draw all the pizels yourself, without using 3rd party libraries. Like so:
var greenRectangleBuffer = new byte[2 * 10 * 10]; // <-- Color is coded BGR565, so two bytes per pixel.
for (var i = 0; i < 2 * 10 * 10; i += 2) {
    greenRectangleBuffer[i] = 0b00011111;
}
lcd.SetRegion(lcd.ActualWidth / 2 - 5, lcd.ActualHeight / 2 - 5, 10, 10);
lcd.SendBitmap(greenRectangleBuffer);

// If we want to be fancy,. we can use SixLabors libraries with all the bell and whistles. Like, to draw text with antialiasing.
// Need to pick a font. These are different for different OSes.
var font = "DejaVu Sans"; // <-- This works on Rapsberry Pi.
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
    font = "Consolas";
}
var fontsys = SystemFonts.CreateFont(font, 16, FontStyle.Regular);

// Some variables to be reused.
var helloImage = new Image<Bgr565>(80, 16);
var randomNumber = new Random();
while (true) {
    Thread.Sleep(100);
    // At random Y coordinate, and in random color...
    var randomY = randomNumber.Next(lcd.ActualHeight / 16) * 16;
    var randomColor = Color.FromRgb((byte)randomNumber.Next(0xFF), (byte)randomNumber.Next(0xFF), (byte)randomNumber.Next(0xFF));
    var randomBackgroundColor = Color.FromRgb((byte)randomNumber.Next(0xFF), (byte)randomNumber.Next(0xFF), (byte)randomNumber.Next(0xFF));

    // ...we'll write hello like there's no tomorrow!
    helloImage.Mutate(ctx => ctx.Fill(randomBackgroundColor).DrawText("Hello!", fontsys, randomColor, new SixLabors.ImageSharp.PointF(0, 0)));

    lcd.SetRegion(0, randomY, 80, 20);
    lcd.SendBitmap(helloImage.ToBgr565Array());
}

public static class SixLaborsExtensions {
    /// <summary>
    /// Extension method to transcode SixLabors image representation to byte array that is supported by ST7735.
    /// </summary>
    public static byte[] ToBgr565Array(this Image<Bgr565> image) {
        var returnArray = new byte[image.Width * image.Height * 2];
        for (var j = 0; j < image.Height; j++) {
            for (var i = 0; i < image.Width; i++) {
                returnArray[2 * (i + image.Width * j)] = (byte)(image[i, j].PackedValue >> 8);
                returnArray[2 * (i + image.Width * j) + 1] = (byte)image[i, j].PackedValue;
            }
        }
        return returnArray;
    }
}
