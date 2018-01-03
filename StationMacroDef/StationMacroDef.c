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

// Cross CI rotes (c_id: 3 -> ci_id: 2): 
/*53*/ { { { S1_41, LZ_BTN }, { S7_38, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0401_81, T0307_59, QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { HHBJ_PSD0_108, 0xFFFF }, 1, { HHL_PSD0_77, 0xFFFF }, 4, { C_EB | HHBJ_EB0_96, C_HOLD | HHBJ_HD0_102, C_EB | HHL_EB0_65, C_HOLD | HHL_HD0_71 } },
/*54*/ { { { S10_50, LZ_BTN }, { S8_39, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 4, 1, { T0408_86, W0406_93, T0404_83, T0402_82, T0310_60, QDZW }, CDF,
            1, 0, 0, { { D6_20, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZGJ_PSD1_109, 0xFFFF }, 2, { HHBJ_PSD1_111, HHL_PSD1_80 }, 6, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103, C_EB | HHBJ_EB1_99, C_HOLD | HHBJ_HD1_105, C_EB | HHL_EB1_68, C_HOLD | HHL_HD1_74 } },

// Local routes (ci_id: 3): 
/*55*/ { { { S2_42, LZ_BTN }, { S8_48, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP, 
            1, 1, 1, { T0402_82, T0404_83, W0406_93 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { HHBJ_PSD1_111, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | HHBJ_EB1_99, C_HOLD | HHBJ_HD1_105 } },
/*56*/ { { { S5_45, LZ_BTN }, { S12_52, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 4, 1, { T0409_87, W0411_90, W0414_91, W0406_93, T0408_86, QDZW }, CDF,
            3, 2, 0, { { D1_15, REVERS }, { D4_18, REVERS }, { D6_20, REVERS },{ D2_16, NORMAL },{ D3_17, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { ZGJ_PSD1_109, 0xFFFF }, 2, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103 } },
/*57*/ { { { S6_46, LZ_BTN }, { S12_52, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, YELLOW, NO_OLP,
            1, 3, 1, { T0412_89, W0414_91, W0406_93, T0408_86, QDZW }, CDF,
            3, 2, 0, { { D2_16, NORMAL }, { D4_18, NORMAL }, { D6_20, REVERS },{ D1_15, NORMAL },{ D3_17, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            0, { 0xFFFF, 0xFFFF }, 1, { ZGJ_PSD1_109, 0xFFFF }, 2, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103 } },
/*58*/ { { { S8_48, LZ_BTN }, { S12_52, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 2, 1, { T0404_83, W0406_93, T0408_86, QDZW }, CDF,
            1, 0, 0, { { D6_20, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            0, { 0xFFFF, 0xFFFF }, 1, { ZGJ_PSD1_109, 0xFFFF }, 2, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103 } },
/*59*/ { { { S9_49, LZ_BTN }, { S1_41, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, NO_OLP, 
            1, 2, 1, { T0405_84, W0503_92, T0401_81, QDZW }, CDF,
            1, 0, 0, { { D5_19, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZGJ_PSD0_106, 0xFFFF }, 1, { HHBJ_PSD0_108, 0xFFFF }, 4, { C_EB | ZGJ_EB0_94, C_HOLD | ZGJ_HD0_100, C_EB | HHBJ_EB0_96, C_HOLD | HHBJ_HD0_102 } },
/*60*/ { { { S9_49, LZ_BTN }, { S3_43, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0405_84, W0503_92, W0411_90, T0409_87,QDZW }, CDF, 
            3, 2, 0, { { D5_19, REVERS }, { D3_17, NORMAL }, { D1_15, NORMAL },{ D2_16, NORMAL },{ D4_18, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { ZGJ_PSD0_106, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | ZGJ_EB0_94, C_HOLD | ZGJ_HD0_100 } },
/*61*/ { { { S9_49, LZ_BTN }, { S4_44, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0405_84, W0503_92, W0411_90, W0414_91, T0412_89,QDZW }, CDF, 
            3, 2, 0, { { D5_19, REVERS }, { D3_17, REVERS }, { D2_16, REVERS },{ D1_15, NORMAL },{ D4_18, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { ZGJ_PSD0_106, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | ZGJ_EB0_94, C_HOLD | ZGJ_HD0_100 } },
/*62*/ { { { S10_50, LZ_BTN }, { S3_43, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 4, 1, { T0408_86, W0406_93, W0414_91, W0411_90, T0409_87,QDZW }, CDF, 
            3, 2, 0, { { D6_20, REVERS }, { D4_18, REVERS }, { D1_15, REVERS },{ D2_16, NORMAL },{ D3_17, NORMAL } }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { ZGJ_PSD1_109, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103 } },
/*63*/ { { { S10_50, LZ_BTN }, { S4_44, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, YELLOW, NO_OLP, 
            1, 3, 1, { T0408_86, W0406_93, W0414_91, T0412_89,QDZW }, CDF, 
            3, 2, 0, { { D6_20, REVERS }, { D4_18, NORMAL }, { D2_16, NORMAL },{ D3_17, NORMAL },{ D1_15, NORMAL }, }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } },
            1, { ZGJ_PSD1_109, 0xFFFF }, 0, { 0xFFFF, 0xFFFF }, 2, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103 } },
/*64*/ { { { S11_51, LZ_BTN }, { S9_49, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRDOWN, LZ, NO }, GREEN, S9_49, 
            1, 1, 1, { T0407_85, T0405_84, W0503_92 }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { QGJ_PSD0_107, 0xFFFF }, 1, { ZGJ_PSD0_106, 0xFFFF }, 4, { C_EB | QGJ_EB0_95, C_HOLD | QGJ_HD0_101, C_EB | ZGJ_EB0_94, C_HOLD | ZGJ_HD0_100 } },
/*65*/ { { { S12_52, LZ_BTN }, { S14_53, LZ_BTN }, { 0xFF, 0 } }, 0xFFFF, { DIRUP, LZ, NO }, GREEN, NO_OLP,
            1, 1, 1, { T0408_86, T0410_88,QDZW }, CDF, 
            0, 0, 0, { 0xFFFF }, 0, { { 0xFFFF, 0, { 0xFFFF, 0xFF } } }, 
            1, { ZGJ_PSD1_109, 0xFFFF }, 1, { QGJ_PSD1_110, 0xFFFF }, 4, { C_EB | ZGJ_EB1_97, C_HOLD | ZGJ_HD1_103, C_EB | QGJ_EB1_98, C_HOLD | QGJ_HD1_104 } },
};

/* Other ci devices:
#define     S7_38       
#define     S8_39       
#define     T0307_59        
#define     T0310_60        
#define     W0305_61        
#define     W0308_62        
*/

const SwSe_Data_S c_SwSeTable[]=
{
    { D1_15 , D4_18 , W0411_90, W0414_91 },
    { D2_16 , D3_17 , W0414_91, W0411_90 },
    { D3_17 , D2_16 , W0411_90, W0414_91 },
    { D4_18 , D1_15 , W0414_91, W0411_90 },
    { D5_19 , 0xFFFF, W0503_92, 0xFFFF },
    { D6_20 , 0xFFFF, W0406_93, 0xFFFF },
};

const Overlap_Cfg_S c_olpCfg[7] = 
{       /*信号机ID 触发区段ID 方向   延时时间        逻辑区段数 逻辑区段ID						道岔数 道岔ID和位置*/
    /*01*/{S9_49,   T0405_84,DIRDOWN, FOURTYFIVE_S,       1,      {W0503_92},						1,0,0,{{D5_19,NORMAL},{0xFF,NORMAL},{ 0xFF,NORMAL},{ 0xFF,NORMAL} }},
    /*02*/{S9_49,   T0405_84,DIRDOWN, FOURTYFIVE_S,       2,      {W0503_92,W0411_90},			    3,2,0,{{D5_19,REVERS},{D3_17,NORMAL },{D1_15,NORMAL},{D2_16,NORMAL},{ D4_18,NORMAL } }},
    /*03*/{S9_49,   T0405_84,DIRDOWN, FOURTYFIVE_S,       3,      {W0503_92,W0411_90,W0414_91},     3,2,0,{{D5_19,REVERS},{D3_17,REVERS },{D2_16,REVERS},{D1_15,NORMAL},{ D4_18,NORMAL } }},
    /*04*/{S11_51,  T0407_85,DIRDOWN, FOURTYFIVE_S,       1,      {T0405_84},						0,0,0,{{0xFF,REVERS},{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL } }},
    /*05*/{S2_42,   T0402_82,DIRUP,	  FOURTYFIVE_S,       1,      {T0404_83},						0,0,0,{{0xFF,REVERS},{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL } }},
    /*06*/{S12_52,  T0408_86,DIRUP,   FOURTYFIVE_S,       1,      {T0410_88},						0,0,0,{{0xFF,REVERS},{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL } }},
    /*07*/{S10_41,  T0310_60,DIRUP,   FOURTYFIVE_S,       1,      {T0402_82},						0,0,0,{{0xFF,REVERS},{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL },{ 0xFF,NORMAL } }}
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

const VirtualZC_ST c_vZCCfg[] = 
{
    /*0*/{T0401_ZC,1,{T0401_81},{ DIRUP }},
    /*1*/{T0402_ZC,1,{T0402_ZC},{ DIRUP }},
    ///*2*/{G114_ZC,2,{G114,G112},{ DIRDOWN,DIRDOWN }},
    ///*3*/{G115_ZC,2,{G115,G113},{ DIRDOWN,DIRDOWN }}
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
