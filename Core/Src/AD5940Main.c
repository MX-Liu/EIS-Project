// /*!
//  *****************************************************************************
//  @file:    AD5940Main.c
//  @author:  Neo Xu
//  @brief:   Standard 4-wire or 2-wire impedance measurement example.
//  -----------------------------------------------------------------------------

// Copyright (c) 2017-2019 Analog Devices, Inc. All Rights Reserved.

// This software is proprietary to Analog Devices, Inc. and its licensors.
// By using this software you agree to the terms of the associated
// Analog Devices Software License Agreement.
 
// *****************************************************************************/
// #include "Impedance.h"

// /**
//    User could configure following parameters
// **/

// #define APPBUFF_SIZE 512
// uint32_t AppBuff[APPBUFF_SIZE];

// int32_t ImpedanceShowResult(uint32_t *pData, uint32_t DataCount)
// {
//   float freq;
	
// 	float phase;

//   fImpPol_Type *pImp = (fImpPol_Type*)pData;
//   AppIMPCtrl(IMPCTRL_GETFREQ, &freq);

 
  
// 	//printf("%.2f,", freq);
	
//   /*Process data*/
//   for(int i=0;i<DataCount;i++)
//   {
// 		if(pImp[i].Phase*180/MATH_PI > 180)
// 		{
// 			phase = pImp[i].Phase*180/MATH_PI - 360;
// 		}
// 		else
// 		{
// 			phase = pImp[i].Phase*180/MATH_PI;
// 		}
//     //printf("RzMag: %f Ohm , RzPhase: %f \n",pImp[i].Magnitude,pImp[i].Phase*180/MATH_PI);
//   }
//   return 0;
// }

// static int32_t AD5940PlatformCfg(void)
// {
//   CLKCfg_Type clk_cfg;
//   FIFOCfg_Type fifo_cfg;
//   AGPIOCfg_Type gpio_cfg;

//   /* Use hardware reset */
//   AD5940_HWReset();
//   AD5940_Initialize();
//   /* Platform configuration */
//   /* Step1. Configure clock */
//   clk_cfg.ADCClkDiv = ADCCLKDIV_1;
//   clk_cfg.ADCCLkSrc = ADCCLKSRC_HFOSC;
//   clk_cfg.SysClkDiv = SYSCLKDIV_1;
//   clk_cfg.SysClkSrc = SYSCLKSRC_HFOSC;
//   clk_cfg.HfOSC32MHzMode = bFALSE;
//   clk_cfg.HFOSCEn = bTRUE;
//   clk_cfg.HFXTALEn = bFALSE;
//   clk_cfg.LFOSCEn = bTRUE;
//   AD5940_CLKCfg(&clk_cfg);
//   /* Step2. Configure FIFO and Sequencer*/
//   fifo_cfg.FIFOEn = bFALSE;
//   fifo_cfg.FIFOMode = FIFOMODE_FIFO;
//   fifo_cfg.FIFOSize = FIFOSIZE_4KB;                       /* 4kB for FIFO, The reset 2kB for sequencer */
//   fifo_cfg.FIFOSrc = FIFOSRC_DFT;
//   fifo_cfg.FIFOThresh = 4;//AppIMPCfg.FifoThresh;        /* DFT result. One pair for RCAL, another for Rz. One DFT result have real part and imaginary part */
//   AD5940_FIFOCfg(&fifo_cfg);
//   fifo_cfg.FIFOEn = bTRUE;
//   AD5940_FIFOCfg(&fifo_cfg);
  
//   /* Step3. Interrupt controller */
//   AD5940_INTCCfg(AFEINTC_1, AFEINTSRC_ALLINT, bTRUE);   /* Enable all interrupt in INTC1, so we can check INTC flags */
//   AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
//   AD5940_INTCCfg(AFEINTC_0, AFEINTSRC_DATAFIFOTHRESH, bTRUE); 
//   AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
//   /* Step4: Reconfigure GPIO */
//   gpio_cfg.FuncSet = GP0_INT|GP1_SLEEP|GP2_SYNC;
//   gpio_cfg.InputEnSet = 0;
//   gpio_cfg.OutputEnSet = AGPIO_Pin0|AGPIO_Pin1|AGPIO_Pin2;
//   gpio_cfg.OutVal = 0;
//   gpio_cfg.PullEnSet = 0;
//   AD5940_AGPIOCfg(&gpio_cfg);
//   AD5940_SleepKeyCtrlS(SLPKEY_UNLOCK);  /* Allow AFE to enter sleep mode. */
//   return 0;
// }

