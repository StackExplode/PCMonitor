#include "lcdui.h"
#include "lcdfont.h"


static const u16 Line_Color[5] = {RED,BLUE,GREEN,YELLOW,GRAY};

void UI_FillRec(u16 x,u16 y,u16 width,u16 height,u16 col)
{
	LCD_SetWindows(x,y,x+width-1,y+height-1);
	register int max = width*height-1;
	for(register int i=0;i<max;i++)
		Lcd_WriteData_16Bit(col);
	
}


__inline void UI_DrawChar_32x16(u16 x,u16 y,char ch,u16 fc,u16 bc)
{
	const u8* data = CharTbl32x16[ch - 0x20];
	LCD_SetWindows(x,y,x+16-1,y+32-1);
		
	for(int i=0;i<64;i++)
	{
		for(u8 j=0;j<8;j++)
		{
			if(data[i] & (1<<j))
				Lcd_WriteData_16Bit(fc);
			else
				Lcd_WriteData_16Bit(bc);
		}
	}

}

__inline void UI_DrawChar_16x8(u16 x,u16 y,char ch,u16 fc,u16 bc)
{
	const u8* data = CharTbl16x8[ch - 0x20];
	LCD_SetWindows(x,y,x+8-1,y+16-1);
		
	for(int i=0;i<16;i++)
	{
		for(u8 j=0;j<8;j++)
		{
			if(data[i] & (1<<j))
				Lcd_WriteData_16Bit(fc);
			else
				Lcd_WriteData_16Bit(bc);
		}
	}

}

__inline void UI_DrawChar_24x12(u16 x,u16 y,char ch,u16 fc,u16 bc)
{
	const u8* data = CharTbl24x12[ch - 0x20];
	LCD_SetWindows(x,y,x+12-1,y+24-1);
		
	for(int i=0;i<48;i++)
	{
		for(u8 j=0;j<(8 - (i%2)*4);j++)
		{
			if(data[i] & (1<<j))
				Lcd_WriteData_16Bit(fc);
			else
				Lcd_WriteData_16Bit(bc);
		}
	}
}

__inline void UI_DrawChar_12x16(u16 x,u16 y,char ch,u16 fc,u16 bc)
{
	const u8* data = CharTbl12x16[ch - 0x20];
	LCD_SetWindows(x,y,x+12-1,y+16-1);
		
	for(int i=0;i<32;i++)
	{
		for(u8 j=0;j < (8 - (i%2)*4);j++)
		{
			if(data[i] & (1<<j))
				Lcd_WriteData_16Bit(fc);
			else
				Lcd_WriteData_16Bit(bc);
		}
	}
}

void UI_DrawString_32x16(u16 x,u16 y,char* s,u16 fc,u16 bc)
{
	while(*s != 0)
	{
		UI_DrawChar_32x16(x,y,*s,fc,bc);
		s++;
		x += 16;
	}
}

void UI_DrawString_16x8(u16 x,u16 y,char* s,u16 fc,u16 bc)
{
	while(*s != 0)
	{
		UI_DrawChar_16x8(x,y,*s,fc,bc);
		s++;
		x += 8;
	}
}

void UI_DrawString_24x12(u16 x,u16 y,char* s,u16 fc,u16 bc)
{
	while(*s != 0)
	{
		UI_DrawChar_24x12(x,y,*s,fc,bc);
		s++;
		x += 12;
	}
}

void UI_DrawString_12x16(u16 x,u16 y,char* s,u16 fc,u16 bc)
{
	while(*s != 0)
	{
		UI_DrawChar_12x16(x,y,*s,fc,bc);
		s++;
		x += 12;
	}
}

void UI_DrawString_12x16_Ex(u16 x,u16 y,char* s,u16 fc,u16 bc,u8 delta)
{
	while(*s != 0)
	{
		UI_DrawChar_12x16(x,y,*s,fc,bc);
		s++;
		x += delta;
	}
}



