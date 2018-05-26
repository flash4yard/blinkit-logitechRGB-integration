# Blinkit v2.3 | Blink Logitech devices on Steem actions (backend)
Integration of Logitech RGB keyboards into the BlinkIT Software by @techtek

## Blinkit
This software is part of the Blinkit software which can be found [here](https://github.com/techtek/Blinkit). 


## Use the Software as a Standalone

Download the Logitechblink.exe and the config folder.
They have to be in the same directory!
Make sure the path given in the "logitechpath.txt" points to the correct path to the LogitechLED.dll (32 Bit version)
This dll is a part of the Logitech Gaming Software

All parameters can be edited in the other .txt files.

### color
There a RGB value (0-255 for each color) can be set. Make sure every part is three digits long and is seperated with a space.
e. g. : 255 005 123

### delay
Time the keyboard is on and off in ms. Do not insert values below 30ms because the driver can not handle such high speed.

### nrblinks
A value that defines the number how often the keyboard should blink

### path
Correct path to the LogitechLED.dll (32 Bit version)
Standard is provided in the file already.
