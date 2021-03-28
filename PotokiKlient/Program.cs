using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static int threadcounter = 1;
        public static NamedPipeClientStream client;

        static void Main(string[] args)
        {
            client = new NamedPipeClientStream(".", "A", PipeDirection.InOut, PipeOptions.Asynchronous);
            client.Connect();

            var t1 = new System.Threading.Thread(StartSend);

            t1.Start();
        }

        public static void StartSend()
        {
            int thisThread = threadcounter;

            StartReadingAsync(client);

            for (int i = 0; i < 10000; i++)
            {
                Thread.Sleep(1000);
                var buf = new byte[8];
                buf[0] = (byte)i;
                client.WriteAsync(buf, 0, 8);

                Console.WriteLine($@"Thread{thisThread} Wrote: {buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5] + buf[6] + buf[7] }");
            }
        }

        public static async Task StartReadingAsync(NamedPipeClientStream pipe)
        {
            var bufferLength = 8;
            byte[] pBuffer = new byte[bufferLength];

            await pipe.ReadAsync(pBuffer, 0, bufferLength).ContinueWith(async c =>
            {
                Console.WriteLine($@"read data {pBuffer[0] + pBuffer[1] + pBuffer[2] + pBuffer[3] + pBuffer[4] + pBuffer[5] + pBuffer[6] + pBuffer[7]}");
                await StartReadingAsync(pipe); // read the next data <-- 
            });
        }
    }
}