void UI_TestDraw(void)
{
	u16 cl = ColorFromRGB(0x10,0x2c,0xff);
	
	UI_FillRec(0,0,LCD_W,64+3,cl);
	UI_DrawString_Big(4*16,0,"32",YELLOW,cl);
	UI_DrawString_Mid(6*16,8,"%",LGRAY,cl);
	UI_DrawString_Big(6*16+1*12,0," 085",WHITE,cl);
	UI_DrawString_Mid(10*16+1*12+4,8,"W",LGRAY,cl);
	UI_DrawString_Big(10*16+2*12+4,0," 1.25",WHITE,cl);
	UI_DrawString_Mid(15*16+2*12+8,8,"V",LGRAY,cl);
	
	UI_DrawString_Big(0,32,"65",RED,cl);
	UI_DrawString_Big(2*16+8,32,"C ",LGRAY,cl);
	UI_DrawString_Small(32+3,28,"o",LGRAY,cl);
	UI_DrawString_Big(0,0,"CPU:",cl,WHITE);
	
	UI_DrawString_Big(4*16+8,32,"5.1",Color24To16(0xff6b88),cl);
	UI_DrawString_Big(7*16+8,32,"/",LGRAY,cl);
	UI_DrawString_Big(8*16+8,32,"3.6",Color24To16(0x00FFFF),cl);
	UI_DrawString_Mid(11*16+16,40,"GHz  ",LGRAY,cl);
	UI_DrawString_Mid(11*16+5*12+16,40,"00/12",WHITE,cl);
	
	UI_FillRec(0,64+3,LCD_W,2,WHITE);
	
	cl = Color24To16(0x15ad66);
	UI_FillRec(0,64+5,LCD_W,64+3,cl);
	UI_DrawString_Big(0,64+8,"GPU:",GPU_LOAD_COLOR,WHITE);
	UI_DrawString_Big(4*16,64+8,"32",YELLOW,cl);
	UI_DrawString_Mid(6*16,64+16,"%",WHITE,cl);
	
}


void UI_PrintFrame(void)
{
	
	u16 cl = CPU_LOAD_COLOR;
	UI_DrawString_32x16(0,0,"CPU",BLACK,cl);
	UI_DrawString_32x16(0,32,"??%",BLACK,cl);
	
	cl = CPU_TEMP_COLOR;
	UI_DrawString_32x16(48,0,"TEMP",BLACK,cl);
	UI_DrawString_32x16(48,32,"??.?",BLACK,cl);
	
	cl = CPU_PWR_COLOR;
	UI_DrawString_32x16(48+64,00,"000.0 W ",BLACK,cl);
	cl = CPU_FREQ_COLOR;
	UI_DrawString_32x16(48+64,32,"1234 MHz",BLACK,cl);

	
	cl = GPU_LOAD_COLOR;
	UI_DrawString_32x16(0,64,"GPU",BLACK,cl);
	UI_DrawString_32x16(0,64+32,"??%",BLACK,cl);
	
	cl = GPU_TEMP_COLOR;
	UI_DrawString_32x16(48,64,"TEMP",BLACK,cl);
	UI_DrawString_32x16(48,64+32,"??.?",BLACK,cl);
	
	cl = GPU_GRAM_COLOR;
	UI_DrawString_16x8(48+64,64," GRAM ",BLACK,cl);
	UI_DrawString_16x8(48+64,64+16," ??.?%",BLACK,cl);
	UI_DrawString_16x8(48+64,64+16*2,"????MB",BLACK,cl);
	UI_DrawString_16x8(48+64,64+16*3,"????MB",BLACK,cl);
	
	cl = GPU_MCMM_COLOR;
	UI_DrawString_16x8(160,64,"M.C. ",BLACK,cl);
	UI_DrawString_16x8(160,64+16,"12.3%",BLACK,cl);
	UI_DrawString_16x8(160+40,64+16*2,"Mem. ",BLACK,cl);
	UI_DrawString_16x8(160+40,64+16*3," 1234",BLACK,cl);
	
	cl = GPU_CENG_COLOR;
	UI_DrawString_16x8(160,64+16*2,"Core ",BLACK,cl);
	UI_DrawString_16x8(160,64+16*3," 1234",BLACK,cl);	
	UI_DrawString_16x8(160+40,64,"VEng.",BLACK,cl);
	UI_DrawString_16x8(160+40,64+16,"12.3%",BLACK,cl);
	
	cl = RAM_FREE_COLOR;
	UI_DrawString_32x16(0,128,"RAM",BLACK,cl);
	UI_DrawString_32x16(0,128+32,"??%",BLACK,cl);
	UI_DrawString_16x8(0,128+64,"12.3GB",BLACK,cl);
	UI_DrawString_16x8(0,128+64+16,"12.3GB",BLACK,cl);
	/*Disk Info Start*/
	
	const char ch[]="NAWRUT";
	const u16 bgs[] = {DISK_TT_COLOR1,DISK_TT_COLOR2};
	for(u8 i=0;i<6;i++)
	{
		cl = bgs[i%2];
		UI_DrawChar_16x8(48,128+16*i,ch[i],Color24To16(0xa22041),cl);
	}
	for(u8 i=0;i<4;i++)
	{
		cl = bgs[(i+1)%2];
		UI_DrawString_16x8(56+i*40,128,"DISK",Color24To16(0x192f60 ),cl);
		UI_DrawChar_16x8(56+i*40+4*8,128,i+'0',Color24To16(0x192f60 ),cl);
	}
	for(u8 i=0;i<4;i++)
	{
		u8 k = 0;
		cl = bgs[(i+k)%2];
		UI_DrawString_16x8(56+40*i,144+16*k,"000.0",BLACK,cl);
		k++;
		cl = bgs[(i+k)%2];
		UI_DrawString_16x8(56+40*i,144+16*k,"1024k",BLACK,cl);
		k++;
		cl = bgs[(i+k)%2];
		UI_DrawString_16x8(56+40*i,144+16*k,"1024k",BLACK,cl);
		k++;
		cl = bgs[(i+k)%2];
		UI_DrawString_16x8(56+40*i,144+16*k,"1024M",BLACK,cl);
		k++;
		cl = bgs[(i+k)%2];
		UI_DrawString_16x8(56+40*i,144+16*k,"1024G",BLACK,cl);
	}
	cl = DISK_TT_COLOR2;
	UI_DrawString_16x8(216,144-16,"   ",BLACK,Color24To16(0xa22041));
	cl = DISK_TT_COLOR1;
	UI_DrawString_16x8(216,144," % ",Color24To16(0xa22041),cl);
	cl = DISK_TT_COLOR2;
	UI_DrawString_16x8(216,144+16*1,"B/s",Color24To16(0xa22041),cl);
	cl = DISK_TT_COLOR1;
	UI_DrawString_16x8(216,144+16*2,"B/s",Color24To16(0xa22041),cl);
	cl = DISK_TT_COLOR2;
	UI_DrawString_16x8(216,144+16*3," GB",Color24To16(0xa22041),cl);
	cl = DISK_TT_COLOR1;
	UI_DrawString_16x8(216,144+16*4," GB",Color24To16(0xa22041),cl);
	
	
	/*Disk Info End*/
	
	cl = FAN_SPEED_COLOR;
	UI_DrawString_32x16(0,224,"FAN",BLACK,cl);
	UI_DrawString_32x16(0,224+32,"50%",BLACK,cl);
	cl = NET_UP_COLOR;
	UI_DrawString_32x16(48,224,"U1024k/s",BLACK,cl);
	cl = NET_DOWN_COLOR;
	UI_DrawString_32x16(48,224+32,"D1024k/s",BLACK,cl);
	cl = AIR_TEMP_COLOR;
	UI_DrawString_32x16(176,224,"AIRT",BLACK,cl);
	UI_DrawString_32x16(176,224+32,"31.2",BLACK,cl);
	
	//Last Line
	cl = BLACK;
	UI_DrawString_16x8(4,288,"2020",WHITE,cl);
	UI_DrawString_16x8(0,288+16,"01-01",WHITE,cl);
	
	UI_DrawString_32x16(52,288,"11:22:33",WHITE,cl);
	UI_DrawString_32x16(192,288,"SUN",RED,cl);
	
}