// void AD5940ImpedanceStructInit(void)
// {
//   AppIMPCfg_Type *pImpedanceCfg;
  
//   AppIMPGetCfg(&pImpedanceCfg);
//   /* Step1: configure initialization sequence Info */
//   pImpedanceCfg->SeqStartAddr = 0;
//   pImpedanceCfg->MaxSeqLen = 512; /* @todo add checker in function */

//   pImpedanceCfg->RcalVal = 10000.0;
//   pImpedanceCfg->SinFreq = 60000.0;
//   pImpedanceCfg->FifoThresh = 4;
	
// 	/* Set switch matrix to onboard(EVAL-AD5940ELECZ) dummy sensor. */
// 	/* Note the RCAL0 resistor is 10kOhm. */
// 	pImpedanceCfg->DswitchSel = SWD_CE0;
// 	//pImpedanceCfg->PswitchSel = SWP_RE0;
	
// 	pImpedanceCfg->PswitchSel = SWP_CE0;
	
// 	//pImpedanceCfg->NswitchSel = SWN_SE0;
	
// 	pImpedanceCfg->NswitchSel = SWN_AIN1;
// 	//pImpedanceCfg->TswitchSel = SWT_SE0LOAD;
	
// 	pImpedanceCfg->TswitchSel = SWT_AIN1;
	
// 	/* The dummy sensor is as low as 5kOhm. We need to make sure RTIA is small enough that HSTIA won't be saturated. */
// 	//pImpedanceCfg->HstiaRtiaSel = HSTIARTIA_5K;	
// 	pImpedanceCfg->HstiaRtiaSel = HSTIARTIA_200;
	
// 	/* Configure the sweep function. */
// 	pImpedanceCfg->SweepCfg.SweepEn = bTRUE;
// 	pImpedanceCfg->SweepCfg.SweepStart = 100;	/* Start from 1kHz */
// 	pImpedanceCfg->SweepCfg.SweepStop = 200e3f;		/* Stop at 100kHz */
// 	pImpedanceCfg->SweepCfg.SweepPoints = 201;		/* Points is 101 */
// 	//pImpedanceCfg->SweepCfg.SweepLog = bTRUE;
// 	pImpedanceCfg->SweepCfg.SweepLog = bTRUE;
// 	/* Configure Power Mode. Use HP mode if frequency is higher than 80kHz. */
// 	pImpedanceCfg->PwrMod = AFEPWR_HP;
	
// 	/* Configure filters if necessary */
// 	pImpedanceCfg->ADCSinc3Osr = ADCSINC3OSR_2;		/* Sample rate is 800kSPS/2 = 400kSPS */
//   pImpedanceCfg->DftNum = DFTNUM_16384;
//   pImpedanceCfg->DftSrc = DFTSRC_SINC3;
// }

// void AD5940_Main(void)
// {
//   uint32_t temp;  
//   AD5940PlatformCfg();
//   AD5940ImpedanceStructInit();
  
//   AppIMPInit(AppBuff, APPBUFF_SIZE);    /* Initialize IMP application. Provide a buffer, which is used to store sequencer commands */
//   AppIMPCtrl(IMPCTRL_START, 0);          /* Control IMP measurement to start. Second parameter has no meaning with this command. */
 
//   while(1)
//   {
//     if(AD5940_GetMCUIntFlag())
//     {
//       AD5940_ClrMCUIntFlag();
//       temp = APPBUFF_SIZE;
//       AppIMPISR(AppBuff, &temp);
//       ImpedanceShowResult(AppBuff, temp);
//     }
//   }
// }

