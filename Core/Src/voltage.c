#include "ad5940.h"
#include <stdio.h>
#include "string.h"
#include "math.h"
#include "voltage.h"

/* Default LPDAC resolution(2.5V internal reference). */
#define DAC12BITVOLT_1LSB   (2200.0f/4095)  //mV
#define DAC6BITVOLT_1LSB    (DAC12BITVOLT_1LSB*64)  //mV

AppVOLCfg_Type AppVOLCfg = 
{
    .bParaChanged = bFALSE,
    .SeqStartAddr = 0,
    .MaxSeqLen = 0,
    
    .SeqStartAddrCal = 0,
    .MaxSeqLenCal = 0,
    
    .VolODR = 1000.0,           /* 20.0 Hz*/
    .NumOfData = -1,
    .SysClkFreq = 16000000.0,
    .WuptClkFreq = 32000.0,
    .AdcClkFreq = 16000000.0,
    .RcalVal = 10000.0,
    
    .DswitchSel = SWD_CE0,
    .PswitchSel = SWP_CE0,
    .NswitchSel = SWN_AIN1,
    .TswitchSel = SWT_AIN1,
    
    .PwrMod = AFEPWR_HP,
    
    .HstiaRtiaSel = HSTIARTIA_5K,
    .ExcitBufGain = EXCITBUFGAIN_2,
    .HsDacGain = HSDACGAIN_1,
    .HsDacUpdateRate = 7,
    
    .DacVoltPP = 600.0,
    .BiasVolt = -0.0f,
    
    .SinFreq = 100000.0, /* 1000Hz */
    
    .DftNum = DFTNUM_16384,
    .DftSrc = DFTSRC_SINC3,
    .HanWinEn = bTRUE,
    
    .AdcPgaGain = ADCPGA_1,
    .ADCSinc3Osr = ADCSINC3OSR_2,
    .ADCSinc2Osr = ADCSINC2OSR_44,
    
    .ADCAvgNum = ADCAVGNUM_16,
    
    .SweepCfg.SweepEn = bFALSE,
    .SweepCfg.SweepStart = 1000,
    .SweepCfg.SweepStop = 100000.0,
    .SweepCfg.SweepPoints = 101,
    .SweepCfg.SweepLog = bFALSE,
    .SweepCfg.SweepIndex = 0,
    
    .FifoThresh = 64,
    .APPInited = bFALSE,
    .StopRequired = bFALSE,
};

int32_t get_APPVOLGetCfg(void *pCfg)
{
    if(pCfg)
    {
        *(AppVOLCfg_Type**) pCfg = &AppVOLCfg;
		
		
        return AD5940ERR_OK;
    }
    else
    {
        return AD5940ERR_PARA;
    }
}

int32_t AppVOLCtrl(uint32_t Command, void *pPara)
{
  
	switch (Command)
	{
		case VOLCTRL_START:
		{
			WUPTCfg_Type wupt_cfg;
			if(AD5940_WakeUp(10) > 10)  /* Wakeup AFE by read register, read 10 times at most */
				return AD5940ERR_WAKEUP;  /* Wakeup Failed */
			if(AppVOLCfg.APPInited == bFALSE)
				return AD5940ERR_APPERROR;
			/* Start it */
			wupt_cfg.WuptEn = bTRUE;
			wupt_cfg.WuptEndSeq = WUPTENDSEQ_A;
			wupt_cfg.WuptOrder[0] = SEQID_0;
			wupt_cfg.SeqxSleepTime[SEQID_0] = 1;
			wupt_cfg.SeqxWakeupTime[SEQID_0] = (uint32_t)(AppVOLCfg.WuptClkFreq/AppVOLCfg.VolODR)-1;
			
			AD5940_WUPTCfg(&wupt_cfg);
			AppVOLCfg.FifoDataCount = 0;  /* restart */
			break;
		}
		case VOLCTRL_GETFREQ:
      {
        if(pPara == 0)
          return AD5940ERR_PARA;
        if(AppVOLCfg.SweepCfg.SweepEn == bTRUE)
          *(float*)pPara = AppVOLCfg.FreqofData;
        else
          *(float*)pPara = AppVOLCfg.SinFreq;
      }
    break;
			
		default:
		break;
	}
	return AD5940ERR_OK;
}


/* generated code snnipet */
float AppVOLGetCurrFreq(void)
{
  if(AppVOLCfg.SweepCfg.SweepEn == bTRUE)
    return AppVOLCfg.FreqofData;
  else
    return AppVOLCfg.SinFreq;
}

