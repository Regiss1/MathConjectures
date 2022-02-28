using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace CollatzConjectureApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger numO = 1;
            BigInteger numOri = 1;
            BigInteger maxNum = 0; 
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);
            ReaderWriterLock locker = new ReaderWriterLock();
            TestaLoop();
            
            void TestaLoop()
            {
                var timer = new System.Threading.Timer((e) =>
                {
                    Timer();
                }, null, startTimeSpan, periodTimeSpan);
                Thread.Sleep(1000);
                while (numO > 0)
                {
                    while (numO % 2 == 0)
                    {
                        numO /= 2;


                        if (numO > maxNum)
                        {
                            Console.WriteLine(String.Format("{0:n0}", numO));
                            string txt = "NOVO RECORDE! " + String.Format("{0:n0}", numOri) + " - " + String.Format("{0:n0}", numO) + Environment.NewLine;
                            WriteLog(txt);
                            maxNum = numO;
                        }
                    }
                    if (numO != 1 && numO % 2 != 0)
                    {
                        numO = numO * 3 + 1;

                    }
                    if (numO > maxNum)
                    {
                        Console.WriteLine(String.Format("{0:n0}", numO));
                        string txt = "NOVO RECORDE! " + String.Format("{0:n0}", numOri) + " - " + String.Format("{0:n0}", numO) + Environment.NewLine;
                        WriteLog(txt);
                        maxNum = numO;
                    }
                    if (numO == 1)
                    {
                        string txt = $"Numero: {numOri} Testado" + Environment.NewLine;
                        WriteLog(txt);
                        numOri++;
                        numO = numOri;
                    }
                }
            }
            void Timer()
            {
                string txt = "Data/Hora:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + Environment.NewLine;
                WriteLog(txt);
                return;
            }
            void WriteLog(string text)
            {
                try
                {
                    locker.AcquireWriterLock(int.MaxValue); 
                    System.IO.File.AppendAllText(@"./Log.txt", text);
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
        }
    }
}
