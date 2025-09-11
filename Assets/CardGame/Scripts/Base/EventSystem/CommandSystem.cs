using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 命令接口，所有命令实现这个接口
public interface ICommand 
{
    void Execute();
}
public interface ISendCommand
{
}
public static class CommandSenderExtensions
{
    // 发送命令给特定接收者
    public static void Execute<T>(this ISendCommand sender,T command) where T : struct, ICommand
    {
        command.Execute();
    }
}