/* Application sequence initialization */
static AD5940Err AppVOLSeqCfgGen(void)
{
	AD5940Err error = AD5940ERR_OK;
	
	const uint32_t *pSeqCmd;
	uint32_t SeqLen;
	AFERefCfg_Type aferef_cfg;
	HSLoopCfg_Type HsLoopCfg;
	DSPCfg_Type dsp_cfg;
	float sin_freq;
	
	/* Start sequence generator here */
	AD5940_SEQGenCtrl(bTRUE);
	AD5940_AFECtrlS(AFECTRL_ALL, bFALSE);  /* Init all to disable state */
	
	/****configuration the AFCON Register***/
	aferef_cfg.HpBandgapEn = bTRUE;
	aferef_cfg.Hp1V1BuffEn = bTRUE;
	aferef_cfg.Hp1V8BuffEn = bTRUE;
	aferef_cfg.Disc1V1Cap = bFALSE;
	aferef_cfg.Disc1V8Cap = bFALSE;
	aferef_cfg.Hp1V8ThemBuff = bFALSE;
	aferef_cfg.Hp1V8Ilimit = bFALSE;
	aferef_cfg.Lp1V1BuffEn = bFALSE;
	aferef_cfg.Lp1V8BuffEn = bFALSE;
	
	/* LP reference control - turn off them to save power*/
  if(AppVOLCfg.BiasVolt != 0.0f)    /* With bias voltage */
  {
    aferef_cfg.LpBandgapEn = bTRUE;
    aferef_cfg.LpRefBufEn = bTRUE;
  }
  else
  {
    aferef_cfg.LpBandgapEn = bFALSE;
    aferef_cfg.LpRefBufEn = bFALSE;
  }
	AD5940_REFCfgS(&aferef_cfg);
	/***high speed LOOP configuration*****************************/
	
	/***high speed dac configuration***/
	HsLoopCfg.HsDacCfg.ExcitBufGain = AppVOLCfg.ExcitBufGain;
	HsLoopCfg.HsDacCfg.HsDacGain = AppVOLCfg.HsDacGain;
	HsLoopCfg.HsDacCfg.HsDacUpdateRate = AppVOLCfg.HsDacUpdateRate;
	
	/***high speed dac TIA configuration************************/
	HsLoopCfg.HsTiaCfg.DiodeClose = bFALSE;
	
	if(AppVOLCfg.BiasVolt != 0.0f)    /* With bias voltage */
		HsLoopCfg.HsTiaCfg.HstiaBias = HSTIABIAS_VZERO0;
	else
		HsLoopCfg.HsTiaCfg.HstiaBias = HSTIABIAS_1P1;
	
	HsLoopCfg.HsTiaCfg.HstiaCtia = 31; /* 31pF + 2pF */
	HsLoopCfg.HsTiaCfg.HstiaDeRload = HSTIADERLOAD_OPEN;
	HsLoopCfg.HsTiaCfg.HstiaDeRtia = HSTIADERTIA_OPEN;
	HsLoopCfg.HsTiaCfg.HstiaRtiaSel = AppVOLCfg.HstiaRtiaSel;
	
	/***high speed loop switch matrix configuration*************/
	HsLoopCfg.SWMatCfg.Dswitch = AppVOLCfg.DswitchSel;
	HsLoopCfg.SWMatCfg.Pswitch = AppVOLCfg.PswitchSel;
	HsLoopCfg.SWMatCfg.Nswitch = AppVOLCfg.NswitchSel;
	HsLoopCfg.SWMatCfg.Tswitch = SWT_TRTIA|AppVOLCfg.TswitchSel;
	
	/***high speed loop wave generation configuration***********/
	HsLoopCfg.WgCfg.WgType = WGTYPE_SIN;
	HsLoopCfg.WgCfg.GainCalEn = bTRUE;
	HsLoopCfg.WgCfg.OffsetCalEn = bTRUE;
	if(AppVOLCfg.SweepCfg.SweepEn == bTRUE)
	{
		AppVOLCfg.FreqofData = AppVOLCfg.SweepCfg.SweepStart;
		AppVOLCfg.SweepCurrFreq = AppVOLCfg.SweepCfg.SweepStart;
		AD5940_SweepNext(&AppVOLCfg.SweepCfg, &AppVOLCfg.SweepNextFreq);
		sin_freq = AppVOLCfg.SweepCurrFreq;
		//printf("frequency: %0.2f \n", sin_freq);
	}
	else
	{
		sin_freq = AppVOLCfg.SinFreq;
		AppVOLCfg.FreqofData = sin_freq;
	}
	
	HsLoopCfg.WgCfg.SinCfg.SinFreqWord = AD5940_WGFreqWordCal(sin_freq, AppVOLCfg.SysClkFreq);
	HsLoopCfg.WgCfg.SinCfg.SinAmplitudeWord = (uint32_t)(AppVOLCfg.DacVoltPP/800.0f*2047 + 0.5f);
	HsLoopCfg.WgCfg.SinCfg.SinOffsetWord = 0;
	HsLoopCfg.WgCfg.SinCfg.SinPhaseWord = 0;
	AD5940_HSLoopCfgS(&HsLoopCfg);
		
	if(AppVOLCfg.BiasVolt != 0.0f)    /* With bias voltage */
	{
		LPDACCfg_Type lpdac_cfg;
		
		lpdac_cfg.LpdacSel = LPDAC0;
		lpdac_cfg.LpDacVbiasMux = LPDACVBIAS_12BIT; /* Use Vbias to tuning BiasVolt. */
		lpdac_cfg.LpDacVzeroMux = LPDACVZERO_6BIT;  /* Vbias-Vzero = BiasVolt */
		lpdac_cfg.DacData6Bit = 0x40>>1;            /* Set Vzero to middle scale. */
		if(AppVOLCfg.BiasVolt<-1100.0f) AppVOLCfg.BiasVolt = -1100.0f + DAC12BITVOLT_1LSB;
		if(AppVOLCfg.BiasVolt> 1100.0f) AppVOLCfg.BiasVolt = 1100.0f - DAC12BITVOLT_1LSB;
		lpdac_cfg.DacData12Bit = (uint32_t)((AppVOLCfg.BiasVolt + 1100.0f)/DAC12BITVOLT_1LSB);
		lpdac_cfg.DataRst = bFALSE;      /* Do not reset data register */
		lpdac_cfg.LpDacSW = LPDACSW_VBIAS2LPPA|LPDACSW_VBIAS2PIN|LPDACSW_VZERO2LPTIA|LPDACSW_VZERO2PIN|LPDACSW_VZERO2HSTIA;
		lpdac_cfg.LpDacRef = LPDACREF_2P5;
		lpdac_cfg.LpDacSrc = LPDACSRC_MMR;      /* Use MMR data, we use LPDAC to generate bias voltage for LPTIA - the Vzero */
		lpdac_cfg.PowerEn = bTRUE;              /* Power up LPDAC */
		AD5940_LPDACCfgS(&lpdac_cfg);
	}
	
	/***********digital signal process part configuration********************************/
	
	/**************configuration of ADC input matrix and PGA*****************************/
	dsp_cfg.ADCBaseCfg.ADCMuxN = ADCMUXN_HSTIA_N;
	dsp_cfg.ADCBaseCfg.ADCMuxP = ADCMUXP_HSTIA_P;
	dsp_cfg.ADCBaseCfg.ADCPga = AppVOLCfg.AdcPgaGain;
	memset(&dsp_cfg.ADCDigCompCfg, 0, sizeof(dsp_cfg.ADCDigCompCfg));
	
	
	dsp_cfg.ADCFilterCfg.ADCAvgNum = AppVOLCfg.ADCAvgNum;
	dsp_cfg.ADCFilterCfg.ADCRate = ADCRATE_800KHZ;	/* Tell filter block clock rate of ADC*/
	dsp_cfg.ADCFilterCfg.ADCSinc2Osr = AppVOLCfg.ADCSinc2Osr;
	dsp_cfg.ADCFilterCfg.ADCSinc3Osr = AppVOLCfg.ADCSinc3Osr;
	dsp_cfg.ADCFilterCfg.BpNotch = bTRUE;
	dsp_cfg.ADCFilterCfg.BpSinc3 = bFALSE;
	dsp_cfg.ADCFilterCfg.Sinc2NotchEnable = bTRUE;
	dsp_cfg.DftCfg.DftNum = AppVOLCfg.DftNum;
	dsp_cfg.DftCfg.DftSrc = AppVOLCfg.DftSrc;
	dsp_cfg.DftCfg.HanWinEn = AppVOLCfg.HanWinEn;
	
	memset(&dsp_cfg.StatCfg, 0, sizeof(dsp_cfg.StatCfg)); // since the statistics value not be output 
	
	AD5940_DSPCfgS(&dsp_cfg);
		
	/* Enable all of them. They are automatically turned off during hibernate mode to save power */
	if(AppVOLCfg.BiasVolt == 0.0f)
		AD5940_AFECtrlS(AFECTRL_HSTIAPWR|AFECTRL_INAMPPWR|AFECTRL_EXTBUFPWR|\
					AFECTRL_WG|AFECTRL_DACREFPWR|AFECTRL_HSDACPWR|\
					AFECTRL_SINC2NOTCH, bTRUE);
	else
		AD5940_AFECtrlS(AFECTRL_HSTIAPWR|AFECTRL_INAMPPWR|AFECTRL_EXTBUFPWR|\
					AFECTRL_WG|AFECTRL_DACREFPWR|AFECTRL_HSDACPWR|\
					AFECTRL_SINC2NOTCH|AFECTRL_DCBUFPWR, bTRUE);
		/* Sequence end. */
	AD5940_SEQGenInsert(SEQ_STOP()); /* Add one extra command to disable sequencer for initialization sequence because we only want it to run one time. */
	
	/* Stop here */
	error = AD5940_SEQGenFetchSeq(&pSeqCmd, &SeqLen);
		
	AD5940_SEQGenCtrl(bFALSE); /* Stop sequencer generator */
	if(error == AD5940ERR_OK)
	{
		AppVOLCfg.InitSeqInfo.SeqId = SEQID_1;
		AppVOLCfg.InitSeqInfo.SeqRamAddr = AppVOLCfg.SeqStartAddr;
		AppVOLCfg.InitSeqInfo.pSeqCmd = pSeqCmd;
		AppVOLCfg.InitSeqInfo.SeqLen = SeqLen;
		/* Write command to SRAM */
		AD5940_SEQCmdWrite(AppVOLCfg.InitSeqInfo.SeqRamAddr, pSeqCmd, SeqLen);
	}
	else
		return error; /* Error */
	return AD5940ERR_OK;
}

