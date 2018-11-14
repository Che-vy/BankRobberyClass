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
        public static bool reset;
        static void Main(string[] args)
        {
            Program p = new Program();
            Semaphore sem = new Semaphore(2, 8);
            int pwLength = -1, inc = 0;
            int currentThreadIndex = 0, currentLetterIndex = 0;
            int[] currentLetterIndex_array = new int[8];
            int currentBalance = 0;
            Thread[] threadPool = new Thread[8];
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();

            List<char[]> s = new List<char[]>();

            List<int[]> pwList = new List<int[]>();


            while (FindLength(new char[inc], bbb) == -1)
            {
                inc++;
                pwLength = inc;
                Console.Out.WriteLine("PW Length: " + pwLength);
            }



            for (int i = 0; i < 8; i++)
            {
                s.Add(new char[pwLength]);
                pwList.Add(new int[pwLength]);
                s[i][0] = BankOfBitsNBytes.acceptablePasswordChars[currentLetterIndex];
                pwList[i][0] = currentLetterIndex;
                currentLetterIndex_array[i] = currentLetterIndex;
                currentLetterIndex++;
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
                int startb = currentBalance;
                reset = true;

                for (int i = 0; i < 8; i++)
                {
                    s.Add(new char[pwLength]);
                    pwList.Add(new int[pwLength]);
                    s[i][0] = BankOfBitsNBytes.acceptablePasswordChars[currentLetterIndex];
                    pwList[i][0] = currentLetterIndex;
                    currentLetterIndex_array[i] = currentLetterIndex;
                    currentLetterIndex++;
                }
                while (reset)
                {
                   
                    List<char[]> ss = s;

                    for (int i = 0; i < threadPool.Length; i++)
                    {

                        int ii = i;
                        List<delg> delgList = new List<delg>()
                    {

                        () => bbb.WithdrawMoney(s[ii]),
                        () => {
                            for(int j = 0; j < pwLength; j++){
                                int jj = j;
                                char c = s[ii].ElementAt(jj);
                                s[ii].SetValue(IntToChar(pwList[ii].ElementAt(jj)), jj);
                            }
                        },
                        () => IncrementPW(pwList.ElementAt(ii), ref currentLetterIndex_array[ii], ss.ElementAt(ii), ii),

                     };

                        if (threadPool[ii] == null || !threadPool[ii].IsAlive)
                        {
                            currentLetterIndex_array[ii]++;
                            ThreadStart ts = new ThreadStart(() => { p.DoPwCheck(delgList); });         //Create a thread start give
                            threadPool[ii] = new Thread(ts);                          //Create thread with threadstart
                            threadPool[ii].Start();
                        }
                    }
                    //reset = false;

                    if (currentBalance > startb)
                        reset = false;

                }
                //Console.WriteLine("Current dough: " + currentBalance);

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

        public void DoPwCheck(List<delg> delgList)
        {
            Stack<delg> stack = new Stack<delg>(delgList);
            stack.Reverse();

            while (stack.Count > 0)
            {
                stack.Pop().Invoke();
            }

        }

        public static void IncrementPW(int[] _pw, ref int currentFirstLetter, char[] pwres, int index)
        {
            string ggg = "";
            foreach (char c in pwres) { ggg += c; }
            Console.WriteLine("In IncrementPW" + ggg);

            int l = _pw.Length;
            int decrement = 1;
            bool changed = false;

            while (!changed)
            {
                if (decrement < l && !IntToChar(_pw[l - decrement]).Equals(BankOfBitsNBytes.acceptablePasswordChars[BankOfBitsNBytes.acceptablePasswordChars.Length - 1]))
                {
                    _pw[l - decrement]++;
                    changed = true;
                    pwres[l - decrement] = BankOfBitsNBytes.acceptablePasswordChars[_pw[l - decrement]];
                }
                else
                {
                    if (decrement < l)
                    {
                        decrement = 0;
                        _pw[l - decrement] = 0;
                    }
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
