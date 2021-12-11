using System;
using System.Device.Spi;
using System.Threading;
using Iot.Device.St7735;

Console.WriteLine("Hello, World!");

// Preparing SPI bus.
var spiSettings = new SpiConnectionSettings(0, 1);
spiSettings.ClockFrequency = 12_000_000;
var spiBus = SpiDevice.Create(spiSettings); // Ft4222Spi(new SpiConnectionSettings(0, 1) { ClockFrequency = 1_000_000, Mode = SpiMode.Mode0 });

// Pimoroni is using 0.96" 80x160 LCD displays for their Automation and Enviro pHats.
var lcd = new PimoroniOled(spiBus, 9);
lcd.SetOrientation(Orientation.Rotated90);

Console.WriteLine("Let's go!");

lcd.SetRegion(0, 0, lcd.ActualWidth, lcd.ActualHeight);
lcd.SendBitmap(new byte[2 * lcd.ActualWidth * lcd.ActualHeight]);

var randomNumber = new Random();
var whiteRectangleBuffer = new byte[2 * 10 * 5];
for (var i = 0; i < 2 * 10 * 5; i++) {
    whiteRectangleBuffer[i] = 0xFF;
}

var greenRectangleBuffer = new byte[2 * 5 * 10];
for (var i = 0; i < 2 * 5 * 10; i += 2) {
    greenRectangleBuffer[i] = 0b00011111;
}

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
