﻿// Decompiled with JetBrains decompiler
// Type: ZeroGravity.Data.GeneratorPowerAuxData
// Assembly: HELLION_Dedicated, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DB91419-85E5-4D49-97FF-EABAD43D2995
// Assembly location: F:\Hellion\HellionExtendedServer\GameLibraries\HELLION_Dedicated.exe

namespace ZeroGravity.Data
{
  public class GeneratorPowerAuxData : SubSystemAuxData
  {
    public float ResponseTime;

    public override SubSystemAuxDataType AuxDataType
    {
      get
      {
        return SubSystemAuxDataType.PowerGenerator;
      }
    }
  }
}