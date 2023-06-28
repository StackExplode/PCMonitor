#include "tim4_beep.h"
#include "delay.h"



void Beep_Init(void)
{
	GPIO_InitTypeDef     GPIO_InitStrue;
    TIM_OCInitTypeDef     TIM_OCInitStrue;
    TIM_TimeBaseInitTypeDef     TIM_TimeBaseInitStrue;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB,ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM4,ENABLE);        //ʹ��TIM3�����GPIOʱ��
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO,ENABLE);// ʹ��GPIOBʱ��(LED��BP5����),ʹ��AFIOʱ��(��ʱ��3ͨ��2��Ҫ��ӳ�䵽BP5����)
    
	
	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_8;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_AF_PP;    // ��������
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //�����������ٶ�
    GPIO_Init(GPIOB,&GPIO_InitStrue);  
	
	//GPIO_PinRemapConfig(GPIO_Remap_TIM4,DISABLE);
	
	 //TIM_InternalClockConfig(TIM4); 
	
	TIM_TimeBaseInitStrue.TIM_Period=200;    //�����Զ���װ��ֵ
    TIM_TimeBaseInitStrue.TIM_Prescaler=720 - 1;        //Ԥ��Ƶϵ��
    TIM_TimeBaseInitStrue.TIM_CounterMode=TIM_CounterMode_Up;    //�������������
    TIM_TimeBaseInitStrue.TIM_ClockDivision=TIM_CKD_DIV1;        //ʱ�ӵķ�Ƶ���ӣ�����һ������ʱ���ã�һ����ΪTIM_CKD_DIV1
    TIM_TimeBaseInit(TIM4,&TIM_TimeBaseInitStrue);        //TIM3��ʼ������(����PWM������)
	
	
	TIM_OCInitStrue.TIM_OCMode=TIM_OCMode_PWM2;        // PWMģʽ2:CNT>CCRʱ�����Ч
    TIM_OCInitStrue.TIM_OCPolarity=TIM_OCPolarity_Low;// ���ü���-��ЧΪ�ߵ�ƽ
    TIM_OCInitStrue.TIM_OutputState=TIM_OutputState_Enable;// ���ʹ��
	TIM_OC3Init(TIM4,&TIM_OCInitStrue);        //TIM3��ͨ��2PWM ģʽ����

	TIM_SetCompare3(TIM4,0);
    TIM_OC3PreloadConfig(TIM4,TIM_OCPreload_Enable);        //ʹ��Ԥװ�ؼĴ���
    	
	
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



