/*!
 *****************************************************************************
 @file:    AD5940Main.c
 @author:  Neo Xu
 @brief:   Standard 4-wire or 2-wire impedance measurement example.
 -----------------------------------------------------------------------------

Copyright (c) 2017-2019 Analog Devices, Inc. All Rights Reserved.

This software is proprietary to Analog Devices, Inc. and its licensors.
By using this software you agree to the terms of the associated
Analog Devices Software License Agreement.
 
*****************************************************************************/
#include "Impedance.h"
#include "Amperometric.h"
#include "voltage.h"
//#include "ADuCM3029.h"

/**
   User could configure following parameters
**/

#define APPBUFF_SIZE 512
uint32_t AppBuff[APPBUFF_SIZE];

float LFOSCFreq;
uint32_t old_y = 0;
int32_t vol_sum=0;
int16_t vol_ave = 0;

int32_t VoltageShowResult(uint32_t *pData, uint32_t DataCount)
{
	float freq;
	AppVOLCtrl(VOLCTRL_GETFREQ, &freq);
	char buf[5];
	for(int i=0;i<DataCount;i++)
  {
		printf("%d\n", pData[i]);
  }
  return 0;
}
int32_t AMPShowResult(float *pData, uint32_t DataCount)
{
  /* Print data*/
  for(int i=0;i<DataCount;i++)
  {
		printf("%.1f\n", pData[i]*1000); //nA
  }
  return 0;
}


int32_t ImpedanceShowResult(uint32_t *pData, uint32_t DataCount)
{
  float freq;	
	float phase;

  fImpPol_Type *pImp = (fImpPol_Type*)pData;
  AppIMPCtrl(IMPCTRL_GETFREQ, &freq);

  uint32_t intfreq = freq;
	printf("%d,", intfreq);
	
  /*Process data*/
  for(int i=0;i<DataCount;i++)
  {
		if(pImp[i].Phase*180/MATH_PI > 180)
		{
			phase = pImp[i].Phase*180/MATH_PI - 360;
		}
		else
		{
			phase = pImp[i].Phase*180/MATH_PI;
		}
		int32_t intpase = phase;
    uint32_t intmag = pImp[i].Magnitude;
		printf("%d, %d \n",intmag,intpase);
  }
  return 0;
}

/* Initialize AD5940 basic blocks like clock for amperometric */
static int32_t AD5940PlatformCfg_AMP(void)
{
  CLKCfg_Type clk_cfg;
  FIFOCfg_Type fifo_cfg;
	SEQCfg_Type seq_cfg;
  AGPIOCfg_Type gpio_cfg;
	LFOSCMeasure_Type LfoscMeasure;
  /* Use hardware reset */
  AD5940_HWReset();
  /* Platform configuration */
  AD5940_Initialize();
  /* Step1. Configure clock */
  clk_cfg.HFOSCEn = bTRUE;
  clk_cfg.HFXTALEn = bFALSE;
  clk_cfg.LFOSCEn = bTRUE;
  clk_cfg.HfOSC32MHzMode = bFALSE;
  clk_cfg.SysClkSrc = SYSCLKSRC_HFOSC;
  clk_cfg.SysClkDiv = SYSCLKDIV_1;
  clk_cfg.ADCCLkSrc = ADCCLKSRC_HFOSC;
  clk_cfg.ADCClkDiv = ADCCLKDIV_1;
  AD5940_CLKCfg(&clk_cfg);
  /* Step2. Configure FIFO and Sequencer*/
  fifo_cfg.FIFOEn = bFALSE;
  fifo_cfg.FIFOMode = FIFOMODE_FIFO;
  fifo_cfg.FIFOSize = FIFOSIZE_4KB;                      /* 4kB for FIFO, The reset 2kB for sequencer */
  fifo_cfg.FIFOSrc = FIFOSRC_DFT;
  fifo_cfg.FIFOThresh = 4;      
  AD5940_FIFOCfg(&fifo_cfg);                             /* Disable to reset FIFO. */
	fifo_cfg.FIFOEn = bTRUE;  
  AD5940_FIFOCfg(&fifo_cfg);                             /* Enable FIFO here */
  /* Configure sequencer and stop it */
  seq_cfg.SeqMemSize = SEQMEMSIZE_2KB;
  seq_cfg.SeqBreakEn = bFALSE;
  seq_cfg.SeqIgnoreEn = bFALSE;
  seq_cfg.SeqCntCRCClr = bTRUE;
  seq_cfg.SeqEnable = bFALSE;
  seq_cfg.SeqWrTimer = 0;
  AD5940_SEQCfg(&seq_cfg);
	
  /* Step3. Interrupt controller */
  AD5940_INTCCfg(AFEINTC_1, AFEINTSRC_ALLINT, bTRUE);           /* Enable all interrupt in Interrupt Controller 1, so we can check INTC flags */
  AD5940_INTCCfg(AFEINTC_0, AFEINTSRC_DATAFIFOTHRESH, bTRUE);   /* Interrupt Controller 0 will control GP0 to generate interrupt to MCU */
  AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
  /* Step4: Reconfigure GPIO */
  gpio_cfg.FuncSet = GP6_SYNC|GP5_SYNC|GP4_SYNC|GP2_SYNC|GP1_SLEEP|GP0_INT;
  gpio_cfg.InputEnSet = 0;
  gpio_cfg.OutputEnSet = AGPIO_Pin0|AGPIO_Pin1|AGPIO_Pin4|AGPIO_Pin5|AGPIO_Pin6|AGPIO_Pin2;
  gpio_cfg.OutVal = 0;
  gpio_cfg.PullEnSet = 0;
  AD5940_AGPIOCfg(&gpio_cfg);
	
  AD5940_SleepKeyCtrlS(SLPKEY_UNLOCK);  /* Allow AFE to enter sleep mode. */
  /* Measure LFOSC frequency */
  LfoscMeasure.CalDuration = 1000.0;  /* 1000ms used for calibration. */
  LfoscMeasure.CalSeqAddr = 0;
  LfoscMeasure.SystemClkFreq = 16000000.0f; /* 16MHz in this firmware. */
  AD5940_LFOSCMeasure(&LfoscMeasure, &LFOSCFreq);
  printf("Freq:%f\n", LFOSCFreq); 
  return 0;
}



