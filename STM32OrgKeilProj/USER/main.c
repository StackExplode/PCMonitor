#include "stm32f10x.h"
#include "sys.h"
#include "delay.h"
#include "lcd.h"
#include "hw_config.h"
#include "JBUS.h"
#include "tim3.h"
#include "lcdui.h"
#include "mygpio.h"
#include "tim4_beep.h"

#pragma pack (1) 
typedef struct HD_Info_Data
{
	u8 PWM;
	u8 CPU_Load;
	u16 CPU_Temp;
	u8 GPU_Load;
	u16 GPU_Temp;
	u16 CPU_Freq;
	u16 CPU_PWR;
	u16 GRAM_Percent;
	u16 GRAM_Used;
	u16 GRAM_Total;
	u16 GPU_MCLoad;
	u16 GPU_VELoad;
	u16 GPU_CoreFreq;
	u16 GPU_MemFreq;
	
}HD_Info_Data;	
#pragma pack ()

void LED_Init(void)
{
	GPIO_InitTypeDef  GPIO_InitStructure;
	 
  RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC,ENABLE);
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_13;			    //LED0-->PB.5 端口配置
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 	 //推挽输出
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;	 //IO口速度为50MHz
  GPIO_Init(GPIOC, &GPIO_InitStructure);			     //初始化GPIOB.5
  GPIO_SetBits(GPIOC,GPIO_Pin_13);					//PB.5 输出高  
}

u8 Handle_0x07(JBUS_DATA_TYPE *data)
{
	
	data->Flag.ACK = 1;
	JBUS_FillBuffer("Bonjor!",8);
	JBUS_StartDataSend(data->OrgAddr,0x07,data->Flag);
	return 0;
}

u8 Handle_0x10(JBUS_DATA_TYPE *data)
{
	
	u8 pwm = data->Data[0];
	if(pwm == 0)
		PBout(7) = 0;
	else
	{
		PBout(7) = 1;
		TIM3_SetPWM(pwm);
	}
	JBUS_Finish_Handle();
	return 0;
}

u8 Handle_0x11(JBUS_DATA_TYPE *data)
{
	
	HD_Info_Data* hd = (HD_Info_Data*)data->Data;
	u8 pwm = hd->PWM;
	if(pwm == 0)
		PBout(7) = 0;
	else
	{
		PBout(7) = 1;
		TIM3_SetPWM(pwm);
	}
	UI_Change_CPU_Usage(hd->CPU_Load);
	UI_Change_CPU_Temp(hd->CPU_Temp);
	UI_Change_GPU_Usage(hd->GPU_Load);
	UI_Change_GPU_Temp(hd->GPU_Temp);
	UI_Change_CPU_Freq(hd->CPU_Freq);
	UI_Change_CPU_Pwr(hd->CPU_PWR);
	UI_Change_GRAM_Info(hd->GRAM_Percent,hd->GRAM_Used,hd->GRAM_Total);
	UI_Change_GPU_Misc(hd->GPU_MCLoad,hd->GPU_VELoad,hd->GPU_CoreFreq,hd->GPU_MemFreq);
	JBUS_Finish_Handle();
	return 0; 
}





int main(void)
{	
	SystemInit();
	delay_init();
	TIM3_Init();
	//TIM3_SetPWM(0x60);
	
// 	while(1)
// 	{
// 		TIM3_SetPWM(1000-40);
// 		delay_ms(1000);
// 		TIM3_SetPWM(1000);
// 		delay_ms(1000);delay_ms(1000);
// 		TIM3_SetPWM(1000-125);
// 		delay_ms(1000);delay_ms(1000);delay_ms(1000);
// 	}
	//Add some comment
	
	MyGPIO_Init();
	Beep_Init();
	
	
	//delay_ms(1800);
	USB_Port_Set(0); 	//USB先断开
	delay_ms(200);
	USB_Port_Set(1);	//USB再次连接
	USB_Interrupts_Config();
	Set_USBClock();     
	USB_Init();
	//delay_ms(1200);
	
	LCD_Init();
	LCD_direction(2);
	LCD_Clear(BLACK);
	
	//UI_PrintFrame();
	
	UI_TestDraw();
	
	
	//JBUS_Bind_Handler(0x07,Handle_0x07);
	//JBUS_Bind_Handler(0x10,Handle_0x10);
	//JBUS_Bind_Handler(0x11,Handle_0x11);
	
	while(1)
	{
		JBUS_Execution();
		MyGPIO_ScanMode();
		
	}
	
	return -1;
}
