#include "StationMacroDef.h"
#include "gMacroDef.h"
#include "parameter.h"
#include "DealWithLCP.h"
#include "DealWithAutoRoute.h"
#include "DealWithATBRoute.h"
#include "DealWithACallonRoute.h"
#include "OverlapProc.h"
#include "DealWithYard.h"

const R_StData_S c_routeTableCfg[] = { 

// Cross CI rotes (c_id: 2 -> ci_id: 1): 
/*42*/ { { { S1_32, LZ_BTN }, { S15_31, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0301_54, T0211_35, QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZS_PSD0_75, 0xFFFF }, 1, { SHJ_PSD0_50, 0xFFFF }, 4, { C_EB | ZS_EB0_63, C_HOLD | ZS_HD0_69, C_EB | SHJ_EB0_42, C_HOLD | SHJ_HD0_46 } },
/*43*/ { { { S7_38, LZ_BTN }, { S6_25, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 6, 1, { T0307_59, W0305_61, W0308_62, T0306_58, T0304_57, T0302_55, T0208_33, QDZW }, CDF, 
            2, 0, 0, { { D1_13, REVERS }, { D2_14, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { HHL_PSD0_77, 0xFFFF }, 3, { SXL_PSD1_79, ZS_PSD1_78, SHJ_PSD1_52 }, 8, { C_EB | HHL_EB0_65, C_HOLD | HHL_HD0_71, C_EB | SXL_EB1_67, C_HOLD | SXL_HD1_73, C_EB | ZS_EB1_66, C_HOLD | ZS_HD1_72, C_EB | SHJ_EB1_44, C_HOLD | SHJ_HD1_48 } },
/*44*/ { { { S8_39, LZ_BTN }, { S6_25, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 5, 1, { T0310_60, W0308_62, T0306_58, T0304_57, T0302_55, T0208_33, QDZW }, CDF, 
            1, 1, 0, { { D2_14, NORMAL },{ D1_13, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { HHL_PSD1_80, 0xFFFF }, 3, { SXL_PSD1_79, ZS_PSD1_78, SHJ_PSD1_52 }, 8, { C_EB | HHL_EB1_68, C_HOLD | HHL_HD1_74, C_EB | SXL_EB1_67, C_HOLD | SXL_HD1_73, C_EB | ZS_EB1_66, C_HOLD | ZS_HD1_72, C_EB | SHJ_EB1_44, C_HOLD | SHJ_HD1_48 } },

// Local routes (ci_id: 2): 
/*45*/ { { { S2_33, LZ_BTN }, { S4_35, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0302_55, T0304_57, QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZS_PSD1_78, 0xFFFF }, 1, { SXL_PSD1_79, 0xFFFF }, 4, { C_EB | ZS_EB1_66, C_HOLD | ZS_HD1_72, C_EB | SXL_EB1_67, C_HOLD | SXL_HD1_73 } },
/*46*/ { { { S3_34, LZ_BTN }, { S1_32, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0303_56, T0301_54, QDZW }, CDF,
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SXL_PSD0_76, 0xFFFF }, 1, { ZS_PSD0_75, 0xFFFF }, 4, { C_EB | SXL_EB0_64, C_HOLD | SXL_HD0_70, C_EB | ZS_EB0_63, C_HOLD | ZS_HD0_69 } },
/*47*/ { { { S4_35, LZ_BTN }, { S6_37, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0304_57, T0306_58, W0308_62 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { SXL_PSD1_79, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | SXL_EB1_67, C_HOLD | SXL_HD1_73 } },
/*48*/ { { { S6_37, LZ_BTN }, { S10_40, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0306_58, W0308_62, T0310_60, QDZW }, CDF,
            1, 1, 0, { { D2_14, NORMAL },{ D1_13, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { HHL_PSD1_80, 0xFFFF }, 2, { C_EB | HHL_EB1_68, C_HOLD | HHL_HD1_74 } },
/*49*/ { { { S7_38, LZ_BTN }, { S3_34, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0307_59, W0305_61, T0303_56, QDZW }, CDF,
            1, 1, 0, { { D1_13, NORMAL },{ D2_14, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { HHL_PSD0_77, 0xFFFF }, 1, { SXL_PSD0_76, 0xFFFF }, 4, { C_EB | HHL_EB0_65, C_HOLD | HHL_HD0_71, C_EB | SXL_EB0_64, C_HOLD | SXL_HD0_70 } },

// Cross CI rotes (c_id: 2 -> ci_id: 3): 
/*50*/ { { { S5_36, LZ_BTN }, { S7_47, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 3, 1, { T0303_56, W0305_61, T0307_59, T0401_81, QDZW }, CDF,
            1, 1, 0, { { D1_13, NORMAL },{ D2_14, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { SXL_PSD0_76, 0xFFFF }, 2, { HHL_PSD0_77, HHBJ_PSD0_108 }, 6, { C_EB | SXL_EB0_64, C_HOLD | SXL_HD0_70, C_EB | HHL_EB0_65, C_HOLD | HHL_HD0_71, C_EB | HHBJ_EB0_96, C_HOLD | HHBJ_HD0_102 } },
/*51*/ { { { S6_37, LZ_BTN }, { S7_47, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 4, 1, { T0306_58, W0308_62, W0305_61, T0307_59, T0401_81, QDZW }, CDF,
            2, 0, 0, { { D2_14, REVERS }, { D1_13, REVERS } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 2, { HHL_PSD0_77, HHBJ_PSD0_108 }, 4, { C_EB | HHL_EB0_65, C_HOLD | HHL_HD0_71, C_EB | HHBJ_EB0_96, C_HOLD | HHBJ_HD0_102 } },
/*52*/ { { { S10_40, LZ_BTN }, { S2_42, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 1, 1, { T0310_60, T0402_82, QDZW }, CDF,
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { HHL_PSD1_80, 0xFFFF }, 1, { HHBJ_PSD1_111, 0xFFFF }, 4, { C_EB | HHL_EB1_68, C_HOLD | HHL_HD1_74, C_EB | HHBJ_EB1_99, C_HOLD | HHBJ_HD1_105 } },
};

/* Other ci devices:
#define     S15_31      
#define     S2_42       
#define     S6_25       
#define     S7_47       
#define     T0208_33        
#define     T0209_34        
#define     T0211_35        
#define     T0401_81        
#define     T0402_82        
#define     T0404_83        
#define     W0206_38        
#define     W0503_92        
*/

const SwSe_Data_S c_SwSeTable[]=
{
	{ D1_13 , D2_14 , W0305_61, W0308_62 },
	{ D2_14 , D1_13 , W0308_62, W0305_61 },
};

const Overlap_Cfg_S c_olpCfg[7]=
{       /*信号机ID 触发区段ID 方向   延时时间        逻辑区段数 逻辑区段ID        道岔数 道岔ID和位置*/
    /*01*/{S1_41,  T0401_81, DIRUP, FOURTYFIVE_S,       1,      {T0307_59},         0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*02*/{S7_38,  T0307_59, DIRUP, FOURTYFIVE_S,       1,      {W0305_61},         1,1,0,{{D1_13,NORMAL},{D2_14,NORMAL },{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*03*/{S7_38,  T0307_59, DIRUP, FOURTYFIVE_S,       2,      {W0305_61,W0308_62},2,1,0,{{D1_13,REVERS},{D2_14,REVERS },{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*04*/{S3_34,  T0303_56, DIRUP, FOURTYFIVE_S,       1,      {T0301_54},			0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*05*/{S8_24,  T0208_33,DIRDOWN,FOURTYFIVE_S,       1,      {T0302_55},			0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*06*/{S2_33,  T0302_55,DIRDOWN, FOURTYFIVE_S,      1,      {T0304_57},         0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL}}},
    /*07*/{S4_35,  T0304_57,DIRDOWN, FOURTYFIVE_S,      2,      {T0306_58},         0,0,0,{{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL},{0xFF,NORMAL}}}
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
    /*0*/{T0301_ZC,2,{T0301_54,T0303_56},{ DIRUP,DIRUP }},
    /*1*/{T0302_ZC,1,{T0302_55},{ DIRUP}},
    /*2*/{T0307_ZC,1,{T0307_59},{ DIRDOWN}},
    /*3*/{T0310_ZC,1,{T0310_60},{ DIRDOWN }}
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
