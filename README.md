# Primes Class

## Description:
These are standalone C# class that can be used to find prime numbers and determine primacy of a passed value.  All 4 classes fulfill the same methods, but there are efficiency differences between them.  See the Methodology and Conclusions file for more information on how the classes perform compared to one another.

## Features:
Each of the four Primes classes has 2 methods:
- isPrime:
	- Description: this method determines whether or not the passed value is prime.
	- Accepted data types: long
	- Return values:
		- 0 - indicates the passed value is not prime
		- 1 - indicates the passed value is prime
		- -1 - indicates the passed value was too large to determine primacy. There are hard-coded values in the code that determine the maximum possible primes that can be found by these classes; these values can be altered, but larger maximum values can lead to decreased efficiency.
- getNextPrime:
	- Description: this method returns the next prime number greater than or equal to the passed value.
	- Accepted data types: long
	- Return values:
		- long value corresponding to the next prime number
		- -1 - indicates the next prime was too large for the class to determine primacy

## How to use:
Simply add the file with the Primes classes as a dependency on any C# project.  You may want to remove any of the Primes classes that do not fit your needs, and/or rename them as you see fit.  I recommend Primes3 for general use, but Primes1 may be better in some cases.  Primes2 and Primes4 are generally less efficient, but I have kept them for comparison purposes.  See Methodology and Conclusions file for more details.

## Technologies:
I used Microsoft Visual Studio to write the Primes classes, and the MSTest framework for testing.  Git Bash, in conjunction with GitHub, were used for version control.

## Collaborators:
- Travis Hayes

## License:
This project is licensed under the GNU General Public License, version 3.