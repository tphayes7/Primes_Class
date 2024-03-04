/*
This is a list of classes used to find a verify prime numbers.
Copyright (C) 2024  Travis Hayes

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see https://www.gnu.org/licenses/.
*/

using System;
using System.Collections;

namespace Primes;

public static class Primes1
{
	private static List<long> primesList;
    private static long maxList;
    private static long maxPrime;

    static Primes1()
    //Populates the primesList.  Change maxList to make the list larger/smaller.
	{
        //Preset 2 to the list.
        primesList = new List<long>();
        primesList.Add(2);

        //The highest possible prime that can be found by this program is maxList^2.
        maxList = 10000000;
        maxPrime = maxList * maxList;

        //Loop through all integers from 3 through maxNum.
        for (long i = 3; i <= maxList; i++)
        {
            if (isPrime(i) == 1) { primesList.Add(i); }
        }
    }

    public static int isPrime(long toCheck)
    //Checks to see if a given number is prime.
    {
        //If the number to check is less than the largest value on the list, just check to see if the number already exists on the list.
        if (toCheck <= primesList.Last())
        {
            if (primesList.Contains(toCheck))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else if (toCheck <= maxPrime)
        {
            //Loop through the list of primes.  If a number is not divisible by a composite number's prime factors,
            //it follows that it is also not divisible by the composite number itself.  As such, there is no reason
            //to check divisibility by any composite number.
            for (int i = 0; i < primesList.Count; i++)
            {
                //If the number is divisible, it is not prime.
                if (toCheck % primesList[i] == 0)
                {
                    return 0;
                }
                //If the integer is not divisible by any factor less than the square root, it follows that it will not
                //be divisible by any factor greater than the square root.  The number is prime.
                if (Math.Sqrt(toCheck) < primesList[i])
                {
                    return 1;
                }
            }
        }
        //Number is too large to determine if prime based on the current primes list.
        return -1;
    }

    public static long getNextPrime(long firstNum)
    //Returns the first prime after the passed value.
    {
        //Start loop at the passed value.  maxLoops is a failsafe to keep the process from taking too long, but can be set to any desired value.
        int maxLoops = 100000000;
        for (long i = firstNum; i <= (firstNum + maxLoops); i++)
        {
            //Check if i is prime.
            int check = isPrime(i);
            //isPrime can return a 1, 0, or -1.  If it returns 1, the number is prime, and should be returned.  If it returns -1, the number is too large to check
            //with the current list; break from the loop and return -1 to indicate an indeterminate result.  Otherwise, the number is not prime, and the loop
            //should continue.
            if (check == 1)
            {
                return i;
            }
            else if (check == -1)
            {
                return -1;
            }
        }
        return -1;
    }
}

public static class Primes2
{
    public static int isPrime(long toCheck)
    //Checks to see if a given number is prime.
    {
        //Check edge cases.
        if (toCheck == 2 || toCheck == 3)
        {
            return 1;
        }
        if (toCheck < 2)
        {
            return 0;
        }
        //Check divisibility by 2 separately because the loop will skip even numbers.
        if (toCheck % 2 == 0)
        {
            return 0;
        }
        //maxPrime is the maximum prime value that can be found by this program.  It can be set to any desired value, but for large primes this could
        //result in a long processing time.  Alternatively, for open-ended use, the loop terminator can be set to i <= Math.Sqrt(toCheck).
        long maxPrime = 100000000000000;
        if (toCheck > maxPrime)
        {
            return -1;
        }
        for (long i = 3; i <= maxPrime; i += 2)
        {
            //If the number is divisible, it is not prime.
            if (toCheck % i == 0)
            {
                return 0;
            }
            //If the integer is not divisible by any factor less than the square root, it follows that it will not
            //be divisible by any factor greater than the square root.  The number is prime.
            if (Math.Sqrt(toCheck) < i)
            {
                return 1;
            }
        }
        return -1;
    }

