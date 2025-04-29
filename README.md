# OS-Project-2
A project that entailed the implementation of different CPU Scheduling Algorithms

The algorithms that I implemented where First Come First Served, Shortest Job First, Priority, Round Robin, Shortest Job Remaining, and Longest Job Remaining.
Instead of just ripping the base code for the first four algorithms I decided to start from scratch and implement them and the last two via a process class. Said class is then used to create various different processes with differing burst/arrival times and priorities, with those processes getting fed theough a series of while and foreach loops that, depending on the algorith they are run through, will go and work through all of the processes.

In order to run this project all you have to do is build it, add the specific algorithm method you'd like to use, and add in the arguments which in order are, number of processes, minimum burst time, maximum burst time, minimum arrival time, maximum arrival time, minimum priority, and maximum priority. 
