#ifndef __TIM4_BEEP_H__
#define __TIM4_BEEP_H__

#include "sys.h"





void Beep_Init(void);
void Beep_StartAlways(u16 reg_arr);
void Beep_Stop(void);
#define Beep_FreqAlyways(hz)	(Beep_StartAlways((u16)(100000.0 / hz)))

#endif 