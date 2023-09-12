# XpadControl

![GitHub](https://img.shields.io/github/license/Adam-Software/XpadControl)


A cross-platform console application  for controlling the Adam robot via a local/remote socket using an xbox gamepad and its analogues.

### Capture and simulation of game controllers

To capture the gamepad in Windows, the [XInputium](https://github.com/AderitoSilva/XInputium) library is used. In Linux the [Gamepad](https://github.com/nahueltaibo/gamepad) library is used.

To simulate the operation of controllers in Linux, you can use [uinput-joystick-demo](https://github.com/GrantEdwards/uinput-joystick-demo). In Windows [x360ce](https://github.com/x360ce/x360ce)

## Install xpad driver in Linux (raspberrypi)

```bash
$ sudo apt-get install raspberrypi-kernel-headers
$ sudo apt-get install dkms 
$ sudo git clone https://github.com/paroj/xpad.git /usr/src/xpad-0.4
$ sudo dkms install -m xpad -v 0.4
```

## Command line arguments

You can view the command line arguments by typing --help or -h in Linux:

```bash
$ dotnet XpadControl.dll -h
        -c, --config[optional]... Set the path to the settings file
        -v, --version[optional]... Show app version
        -h, --help[optional]... Show this help
```

or Windows

```cmd

C:\XpadControl\bin\Debug\net7.0-windows>XpadControl.exe -h

Usage:
        -c, --config[optional]... Set the path to the settings file
        -v, --version[optional]... Show app version
        -h, --help[optional]... Show this help
```

### Managing configuration files

When running the program without arguments, a configuration file named `appsettings.default.json` will be used, located in the same directory as the executable file.
Console output is disabled in it, and the log level of the record is error.

To set your own configuration file, you need to copy and rename the default ones, and then use the -c argument when starting the program.

If the file is located in the same directory as the executable, it is enough to specify only the name:

```bash
$ dotnet XpadControl.dll -c mysettings.json

```

If the configuration file is located in a nested directory relative to the executable file:

```bash
$ dotnet XpadControl.dll -c settings/mysettings.json

```

If the configuration file is located in an external directory relative to the executable file, you must specify the full path:

```bash
$ dotnet XpadControl.dll -c /etc/mysettings.json

```