    public static long getNextPrime(long firstNum)
    //Returns the first prime after the passed value.
    {
        //Start loop at the passed value.  maxLoops is a failsafe to keep the process from taking too long, but can be set to any desired value.
        int maxLoops = 100000000;
        for (long i = firstNum; i <= (firstNum + maxLoops); i++)
        {
            //Check if i is prime.
            int check = isPrime(i);
            //isPrime can return a 1, 0, or -1.  If it returns 1, the number is prime, and should be returned.  If it returns -1, the number is too large to check
            //with the current list; break from the loop and return -1 to indicate an indeterminate result.  Otherwise, the number is not prime, and the loop
            //should continue.
            if (check == 1)
            {
                return i;
            }
            else if (check == -1)
            {
                return -1;
            }
        }
        return -1;
    }
}

public static class Primes3
{
    public static int isPrime(long toCheck)
    //Checks to see if a given number is prime.
    {
        //Check edge cases.
        if (toCheck == 2 || toCheck == 3 || toCheck == 5)
        {
            return 1;
        }
        if (toCheck < 2)
        {
            return 0;
        }
        //Check divisibility by 2 and 3 separately because the loop needs to start at 5.
        if (toCheck % 2 == 0 || toCheck % 3 == 0)
        {
            return 0;
        }
        //maxPrime is the maximum prime value that can be found by this program.  It can be set to any desired value, but for large primes this could
        //result in a long processing time.  Alternatively, for open-ended use, the loop terminator can be set to i <= Math.Sqrt(toCheck).
        long maxPrime = 100000000000000;
        if (toCheck > maxPrime)
        {
            return -1;
        }
        //The loop needs to have an oscillating iterator of +2, +4, +2, +4... To accomplish this, we'll use the skipNext flag.
        bool skipNext = false;
        for (long i = 5; i <= maxPrime; i += 2)
        {
            //If the number is divisible, it is not prime.
            if (toCheck % i == 0)
            {
                return 0;
            }
            //If the integer is not divisible by any factor less than the square root, it follows that it will not
            //be divisible by any factor greater than the square root.  The number is prime.
            if (Math.Sqrt(toCheck) < i)
            {
                return 1;
            }
            //Handle the skipNext flag.
            if (skipNext)
            {
                i += 2;
                skipNext = false;
            }
            else
            {
                skipNext = true;
            }
        }
        return -1;
    }

    public static long getNextPrime(long firstNum)
    //Returns the first prime after the passed value.
    {
        //Start loop at the passed value.  maxLoops is a failsafe to keep the process from taking too long, but can be set to any desired value.
        int maxLoops = 100000000;
        for (long i = firstNum; i <= (firstNum + maxLoops); i++)
        {
            //Check if i is prime.
            int check = isPrime(i);
            //isPrime can return a 1, 0, or -1.  If it returns 1, the number is prime, and should be returned.  If it returns -1, the number is too large to check
            //with the current list; break from the loop and return -1 to indicate an indeterminate result.  Otherwise, the number is not prime, and the loop
            //should continue.
            if (check == 1)
            {
                return i;
            }
            else if (check == -1)
            {
                return -1;
            }
        }
        return -1;
    }
}

public static class Primes4
{
    public static int isPrime(long toCheck)
    //Checks to see if a given number is prime.
    {
        //Check edge cases.
        if (toCheck == 2)
        {
            return 1;
        }
        if (toCheck < 2)
        {
            return 0;
        }
        //maxPrime is the maximum prime value that can be found by this program.  It can be set to any desired value, but for large primes this could
        //result in a long processing time.  Alternatively, for open-ended use, the loop terminator can be set to i <= Math.Sqrt(toCheck).
        long maxPrime = 100000000000000;
        if (toCheck > maxPrime)
        {
            return -1;
        }
        //Loop through all integers from 2 until sqrt(toCheck).
        for (long i = 2; i <= maxPrime; i++)
        {
            //If the number is divisible, it is not prime.
            if (toCheck % i == 0)
            {
                return 0;
            }
            //If the integer is not divisible by any factor less than the square root, it follows that it will not
            //be divisible by any factor greater than the square root.  The number is prime.
            if (Math.Sqrt(toCheck) < i)
            {
                return 1;
            }
        }
        return -1;
    }

    public static long getNextPrime(long firstNum)
    //Returns the first prime after the passed value.
    {
        //Start loop at the passed value.  maxLoops is a failsafe to keep the process from taking too long, but can be set to any desired value.
        int maxLoops = 100000000;
        for (long i = firstNum; i <= (firstNum + maxLoops); i++)
        {
            //Check if i is prime.
            int check = isPrime(i);
            //isPrime can return a 1, 0, or -1.  If it returns 1, the number is prime, and should be returned.  If it returns -1, the number is too large to check
            //with the current list; break from the loop and return -1 to indicate an indeterminate result.  Otherwise, the number is not prime, and the loop
            //should continue.
            if (check == 1)
            {
                return i;
            }
            else if (check == -1)
            {
                return -1;
            }
        }
        return -1;
    }
}