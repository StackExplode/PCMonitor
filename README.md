# PCMonitor
A PC state monitor with remote boot and program execution. Work in progress
It is under GPLv3 licence.

## Components

* STM32 program for displaying PC state and booting my PC in bedroom and transfer data
* Linux dot net core app for booting another PC in lab and transfer data between PC and server
* Another Linux dot net core app running in server as backend
* PHP project with code Ignitor framework as front end
* Dot net app as Windows system service to gather PC info and response query(via UDP/HID) and execute command
* Other libs like open hardware monitor code and some shared codes like UDP server code etc.

## Everything is work in progress now!