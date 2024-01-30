using System;
using System.Collections.Generic;
using System.Text;

namespace PCMonitorCommon.Entity;
public static class ComDataFactory
{
    public static ComData CreateComData(FuncType func, params object[] args)
    {
        ComData rt = null;
        switch (func) 
        {
            case FuncType.Cheat: rt = new CheatData();break;
            case FuncType.CheatResponse: rt = new CheaResponsetData();break;

            default: throw new NotImplementedException("The function data is not implemented");
        }

        rt.Init(args);
        return rt;
    }

    public static ComData CreateComData(byte[] buffer)
    {
        ComData rt = null;
        FuncType func = (FuncType)buffer[5];
        switch (func)
        {
            case FuncType.Cheat: rt = new CheatData(); break;
            case FuncType.CheatResponse: rt = new CheaResponsetData(); break;

            default: throw new NotImplementedException("The function data is not implemented");
        }

        rt.FromArray(buffer);
        return rt;
    }

}