/* Initialize AD5940 basic blocks like clock for impedance measuring */
static int32_t AD5940PlatformCfg_IMP(void)
{
  CLKCfg_Type clk_cfg;
  FIFOCfg_Type fifo_cfg;
  AGPIOCfg_Type gpio_cfg;

  /* Use hardware reset */
  AD5940_HWReset();
  /* Platform configuration */
	AD5940_Initialize();
	
  /* Step1. Configure clock */
  clk_cfg.ADCClkDiv = ADCCLKDIV_1;
  clk_cfg.ADCCLkSrc = ADCCLKSRC_HFOSC;
  clk_cfg.SysClkDiv = SYSCLKDIV_1;
  clk_cfg.SysClkSrc = SYSCLKSRC_HFOSC;
  clk_cfg.HfOSC32MHzMode = bFALSE;
  clk_cfg.HFOSCEn = bTRUE;
  clk_cfg.HFXTALEn = bFALSE;
  clk_cfg.LFOSCEn = bTRUE;
  AD5940_CLKCfg(&clk_cfg);
  /* Step2. Configure FIFO and Sequencer*/
  fifo_cfg.FIFOEn = bFALSE;
  fifo_cfg.FIFOMode = FIFOMODE_FIFO;
  fifo_cfg.FIFOSize = FIFOSIZE_4KB;                       /* 4kB for FIFO, The reset 2kB for sequencer */
  fifo_cfg.FIFOSrc = FIFOSRC_DFT;
  fifo_cfg.FIFOThresh = 4;//AppIMPCfg.FifoThresh;        /* DFT result. One pair for RCAL, another for Rz. One DFT result have real part and imaginary part */
  AD5940_FIFOCfg(&fifo_cfg);
  fifo_cfg.FIFOEn = bTRUE;
  AD5940_FIFOCfg(&fifo_cfg);
  
  /* Step3. Interrupt controller */
  AD5940_INTCCfg(AFEINTC_1, AFEINTSRC_ALLINT, bTRUE);   /* Enable all interrupt in INTC1, so we can check INTC flags */
  AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
	
  AD5940_INTCCfg(AFEINTC_0, AFEINTSRC_DATAFIFOTHRESH, bTRUE); 
  AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
	
  /* Step4: Reconfigure GPIO */
  gpio_cfg.FuncSet = GP0_INT|GP1_SLEEP|GP2_SYNC;
  gpio_cfg.InputEnSet = 0;
  gpio_cfg.OutputEnSet = AGPIO_Pin0|AGPIO_Pin1|AGPIO_Pin2;
  gpio_cfg.OutVal = 0;
  gpio_cfg.PullEnSet = 0;
  AD5940_AGPIOCfg(&gpio_cfg);
  AD5940_SleepKeyCtrlS(SLPKEY_UNLOCK);  /* Allow AFE to enter sleep mode. */
  return 0;
}


