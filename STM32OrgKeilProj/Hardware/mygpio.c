#include "mygpio.h"
#include "tim4_beep.h"

void MyGPIO_Init(void)
{
	GPIO_InitTypeDef     GPIO_InitStrue;
                                                     
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB | RCC_APB2Periph_GPIOC, ENABLE);
	

	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_9;     
	GPIO_InitStrue.GPIO_Mode=GPIO_Mode_IN_FLOATING;    
	GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    
	GPIO_Init(GPIOC,&GPIO_InitStrue); 

	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_0;     
	GPIO_InitStrue.GPIO_Mode=GPIO_Mode_Out_PP;
	GPIO_Init(GPIOB,&GPIO_InitStrue); 
	PBout(0) = 1;
	
	GPIO_PinRemapConfig(GPIO_Remap_SWJ_JTAGDisable, ENABLE); 			//JTAG-DP Disabled and SW-DP Enabled
	
}

u8 beep_flag = 0;

void MyGPIO_ScanMode(void)
{
	
	if(PCin(9))
	{
		PBout(0) = 0;
		if(!beep_flag)
		{
			Beep_FreqAlyways(523.25);
			beep_flag = 1;
		}
		
	}
	else
	{
		PBout(0) = 1;
		if(beep_flag)
		{
			Beep_Stop();
			beep_flag = 0;
		}
	}
	
}