using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new NamedPipeServerStream("A", PipeDirection.InOut);
            server.WaitForConnection();

            while (true)
            {
                Thread.Sleep(1000);
                var b = new byte[8];
                server.Read(b, 0, 8);
                Console.WriteLine("Read Byte:" + b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + b[6] + b[7]);
                DateTime.FromBinary(BitConverter.ToInt64(b, 0));
                server.Write(b, 0, 8);
            }

            
        }

       
    }
}