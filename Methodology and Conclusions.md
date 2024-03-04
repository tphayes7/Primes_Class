## Introduction
The purpose of this file is to describe how the Primes classes were built, as well as the results of my testing.  This will be mostly narrative, so if you're not interested in the story of how it came to be, feel free to skip down to the Testing and Conclusions sections.  Also, there are large portions of this that involve mathematical discussions; if you want to see my methodology but want to skip the math, these sections are helpfully designated by 'Math' and 'End Math' tags.

## The Initial Idea
I was taking the Learn C# course on codecademy.com, and at one point it asked me to write a function to determine if a passed value (we'll call it 'x') was prime.  Per the instructions, I was to write a for loop that iterated through every integer from 2 through x-1, and if x was not divisible by any of these integers, the function was to return true.  I realized immediately that this was incredibly inefficient.  At the very least, there was no reason to continue iterating past x/2, because obviously testing against all of those integers would result in a number between 1 and 2, which would not be an integer and therefore indicate that x was not divisible.  So I decided I could improve on this logic.

## Two Mathematical Rules

### Math
I first began to consider how far a function would need to iterate before we could confidently say it would not find any more integers by which x would be divisible.  I had already discovered it wouldn't need to iterate past x/2, but this logic could easily be extended to x/3 as well.  After all, if we divide x by any integer between x/3 and x/2, then the result would be a number between 2 and 3, which would not be an integer.  Further, if we've already checked to see if x is divisible by 2, there's no need to check whether or not x is divisible by x/2.

I realized that each iteration yielded a pair of numbers:  the first number of the pair is the number we're dividing x by (we'll call that 'y'), and the second number is the result of x / y.  So, for instance, if we're checking x against 2, then the pair of numbers is 2 and x/2.  For each pair, there is a smaller number and a larger number (unless they're equal).  Since we're iterating through integers in ascending order, we will always reach the smaller number of the pair first, and that means we no longer need to check the larger number.

Going back, we decided the number we're dividing x by would be called 'y'.  If we can accurately predict when y flips from being the smaller number in the pair to being the larger number in the pair, then we know when we can stop iterating.  So, let's do a little experiment using a non-prime number so that the results will be easy to work with.  If we run through this process with x = 16, and assuming we don't stop when it's divisible since we're just concerned about the pairs, it looks like this:

1. If y = 2, the result of x/y = 8, so our pair is (2, 8).  y is smaller.
2. If y = 3, the result of x/y = 5.333..., so our pair is (3, 5.333...).  y is smaller.
3. If y = 4, the result of x/y = 4, so our pair is (4, 4).  y and x/y are equal.
4. If y = 5, the result of x/y = 3.2, so our pair is (5, 3.2).  x/y is smaller.

Clearly we flipped at y = 4.  And that's significant because 4 is the square root of 16.  Now let's try this with a known prime to see if it continues to work.  We'll use x = 17 this time.

1. If y = 2, the result of x/y = 8.5, so our pair is (2, 8.5).  y is smaller.
2. If y = 3, the result of x/y = 5.666..., so our pair is (3, 5.666...).  y is smaller.
3. If y = 4, the result of x/y = 4.25, so our pair is (4, 4.25).  y is smaller.
4. If y = 5, the result of x/y = 3.4, so our pair is (5, 3.4).  x/y is smaller.

The square root of 17 is ~4.123, which is between 4 and 5, so it makes sense that it would flip between 4 and 5.  Further, 17 isn't divisible by 2, 3, 4, so we can confidently say that 17 is prime after iterating only 3 times, rather than the 15 times we would have iterated based on codecademy's instructions.  We've already improved efficiency a lot.

But I wasn't finished, because I realized something else as I was looking at this:  there's no reason to check if x is divisible by 4 if we've already determined x is not divisible by 2.  Why is this?  Well, 4 is a composite number (that is, not prime).  And every composite number is composed of 2 or more prime factors.

- 4 = 2 * 2
- 6 = 2 * 3
- 8 = 2 * 2 * 2
- 9 = 3 * 3
- 10 = 2 * 5