static AD5940Err AppVOLSeqMeasureGen(void)
{
	AD5940Err error = AD5940ERR_OK;
	const uint32_t *pSeqCmd;
	uint32_t SeqLen;
	uint32_t WaitClks;
	SWMatrixCfg_Type sw_cfg;
	ClksCalInfo_Type clks_cal;
		
	clks_cal.DataType = DATATYPE_SINC2;
	clks_cal.DataCount = 1;
	clks_cal.ADCSinc2Osr = AppVOLCfg.ADCSinc2Osr;
	clks_cal.ADCSinc3Osr = AppVOLCfg.ADCSinc3Osr;
	clks_cal.ADCAvgNum = 0;
	clks_cal.RatioSys2AdcClk = AppVOLCfg.SysClkFreq/AppVOLCfg.AdcClkFreq;
	AD5940_ClksCalculate(&clks_cal, &WaitClks);
		
	AD5940_SEQGenCtrl(bTRUE);
	AD5940_SEQGpioCtrlS(AGPIO_Pin2); /* Set GPIO1, clear others that under control */
	AD5940_SEQGenInsert(SEQ_WAIT(16*250));  /* @todo wait 250us? 16Mhz*/
		
	/*******************************************************************************/
	/* Configure matrix for external Rz */
	sw_cfg.Dswitch = AppVOLCfg.DswitchSel;
	sw_cfg.Pswitch = AppVOLCfg.PswitchSel;
	sw_cfg.Nswitch = AppVOLCfg.NswitchSel;
	sw_cfg.Tswitch = SWT_TRTIA|AppVOLCfg.TswitchSel;
	AD5940_SWMatrixCfgS(&sw_cfg);			
	AD5940_AFECtrlS(AFECTRL_ADCPWR|AFECTRL_WG, bTRUE);  /* Enable Waveform generator */
	
	AD5940_SEQGenInsert(SEQ_WAIT(16*10));  //delay for signal settling DFT_WAIT
	
	AD5940_AFECtrlS(AFECTRL_ADCCNV, bTRUE);  /* Start ADC convert*/
	AD5940_SEQGenInsert(SEQ_WAIT(WaitClks));  /* wait for first data ready */
	
	AD5940_AFECtrlS(AFECTRL_ADCCNV|AFECTRL_WG|AFECTRL_ADCPWR, bFALSE);  /* Stop ADC convert */
	
//	AD5940_AFECtrlS(AFECTRL_HSTIAPWR|AFECTRL_INAMPPWR|AFECTRL_EXTBUFPWR|\
//					AFECTRL_WG|AFECTRL_DACREFPWR|AFECTRL_HSDACPWR|\
//					AFECTRL_SINC2NOTCH, bFALSE);
//					
//	AD5940_SEQGpioCtrlS(0); /* Clr GPIO1 */
//	
//	AD5940_EnterSleepS();/* Goto hibernate */
//	
	/* Sequence end. */	
	error = AD5940_SEQGenFetchSeq(&pSeqCmd, &SeqLen);	
	AD5940_SEQGenCtrl(bFALSE); /* Stop sequencer generator */
	
	if(error == AD5940ERR_OK)
	{
		AppVOLCfg.MeasureSeqInfo.SeqId = SEQID_0;
		AppVOLCfg.MeasureSeqInfo.SeqRamAddr = AppVOLCfg.InitSeqInfo.SeqRamAddr + AppVOLCfg.InitSeqInfo.SeqLen ;
		AppVOLCfg.MeasureSeqInfo.pSeqCmd = pSeqCmd;
		AppVOLCfg.MeasureSeqInfo.SeqLen = SeqLen;
		/* Write command to SRAM */
		AD5940_SEQCmdWrite(AppVOLCfg.MeasureSeqInfo.SeqRamAddr, pSeqCmd, SeqLen);
	}
	else
		return error; /* Error */
	return AD5940ERR_OK;
}

