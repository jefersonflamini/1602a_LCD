using System;
using Unosquare.RaspberryIO;
using System.Threading;
//using Unosquare.RaspberryIO.Abstractions;
//using Unosquare.WiringPi;

namespace LCD_Displays
{
	public class LCD
	{
		public static int LCD_RS;
		public static bool HL_RS;
		public static int LCD_E;
		public static bool HL_E;
		public static int LCD_DATA4;
		public static bool HL_DATA4;
		public static int LCD_DATA5;
		public static bool HL_DATA5;
		public static int LCD_DATA6;
		public static bool HL_DATA6;
		public static int LCD_DATA7;
		public static bool HL_DATA7;
		public static int LCD_WIDTH;
		public static int LCD_LINE_1;
		public static int LCD_LINE_2;
		public int E_PULSE = 1;
		public int E_DELAY = 1;


		public LCD(int pLCD_RS, int pLCD_E, int pLCD_DATA4, int pLCD_DATA5, int pLCD_DATA6, int pLCD_DATA7)
		{
			LCD_WIDTH = 16;         // Zeichen je Zeile
			LCD_LINE_1 = 128;      // Adresse der ersten Display Zeile
			LCD_LINE_2 = 192;      // Adresse der zweiten Display Zeile

			LCD_RS = pLCD_RS;
			LCD_E = pLCD_E;
			LCD_DATA4 = pLCD_DATA4;
			LCD_DATA5 = pLCD_DATA5;
			LCD_DATA6 = pLCD_DATA6;
			LCD_DATA7 = pLCD_DATA7;
		}
		public void InitializeLCD()
		{
			Pi.Init<Unosquare.WiringPi.BootstrapWiringPi>();
			var PIN_RS = Pi.Gpio[LCD_RS];
			var PIN_E = Pi.Gpio[LCD_E];
			var PIN_DATA4 = Pi.Gpio[LCD_DATA4];
			var PIN_DATA5 = Pi.Gpio[LCD_DATA5];
			var PIN_DATA6 = Pi.Gpio[LCD_DATA6];
			var PIN_DATA7 = Pi.Gpio[LCD_DATA7];

			PIN_RS.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_E.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA4.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA5.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA6.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA7.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;


			this.SendByte(51, false);
			this.SendByte(50, false);
			this.SendByte(12, false);
			this.SendByte(06, false);
			this.SendByte(01, false);

		}

		private void SendByte(int bits, bool mode)
		{
			var PIN_RS = Pi.Gpio[LCD_RS];
			var PIN_E = Pi.Gpio[LCD_E];
			var PIN_DATA4 = Pi.Gpio[LCD_DATA4];
			var PIN_DATA5 = Pi.Gpio[LCD_DATA5];
			var PIN_DATA6 = Pi.Gpio[LCD_DATA6];
			var PIN_DATA7 = Pi.Gpio[LCD_DATA7];

			PIN_RS.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_E.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA4.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA5.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA6.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA7.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;

			// Registermodus
			PIN_RS.Write(mode); // H: Data Input, L: Instruction Input

			// Alle Pins einmal Low
			PIN_DATA4.Write(false);
			HL_DATA4 = false;
			PIN_DATA5.Write(false);
			HL_DATA5 = false;
			PIN_DATA6.Write(false);
			HL_DATA6 = false;
			PIN_DATA7.Write(false);
			HL_DATA7 = false;

			if ((bits & 16) == 16)
			{
				PIN_DATA4.Write(true);
				HL_DATA4 = true;
			}

			if ((bits & 32) == 32)
			{
				PIN_DATA5.Write(true);
				HL_DATA5 = true;
			}

			if ((bits & 64) == 64)
			{
				PIN_DATA6.Write(true);
				HL_DATA6 = true;
			}

			if ((bits & 128) == 128)
			{
				PIN_DATA7.Write(true);
				HL_DATA7 = true;
			}

			// Pin E einmal High und einmal Low schalten
			Thread.Sleep(E_DELAY); // Sleep für 1 ms statt 0,5 ms
			PIN_E.Write(true);
			Thread.Sleep(E_PULSE); // Sleep für 1 ms statt 0,5 ms
			PIN_E.Write(false);
			Thread.Sleep(E_DELAY); // Sleep für 1 ms statt 0,5 ms

			// Alle Pins einmal Low
			PIN_DATA4.Write(false);
			HL_DATA4 = false;
			PIN_DATA5.Write(false);
			HL_DATA5 = false;
			PIN_DATA6.Write(false);
			HL_DATA6 = false;
			PIN_DATA7.Write(false);
			HL_DATA7 = false;

			if ((bits & 1) == 1)
			{
				PIN_DATA4.Write(true);
				HL_DATA4 = false;
			}

			if ((bits & 2) == 2)
			{
				PIN_DATA5.Write(true);
				HL_DATA5 = false;
			}

			if ((bits & 4) == 4)
			{
				PIN_DATA6.Write(true);
				HL_DATA6 = false;
			}

			if ((bits & 8) == 8)
			{
				PIN_DATA7.Write(true);
				HL_DATA7 = false;
			}

			// Pin E einmal High und einmal Low schalten
			Thread.Sleep(E_DELAY); // Sleep für 1 ms statt 0,5 ms
			PIN_E.Write(true);
			Thread.Sleep(E_PULSE); // Sleep für 1 ms statt 0,5 ms
			PIN_E.Write(false);
			Thread.Sleep(E_DELAY); // Sleep für 1 ms statt 0,5 ms

		}

		public void SendMessage(string message, int lcdRow = 1)
		{
			message = message.PadRight(LCD_WIDTH);

			var PIN_RS = Pi.Gpio[LCD_RS];
			var PIN_E = Pi.Gpio[LCD_E];
			var PIN_DATA4 = Pi.Gpio[LCD_DATA4];
			var PIN_DATA5 = Pi.Gpio[LCD_DATA5];
			var PIN_DATA6 = Pi.Gpio[LCD_DATA6];
			var PIN_DATA7 = Pi.Gpio[LCD_DATA7];

			PIN_RS.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_E.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA4.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA5.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA6.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;
			PIN_DATA7.PinMode = Unosquare.RaspberryIO.Abstractions.GpioPinDriveMode.Output;

			if (lcdRow == 1)
			{
				this.SendByte(LCD_LINE_1, false);
			}
			else
			{
				this.SendByte(LCD_LINE_2, false);
			}

			for (int i = 0; i < LCD_WIDTH; i++)
			{
				this.SendByte((int)message[i], true);
			}
		}

		public void Clear()
		{
			this.SendMessage("                ", 1);
			this.SendMessage("                ", 2);
		}
	}
}