void UI_Change_CPU_Usage(u8 per)
{
	u16 fc = BLACK;
	if(per > 55)
		fc = YELLOW;
	if(per > 85)
		fc = RED;
	u16 cl = CPU_LOAD_COLOR;
	if(per < 100)
	{
		UI_DrawChar_32x16(0,32,per/10 + '0',fc,cl);
		UI_DrawChar_32x16(16,32,per%10 + '0',fc,cl);
		UI_DrawChar_32x16(32,32,'%',fc,cl);
	}
	else
	{
		UI_DrawChar_32x16(0,32,'M',fc,cl);
		UI_DrawChar_32x16(16,32,'A',fc,cl);
		UI_DrawChar_32x16(32,32,'X',fc,cl);
	}	

}

void UI_Change_CPU_Temp(u16 temp)
{
	u16 fc = BLACK;
	if(temp > 550)
		fc = YELLOW;
	if(temp > 700)
		fc = RED;
	u16 cl = CPU_TEMP_COLOR;
	if(temp < 1000)
	{
		UI_DrawChar_32x16(48,32,temp/100 + '0',fc,cl);
		UI_DrawChar_32x16(64,32,temp%100/10 + '0',fc,cl);
		UI_DrawChar_32x16(80,32,'.',fc,cl);
		UI_DrawChar_32x16(96,32,temp%10 + '0',fc,cl);
	}
	else
	{
		UI_DrawChar_32x16(48,32,temp/1000 + '0',fc,cl);
		UI_DrawChar_32x16(64,32,temp%1000/100 + '0',fc,cl);
		UI_DrawChar_32x16(80,32,temp%100/10 + '0',fc,cl);
		UI_DrawChar_32x16(96,32,temp%10 + '0',fc,cl);
	}
}


