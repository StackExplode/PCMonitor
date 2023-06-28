#ifndef __LCD_UI_H__
#define __LCD_UI_H__

#include "sys.h"
#include "lcd.h"



#define ColorFromRGB(r,g,b)  ((u16)((((r) << 8) & 0xF800) |  \
            (((g) << 3) & 0x7E0)  |  \
            (((b) >> 3))))
			
#define Color24To16(x)	ColorFromRGB((x&0xFF0000)>>16,(x&0x00FF00)>>8,x&0x0000FF)

#define CPU_LOAD_COLOR	ColorFromRGB(0x33,0xCC,0xff)
#define CPU_TEMP_COLOR	ColorFromRGB(0x66,0xcc,0xff)
#define GPU_LOAD_COLOR	Color24To16(0x12AF59)
#define GPU_TEMP_COLOR	ColorFromRGB(0x38,0xb4,0x8b)
#define GPU_GRAM_COLOR	Color24To16(0xf8b500)
#define GPU_MCMM_COLOR	ColorFromRGB(0xff,0xff,0xcc)
#define GPU_CENG_COLOR	Color24To16(0xFFEC47)
#define CPU_FREQ_COLOR	Color24To16(0x2ca9e1 )
#define CPU_PWR_COLOR	Color24To16(0x698aab)
#define RAM_FREE_COLOR	Color24To16(0xe597b2)
#define FAN_SPEED_COLOR	Color24To16(0xf08300)	
#define NET_UP_COLOR	Color24To16(0x895b8a)
#define NET_DOWN_COLOR	Color24To16(0xaa4c8f)
#define AIR_TEMP_COLOR	Color24To16(0xe7609e)
#define DISK_TT_COLOR1	WHITE
#define DISK_TT_COLOR2	Color24To16(0x95859c) 


#define UI_DrawString_Big(a,b,c,d,e)	UI_DrawString_32x16(a,b,c,d,e)
#define UI_DrawString_Mid(a,b,c,d,e)	UI_DrawString_24x12(a,b,c,d,e)
#define UI_DrawString_Small(a,b,c,d,e)	UI_DrawString_16x8(a,b,c,d,e)


void UI_FillRec(u16 x,u16 y,u16 width,u16 height,u16 col);
void UI_DrawChar_32x16(u16 x,u16 y,char ch,u16 fc,u16 bc);
void UI_DrawChar_16x8(u16 x,u16 y,char ch,u16 fc,u16 bc);
void UI_DrawChar_24x12(u16 x,u16 y,char ch,u16 fc,u16 bc);
void UI_DrawChar_12x16(u16 x,u16 y,char ch,u16 fc,u16 bc);
void UI_DrawString_32x16(u16 x,u16 y,char* s,u16 fc,u16 bc);
void UI_DrawString_16x8(u16 x,u16 y,char* s,u16 fc,u16 bc);
void UI_DrawString_24x12(u16 x,u16 y,char* s,u16 fc,u16 bc);
void UI_DrawString_12x16(u16 x,u16 y,char* s,u16 fc,u16 bc);


void UI_TestDraw(void);
void UI_PrintFrame(void);
void UI_Change_CPU_Usage(u8 per);
void UI_Change_CPU_Temp(u16 temp);
void UI_Change_GPU_Usage(u8 per);
void UI_Change_GPU_Temp(u16 temp);
void UI_Change_CPU_Freq(u16 freq);
void UI_Change_CPU_Pwr(u16 pwr);
void UI_Change_GRAM_Info(u16 p,u16 u,u16 t);
void UI_Change_GPU_Misc(u16 mc,u16 eng,u16 co,u16 mem);


#endif