/* Initialize AD5940 basic blocks like clock for volatge measuring */
static int32_t AD5940PlatformCfg_VOL(void)
{
  CLKCfg_Type clk_cfg;
  FIFOCfg_Type fifo_cfg;
  AGPIOCfg_Type gpio_cfg;

  /* Use hardware reset */
  AD5940_HWReset();
  /* Platform configuration */
	AD5940_Initialize();
	
  /* Step1. Configure clock */
  clk_cfg.ADCClkDiv = ADCCLKDIV_1;
  clk_cfg.ADCCLkSrc = ADCCLKSRC_HFOSC;
  clk_cfg.SysClkDiv = SYSCLKDIV_1;
  clk_cfg.SysClkSrc = SYSCLKSRC_HFOSC;
  clk_cfg.HfOSC32MHzMode = bFALSE;
  clk_cfg.HFOSCEn = bTRUE;
  clk_cfg.HFXTALEn = bFALSE;
  clk_cfg.LFOSCEn = bTRUE;
  AD5940_CLKCfg(&clk_cfg);
  /* Step2. Configure FIFO and Sequencer*/
  fifo_cfg.FIFOEn = bFALSE;
  fifo_cfg.FIFOMode = FIFOMODE_FIFO;
  fifo_cfg.FIFOSize = FIFOSIZE_4KB;                       /* 4kB for FIFO, The reset 2kB for sequencer */
  fifo_cfg.FIFOSrc = FIFOSRC_DFT;
  fifo_cfg.FIFOThresh = 4;//AppIMPCfg.FifoThresh;        /* DFT result. One pair for RCAL, another for Rz. One DFT result have real part and imaginary part */
  AD5940_FIFOCfg(&fifo_cfg);
  fifo_cfg.FIFOEn = bTRUE;
  AD5940_FIFOCfg(&fifo_cfg);
  
  /* Step3. Interrupt controller */
  AD5940_INTCCfg(AFEINTC_1, AFEINTSRC_ALLINT, bTRUE);   /* Enable all interrupt in INTC1, so we can check INTC flags */
  AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
	
  AD5940_INTCCfg(AFEINTC_0, AFEINTSRC_DATAFIFOTHRESH, bTRUE); 
  AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
	
  /* Step4: Reconfigure GPIO */
  gpio_cfg.FuncSet = GP0_INT|GP1_SLEEP|GP2_SYNC;
  gpio_cfg.InputEnSet = 0;
  gpio_cfg.OutputEnSet = AGPIO_Pin0|AGPIO_Pin1|AGPIO_Pin2;
  gpio_cfg.OutVal = 0;
  gpio_cfg.PullEnSet = 0;
  AD5940_AGPIOCfg(&gpio_cfg);
  AD5940_SleepKeyCtrlS(SLPKEY_UNLOCK);  /* Allow AFE to enter sleep mode. */
  return 0;
}



/* !!Change the application parameters here if you want to change it to none-default value */
void AD5940AMPStructInit(void)
{
  AppAMPCfg_Type *pAMPCfg;
  
  AppAMPGetCfg(&pAMPCfg);
	pAMPCfg->WuptClkFreq = LFOSCFreq;
  /* Configure general parameters */
  pAMPCfg->SeqStartAddr = 0;
  pAMPCfg->MaxSeqLen = 512;     /* @todo add checker in function */  
  pAMPCfg->RcalVal = 10000.0;
  pAMPCfg->NumOfData = -1;      /* Never stop until you stop it manually by AppAMPCtrl() function */	
	
	
	/* Configure measurement parameters */
  pAMPCfg->AmpODR = 0.01;          	/* Time between samples in seconds */
  pAMPCfg->FifoThresh = 4;      		/* Number of measurements before alerting host microcontroller */
	
  pAMPCfg->SensorBias = 1000;   			/* Sensor bias voltage between reference and sense electrodes*/
	pAMPCfg->LptiaRtiaSel = LPTIARTIA_512K;
	pAMPCfg->LpTiaRl = LPTIARLOAD_10R;
	
	pAMPCfg->Vzero = 1000;        		/* Vzero voltage. Voltage on Sense electrode. Unit is mV*/
	pAMPCfg->ADCRefVolt = 1.82;		/* Measure voltage on Vref_1V8 pin */
	
}

