#ifndef __JBUS_H__
#define __JBUS_H__


#include "stdint.h"
#include "string.h"

#define _sizeof(T) ((size_t)((T*)0 + 1))

/********************************************/
//Configs
#define JBUS_ADDR						0x03
#define JBUS_HANDLER_COUNT				64u
#define JBUS_TRANS_HEADFILE				"hw_config.h"
#define JBUS_MTU						1024u
#define JBUS_WORKBUFF_LEN				(JBUS_MTU + _sizeof(struct JBUS_DATA_TYPE))			
#define JBUS_SEND_BUFFER_POINTER		&USB_TXBuffer[2100-JBUS_MTU]
#define JBUS_LITTLE_ENDIAN				1
#define JBUS_REC_NOCRC					1
#define JBUS_REC_NOSUM					1
#define JBUS_REC_MISC					0
//End of Configs
/********************************************/

#include JBUS_TRANS_HEADFILE

typedef uint32_t  u32;
typedef uint16_t u16;
typedef uint8_t  u8;
typedef int32_t  s32;
typedef int16_t s16;
typedef int8_t  s8;


typedef struct JBUS_FLAG_TYPE	//Watch out, it is little endian
{
	uint8_t ODD : 1;
	uint8_t URG : 1;
	uint8_t NAK : 1;
	uint8_t REL : 1;
	uint8_t ERR : 1;
	uint8_t FIN : 1;
	uint8_t PSH : 1;
	uint8_t ACK : 1;
}JBUS_FLAG_TYPE;

typedef struct JBUS_DATA_TYPE
{
	uint8_t OrgAddr;
	uint8_t DestAddr;
	JBUS_FLAG_TYPE Flag;
	uint8_t Len[2];
	uint8_t Fun;
	uint8_t CRC16[2];
	uint8_t Sum16[2];
	//Head Takes 10 bytes
	uint8_t Data[];	
}JBUS_DATA_TYPE;


typedef enum JBUS_WORKING_FLAG
{
	JB_IDLE = 0,
	JB_RECING,
	JB_REC_FINISH,
	JB_HANDING,
	JB_REPLYING,
	JB_CALLING,
	JB_WAITREPLY
}JBUS_WORKING_FLAG;


typedef u8 (*JBUS_HANDLE_FUN)(JBUS_DATA_TYPE *data);

typedef struct JBUS_Type
{
	uint8_t WorkBuffer[JBUS_WORKBUFF_LEN];
	JBUS_HANDLE_FUN Handlers[JBUS_HANDLER_COUNT];
	uint16_t StartPointer;
	uint16_t WorkPointer;
	uint8_t HandlerPointer;
	JBUS_WORKING_FLAG WorkFlag;
	uint8_t* TXBufferPtr;
	
}JBUS_Type;



/***********************************************************/

extern JBUS_Type JBUSObj;
void JBUS_DataRecCb(uint8_t* data,uint16_t len);
void JBUS_FinishDataSend(void);
u8* JBUS_GetBufferPtr(void);
void JBUS_ShiftWorkPtr(s32 shift);

u16 SwapEndian16(void *data);
u32 SwapEndian32(void* data);
void SwapEndian(u8 *data,u8 *dst,u8 len);

extern void JBUS_Init(void);
extern void JBUS_Execution(void);
void JBUS_StartDataSend(u8 addr,u8 fun,JBUS_FLAG_TYPE flag);
uint8_t JBUS_Bind_Handler(uint8_t func,JBUS_HANDLE_FUN fun);
void JBUS_Delete_Handler(u8 func);
void JBUS_FillBuffer(void* data,u16 len);
void JBUS_Finish_Handle(void);

#if !JBUS_LITTLE_ENDIAN
#undef __REV
#define __REV(x) (x)
#undef __REV16
#define __REV16(x) (x)
#endif


#endif
