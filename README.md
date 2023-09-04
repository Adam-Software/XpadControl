# XpadControl

A cross-platform console application  for controlling the Adam robot via a local/remote socket using an xbox gamepad and its analogues.

### Capture and simulation of game controllers

To capture the gamepad in Windows, the [XInputium](https://github.com/AderitoSilva/XInputium) library is used. In Linux the [Gamepad](https://github.com/nahueltaibo/gamepad) library is used.

To simulate the operation of controllers in Linux, you can use [uinput-joystick-demo](https://github.com/GrantEdwards/uinput-joystick-demo). In Windows [x360ce](https://github.com/x360ce/x360ce)

### Install xpad driver in Linux (raspberrypi)

```bash
$ sudo apt-get install raspberrypi-kernel-headers
$ sudo apt-get install dkms 
$ sudo git clone https://github.com/paroj/xpad.git /usr/src/xpad-0.4
$ sudo dkms install -m xpad -v 0.4
```
