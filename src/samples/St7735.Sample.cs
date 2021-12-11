using System;
using System.Device.Spi;
using System.Threading;
using Tevux.Device.St7735;

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

// Preparing some buffers that will be heavily reused later.
var randomNumber = new Random();
var whiteRectangleBuffer = new byte[2 * 10 * 5];
for (var i = 0; i < 2 * 10 * 5; i++) {
    whiteRectangleBuffer[i] = 0xFF;
}

var greenRectangleBuffer = new byte[2 * 5 * 10];
for (var i = 0; i < 2 * 5 * 10; i += 2) {
    greenRectangleBuffer[i] = 0b00011111;
}

for (var i = 0; i < 4; i++) {
    lcd.SetOrientation((global::Tevux.Device.St7735.Orientation)i);

    lcd.Clear();

    // Painting some markers;
    lcd.SetRegion(0, 0, 10, 5);
    lcd.SendBitmap(whiteRectangleBuffer);
    lcd.SetRegion(lcd.ActualWidth - 10, 0, 10, 5);
    lcd.SendBitmap(whiteRectangleBuffer);

    lcd.SetRegion(0, lcd.ActualHeight - 10, 5, 10);
    lcd.SendBitmap(greenRectangleBuffer);
    lcd.SetRegion(lcd.ActualWidth - 5, lcd.ActualHeight - 10, 5, 10);
    lcd.SendBitmap(greenRectangleBuffer);

    Thread.Sleep(1000);
}


// Unleash the demo! Painting random rectangles all over the screen.
while (true) {
    Thread.Sleep(100);

    var randomX = randomNumber.Next(lcd.ActualWidth - 10);
    var randomY = randomNumber.Next(lcd.ActualHeight - 10);

    lcd.SetRegion(randomX, randomY, 10, 5);
    lcd.SendBitmap(whiteRectangleBuffer);

    Thread.Sleep(100);

    randomX = randomNumber.Next(lcd.ActualWidth - 10);
    randomY = randomNumber.Next(lcd.ActualHeight - 10);

    lcd.SetRegion(randomX, randomY, 5, 10);
    lcd.SendBitmap(greenRectangleBuffer);
}
