#include "StationMacroDef.h"
#include "gMacroDef.h"
#include "parameter.h"
#include "DealWithLCP.h"
#include "DealWithAutoRoute.h"
#include "DealWithATBRoute.h"
#include "DealWithACallonRoute.h"
#include "OverlapProc.h"
#include "DealWithYard.h"
//信号机朝左为Down，朝右为Up
const R_StData_S c_routeTableCfg[] = { 

// Local routes (ci_id: 0): 
/* 0*/ { { { S3_4, LZ_BTN }, { S5_6, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP, 
            1, 1, 1, { T0115_8, W0103_11, W0105_13 }, CDF, 
            1, 0, 0, { { D3_0, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/* 1*/ { { { S4_5, LZ_BTN }, { S6_7, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 1, 1, { T0114_7, W0104_12, W0106_14 }, CDF, 
            1, 0, 0, { { D4_1, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/* 2*/ { { { S5_6, LZ_BTN }, { S9_10, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 2, 1, { W0103_11, W0105_13, T0107_2, W0109_15 }, CDF, 
            2, 2, 0, { { D5_2, NORMAL }, { D7_4, NORMAL },{ D6_3, NORMAL },{ D8_5, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD0_26, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/* 3*/ { { { S5_6, LZ_BTN }, { S10_11, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, S10_11, 
            1, 3, 1, { W0103_11, W0105_13, W0106_14, T0108_3, W0110_16 }, CDF, 
            2, 2, 0, { { D5_2, REVERS }, { D8_5, REVERS },{ D7_4, NORMAL },{ D6_3, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD1_28, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/* 4*/ { { { S6_7, LZ_BTN }, { S9_10, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 3, 1, { W0104_12, W0106_14, W0105_13, T0107_2, W0109_15 }, CDF, 
            2, 2, 0, { { D6_3, REVERS }, { D7_4, REVERS }, { D5_2, NORMAL },{ D8_5, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD0_26, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/* 5*/ { { { S6_7, LZ_BTN }, { S10_11, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, S10_11,
            1, 2, 1, { W0104_12, W0106_14, T0108_3, W0110_16 }, CDF, 
            2, 2, 0, { { D6_3, NORMAL }, { D8_5, NORMAL },{ D5_2, NORMAL },{ D7_4, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD1_28, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/* 6*/ { { { S7_8, LZ_BTN }, { S1_2, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 3, 1, { T0107_2, W0105_13, W0103_11, T0101_0,QDZW }, CDF, 
            3, 2, 0, { { D7_4, NORMAL }, { D5_2, NORMAL }, { D3_0, NORMAL },{ D6_3, NORMAL },{ D8_5, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD0_26, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/* 7*/ { { { S7_8, LZ_BTN }, { S2_3, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0107_2, W0105_13, W0106_14, W0104_12, T0102_1,QDZW }, CDF, 
            3, 2, 0, { { D7_4, REVERS }, { D6_3, REVERS }, { D4_1, NORMAL },{ D8_5, NORMAL },{ D5_2, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD0_26, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/* 8*/ { { { S7_8, LZ_BTN }, { SJ1_18, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0107_2, W0105_13,W0103_11, T0115_8, QDZW }, CDF, 
            3, 2, 0, { { D7_4, NORMAL }, { D5_2, NORMAL }, { D3_0, REVERS },{ D6_3, NORMAL },{ D8_5, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD0_26, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/* 9*/ { { { S7_8, LZ_BTN }, { SJ2_19, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0107_2, W0105_13, W0106_14, W0104_12, T0114_7, QDZW }, CDF, 
            3, 2, 0, { { D7_4, REVERS }, { D6_3, REVERS }, { D4_1, REVERS },{ D8_5, NORMAL },{ D5_2, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD0_26, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/*10*/ { { { S8_9, LZ_BTN }, { S1_2, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0108_3, W0106_14, W0105_13, W0103_11, T0101_0,QDZW }, CDF, 
            3, 2, 0, { { D8_5, REVERS }, { D5_2, REVERS }, { D3_0, NORMAL },{ D7_4, NORMAL },{ D6_3, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD1_28, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*11*/ { { { S8_9, LZ_BTN }, { S2_3, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 3, 1, { T0108_3, W0106_14,W0104_12, T0102_1,QDZW }, CDF, 
            3, 2, 0, { { D8_5, NORMAL }, { D6_3, NORMAL }, { D4_1, NORMAL },{ D7_4, NORMAL },{ D6_3, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD1_28, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*12*/ { { { S8_9, LZ_BTN }, { SJ1_18, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0108_3, W0106_14, W0105_13, W0103_11, T0115_8, QDZW }, CDF, 
            3, 2, 0, { { D8_5, REVERS }, { D5_2, REVERS }, { D3_0, REVERS } ,{ D7_4, NORMAL },{ D6_3, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD1_28, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*13*/ { { { S8_9, LZ_BTN }, { SJ2_19, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0108_3, W0106_14, W0104_12, T0114_7, QDZW }, CDF, 
            3, 2, 0, { { D8_5, NORMAL }, { D6_3, NORMAL }, { D4_1, REVERS },{ D7_4, NORMAL },{ D5_2, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD1_28, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*14*/ { { { S10_11, LZ_BTN }, { S14_15, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0108_3, W0110_16, T0112_5, QDZW }, CDF, 
            1, 1, 0, { { D10_7, NORMAL },{ D9_6, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SSHJ_PSD1_28, 0xFFFF }, 1, { ZYDJ_PSD1_27, 0xFFFF }, 4, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24, C_EB | ZYDJ_EB1_19, C_HOLD | ZYDJ_HD1_23, } },
/*15*/ { { { S11_12, LZ_BTN }, { S7_8, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, S7_8, 
            1, 2, 1, { T0111_4, W0109_15, T0107_2, W0105_13 }, CDF, 
            1, 1, 0, { { D9_6, NORMAL },{ D10_7, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD0_26, 0xFFFF }, 2, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22 } },
/*16*/ { { { S11_12, LZ_BTN }, { S8_9, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0111_4, W0109_15, W0110_16, T0108_3, W0106_14 }, CDF, 
            2, 0, 0, { { D9_6, REVERS }, { D10_7, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 1, { SSHJ_PSD1_28, 0xFFFF }, 2, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*17*/ { { { S12_13, LZ_BTN }, { S8_9, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0112_5, W0110_16, T0108_3, W0106_14 }, CDF, 
            1, 1, 0, { { D10_7, NORMAL },{ D9_6, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { ZYDJ_PSD1_27, 0xFFFF }, 1, { SSHJ_PSD1_28, 0xFFFF }, 4, { C_EB | ZYDJ_EB1_19, C_HOLD | ZYDJ_HD1_23, C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24 } },
/*18*/ { { { S13_14, LZ_BTN }, { S11_12, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0113_6, T0111_4, W0109_15 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZYDJ_PSD0_25, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { ZYDJ_EB0_17, C_HOLD | ZYDJ_HD0_21 } },
/*19*/ { { { SC1_16, LZ_BTN }, { S3_4, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { ZHG1_9, T0115_8, W0103_11 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*20*/ { { { SC2_17, LZ_BTN }, { S4_5, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 1, 1, { ZHG2_10, T0114_7, W0104_12 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*21*/ { { { SJ1_18, LZ_BTN }, { D5_1, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0115_8, ZHG1_9,QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*22*/ { { { SJ2_19, LZ_BTN }, { D2_0, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0114_7, ZHG2_10,QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },

// Cross CI rotes (c_id: 0 -> ci_id: 1): 
/*23*/ { { { S9_10, LZ_BTN }, { S3_22, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 4, 1, { T0107_2, W0109_15, T0111_4, T0113_6, T0201_29, W0203_37 }, CDF, 
            1, 0, 0, { { D9_6, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SSHJ_PSD0_26, 0xFFFF }, 2, { ZYDJ_PSD0_25, QHJ_PSD0_51 }, 6, { C_EB | SSHJ_EB0_18, C_HOLD | SSHJ_HD0_22, C_EB | ZYDJ_EB0_17, C_HOLD | ZYDJ_HD0_21, C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*24*/ { { { S10_11, LZ_BTN }, { S3_22, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP, 
            1, 5, 1, { T0108_3, W0110_16, W0109_15, T0111_4, T0113_6, T0201_29, W0203_37 }, CDF, 
            2, 0, 0, { { D10_7, REVERS }, { D9_6, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SSHJ_PSD1_28, 0xFFFF }, 2, { ZYDJ_PSD0_25, QHJ_PSD0_51 }, 6, { C_EB | SSHJ_EB1_20, C_HOLD | SSHJ_HD1_24, C_EB | ZYDJ_EB0_17, C_HOLD | ZYDJ_HD0_21, C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*25*/ { { { S14_15, LZ_BTN }, { S2_21, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0112_5, T0202_30, T0204_31 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZYDJ_PSD1_27, 0xFFFF }, 1, { QHJ_PSD1_53, 0xFFFF }, 4, { C_EB | ZYDJ_EB1_19, C_HOLD | ZYDJ_HD1_23, C_EB | QHJ_EB1_45, C_HOLD | QHJ_HD1_49 } },
};

/* Other ci devices:
S2_21
S3_22
T0201_29
T0202_30
T0204_31
W0203_37
*/
const SwSe_Data_S c_SwSeTable[]=
{
    { D3_0  , 0xFFFF, W0103_11 , 0xFFFF, NO  },
    { D4_1  , 0xFFFF, W0104_12 , 0xFFFF, NO  },
    { D5_2  , D8_5  , W0105_13 , W0106_14 , YES },
    { D6_3  , D7_4  , W0106_14 , W0105_13 , YES },
    { D7_4  , D6_3  , W0105_13 , W0106_14 , YES },
    { D8_5  , D5_2  , W0106_14 , W0105_13,  YES },
    { D9_6  , D10_7 , W0109_15 , W0110_16,  YES },
    { D10_7 , D9_6  , W0110_16 , W0109_15,  YES },
};

// 保护区段配置
const Overlap_Cfg_S c_olpCfg[] = 
{
/*00*/ { S7_8  , T0107_2 , DIRDOWN, FOURTYFIVE_S, 1, {W0105_13                     }, 2, 2, 0, {{D7_4  , NORMAL}, {D5_2  , NORMAL}, {D6_3  , NORMAL}, {D8_5  , NORMAL}}},
/*01*/ { S7_8  , T0107_2 , DIRDOWN, FOURTYFIVE_S, 2, {W0105_13,W0106_14            }, 2, 2, 0, {{D7_4  , REVERS}, {D6_3  , REVERS}, {D5_2  , NORMAL}, {D8_5  , NORMAL}}},
/*02*/ { S10_11, T0108_3 , DIRUP  , FOURTYFIVE_S, 1, {W0110_16                     }, 1, 1, 0, {{D10_7 , NORMAL}, {D9_6  , NORMAL}                                    }},
/*03*/ { S10_11, T0108_3 , DIRUP  , FOURTYFIVE_S, 1, {W0110_16,W0109_15            }, 1, 1, 0, {{D10_7 , REVERS}, {D9_6  , REVERS}                                    }},
/*04*/ { S1_20,  T0201_29, DIRDOWN, FOURTYFIVE_S, 1, {T0113_6                      }, 0, 0, 0, {{0xFF,	 NORMAL}, {0xFF  , REVERS}                                    }}
};

// 折返进路配置
const ATBData_ST c_atbCfg[6];
//{
    ///*0*/{ { 0x10,FUN_BTN },1, 1,{ { G202,  34,{ GJY_SC,0x01 } } },{ { G209,    15,{ GJY_F7,0x01 } } } },
    ///*1*/{ { 0x11,FUN_BTN },1, 1,{ { G108,  35,{ SSL_F4,0x01 } } },{ { XJ2G,    29,{ SSL_F8,0x01 } } } },
    ///*2*/{ { 0x12,FUN_BTN },1, 1,{ { G113,  36,{ SSL_F1,0x01 } } },{ { G109,    33,{ SSL_F3,0x01 } } } },
    ///*3*/{ { 0x13,FUN_BTN },1, 1,{ { G104,  37,{ SSL_F6,0x01 } } },{ { G108,    8,{ SSL_F4,0x01 } } } },
    ///*4*/{ { 0x14,FUN_BTN },1, 1,{ { G117,  38,{ SSL_F5,0x01 } } },{ { G109,    9,{ SSL_XC,0x01 } } } },
    ///*5*/{ { 0x15,FUN_BTN },1, 1,{ { G116,  39,{ SSL_F8,0x01 } } },{ { G109,    9,{ SSL_XC,0x01 } } } }

//};

const VirtualZC_ST c_vZCCfg[2]=
{
    /*0*/{T0112_ZC,1,{T0112_5},{ DIRDOWN }},
    /*1*/{T0113_ZC,1,{T0113_6},{ DIRDOWN }}
    ///*2*/{G114_ZC,2,{G114,G112},{ DIRDOWN,DIRDOWN }},
    ///*3*/{G115_ZC,2,{G115,G113},{ DIRDOWN,DIRDOWN }}
};

// 快速进路配置
const Signal_FleetInfo c_fleetCfg[8];
//{
    ///*0*/{{SSL_XC,LZ_BTN},{SSL_F11,LZ_BTN }},
    ///*1*/{{SSL_F11,LZ_BTN},{GJY_F7,LZ_BTN }},
    ///*2*/{{GJY_F7,LZ_BTN},{GJY_XC,LZ_BTN }},
    ///*3*/{{GJY_XC,LZ_BTN},{ZXZ_XC,LZ_BTN }},

    ///*4*/{{GJY_SC,LZ_BTN},{SSL_F10,LZ_BTN }},
    ///*5*/{{SSL_F10,LZ_BTN},{SSL_F6,LZ_BTN }},
    ///*6*/{{SSL_F6,LZ_BTN},{SSL_SC,LZ_BTN }},
    ///*7*/{{SSL_SC,LZ_BTN},{XEQ_SC,LZ_BTN }},
//};
