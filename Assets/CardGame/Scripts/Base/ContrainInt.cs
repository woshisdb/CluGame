using System;

public class ContrainInt
{
    public int nowVal;
    public int maxVal;
    public int minVal;

    public ContrainInt(int now,int max,int min)
    {
        this.maxVal = max;
        this.minVal = min;
        this.nowVal = now;
    }

    public int Add(int x)
    {
        nowVal = Math.Min(nowVal+x,maxVal);
        return nowVal;
    }
    public int Reduce(int x)
    {
        nowVal = Math.Max(nowVal-x,minVal);
        return nowVal;
    }
    public int GetNow()
    {
        return nowVal;
    }

    public int SetNow(int now)
    {
        this.nowVal = Math.Min(now,maxVal);
        this.nowVal = Math.Max(now,minVal);
        return this.nowVal;
    }
}