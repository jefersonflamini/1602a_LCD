# 1602a_LCD
Driver for the 1602a LCD display connected to the Raspberry Pi written in C#.

# Installation
Put the file "1602a_LCD.cs" into your project and include the namepsace `#using 1602a_LCD` into your main Program.cs

This code uses Unosquares "Unosquare.Raspberry.IO" and "Unosquare.WiringPi", which essentially enables to use WiringPi, which is written for C++, with C#. Both packages can be obtained via the NuGet-Package-Manger in Visual Studio. 

# Prepare code
Create a new instance of `LCD` by using:

```cs
LCD LCD = new LCD(LCD_RS, LCD_E, LCD_DATA4, LCD_DATA5, LCD_DATA6, LCD_DATA7);
```

where you need to specify the GPIO pin ids for the parameters `LCD_RS`, `LCD_E`, `LCD_DATA4`, `LCD_DATA5`,`LCD_DATA6`,`LCD_DATA7`
according to WiringPi.

Then you need to initialize it by

```cs
LCD.Initialize();
```


# Usage
## Send Message
To send a message to the LCD display, you can use:

```cs
LCD.SendMessage(string message, int lcdRow);
```

If `lcdRow` is omitted, the message will be sent to the first row.

## Clear Screen
To clear the LCD display, you can use:
```cs
LCD.Clear();
```

# Road-Map
* Add scrollable Text
* Add cursor function
* Add custom figures

# Credits
The code is based on [this python snippet](http://www.tutorials-raspberrypi.de/wp-content/uploads/scripts/hd44780_test.py).
