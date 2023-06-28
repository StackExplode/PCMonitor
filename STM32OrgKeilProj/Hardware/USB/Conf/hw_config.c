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
//本程序只供学习使用，未经作者许可，不得用于其它任何用途
//ALIENTEK战舰STM32开发板V3
//USB-hw_config 代码	   
//正点原子@ALIENTEK
//技术论坛:www.openedv.com
//创建日期:2015/1/28
//版本：V1.0
//版权所有，盗版必究。
//Copyright(C) 广州市星翼电子科技有限公司 2009-2019
//All rights reserved									  
////////////////////////////////////////////////////////////////////////////////// 
 
//_usb_usart_fifo uu_txfifo;					//USB串口发送FIFO结构体 
//u8  USART_PRINTF_Buffer[USB_USART_REC_LEN];	//usb_printf发送缓冲区

////用类似串口1接收数据的方法,来处理USB虚拟串口接收到的数据.
//u8 USB_USART_RX_BUF[USB_USART_REC_LEN]; 	//接收缓冲,最大USART_REC_LEN个字节.
////接收状态
////bit15，	接收完成标志
////bit14，	接收到0x0d
////bit13~0，	接收到的有效字节数目
//u16 USB_USART_RX_STA=0;       				//接收状态标记	 

//extern LINE_CODING linecoding;							//USB虚拟串口配置信息
/////////////////////////////////////////////////////////////////////////////////
//各USB例程通用部分代码,ST各各USB例程,此部分代码都可以共用.
//此部分代码一般不需要修改!

//USB唤醒中断服务函数
void USBWakeUp_IRQHandler(void) 
{
	EXTI_ClearITPendingBit(EXTI_Line18);//清除USB唤醒中断挂起位
} 

//USB中断处理函数
void USB_LP_CAN1_RX0_IRQHandler(void) 
{
	USB_Istr();
} 

//USB时钟配置函数,USBclk=48Mhz@HCLK=72Mhz
void Set_USBClock(void)
{
	RCC_USBCLKConfig(RCC_USBCLKSource_PLLCLK_1Div5);//USBclk=PLLclk/1.5=48Mhz	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USB, ENABLE);	 //USB时钟使能		 
} 

//USB进入低功耗模式
//当USB进入suspend模式时,MCU进入低功耗模式
//需自行添加低功耗代码(比如关时钟等)
void Enter_LowPowerMode(void)
{
 	//printf("usb enter low power mode\r\n");
	bDeviceState=SUSPENDED;
} 

//USB退出低功耗模式
//用户可以自行添加相关代码(比如重新配置时钟等)
void Leave_LowPowerMode(void)
{
	DEVICE_INFO *pInfo=&Device_Info;
	//printf("leave low power mode\r\n"); 
	if (pInfo->Current_Configuration!=0)bDeviceState=CONFIGURED; 
	else bDeviceState = ATTACHED; 
} 

//USB中断配置
void USB_Interrupts_Config(void)
{ 
	NVIC_InitTypeDef NVIC_InitStructure;
	EXTI_InitTypeDef EXTI_InitStructure;

 
	/* Configure the EXTI line 18 connected internally to the USB IP */
	EXTI_ClearITPendingBit(EXTI_Line18);
											  //  开启线18上的中断
	EXTI_InitStructure.EXTI_Line = EXTI_Line18; // USB resume from suspend mode
	EXTI_InitStructure.EXTI_Trigger = EXTI_Trigger_Rising;	//line 18上事件上升降沿触发
	EXTI_InitStructure.EXTI_LineCmd = ENABLE;
	EXTI_Init(&EXTI_InitStructure); 	 

	/* Enable the USB interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USB_LP_CAN1_RX0_IRQn;	//组2，优先级次之 
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
	NVIC_Init(&NVIC_InitStructure);
	
	/* Enable the USB Wake-up interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USBWakeUp_IRQn;   //组2，优先级最高	
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0;
	NVIC_Init(&NVIC_InitStructure);   
}	

//USB接口配置(配置1.5K上拉电阻,战舰V3不需要配置,恒上拉)
//NewState:DISABLE,不上拉
//         ENABLE,上拉
void USB_Cable_Config (FunctionalState NewState)
{ 
	//if (NewState!=DISABLE)printf("usb pull up enable\r\n"); 
	//else printf("usb pull up disable\r\n"); 
}

//USB使能连接/断线
//enable:0,断开
//       1,允许连接	   
void USB_Port_Set(u8 enable)
{
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA,ENABLE);    //使能PORTA时钟		 
	if(enable)_SetCNTR(_GetCNTR()&(~(1<<1)));//退出断电模式
	else
	{	 
		_SetCNTR(_GetCNTR()|(1<<1));  // 断电模式
		GPIOA->CRH&=0XFFF00FFF;
		GPIOA->CRH|=0X00033000;
		PAout(12)=0;	    		  
	}
}  

//获取STM32的唯一ID
//用于USB配置信息
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

//将32位的值转换成unicode.
//value,要转换的值(32bit)
//pbuf:存储地址
//len:要转换的长度
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