void AD5940ImpedanceStructInit(void)
{
  AppIMPCfg_Type *pImpedanceCfg;
  
  AppIMPGetCfg(&pImpedanceCfg);
  /* Step1: configure initialization sequence Info */
  pImpedanceCfg->SeqStartAddr = 0;
  pImpedanceCfg->MaxSeqLen = 512; /* @todo add checker in function */

	pImpedanceCfg->ImpODR = 10;
  pImpedanceCfg->RcalVal = 10000.0;
  pImpedanceCfg->SinFreq = 1000.0;
  pImpedanceCfg->FifoThresh = 4;
	
	/* Set switch matrix to onboard(EVAL-AD5940ELECZ) dummy sensor. */
	/* Note the RCAL0 resistor is 10kOhm. */
	pImpedanceCfg->DswitchSel = SWD_CE0;
	
	//pImpedanceCfg->PswitchSel = SWP_RE0;	
	pImpedanceCfg->PswitchSel = SWP_CE0;
	
	//pImpedanceCfg->NswitchSel = SWN_SE0;
	pImpedanceCfg->NswitchSel = SWN_AIN1;
	
	//pImpedanceCfg->TswitchSel = SWT_SE0LOAD;
	pImpedanceCfg->TswitchSel = SWT_AIN1;
	
	
	/* The dummy sensor is as low as 5kOhm. We need to make sure RTIA is small enough that HSTIA won't be saturated. */
	//pImpedanceCfg->HstiaRtiaSel = HSTIARTIA_5K;	
	
	pImpedanceCfg->HstiaRtiaSel = HSTIARTIA_200;
	
	/* Configure the sweep function. */
	pImpedanceCfg->SweepCfg.SweepEn = bTRUE;
	pImpedanceCfg->SweepCfg.SweepStart = 100;	/* Start from 1kHz */
	pImpedanceCfg->SweepCfg.SweepStop = 200e3f;		/* Stop at 100kHz */
	pImpedanceCfg->SweepCfg.SweepPoints = 201;		/* Points is 101 */
	//pImpedanceCfg->SweepCfg.SweepLog = bTRUE;
	pImpedanceCfg->SweepCfg.SweepLog = bTRUE;
	/* Configure Power Mode. Use HP mode if frequency is higher than 80kHz. */
	pImpedanceCfg->PwrMod = AFEPWR_HP;
	
	/* Configure filters if necessary */
	pImpedanceCfg->ADCSinc3Osr = ADCSINC3OSR_2;		/* Sample rate is 800kSPS/2 = 400kSPS */
  pImpedanceCfg->DftNum = DFTNUM_16384;
  pImpedanceCfg->DftSrc = DFTSRC_SINC3;
}


