@echo off
REM Run this file to build the project outside of the IDE.
REM WARNING: if using a different machine, copy the .rsp files together with this script.
echo main.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/USER/main.gcc.rsp" || exit 1
echo stm32f10x_it.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/USER/stm32f10x_it.gcc.rsp" || exit 1
echo system_stm32f10x.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/USER/system_stm32f10x.gcc.rsp" || exit 1
echo lcdui.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/USER/lcdui.gcc.rsp" || exit 1
echo delay.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/SYSTEM/delay/delay.gcc.rsp" || exit 1
echo sys.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/SYSTEM/sys/sys.gcc.rsp" || exit 1
echo usart.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/SYSTEM/usart/usart.gcc.rsp" || exit 1
echo core_cm3.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/CORE/core_cm3.gcc.rsp" || exit 1
echo misc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/misc.gcc.rsp" || exit 1
echo stm32f10x_adc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_adc.gcc.rsp" || exit 1
echo stm32f10x_bkp.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_bkp.gcc.rsp" || exit 1
echo stm32f10x_can.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_can.gcc.rsp" || exit 1
echo stm32f10x_cec.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_cec.gcc.rsp" || exit 1
echo stm32f10x_crc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_crc.gcc.rsp" || exit 1
echo stm32f10x_dac.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_dac.gcc.rsp" || exit 1
echo stm32f10x_dbgmcu.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_dbgmcu.gcc.rsp" || exit 1
echo stm32f10x_dma.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_dma.gcc.rsp" || exit 1
echo stm32f10x_exti.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_exti.gcc.rsp" || exit 1
echo stm32f10x_flash.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_flash.gcc.rsp" || exit 1
echo stm32f10x_fsmc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_fsmc.gcc.rsp" || exit 1
echo stm32f10x_gpio.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_gpio.gcc.rsp" || exit 1
echo stm32f10x_i2c.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_i2c.gcc.rsp" || exit 1
echo stm32f10x_iwdg.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_iwdg.gcc.rsp" || exit 1
echo stm32f10x_pwr.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_pwr.gcc.rsp" || exit 1
echo stm32f10x_rcc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_rcc.gcc.rsp" || exit 1
echo stm32f10x_rtc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_rtc.gcc.rsp" || exit 1
echo stm32f10x_sdio.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_sdio.gcc.rsp" || exit 1
echo stm32f10x_spi.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_spi.gcc.rsp" || exit 1
echo stm32f10x_tim.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_tim.gcc.rsp" || exit 1
echo stm32f10x_usart.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_usart.gcc.rsp" || exit 1
echo stm32f10x_wwdg.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/STM32F10x_FWLib/src/stm32f10x_wwdg.gcc.rsp" || exit 1
echo lcd.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/lcd.gcc.rsp" || exit 1
echo SPI.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/SPI.gcc.rsp" || exit 1
echo hw_config.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/hw_config.gcc.rsp" || exit 1
echo usb_desc.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/usb_desc.gcc.rsp" || exit 1
echo usb_endp.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/usb_endp.gcc.rsp" || exit 1
echo usb_istr.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/usb_istr.gcc.rsp" || exit 1
echo usb_prop.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/usb_prop.gcc.rsp" || exit 1
echo usb_pwr.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Conf/usb_pwr.gcc.rsp" || exit 1
echo usb_core.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_core.gcc.rsp" || exit 1
echo usb_init.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_init.gcc.rsp" || exit 1
echo usb_int.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_int.gcc.rsp" || exit 1
echo usb_mem.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_mem.gcc.rsp" || exit 1
echo usb_regs.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_regs.gcc.rsp" || exit 1
echo usb_sil.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/USB/Lib/src/usb_sil.gcc.rsp" || exit 1
echo JBUS.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/JBUS.gcc.rsp" || exit 1
echo tim3.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/tim3.gcc.rsp" || exit 1
echo mygpio.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/mygpio.gcc.rsp" || exit 1
echo tim4_beep.c
E:\Keil_v5\ARM\ARMCC\bin\armcc.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/Hardware/tim4_beep.gcc.rsp" || exit 1
echo startup_stm32f10x_hd.S
E:\Keil_v5\ARM\ARMCC\bin\armasm.exe --Via "VisualGDB/Debug/_4_/STM32/ComputerMonitor/Template/CORE/startup_stm32f10x_hd.gcc.rsp" || exit 1
echo Linking ../VisualGDB/Debug/PCMonitorMCU...
E:\Keil_v5\ARM\ARMCC\bin\armlink.exe --Via ../VisualGDB/Debug/PCMonitorMCU.link.rsp || exit 1
E:\Keil_v5\ARM\ARMCC\bin\fromelf.exe --Via ../VisualGDB/Debug/PCMonitorMCU.mkbin.rsp || exit 1
E:\Keil_v5\ARM\ARMCC\bin\fromelf.exe --Via ../VisualGDB/Debug/PCMonitorMCU.mkihex.rsp || exit 1
