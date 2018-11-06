using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankOfBitsAndBytes
{
    class Program
    {
        public delegate void delg();

        static void Main(string[] args)
        {
            Semaphore sem = new Semaphore(2, 8);
            int pwLength = -1, inc = 0;
            int currentThreadIndex = 0, currentLetterIndex = 0;
            int currentBalance = 0;
            Thread[] threadPool = new Thread[8];

            List<int[]> pwList = new List<int[]>();
            foreach (Thread t in threadPool)
            {

            }

            BankOfBitsNBytes bbb = new BankOfBitsNBytes();

            while (FindLength(new char[inc], bbb) == -1)
            {
                inc++;
                pwLength = inc;
            }

            Console.Out.WriteLine("pw length = " + pwLength);

            //foreach (Thread t in threadPool)
            //{
            //    ThreadStart ts = new ThreadStart(() => { someInt++; });     //Create a thread start given delegate
            //    Thread t = new Thread(ts);                          //Create thread with threadstart
            //    t.Start();
            //}

            while (currentBalance < 1000000)
            {

                for (int i = 0; i < threadPool.Length; i++)
                {
                    char[] s = new char[pwLength];

                    List<delg> delgList = new List<delg>()
                    {
                        () => IncrementPW(pwList.ElementAt(i), ref currentLetterIndex),
                        () => {
                            for(int j = 0; j < pwLength; j++){
                                s[j] += IntToChar(s[j]);
                            }
                        },
                        () => bbb.WithdrawMoney(s)
                     };


                    ThreadStart ts = new ThreadStart(
                        () => {
                        
}
                        );     //Create a thread start give
                    Thread t = new Thread(ts);                          //Create thread with threadstart
                    t.Start();
                    currentLetterIndex++;
                }




            }


            //Stack<delg> toProcess = new Stack<delg>(delgList);

            //while (toProcess.Count > 0)
            //{
            //    delg nextToProc = toProcess.Pop();
            //    StartThread(nextToProc);
            //}


            //for (int i = 0; i < 100; i++)
            //{
            //    char[] pwToGuess = BankOfBitsNBytes.GenerateRandomCharArray(pwLength);
            //    string s = BankOfBitsNBytes.CharArrayToString(pwToGuess);
            //    int returnedAmt = bbb.WithdrawMoney(BankOfBitsNBytes.GenerateRandomCharArray(pwLength));
            //    Console.WriteLine("Returned amt: " + returnedAmt);
            //}
            Console.ReadLine();
        }

        public static void IncrementPW(int[] _pw, ref int currentFirstLetter)
        {
            int l = _pw.Length;
            int decrement = 1;
            bool changed = false;

            while (!changed)
            {
                if (!IntToChar(_pw[l - decrement]).Equals(BankOfBitsNBytes.acceptablePasswordChars.Length - 1))
                {
                    _pw[l - decrement]++;
                    changed = true;
                }
                else
                {
                    if (decrement <= l)
                        _pw[l - decrement] = 0;
                    else
                    {
                        _pw[l - decrement] = currentFirstLetter;
                        currentFirstLetter++;
                    }
                    decrement++;
                }
            }
        }

        public static int FindLength(char[] _pw, BankOfBitsNBytes _bbb)
        {
            return _bbb.WithdrawMoney(FillRandomly(_pw));
        }

        public static int FindLength(int currentThreadIndex, List<char[]> _pwList, BankOfBitsNBytes _bbb)
        {
            return _bbb.WithdrawMoney(FillRandomly(_pwList[currentThreadIndex]));
        }

        public static char[] FillRandomly(char[] toFill)
        {
            for (int i = 0; i < toFill.Length; i++)
            {
                toFill[i] = 'a';
                //toFill[i] = BankOfBitsNBytes.acceptablePasswordChars[new Random().Next(0, BankOfBitsNBytes.acceptablePasswordChars.Length - 1)];
            }

            return toFill;
        }

        public static char IntToChar(int i)
        {
            return BankOfBitsNBytes.acceptablePasswordChars[i];
        }

        public static void StartThread(delg d)
        {
            ThreadStart ts = new ThreadStart(d);
            Thread t = new Thread(ts);
            t.Start();
        }
    }
}
