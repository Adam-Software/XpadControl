# XpadControl

![GitHub](https://img.shields.io/github/license/Adam-Software/XpadControl) [![.NET Core Desktop](https://github.com/Adam-Software/XpadControl/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Adam-Software/XpadControl/actions/workflows/dotnet-desktop.yml) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/Adam-Software/XpadControl) ![GitHub tag (with filter)](https://img.shields.io/github/v/tag/Adam-Software/XpadControl)




A cross-platform console application  for controlling the Adam robot via a local/remote socket using an xbox gamepad and its analogues.

## Capture and simulation of game controllers

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
        -c, --set-config-path[optional]... Loads the settings from the specified configuration file path and runs the program
        -s, --show-config-path[optional]... Shows path loaded config and close program
        -v, --version[optional]... Show app version and close program
        -h, --help[optional]... Show this help and close program
```

or Windows

```cmd

C:\XpadControl\bin\Debug\net7.0-windows>XpadControl.exe -h

Usage:
        -c, --set-config-path[optional]... Loads the settings from the specified configuration file path and runs the program
        -s, --show-config-path[optional]... Shows path loaded config and close program
        -v, --version[optional]... Show app version and close program
        -h, --help[optional]... Show this help and close program
```

### Managing configuration files

When running the program without arguments, a configuration file named `appsettings.default.json` will be used, it is located in the "Configs" subfolder in relation to the executable file..
Console output is disabled in it, and the log level of the record is error.

To set your own configuration file, you need to copy and rename the default ones, and then use the `-c` argument when starting the program.

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

To check where the settings are loaded from, run the program with the `-s` key

```bash
$ dotnet XpadControl.dll -s
Setting loaded from /usr/src/XpadControl/bin/Debug/net7.0-windows/publish/Configs/appsettings.default.json
```

### 

## Tested WebSocket server

The test web socket server is located [here](https://raw.githubusercontent.com/Adam-Software/Adam-SDK/main/servers/GamepadDebugServer.py)


