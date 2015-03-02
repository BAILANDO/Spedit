﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace Spedit.Interop
{
    public static class PipeInteropClient
    {
        public static void ConnectToMasterPipeAndSendData(string data)
        {
            byte[] stringData = Encoding.UTF8.GetBytes(data);
            int stringLength = stringData.Length;
            byte[] array = new byte[sizeof(Int32) + stringLength];
            using (MemoryStream stream = new MemoryStream(array))
            {
                byte[] stringLengthData = BitConverter.GetBytes(stringLength);
                stream.Write(stringLengthData, 0, stringLengthData.Length);
                stream.Write(stringData, 0, stringData.Length);
            }
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "SpeditNamedPipeServer", PipeDirection.Out, PipeOptions.Asynchronous);
            pipeClient.Connect(5000);
            pipeClient.Write(array, 0, array.Length);
            pipeClient.Flush();
            pipeClient.Close();
            pipeClient.Dispose();
        }
    }
}