You get the picture.  More importantly, because composite numbers are multiples of these prime factors, then that means that if x is not divisible by any of the prime factors of a composite number, then it follows that x is not divisible by the composite number itself.  And since the prime factors of a composite number are always less than the composite number itself, we will have already checked all the prime factors of a composite number before our loop reaches the composite number.  So there will never be a need to check whether or not x is divisible by any composite number.  Which means, our loop only needs to check x against prime numbers.  Going back to our x = 17 example, we get this:

1. If y = 2, the result of x/y = 8.5, so x is not divisible by y.
2. If y = 3, the result of x/y = 5.666..., so x is not divisible by y.
3. We don't need to check y = 4 because 4 is not prime.
4. We don't need to check y >= 5 because sqrt(17) < 5.

So now we've determined that 17 is prime after checking divisibility by only 2 and 3, rather than having to go all the way through 16.  That's an 86.67% boost in efficiency.
### End Math
<br/>

So we now have our 2 mathematical rules that we can apply to the code:
1. We do not need to iterate past sqrt(input).
2. We only need to check divisibility by primes.

## The Primes1 Class
Given the two mathematical rules I found, I knew rule #2 was going to be the tricky one.  Determining square root is easy; there's already a built in method for that.  But for the method to only check divisibility by primes, then that meant we first needed to create a list of primes.  To do this, I created a constructor for the class that would generate that list up to a hard-coded maximum value:

```
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
```

There are a few things going on here.  First, I had to seed the list with the first prime (which is 2); if the list had no entries, then `isPrime` wouldn't be able to check divisibility by anything.  Second, I set the maximum length of the list to 10 million; this is probably excessive, but I wanted to see how it performed with very large numbers.  Given that rule #1 says we don't have to iterate past the square root of an input value, this means that the largest prime the class would be able to determine would be 10 million squared, which is 100 trillion.  Making a list of every prime from 2 through 10 million does lead to some efficiency issues at initialization, but I'll be discussing that more later.

For its part, `isPrime` is both called by the constructor and can be called from other programs.  That method looks like this:

```
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
```

Since, in the constructor, the list is only seeded with 2 and the loop starts at `i = 3`, the first condition will never trip during construction.  However, if `isPrime` is called from another program after the constructor has run, it may be that the passed value is already on the list.  Since we have our handy list of primes, there's no reason to iterate through a divisibility check if the passed value is less than the largest prime on our list.  Likewise, if the number is greater than `maxPrime` (10 trillion, per above), the class cannot determine conclusively if it is prime, so we immediately return -1.  Otherwise, we run through our check, checking divisibility by all the primes on the list (per rule #2) until we reach the square root of the passed value (per rule #1), at which time if the passed value was not divisible by anything, we can confidently say it is prime.

I also went ahead and created a `getNextPrime` method for finding primes.  This is fairly simple and straitforward:

```
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
```

