#include "usb_lib.h"
#include "usb_prop.h"
#include "usb_desc.h"
#include "usb_istr.h"
#include "hw_config.h"
#include "usb_pwr.h" 
#include "usart.h"  
#include "string.h"	
#include "stdarg.h"		 	
#include "JBUS.h"
//////////////////////////////////////////////////////////////////////////////////	 
//������ֻ��ѧϰʹ�ã�δ���������ɣ��������������κ���;
//ALIENTEKս��STM32������V3
//USB-hw_config ����	   
//����ԭ��@ALIENTEK
//������̳:www.openedv.com
//��������:2015/1/28
//�汾��V1.0
//��Ȩ���У�����ؾ���
//Copyright(C) �������������ӿƼ����޹�˾ 2009-2019
//All rights reserved									  
////////////////////////////////////////////////////////////////////////////////// 
 
//_usb_usart_fifo uu_txfifo;					//USB���ڷ���FIFO�ṹ�� 
//u8  USART_PRINTF_Buffer[USB_USART_REC_LEN];	//usb_printf���ͻ�����

////�����ƴ���1�������ݵķ���,������USB���⴮�ڽ��յ�������.
//u8 USB_USART_RX_BUF[USB_USART_REC_LEN]; 	//���ջ���,���USART_REC_LEN���ֽ�.
////����״̬
////bit15��	������ɱ�־
////bit14��	���յ�0x0d
////bit13~0��	���յ�����Ч�ֽ���Ŀ
//u16 USB_USART_RX_STA=0;       				//����״̬���	 

//extern LINE_CODING linecoding;							//USB���⴮��������Ϣ
/////////////////////////////////////////////////////////////////////////////////
//��USB����ͨ�ò��ִ���,ST����USB����,�˲��ִ��붼���Թ���.
//�˲��ִ���һ�㲻��Ҫ�޸�!

//USB�����жϷ�����
void USBWakeUp_IRQHandler(void) 
{
	EXTI_ClearITPendingBit(EXTI_Line18);//���USB�����жϹ���λ
} 

//USB�жϴ�������
void USB_LP_CAN1_RX0_IRQHandler(void) 
{
	USB_Istr();
} 

//USBʱ�����ú���,USBclk=48Mhz@HCLK=72Mhz
void Set_USBClock(void)
{
	RCC_USBCLKConfig(RCC_USBCLKSource_PLLCLK_1Div5);//USBclk=PLLclk/1.5=48Mhz	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USB, ENABLE);	 //USBʱ��ʹ��		 
} 

//USB����͹���ģʽ
//��USB����suspendģʽʱ,MCU����͹���ģʽ
//���������ӵ͹��Ĵ���(�����ʱ�ӵ�)
void Enter_LowPowerMode(void)
{
 	//printf("usb enter low power mode\r\n");
	bDeviceState=SUSPENDED;
} 

//USB�˳��͹���ģʽ
//�û���������������ش���(������������ʱ�ӵ�)
void Leave_LowPowerMode(void)
{
	DEVICE_INFO *pInfo=&Device_Info;
	//printf("leave low power mode\r\n"); 
	if (pInfo->Current_Configuration!=0)bDeviceState=CONFIGURED; 
	else bDeviceState = ATTACHED; 
} 