/* This function provide application initialize. It can also enable Wupt that will automatically trigger sequence. Or it can configure  */
int32_t AppVOLInit(uint32_t *pBuffer, uint32_t BufferSize)
{
	AD5940Err error = AD5940ERR_OK;  
	SEQCfg_Type seq_cfg;
	FIFOCfg_Type fifo_cfg;
	
	if(AD5940_WakeUp(10) > 10)  /* Wakeup AFE by read register, read 10 times at most */
		return AD5940ERR_WAKEUP;  /* Wakeup Failed */
	
	/* Configure sequencer and stop it */
	seq_cfg.SeqMemSize = SEQMEMSIZE_2KB;  /* 2kB SRAM is used for sequencer, others for data FIFO */
	seq_cfg.SeqBreakEn = bFALSE;
	seq_cfg.SeqIgnoreEn = bTRUE;
	seq_cfg.SeqCntCRCClr = bTRUE;
	seq_cfg.SeqEnable = bFALSE;
	seq_cfg.SeqWrTimer = 0;
	AD5940_SEQCfg(&seq_cfg);
	
	/* Reconfigure FIFO */
	AD5940_FIFOCtrlS(DFTSRC_SINC3, bFALSE);	/* Disable FIFO firstly */
	fifo_cfg.FIFOEn = bTRUE;
	fifo_cfg.FIFOMode = FIFOMODE_FIFO;
	fifo_cfg.FIFOSize = FIFOSIZE_4KB; /* 4kB for FIFO, The reset 2kB for sequencer */
	fifo_cfg.FIFOSrc = FIFOSRC_SINC3;
	fifo_cfg.FIFOThresh = AppVOLCfg.FifoThresh;           
	AD5940_FIFOCfg(&fifo_cfg);
	AD5940_INTCClrFlag(AFEINTSRC_ALLINT);
	
	/* Start sequence generator */
	/* Initialize sequencer generator */
	if((AppVOLCfg.APPInited == bFALSE)||\
		(AppVOLCfg.bParaChanged == bTRUE))
	{
		if(pBuffer == 0)  return AD5940ERR_PARA;
		if(BufferSize == 0) return AD5940ERR_PARA;
		
		AD5940_SEQGenInit(pBuffer, BufferSize);
	
		/* Generate initialize sequence */
		error = AppVOLSeqCfgGen(); /* Application initialization sequence using either MCU or sequencer */
		if(error != AD5940ERR_OK) return error;
	
		/* Generate measurement sequence */
		error = AppVOLSeqMeasureGen();
		if(error != AD5940ERR_OK) return error;
	
		AppVOLCfg.bParaChanged = bFALSE; /* Clear this flag as we already implemented the new configuration */
	}
	
	/* Initialization sequencer  */
	AppVOLCfg.InitSeqInfo.WriteSRAM = bFALSE;
	AD5940_SEQInfoCfg(&AppVOLCfg.InitSeqInfo);
	seq_cfg.SeqEnable = bTRUE;
	AD5940_SEQCfg(&seq_cfg);  /* Enable sequencer */
	AD5940_SEQMmrTrig(AppVOLCfg.InitSeqInfo.SeqId);
	while(AD5940_INTCTestFlag(AFEINTC_1, AFEINTSRC_ENDSEQ) == bFALSE);
	
	/* Measurement sequence  */
	AppVOLCfg.MeasureSeqInfo.WriteSRAM = bFALSE;
	AD5940_SEQInfoCfg(&AppVOLCfg.MeasureSeqInfo);
	
	seq_cfg.SeqEnable = bTRUE;
	AD5940_SEQCfg(&seq_cfg);  /* Enable sequencer, and wait for trigger */
	AD5940_ClrMCUIntFlag();   /* Clear interrupt flag generated before */
	AD5940_AFEPwrBW(AppVOLCfg.PwrMod, AFEBW_250KHZ);
	AppVOLCfg.APPInited = bTRUE;  /* IMP application has been initialized. */
	return AD5940ERR_OK;
}

