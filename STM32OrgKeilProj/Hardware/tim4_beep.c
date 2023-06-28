#include "tim4_beep.h"
#include "delay.h"



void Beep_Init(void)
{
	GPIO_InitTypeDef     GPIO_InitStrue;
    TIM_OCInitTypeDef     TIM_OCInitStrue;
    TIM_TimeBaseInitTypeDef     TIM_TimeBaseInitStrue;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB,ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM4,ENABLE);        //使能TIM3和相关GPIO时钟
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO,ENABLE);// 使能GPIOB时钟(LED在BP5引脚),使能AFIO时钟(定时器3通道2需要重映射到BP5引脚)
    
	
	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_8;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_AF_PP;    // 复用推挽
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //设置最大输出速度
    GPIO_Init(GPIOB,&GPIO_InitStrue);  
	
	//GPIO_PinRemapConfig(GPIO_Remap_TIM4,DISABLE);
	
	 //TIM_InternalClockConfig(TIM4); 
	
	TIM_TimeBaseInitStrue.TIM_Period=200;    //设置自动重装载值
    TIM_TimeBaseInitStrue.TIM_Prescaler=720 - 1;        //预分频系数
    TIM_TimeBaseInitStrue.TIM_CounterMode=TIM_CounterMode_Up;    //计数器向上溢出
    TIM_TimeBaseInitStrue.TIM_ClockDivision=TIM_CKD_DIV1;        //时钟的分频因子，起到了一点点的延时作用，一般设为TIM_CKD_DIV1
    TIM_TimeBaseInit(TIM4,&TIM_TimeBaseInitStrue);        //TIM3初始化设置(设置PWM的周期)
	
	
	TIM_OCInitStrue.TIM_OCMode=TIM_OCMode_PWM2;        // PWM模式2:CNT>CCR时输出有效
    TIM_OCInitStrue.TIM_OCPolarity=TIM_OCPolarity_Low;// 设置极性-有效为高电平
    TIM_OCInitStrue.TIM_OutputState=TIM_OutputState_Enable;// 输出使能
	TIM_OC3Init(TIM4,&TIM_OCInitStrue);        //TIM3的通道2PWM 模式设置

	TIM_SetCompare3(TIM4,0);
    TIM_OC3PreloadConfig(TIM4,TIM_OCPreload_Enable);        //使能预装载寄存器
    	
	
	TIM_ARRPreloadConfig(TIM4, ENABLE); 
	
    //TIM_Cmd(TIM4,ENABLE);        

}



void Beep_StartAlways(u16 reg_arr)
{
	TIM4->ARR = reg_arr;
	TIM4->CNT = 0;
	TIM4->CCR3 = reg_arr / 2;
	
	TIM_Cmd(TIM4,ENABLE);  
}

void Beep_Stop(void)
{
	TIM_Cmd(TIM4,DISABLE); 
}



























