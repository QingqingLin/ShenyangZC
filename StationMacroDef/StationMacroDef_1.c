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

// Cross CI rotes (c_id: 1 -> ci_id: 0): 
/*26*/ { { { S1_20, LZ_BTN }, { S13_14, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0201_29, T0113_6, QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { QHJ_PSD0_51, 0xFFFF }, 1, { ZYDJ_PSD0_25, 0xFFFF }, 5, { C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47, C_EB | ZYDJ_EB0_17, C_HOLD | ZYDJ_HD0_21, C_VZC | T0113_ZC } },
/*27*/ { { { S6_25, LZ_BTN }, { S12_13, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 4, 1, { T0208_33, W0206_38, T0204_31, T0202_30, T0112_5, W0110_16 }, CDF, 
            1, 0, 0, { { D2_9, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SHJ_PSD1_52, 0xFFFF }, 2, { QHJ_PSD1_53, ZYDJ_PSD1_27 }, 7, { C_EB | SHJ_EB1_44, C_HOLD | SHJ_HD1_48, C_EB | QHJ_EB1_45, C_HOLD | QHJ_HD1_49, C_EB | ZYDJ_EB1_19, C_HOLD | ZYDJ_HD1_23, C_VZC | T0112_ZC } },
/*28*/ { { { S7_26, LZ_BTN }, { S12_13, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 5, 1, { T0215_36, W0213_39, W0206_38, T0204_31, T0202_30, T0112_5, W0110_16 }, CDF, 
            2, 1, 0, { { D3_10, NORMAL }, { D2_9, REVERS },{ D1_8, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 2, { QHJ_PSD1_53, ZYDJ_PSD1_27 }, 5, { C_EB | QHJ_EB1_45, C_HOLD | QHJ_HD1_49, C_EB | ZYDJ_EB1_19, C_HOLD | ZYDJ_HD1_23, C_VZC | T0112_ZC } },

// Local routes (ci_id: 1): 
/*29*/ { { { S2_21, LZ_BTN }, { S4_23, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0202_30, T0204_31, W0206_38 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { QHJ_PSD1_53, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | QHJ_EB1_45, C_HOLD | QHJ_HD1_49 } },
/*30*/ { { { S3_22, LZ_BTN }, { S9_28, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 3, 1, { T0201_29, W0203_37, W0213_39, T0215_36, W0217_40 }, CDF, 
            2, 0, 0, { { D1_8, REVERS }, { D3_10, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { QHJ_PSD0_51, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*31*/ { { { S3_22, LZ_BTN }, { S11_29, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 2, 1, { T0201_29, W0203_37, T0205_32, W0207_41 }, CDF, 
            2, 0, 0, { { D1_8, NORMAL },{ D3_10, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { QHJ_PSD0_51, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*32*/ { { { S4_23, LZ_BTN }, { S8_27, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0204_31, W0206_38, T0208_33, QDZW }, CDF, 
            1, 0, 0, { { D2_9, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 1, { SHJ_PSD1_52, 0xFFFF }, 2, { C_EB | SHJ_EB1_44, C_HOLD | SHJ_HD1_48 } },
/*33*/ { { { S4_23, LZ_BTN }, { S9_28, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0204_31, W0206_38, W0213_39, T0215_36, W0217_40 }, CDF, 
            2, 1, 0, { { D2_9, REVERS }, { D3_10, NORMAL },{ D1_8, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*34*/ { { { S5_24, LZ_BTN }, { S1_20, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0205_32, W0203_37, T0201_29, QDZW }, CDF, 
            1, 1, 0, { { D1_8, NORMAL },{ D3_10, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { QHJ_PSD0_51, 0xFFFF }, 2, { C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*35*/ { { { S7_26, LZ_BTN }, { S1_20, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0215_36, W0213_39, W0203_37, T0201_29, QDZW }, CDF, 
            2, 0, 0, { { D3_10, REVERS }, { D1_8, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 1, { QHJ_PSD0_51, 0xFFFF }, 2, { C_EB | QHJ_EB0_43, C_HOLD | QHJ_HD0_47 } },
/*36*/ { { { S13_30, LZ_BTN }, { S5_24, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0209_34, W0207_41, T0205_32, W0203_37 }, CDF, 
            1, 1, 0, { { D7_12, NORMAL },{ D5_11, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*37*/ { { { S13_30, LZ_BTN }, { S7_26, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0209_34, W0207_41, W0217_40, T0215_36, W0213_39 }, CDF, 
            2, 0, 0, { { D7_12, REVERS }, { D5_11, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 0, { 0xFFFF, 0xFFFF } },
/*38*/ { { { S15_31, LZ_BTN }, { S13_30, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0211_35, T0209_34, W0207_41 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SHJ_PSD0_50, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SHJ_EB0_42, C_HOLD | SHJ_HD0_46 } },

// Cross CI rotes (c_id: 1 -> ci_id: 2): 
/*39*/ { { { S8_27, LZ_BTN }, { S2_33, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0208_33, T0302_55, T0304_57 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SHJ_PSD1_52, 0xFFFF }, 1, { ZS_PSD1_78, 0xFFFF }, 4, { C_EB | SHJ_EB1_44, C_HOLD | SHJ_HD1_48, C_EB | ZS_EB1_66, C_HOLD | ZS_HD1_72 } },
/*40*/ { { { S9_28, LZ_BTN }, { S5_36, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 6, 1, { T0215_36, W0217_40, W0207_41, T0209_34, T0211_35, T0301_54, T0303_56, W0305_61 }, CDF, 
            2, 0, 0, { { D5_11, REVERS }, { D7_12, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 3, { SHJ_PSD0_50, ZS_PSD0_75, SXL_PSD0_76 }, 6, { C_EB | SHJ_EB0_42, C_HOLD | SHJ_HD0_46, C_EB | ZS_EB0_63, C_HOLD | ZS_HD0_69, C_EB | SXL_EB0_64, C_HOLD | SXL_HD0_70 } },
/*41*/ { { { S11_29, LZ_BTN }, { S5_36, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 5, 1, { T0205_32, W0207_41, T0209_34, T0211_35, T0301_54, T0303_56, W0305_61 }, CDF, 
            1, 0, 0, { { D7_12, NORMAL },{ D5_11, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 3, { SHJ_PSD0_50, ZS_PSD0_75, SXL_PSD0_76 }, 6, { C_EB | SHJ_EB0_42, C_HOLD | SHJ_HD0_46, C_EB | ZS_EB0_63, C_HOLD | ZS_HD0_69, C_EB | SXL_EB0_64, C_HOLD | SXL_HD0_70 } },
};

/* Other ci devices:
#define     S12_13      
#define     S13_14      
#define     S2_33       
#define     S5_36       
#define     T0111_4     
#define     T0112_5     
#define     T0113_6     
#define     T0301_54        
#define     T0302_55        
#define     T0303_56        
#define     T0304_57        
#define     W0110_16        
#define     W0305_61        
*/

const SwSe_Data_S c_SwSeTable[]=
{
    { D1_8  , D3_10 , W0203_37, W0213_39 },
    { D2_9  , 0xFFFF, W0206_38, 0xFFFF },
    { D3_10 , D1_8  , W0213_39, W0203_37 },
    { D5_11 , D7_12 , W0217_40, W0207_41 },
    { D7_12 , D5_11 , W0207_41, W0217_40 },
};

const Overlap_Cfg_S c_olpCfg[] = 
{       /*信号机ID 触发区段ID 方向     延时时间        逻辑区段数     逻辑区段ID        道岔数 道岔ID和位置*/
    /*01*/{S14_15,   T0112_5, DIRUP,  FOURTYFIVE_S,       1,      {T0202_30},       0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{ 0xFF,NORMAL},{ 0xFF,NORMAL} }},
    /*02*/{S2_21,    T0202_30,DIRUP,  FOURTYFIVE_S,       1,      {T0204_31},       0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{ 0xFF,NORMAL},{ 0xFF,NORMAL} }},
    /*03*/{S15_31,   T0211_35,DIRDOWN,FOURTYFIVE_S,       1,      {T0209_34},       0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{ 0xFF,NORMAL},{ 0xFF,NORMAL} }},
    /*04*/{S1_32,    T0301_54,DIRDOWN,FOURTYFIVE_S,       1,      {T0211_35},	    0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{ 0xFF,NORMAL},{ 0xFF,NORMAL} }}
};

const ATBData_ST c_atbCfg[6];
//{
    ///*0*/{ { 0x10,FUN_BTN },1, 1,{ { G202,  34,{ GJY_SC,0x01 } } },{ { G209,    15,{ GJY_F7,0x01 } } } },
    ///*1*/{ { 0x11,FUN_BTN },1, 1,{ { G108,  35,{ SSL_F4,0x01 } } },{ { XJ2G,    29,{ SSL_F8,0x01 } } } },
    ///*2*/{ { 0x12,FUN_BTN },1, 1,{ { G113,  36,{ SSL_F1,0x01 } } },{ { G109,    33,{ SSL_F3,0x01 } } } },
    ///*3*/{ { 0x13,FUN_BTN },1, 1,{ { G104,  37,{ SSL_F6,0x01 } } },{ { G108,    8,{ SSL_F4,0x01 } } } },
    ///*4*/{ { 0x14,FUN_BTN },1, 1,{ { G117,  38,{ SSL_F5,0x01 } } },{ { G109,    9,{ SSL_XC,0x01 } } } },
    ///*5*/{ { 0x15,FUN_BTN },1, 1,{ { G116,  39,{ SSL_F8,0x01 } } },{ { G109,    9,{ SSL_XC,0x01 } } } }

//};

const VirtualZC_ST c_vZCCfg[4] = 
{
    /*0*/{T0201_ZC,1,{T0201_29},{ DIRUP }},
    /*1*/{T0202_ZC,1,{T0202_30},{ DIRUP }},
    /*2*/{T0211_ZC,1,{T0211_35},{ DIRDOWN }},
    /*3*/{T0208_ZC,1,{T0208_33},{ DIRDOWN }}
};

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