/* Modify registers when AFE wakeup */
static AD5940Err AppVOLRegModify(int32_t * const pData, uint32_t *pDataCount)
{
	if(AppVOLCfg.NumOfData > 0)
	{
		AppVOLCfg.FifoDataCount += *pDataCount/4;
		if(AppVOLCfg.FifoDataCount >= AppVOLCfg.NumOfData)
		{
		AD5940_WUPTCtrl(bFALSE);
		return AD5940ERR_OK;
		}
	}
	if(AppVOLCfg.StopRequired == bTRUE)
	{
		AD5940_WUPTCtrl(bFALSE);
		return AD5940ERR_OK;
	}
	
	return AD5940ERR_OK;
}

/* Calculate voltage */
/* float AppAMPCalcVoltage(uint32_t ADCcode)
{
	float kFactor = 1.835/1.82;
	float fVolt = 0.0;
	int32_t tmp = 0;
	tmp = ADCcode - 32768;
	switch(AppVOLCfg.ADCPgaGain)
	{
		case ADCPGA_1:
		fVolt = ((float)(tmp)/32768)*(AppVOLCfg.ADCRefVolt/1)*kFactor;
		break;
		case ADCPGA_1P5:
		fVolt = ((float)(tmp)/32768)*(AppVOLCfg.ADCRefVolt/1.5f)*kFactor;
		break;
		case ADCPGA_2:
		fVolt = ((float)(tmp)/32768)*(AppVOLCfg.ADCRefVolt/2)*kFactor;
		break;
		case ADCPGA_4:
		fVolt = ((float)(tmp)/32768)*(AppVOLCfg.ADCRefVolt/4)*kFactor;
		break;
		case ADCPGA_9:
		fVolt = ((float)(tmp)/32768)*(AppVOLCfg.ADCRefVolt/9)*kFactor;
		break;
	} 
	return fVolt;
} */

