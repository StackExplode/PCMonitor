#include "tim3.h"

void TIM3_Init(void)
{
	GPIO_InitTypeDef     GPIO_InitStrue;
    TIM_OCInitTypeDef     TIM_OCInitStrue;
    TIM_TimeBaseInitTypeDef     TIM_TimeBaseInitStrue;
    
    
    RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3,ENABLE);        //ʹ��TIM3�����GPIOʱ��
    RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO,ENABLE);// ʹ��GPIOBʱ��(LED��BP5����),ʹ��AFIOʱ��(��ʱ��3ͨ��2��Ҫ��ӳ�䵽BP5����)
    RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB,ENABLE);
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC,ENABLE);
    
    GPIO_InitStrue.GPIO_Pin=GPIO_Pin_7;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_AF_PP;    // ��������
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //�����������ٶ�
    GPIO_Init(GPIOC,&GPIO_InitStrue);                //GPIO�˿ڳ�ʼ������
    
	GPIO_InitStrue.GPIO_Pin=GPIO_Pin_7;     // TIM_CH2
    GPIO_InitStrue.GPIO_Mode=GPIO_Mode_Out_OD;    // ��������
    GPIO_InitStrue.GPIO_Speed=GPIO_Speed_50MHz;    //�����������ٶ�
    GPIO_Init(GPIOB,&GPIO_InitStrue); 
	PBout(7) = 0;
	
    //GPIO_PinRemapConfig(GPIO_PartialRemap_TIM3,ENABLE);
	GPIO_PinRemapConfig(GPIO_FullRemap_TIM3,ENABLE);
    
    TIM_TimeBaseInitStrue.TIM_Period=1000 - 1;    //200
    TIM_TimeBaseInitStrue.TIM_Prescaler=720*2 - 1;        //18
    TIM_TimeBaseInitStrue.TIM_CounterMode=TIM_CounterMode_Up;    //�������������
    TIM_TimeBaseInitStrue.TIM_ClockDivision=TIM_CKD_DIV1;        //ʱ�ӵķ�Ƶ���ӣ�����һ������ʱ���ã�һ����ΪTIM_CKD_DIV1
    TIM_TimeBaseInit(TIM3,&TIM_TimeBaseInitStrue);        //TIM3��ʼ������(����PWM������)
    
    TIM_OCInitStrue.TIM_OCMode=TIM_OCMode_PWM2;        // PWMģʽ2:CNT>CCRʱ�����Ч
    TIM_OCInitStrue.TIM_OCPolarity=TIM_OCPolarity_High;// ���ü���-��ЧΪ�ߵ�ƽ
    TIM_OCInitStrue.TIM_OutputState=TIM_OutputState_Enable;// ���ʹ��
    TIM_OC2Init(TIM3,&TIM_OCInitStrue);        //TIM3��ͨ��2PWM ģʽ����

    TIM_OC2PreloadConfig(TIM3,TIM_OCPreload_Enable);        //ʹ��Ԥװ�ؼĴ���
    
    TIM_Cmd(TIM3,ENABLE);        //ʹ��TIM3
}

__forceinline void TIM3_SetPWM(u16 pwm)
{
	 TIM_SetCompare2(TIM3,pwm);  
}