void UI_Change_GPU_Usage(u8 per)
{
	u16 fc = BLACK;
	if(per > 55)
		fc = YELLOW;
	if(per > 85)
		fc = RED;
	u16 cl = GPU_LOAD_COLOR;
	char ch[4];
	if(per < 100)
	{
		ch[0] = per/10 + '0';
		ch[1] = per%10 + '0';
		ch[2] = '%';
	}
	else
	{
		ch[0] = 'M';
		ch[1] = 'A';
		ch[2] = 'X';
	}	
	ch[3] = 0;
	UI_DrawString_32x16(0,64+32,ch,fc,cl);
}

void UI_Change_GPU_Temp(u16 temp)
{
	u16 fc = BLACK;
	if(temp > 550)
		fc = YELLOW;
	if(temp > 700)
		fc = RED;
	u16 cl = GPU_TEMP_COLOR;
	char ch[5];
	if(temp < 1000)
	{
		ch[0] = temp/100 + '0';
		ch[1] = temp%100/10 + '0';
		ch[2] = '.';
		ch[3] = temp%10 + '0';
	}
	else
	{
		ch[0] = temp/1000 + '0';
		ch[1] = temp%1000/100 + '0';
		ch[2] = temp%100/10 + '0';
		ch[3] = temp%10 + '0';
	}
	ch[4] = 0;
	UI_DrawString_32x16(48,64+32,ch,fc,cl);
}

void UI_Change_CPU_Freq(u16 freq)
{
	u16 cl = CPU_FREQ_COLOR;
	
	char ch[5];
	ch[0] = freq/1000 + '0';
	ch[1] = freq%1000/100 + '0';
	ch[2] = freq%100/10 + '0';
	ch[3] = freq%10 + '0';
	ch[4] = 0;
	
	UI_DrawString_32x16(48+64,32,ch,BLACK,cl);
}

void UI_Change_CPU_Pwr(u16 pwr)
{
	u16 cl = CPU_PWR_COLOR;
	
	char ch[6];
	ch[0] = pwr/1000 + '0';
	ch[1] = pwr%1000/100 + '0';
	ch[2] = pwr%100/10 + '0';
	ch[3] = '.';
	ch[4] = pwr%10 + '0';
	ch[5] = 0;
	
	UI_DrawString_32x16(48+64,00,ch,BLACK,cl);
}

void UI_Change_GRAM_Info(u16 p,u16 u,u16 t)
{
	u16 cl = GPU_GRAM_COLOR;
	u16 fc = BLACK;
	if(p > 900)
		fc = RED;
	
	char ch[7];
	ch[0] = p/100 + '0';
	ch[1] = p%100/10 + '0';
	ch[2] = '.';
	ch[3] = p%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(48+64+8,64+16,ch,fc,cl);
	
	ch[0] = u/1000 + '0';
	ch[1] = u%1000/100 + '0';
	ch[2] = u%100/10 + '0';
	ch[3] = u%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(48+64,64+16*2,ch,BLACK,cl);
	
	ch[0] = t/1000 + '0';
	ch[1] = t%1000/100 + '0';
	ch[2] = t%100/10 + '0';
	ch[3] = t%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(48+64,64+16*3,ch,BLACK,cl);
}

void UI_Change_GPU_Misc(u16 mc,u16 eng,u16 co,u16 mem)
{
	
	u16 cl = GPU_MCMM_COLOR;
	char ch[7];
	ch[0] = mc/100 + '0';
	ch[1] = mc%100/10 + '0';
	ch[2] = '.';
	ch[3] = mc%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(160,64+16,ch,BLACK,cl);
	
	ch[0] = mem/1000 + '0';
	ch[1] = mem%1000/100 + '0';
	ch[2] = mem%100/10 + '0';
	ch[3] = mem%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(160+40+8,64+16*3,ch,BLACK,cl);
	
	cl = GPU_CENG_COLOR;
	ch[0] = co/1000 + '0';
	ch[1] = co%1000/100 + '0';
	ch[2] = co%100/10 + '0';
	ch[3] = co%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(160+8,64+16*3,ch,BLACK,cl);	
	
	ch[0] = eng/100 + '0';
	ch[1] = eng%100/10 + '0';
	ch[2] = '.';
	ch[3] = eng%10 + '0';
	ch[4] = 0;
	UI_DrawString_16x8(160+40,64+16,ch,BLACK,cl);
}