int32_t AppVOLDataProcess(int32_t * const pData, uint32_t *pDataCount)
{
  uint32_t DataCount = *pDataCount;
	
  /* Convert DFT result to int32_t type */
  for(uint32_t i=0; i<DataCount; i++)
  {
    pData[i] &= 0xffff;
		//pData[i] -= 32768;
  }
  
  return 0;
}



int32_t AppVOLISR(void *pBuff, uint32_t *pCount)
{
	uint32_t FifoCnt;
	*pCount = 0;
	
	if(AD5940_WakeUp(10) > 10)  /* Wakeup AFE by read register, read 10 times at most */
		return AD5940ERR_WAKEUP;  /* Wakeup Failed */
	
	AD5940_SleepKeyCtrlS(SLPKEY_LOCK);  /* Prohibit AFE to enter sleep mode. */
	
	if(AD5940_INTCTestFlag(AFEINTC_0, AFEINTSRC_DATAFIFOTHRESH) == bTRUE)
	{
		FifoCnt = AD5940_FIFOGetCnt();
		AD5940_FIFORd((uint32_t *)pBuff, FifoCnt);
		AD5940_INTCClrFlag(AFEINTSRC_DATAFIFOTHRESH);
		AppVOLRegModify(pBuff, &FifoCnt);   /* If there is need to do AFE re-configure, do it here when AFE is in active state */
		AD5940_SleepKeyCtrlS(SLPKEY_UNLOCK);  /* Allow AFE to enter sleep mode. */
		/* Process data */ 
		AppVOLDataProcess((int32_t*)pBuff,&FifoCnt); 
	
		*pCount = FifoCnt;
		return 0;
	}
	
	return 0;
} 






