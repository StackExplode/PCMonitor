#include "tim3.h"

void TIM3_Init(void)
{
	GPIO_InitTypeDef     GPIO_InitStrue;
    TIM_OCInitTypeDef     TIM_OCInitStrue;
    TIM_TimeBaseInitTypeDef     TIM_TimeBaseInitStrue;
    
    
    RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3,ENABLE);        //使能TIM3和相关GPIO时钟
    RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO,ENABLE);// 使能GPIOB时钟(LED在BP5引脚),使能AFIO时钟(定时器3通道2需要重映射到BP5引脚)
    RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB,ENABLE);
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC,ENABLE);
    
    GPIO_InitStrue.GPIO_Pin=GPIO_Pin_7;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_AF_PP;    // 复用推挽
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //设置最大输出速度
    GPIO_Init(GPIOC,&GPIO_InitStrue);                //GPIO端口初始化设置
    
	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_7;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_Out_OD;    // 复用推挽
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //设置最大输出速度
    GPIO_Init(GPIOB,&GPIO_InitStrue); 
	PBout(7) = 0;
	
    //GPIO_PinRemapConfig(GPIO_PartialRemap_TIM3,ENABLE);
	GPIO_PinRemapConfig(GPIO_FullRemap_TIM3,ENABLE);
    
    TIM_TimeBaseInitStrue.TIM_Period=1000 - 1;    //200
    TIM_TimeBaseInitStrue.TIM_Prescaler=720*2 - 1;        //18
    TIM_TimeBaseInitStrue.TIM_CounterMode=TIM_CounterMode_Up;    //计数器向上溢出
    TIM_TimeBaseInitStrue.TIM_ClockDivision=TIM_CKD_DIV1;        //时钟的分频因子，起到了一点点的延时作用，一般设为TIM_CKD_DIV1
    TIM_TimeBaseInit(TIM3,&TIM_TimeBaseInitStrue);        //TIM3初始化设置(设置PWM的周期)
    
    TIM_OCInitStrue.TIM_OCMode=TIM_OCMode_PWM2;        // PWM模式2:CNT>CCR时输出有效
    TIM_OCInitStrue.TIM_OCPolarity=TIM_OCPolarity_High;// 设置极性-有效为高电平
    TIM_OCInitStrue.TIM_OutputState=TIM_OutputState_Enable;// 输出使能
    TIM_OC2Init(TIM3,&TIM_OCInitStrue);        //TIM3的通道2PWM 模式设置

    TIM_OC2PreloadConfig(TIM3,TIM_OCPreload_Enable);        //使能预装载寄存器
    
    TIM_Cmd(TIM3,ENABLE);        //使能TIM3
}

__forceinline void TIM3_SetPWM(u16 pwm)
{
	 TIM_SetCompare2(TIM3,pwm);  
}