void AD5940VoltageStructInit(void)
{
  AppVOLCfg_Type *pVoltageCfg;
  
  get_APPVOLGetCfg(&pVoltageCfg);
  /* Step1: configure initialization sequence Info */
  pVoltageCfg->SeqStartAddr = 0;
  pVoltageCfg->MaxSeqLen = 512; /* @todo add checker in function */

	pVoltageCfg->VolODR = 100;
  pVoltageCfg->RcalVal = 10000.0;
  pVoltageCfg->SinFreq = 100000;
  pVoltageCfg->FifoThresh = 4;
	
	/* Set switch matrix to onboard(EVAL-AD5940ELECZ) dummy sensor. */
	/* Note the RCAL0 resistor is 10kOhm. */
	pVoltageCfg->DswitchSel = SWD_CE0;
	//pImpedanceCfg->PswitchSel = SWP_RE0;
	
	pVoltageCfg->PswitchSel = SWP_CE0;
	//pImpedanceCfg->NswitchSel = SWN_SE0;
	
	pVoltageCfg->NswitchSel = SWN_AIN1;
	//pImpedanceCfg->TswitchSel = SWT_SE0LOAD;
	pVoltageCfg->TswitchSel = SWT_AIN1;
	
	/* The dummy sensor is as low as 5kOhm. We need to make sure RTIA is small enough that HSTIA won't be saturated. */
	//pImpedanceCfg->HstiaRtiaSel = HSTIARTIA_5K;	
	
	pVoltageCfg->HstiaRtiaSel = HSTIARTIA_200;
	
	/* Configure the sweep function. */
	pVoltageCfg->SweepCfg.SweepEn = bFALSE;
	pVoltageCfg->SweepCfg.SweepStart = 10;	/* Start from 1kHz */
	pVoltageCfg->SweepCfg.SweepStop = 200e3f;		/* Stop at 100kHz */
	pVoltageCfg->SweepCfg.SweepPoints = 201;		/* Points is 101 */
	//pImpedanceCfg->SweepCfg.SweepLog = bTRUE;
	pVoltageCfg->SweepCfg.SweepLog = bFALSE;
	/* Configure Power Mode. Use HP mode if frequency is higher than 80kHz. */
	pVoltageCfg->PwrMod = AFEPWR_HP;
	
	/* Configure filters if necessary */
	pVoltageCfg->ADCSinc3Osr = ADCSINC3OSR_2;		/* Sample rate is 800kSPS/2 = 400kSPS */
  pVoltageCfg->DftNum = DFTNUM_16384;
  pVoltageCfg->DftSrc = DFTSRC_SINC3;
}



void AD5940_Main_IMP(void)
{
  uint32_t temp;  
  AD5940PlatformCfg_IMP();
  AD5940ImpedanceStructInit();
  AppIMPInit(AppBuff, APPBUFF_SIZE);    /* Initialize IMP application. Provide a buffer, which is used to store sequencer commands */
  AppIMPCtrl(IMPCTRL_START, 0);          /* Control IMP measurement to start. Second parameter has no meaning with this command. */
 
  while(1)
  {
    if(AD5940_GetMCUIntFlag())
    {
      AD5940_ClrMCUIntFlag();
      temp = APPBUFF_SIZE;
      AppIMPISR(AppBuff, &temp);
      ImpedanceShowResult(AppBuff, temp);
    }
  }
}


void AD5940_Main_VOL(void)
{
	uint32_t temp;
	AD5940PlatformCfg_VOL();
	AD5940VoltageStructInit();
	AppVOLInit(AppBuff, APPBUFF_SIZE);    /* Initialize IMP application. Provide a buffer, which is used to store sequencer commands */
  AppVOLCtrl(IMPCTRL_START, 0);          /* Control IMP measurement to start. Second parameter has no meaning with this command. */
	
	uint32_t cnt = 0;
	while(1)
  {
    if(AD5940_GetMCUIntFlag())
    {
      AD5940_ClrMCUIntFlag();
      temp = APPBUFF_SIZE;
      AppVOLISR(AppBuff, &temp);
			VoltageShowResult(AppBuff, temp);
      
    }
  }
	
}


void AD5940_Main_AMP(void)
{
  uint32_t temp;
  
  AD5940PlatformCfg_AMP();
  AD5940AMPStructInit(); /* Configure your parameters in this function */ 
  AppAMPInit(AppBuff, APPBUFF_SIZE);    /* Initialize AMP application. Provide a buffer, which is used to store sequencer commands */
	AppAMPCtrl(AMPCTRL_START, 0);         /* Control AMP measurement to start. Second parameter has no meaning with this command. */
	
  while(1)
  {
    /* Check if interrupt flag which will be set when interrupt occurred. */
    if(AD5940_GetMCUIntFlag())
    {
      AD5940_ClrMCUIntFlag(); /* Clear this flag */
      temp = APPBUFF_SIZE;
      AppAMPISR(AppBuff, &temp); /* Deal with it and provide a buffer to store data we got */
      AMPShowResult((float*)AppBuff, temp); /* Show the results to UART */
    }
  }
}