//USB�ж�����
void USB_Interrupts_Config(void)
{ 
	NVIC_InitTypeDef NVIC_InitStructure;
	EXTI_InitTypeDef EXTI_InitStructure;

 
	/* Configure the EXTI line 18 connected internally to the USB IP */
	EXTI_ClearITPendingBit(EXTI_Line18);
											  //  ������18�ϵ��ж�
	EXTI_InitStructure.EXTI_Line = EXTI_Line18; // USB resume from suspend mode
	EXTI_InitStructure.EXTI_Trigger = EXTI_Trigger_Rising;	//line 18���¼��������ش���
	EXTI_InitStructure.EXTI_LineCmd = ENABLE;
	EXTI_Init(&EXTI_InitStructure); 	 

	/* Enable the USB interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USB_LP_CAN1_RX0_IRQn;	//��2�����ȼ���֮ 
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
	NVIC_Init(&NVIC_InitStructure);
	
	/* Enable the USB Wake-up interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USBWakeUp_IRQn;   //��2�����ȼ����	
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0;
	NVIC_Init(&NVIC_InitStructure);   
}	

//USB�ӿ�����(����1.5K��������,ս��V3����Ҫ����,������)
//NewState:DISABLE,������
//         ENABLE,����
void USB_Cable_Config (FunctionalState NewState)
{ 
	//if (NewState!=DISABLE)printf("usb pull up enable\r\n"); 
	//else printf("usb pull up disable\r\n"); 
}

//USBʹ������/����
//enable:0,�Ͽ�
//       1,��������	   
void USB_Port_Set(u8 enable)
{
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA,ENABLE);    //ʹ��PORTAʱ��		 
	if(enable)_SetCNTR(_GetCNTR()&(~(1<<1)));//�˳��ϵ�ģʽ
	else
	{	 
		_SetCNTR(_GetCNTR()|(1<<1));  // �ϵ�ģʽ
		GPIOA->CRH&=0XFFF00FFF;
		GPIOA->CRH|=0X00033000;
		PAout(12)=0;	    		  
	}
}  

//��ȡSTM32��ΨһID
//����USB������Ϣ
void Get_SerialNum(void)
{
	u32 Device_Serial0, Device_Serial1, Device_Serial2;
	Device_Serial0 = *(u32*)(0x1FFFF7E8);
	Device_Serial1 = *(u32*)(0x1FFFF7EC);
	Device_Serial2 = *(u32*)(0x1FFFF7F0);
	Device_Serial0 += Device_Serial2;
	if (Device_Serial0 != 0)
	{
		IntToUnicode (Device_Serial0,&CustomHID_StringSerial[2] , 8);
		IntToUnicode (Device_Serial1,&CustomHID_StringSerial[18], 4);
	}
} 

//��32λ��ֵת����unicode.
//value,Ҫת����ֵ(32bit)
//pbuf:�洢��ַ
//len:Ҫת���ĳ���
void IntToUnicode (u32 value , u8 *pbuf , u8 len)
{
	u8 idx = 0;
	for( idx = 0 ; idx < len ; idx ++)
	{
		if( ((value >> 28)) < 0xA )
		{
			pbuf[ 2* idx] = (value >> 28) + '0';
		}
		else
		{
			pbuf[2* idx] = (value >> 28) + 'A' - 10; 
		} 
		value = value << 4; 
		pbuf[ 2* idx + 1] = 0;
	}
}
/////////////////////////////////////////////////////////////////////////////////
 #include "string.h"


void USB_Send(unsigned char *p,u8 len)
{
	UserToPMABufferCopy(p, GetEPTxAddr(ENDP1), len);
	SetEPTxCount(ENDP1, len); 

	SetEPTxValid(ENDP1);

	while(GetEPTxStatus(ENDP1)!=EP_TX_NAK);
}

void USB_Send_64(unsigned char *p,u8 block)
{
	UserToPMABufferCopy(p, GetEPTxAddr(ENDP1), 64);
	SetEPTxCount(ENDP1, 64); 
	SetEPTxValid(ENDP1);
	if(block)
		while(GetEPTxStatus(ENDP1)!=EP_TX_NAK);
}

u8 USB_TXBuffer[2100];
u16 USB_TXCounter=0;

void __inline USB_AppendBuff(u8 p)
{
	USB_TXBuffer[USB_TXCounter++] = p;
}


void USB_Send_Chars(unsigned char *s,u16 len)
{
//	UserToPMABufferCopy(s, GetEPTxAddr(ENDP1), len);

//	SetEPTxCount(ENDP1, len); 

//	SetEPTxValid(ENDP1);
	
	
	if(len <= 64)
	{
		unsigned char p[64]={0};
		memcpy(p,s,len);
		USB_Send_64(p,1);
	}
	else
	{
		s16 ll = len;
		u8 * curr = s;
		for(;ll > 0;ll-=64)
		{
			unsigned char p[64]={0};
			memcpy(p,curr,64);
			USB_Send_64(p,1);
			curr += 64;
		}
//		if(ll < 0)
//		{
//			unsigned char p[64]={0};
//			memcpy(p,curr,64+ll);
//			USB_Send_64(p,1);
//		}
	}
	
	USB_TXCounter = 0;
	JBUS_FinishDataSend();	
}