## Drawbacks of Primes1
Although initial tests did indicate that `Primes1` was *blazing fast* (as you'll see Testing and Conclusions section), I did realize there were 2 big drawbacks:
1. The list of primes takes up a fair bit of memory.
2. Although once the list is generated `Primes1` is incredibly efficient, it does take a few seconds to generate the list in the first place.

Issue #2 could be mitigated if a list of primes were stored to a record, rather than algorithmically generated every time, although there would still be some delay for server communications.  However, issue #1 is pretty much unavoidable with this version of the `Primes` class.  In some contexts, this extra RAM requirement would be inconsequential, but in others it could be important.  I realized that if I could get similar efficiency results without making a list of primes, I would have an ideal solution.

## Primes2

### Math
Maybe instead of only checking divisibility by primes, I could get similar results by only discounting *some* composite numbers, particularly those that can be easily determined to be composite.  For instance, it would be easy to check divisibility by odd numbers, and exclude all even numbers (except 2).  Theoretically, that would take ~50% as long as checking divisibility by all integers.

What if I removed multiples of 3 as well?  Well, it's bit trickier to see how much of a difference that would make.  Just removing multiples of 3 (and not multiples of 2) would remove ~1/3 of the list.  However, half of all multiples of 3 are also multiples of 2 (3[no], 6[yes], 9[no], 12[yes],15[no], 18[yes], etc.), so if we're already removing multiples of 2, then there's some overlap.  The actual benefit of removing multiples of 3 after removing multiples of 2 is:

1/3 * 1/2 = 1/6

So if we remove all multiples of 2 and 3 from the list, then our list is:

1 - (1/2 + 1/6) = 1/3

So, theoretically, removing multiples of both 2 and 3 should result in the process taking ~33% as long as checking divisibility by all integers.  This means there's diminishing returns on this process.

How much good does it do to remove multiples of 5?  Well, 2/3 of multiples of 5 are also multiples of 2 and/or 3 (5[no], 10[yes], 15[yes], 20[yes], 25[no] 30[yes]), so removing 5's only yields:

1/5 * 1/3 = 1/15

Which, when combined with the benefit of removing 2's and 3's:

1 - (1/2 + 1/6 + 1/15) = 4/15

So removing multiples of 5 didn't do us a whole lot of good.  It's impossible to tell how much less efficient this would be than only checking divisibility by primes since that will depend on how large the value we're checking against is.  However, given how little good removing multiples of 5 did, I think it's reasonable to only try removing multiples of 2 and 3.
### End Math
<br/>

So instead of making a list, we can get a decent amount of efficiency improvement by removing multiples of 2 and 3 from our list of potential factors.  This can be done without creating a list of primes.  First, I created `Primes2`, which only checks divisibility by odd numbers and 2.

```
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
```

`Primes2` no longer requires a list of primes, which means it also doesn't have a constructor.  However, there are some edge cases that need to be accounted for.  Also, I've set `maxPrime` to 10 trillion to make it equivalent to `Primes1`.  `maxPrime` can be any value, or even open-ended, so long as it's not too large for a long to hold.

The loop now adds 2 on every iteration rather than 1, because we're skipping all multiples of 2.

## Primes3
`Primes3` was tricky, because I had to figure out a way to skip both multiples of 2 and 3.  I knew if I just checked on every iteration whether or not `i` was divisible by 3, that would just mean extra mathematical checks on every iteration, and wouldn't improve efficiency.  Instead, I needed to come up with some sort of algorithmic way to know if a number was divisible by 3, without actually checked `i % 3`.  Ideally, I would find some mathematical formula to use in the iterator to make it skip multiples of 3.  First I had to figure out how much it needed to iterate:

1. From 3 to 5 skips 2.
2. From 5 to 7 skips 2.
3. From 7 to 11 skips 4.
4. From 11 to 13 skips 2.
5. From 13 to 17 skips 4.
6. From 17 to 19 skips 2.

So we've established a pattern of adding 2, then 4, then 2, then 4, and we have to start the loop at 5.  Unfortunately, I know of no way to have the iterator oscillate between +2 and +4 (if anyone has any ideas, please share).  What I decided to do instead was set an oscillating flag, and then when the flag was set I'd add an extra 2 to the iterator.  I knew this was an extra calculation on each iteration of the loop, but I was hoping it would still be more efficient than only skipping multiples of 2 (see Testing and Conclusions section to see how that turned out).  Here's what that looks like:

```
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
```

As with `Primes2`, `Primes3` has some edge cases that need to be accounted for, and `maxPrime` is again set to 10 trillion.  The primary difference between `Primes2` and `Primes3` is the `skipNext` flag and its handling.  On every other iteration through the loop, the `skipNext` flag will be set, and it will not be set on the intervening iterations.  This results in the +2/+4 oscillation that we were looking for, and successfully omits all multiples of 2 and 3 without ever checking `i % 2` or `i % 3`.

## Primes4
At this point, I had created both my original idea for efficient prime detection (`Primes1`) as well as two possible solutions that address issues with `Primes1` (`Primes2` and `Primes3`).  However, I had no control case to compare to.  For this reason I created `Primes4`, which simply iterates through all integers from 2 through `sqrt(input)`.

```
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
```

`Primes4` is nearly identical to `Primes2`, except it does not skip even numbers.

## Testing
Now that I had my 4 `Primes` classes, it was time to test them.  I wanted to test 2 things:  accuracy, and efficiency.  For accuracy tests, I made sure the results of both `isPrime` and `getNextPrime` were as expected for all passed values between -1 and 50.  This hits all edge cases and sufficiently covers normal cases so that we can confidently say the classes work as expected.  Here's the code I used for that, which uses the MSTest framework:

```
    //**************
    //Accuracy Tests
    //**************

    //isPrime

    [DataTestMethod]
    [DynamicData(nameof(isPrimeData), DynamicDataSourceType.Method)]
    public void isPrime1(long testVal, int expected)
    {
        int actual = Primes1.isPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(isPrimeData), DynamicDataSourceType.Method)]
    public void isPrime2(long testVal, int expected)
    {
        int actual = Primes2.isPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(isPrimeData), DynamicDataSourceType.Method)]
    public void isPrime3(long testVal, int expected)
    {
        int actual = Primes3.isPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(isPrimeData), DynamicDataSourceType.Method)]
    public void isPrime4(long testVal, int expected)
    {
        int actual = Primes4.isPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    //getNextPrime

    [DataTestMethod]
    [DynamicData(nameof(getNextPrimeData), DynamicDataSourceType.Method)]
    public void getNextPrime1(long testVal, long expected)
    {
        long actual = Primes1.getNextPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(getNextPrimeData), DynamicDataSourceType.Method)]
    public void getNextPrime2(long testVal, long expected)
    {
        long actual = Primes2.getNextPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(getNextPrimeData), DynamicDataSourceType.Method)]
    public void getNextPrime3(long testVal, long expected)
    {
        long actual = Primes3.getNextPrime(testVal);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(getNextPrimeData), DynamicDataSourceType.Method)]
    public void getNextPrime4(long testVal, long expected)
    {
        long actual = Primes4.getNextPrime(testVal);
        Assert.AreEqual(expected, actual);
    }


    //**********
    //Data Lists
    //**********

    private static IEnumerable<object[]> isPrimeData()
    {
        //Create a list of integers from -1 to 50 and their expected isPrime results.
        return new[]
        {
            new object[] {-1, 0},
            new object[] {0, 0},
            new object[] {1, 0},
            new object[] {2, 1},
            new object[] {3, 1},
            new object[] {4, 0},
            new object[] {5, 1},
            new object[] {6, 0},
            new object[] {7, 1},
            new object[] {8, 0},
            new object[] {9, 0},
            new object[] {10, 0},
            new object[] {11, 1},
            new object[] {12, 0},
            new object[] {13, 1},
            new object[] {14, 0},
            new object[] {15, 0},
            new object[] {16, 0},
            new object[] {17, 1},
            new object[] {18, 0},
            new object[] {19, 1},
            new object[] {20, 0},
            new object[] {21, 0},
            new object[] {22, 0},
            new object[] {23, 1},
            new object[] {24, 0},
            new object[] {25, 0},
            new object[] {26, 0},
            new object[] {27, 0},
            new object[] {28, 0},
            new object[] {29, 1},
            new object[] {30, 0},
            new object[] {31, 1},
            new object[] {32, 0},
            new object[] {33, 0},
            new object[] {34, 0},
            new object[] {35, 0},
            new object[] {36, 0},
            new object[] {37, 1},
            new object[] {38, 0},
            new object[] {39, 0},
            new object[] {40, 0},
            new object[] {41, 1},
            new object[] {42, 0},
            new object[] {43, 1},
            new object[] {44, 0},
            new object[] {45, 0},
            new object[] {46, 0},
            new object[] {47, 1},
            new object[] {48, 0},
            new object[] {49, 0},
            new object[] {50, 0}
        };
    }

    private static IEnumerable<object[]> getNextPrimeData()
    {
        //Create a list of integers from -1 to 50 and their expected getNextPrime results.
        return new[]
        {
            new object[] {-1, 2},
            new object[] {0, 2},
            new object[] {1, 2},
            new object[] {2, 2},
            new object[] {3, 3},
            new object[] {4, 5},
            new object[] {5, 5},
            new object[] {6, 7},
            new object[] {7, 7},
            new object[] {8, 11},
            new object[] {9, 11},
            new object[] {10, 11},
            new object[] {11, 11},
            new object[] {12, 13},
            new object[] {13, 13},
            new object[] {14, 17},
            new object[] {15, 17},
            new object[] {16, 17},
            new object[] {17, 17},
            new object[] {18, 19},
            new object[] {19, 19},
            new object[] {20, 23},
            new object[] {21, 23},
            new object[] {22, 23},
            new object[] {23, 23},
            new object[] {24, 29},
            new object[] {25, 29},
            new object[] {26, 29},
            new object[] {27, 29},
            new object[] {28, 29},
            new object[] {29, 29},
            new object[] {30, 31},
            new object[] {31, 31},
            new object[] {32, 37},
            new object[] {33, 37},
            new object[] {34, 37},
            new object[] {35, 37},
            new object[] {36, 37},
            new object[] {37, 37},
            new object[] {38, 41},
            new object[] {39, 41},
            new object[] {40, 41},
            new object[] {41, 41},
            new object[] {42, 43},
            new object[] {43, 43},
            new object[] {44, 47},
            new object[] {45, 47},
            new object[] {46, 47},
            new object[] {47, 47},
            new object[] {48, 53},
            new object[] {49, 53},
            new object[] {50, 53}
        };
    }
```

All of these tests passed, so all 4 classes work as expected.  For the efficiency tests, I ran the following tests:

```
    //****************
    //Efficiency Tests
    //****************

    //My results:
    //Initialize Primes1: 8.7 sec
    //Primes1: 2.1 sec (Note: to get an accurate reading for this, the constructor must first be run, so don't ignore isPrime1.)
    //Primes2: 9.2 sec
    //Primes3: 7.4 sec
    //Primes4: 19.8 sec

    [TestMethod]
    public void primes1Efficiency()
    {
        long toTest = 90000000000000;
        for (int i = 0; i < 100; i++)
        {
            toTest = Primes1.getNextPrime(toTest) + 1;
        }
    }

    [TestMethod]
    public void primes2Efficiency()
    {
        long toTest = 90000000000000;
        for (int i = 0; i < 100; i++)
        {
            toTest = Primes2.getNextPrime(toTest) + 1;
        }
    }

    [TestMethod]
    public void primes3Efficiency()
    {
        long toTest = 90000000000000;
        for (int i = 0; i < 100; i++)
        {
            toTest = Primes3.getNextPrime(toTest) + 1;
        }
    }

    [TestMethod]
    public void primes4Efficiency()
    {
        long toTest = 90000000000000;
        for (int i = 0; i < 100; i++)
        {
            toTest = Primes4.getNextPrime(toTest) + 1;
        }
    }
```

The efficiency tests were pretty strenuous, as I had it find the next 100 primes after 90 trillion.  Even so, all 4 classes performed this task fairly quickly.  The results of my most recent test (which was pretty typical) are as follows:

- `Primes1`: 2.1 sec
- `Primes2`: 9.2 sec
- `Primes3`: 7.4 sec
- `Primes4`: 19.8 sec

Note that this is only true of `Primes1` if we omit the time spent initializing the list, which on this test was 8.7 sec.

## Conclusions
For functionality, all 4 classes work exactly the same.  The only reason to prefer one class over another comes down to efficiency.  On the matter of efficiency, all 4 classes can find and verify primes quickly enough that for most practical purposes there is no significant difference between `Primes2-4`, although `Primes1` has an initialization cost.  If you are working on a program that needs to find and/or verify lots of large primes, then `Primes3` is your best bet, since it is slightly faster than `Primes2` without having the drawbacks of `Primes1`.  `Primes1` is lightning fast once it's initialized, but because of the extra memory and processing needed for initialization, it really isn't practical unless you're looking for extremely large primes (perhaps on the order of 10<sup>15</sup> or larger).  So, in summary:

- `Primes1` - not practical for general use, but would be useful when searching for primes in the 10<sup>15</sup>+ range.
- `Primes2` - good efficiency with few drawbacks, but not as efficient as `Primes3`.
- `Primes3` - best for general use.
- `Primes4` - fine for general use, but much less efficient than `Primes2` and `Primes3`.