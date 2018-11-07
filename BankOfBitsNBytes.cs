using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOfBitsAndBytes
{
    class BankOfBitsNBytes
    {
        public static readonly char[] acceptablePasswordChars = new char[] 
        { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        int moneyAmt = 5000;
        public char[] password;
        private int passwordLength = 4;
        

        public BankOfBitsNBytes()
        {
            ResetPassword();
        }

        //Make this thread safe
        public int WithdrawMoney(char[] _password)
        {
            if (_password.Length != password.Length)
            {
                FailedHackDetected();
                return -1;
            }

            bool passwordCorrect = true;
            for (int i = 0; i < _password.Length; ++i)
            {
                if(password[i] != _password[i])
                {
                    passwordCorrect = false;
                    FailedHackDetected();
                }
            }

            if (passwordCorrect)
            {
                if (moneyAmt >= 500)
                {
                    moneyAmt -= 500;
                    ResetPassword();
                    return 500;
                }
                else
                {
                    Console.WriteLine("Not enough money in the bank!");
                    return -2;
                }
            }

            Console.WriteLine("Attempting to withdraw money.");
            return 0;
        }

        private void FailedHackDetected()
        {
            //Console.Out.Write("failed");
        }

        private void ResetPassword()
        {
            password = GenerateRandomCharArray(passwordLength);
            /*Random r = new Random();
            
            password = new char[passwordLength];
            for (int i = 0; i < passwordLength; ++i)
            {
                int randomInt = (r.Next() % acceptablePasswordChars.Length);
                password[i] = acceptablePasswordChars[randomInt];
            }*/
            Console.Out.WriteLine("New password: " + CharArrayToString(password));
        }

        public static Random r = new Random(); //To prevent it being re-created every frame based on sys clock (Which would produce non-random number)
        public static char[] GenerateRandomCharArray(int length)
        {
            char[] toRet = new char[length];
            for (int i = 0; i < length; ++i)
            {
                int randomInt = (r.Next() % acceptablePasswordChars.Length);
                toRet[i] = acceptablePasswordChars[randomInt];
            }
            return toRet;
        }

        public static string CharArrayToString(char[] toString)
        {
            return new string(toString);
        }
    }
}
