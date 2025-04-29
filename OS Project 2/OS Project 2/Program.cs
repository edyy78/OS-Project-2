using System.Diagnostics;

namespace OS_Project_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //for(int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("Slot Num: " + (i + 1));
            //    Console.WriteLine();
            //    ShortestJobRemaining(5, 2, 5, 0, 4, 0, 0);
            //    Console.ReadLine();
            //}
            
            //Low Capasity Tests
            //FirstComeFirstServe(5, 2, 5, 0, 4, 0, 0);
            //ShortestJobFirst(5, 2, 5, 0, 4, 0, 0);
            //Priority(5, 2, 5, 0, 4, 1, 4);
            //RoundRobin(5, 2, 5, 0, 4, 0, 0, 2);
            ShortestJobRemaining(5, 2, 5, 0, 4, 0, 0);
            //LongestJobRemaining(5, 2, 5, 0, 4, 0, 0);

            //High Capasity Tests
            //FirstComeFirstServe(50, 2, 5, 0, 4, 0, 0);
            //ShortestJobFirst(50, 2, 5, 0, 4, 0, 0);
            //Priority(50, 2, 5, 0, 4, 1, 4);
            //RoundRobin(50, 2, 5, 0, 4, 0, 0, 2);
            //ShortestJobRemaining(50, 2, 5, 0, 4, 0, 0);
            //LongestJobRemaining(50, 2, 5, 0, 4, 0, 0);

            //Clone Process Tests
            //FirstComeFirstServe(5, 5, 5, 0, 0, 0, 0);
            //ShortestJobFirst(5, 5, 5, 0, 0, 0, 0);
            //Priority(5, 5, 5, 0, 0, 1, 4);
            //RoundRobin(5, 5, 5, 0, 0, 0, 0, 2);
            //ShortestJobRemaining(5, 5, 5, 0, 0, 0, 0);
            //LongestJobRemaining(5, 5, 5, 0, 0, 0, 0);

            //Long And Short Bursts Tests
            //FirstComeFirstServe(5, 1, 10, 0, 4, 0, 0);
            //ShortestJobFirst(5, 1, 10, 0, 4, 0, 0);
            //Priority(5, 1, 10, 0, 4, 1, 4);
            //RoundRobin(5, 1, 10, 0, 4, 0, 0, 2);
            //ShortestJobRemaining(5, 1, 10, 0, 4, 0, 0);
            //LongestJobRemaining(5, 1, 10, 0, 4, 0, 0);
        }

        static void FirstComeFirstServe(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            Console.WriteLine("First Come First Serve Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>();
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int totalTime = 0;


            for(int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown);
            }

            while(!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach(Process process in unarrivedProcesses.ToList<Process>())
                {
                    if(process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                //Checks Processes That have arrived
                foreach(Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    if(!processIsWorking && process.burstCountdown > 0)
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Aren't Finished
                    else if(processIsWorking && process.burstCountdown > 0)
                    {
                        isdone = false;
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }
                //Removes Processes From arrivedProcesses If They Are Done And Moves Them To doneProcesses
                if(currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    doneProcesses.Add(arrivedProcesses[0]);
                    arrivedProcesses.RemoveAt(0);
                    arrivedProcesses.RemoveAll(item => item == null);
                }

                Console.WriteLine("Arrived Processes");
                for (int i = 0; i < arrivedProcesses.Count; i++)
                {
                    Console.WriteLine("P" + arrivedProcesses[i].name);
                }

                Console.WriteLine("Done Processes");
                for (int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                //Waits Untill All Processes Have Entered doneProcesses Before Saying That The Algorithm Is Done
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
                totalTime++;
            }
            for(int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Time: " + totalTime);
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with FCFS is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with FCFS is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with FCFS is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes with FCFS is " + averageTurnaroundTime + " secs");


        }

        static void ShortestJobFirst(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            Console.WriteLine("Shortest Job First Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>();
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int shortestProcessPos = 0;


            for (int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown);
            }

            while (!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach (Process process in unarrivedProcesses.ToList<Process>())
                {
                    if (process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                //Finds the Position Of The Process With The Shortest Burst Time That Isn't Zero
                if(!processIsWorking)
                {
                    if(arrivedProcesses.Count == 1)
                    {
                        shortestProcessPos = 0;
                    }
                    else
                    {
                        shortestProcessPos = 0;
                        for (int i = 0; i < arrivedProcesses.Count - 1; i++)
                        {
                            Console.WriteLine("I is: " + i + " Pos Of Shortest: " + shortestProcessPos);
                            if (arrivedProcesses[shortestProcessPos].burstCountdown == 0)
                            {
                                shortestProcessPos++;
                            }
                            else if (arrivedProcesses[i+1].burstCountdown < arrivedProcesses[shortestProcessPos].burstCountdown && arrivedProcesses[i+1].burstCountdown != 0)
                            {
                                shortestProcessPos = i+1;
                            }
                        }
                    }
                    
                }
                

                //Checks Processes That have arrived
                foreach (Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    //Modified To Check Whether The Current Porcess Being Looked Over Is The Same As The One Singled Out To Have The Shortest Remaining Burst Time
                    if (!processIsWorking && process.burstCountdown > 0 && shortestProcessPos == arrivedProcesses.IndexOf(process))
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Haven't Started
                    //Modified With (processIsWorking || !processIsWorking) To Detect Processes That Where Skipped Over Due To No Currently Having The Shortest Remaining Time While processIsWorking Is False
                    else if ((processIsWorking || !processIsWorking) && process.burstCountdown > 0)
                    {
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }

                if (currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    Console.WriteLine("Pos Of Current Working: " + shortestProcessPos);
                    doneProcesses.Add(arrivedProcesses[shortestProcessPos]);
                    arrivedProcesses.RemoveAt(shortestProcessPos);
                    arrivedProcesses.RemoveAll(item => item == null);
                }


                Console.WriteLine("Arrived Processes");
                for (int i = 0; i < arrivedProcesses.Count; i++)
                {
                    Console.WriteLine("P" + arrivedProcesses[i].name);
                }

                Console.WriteLine("Done Processes");
                for (int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
            }

            for (int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with SJF is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with SJF is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with SJF is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes with SJF is " + averageTurnaroundTime + " secs");
        }

        static void Priority(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            Console.WriteLine("Priority Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>();
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int highestPriorityPos = 0;
            int currentlyWorkingPos = 0;


            for (int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown + " Priority: " + unarrivedProcesses[i].priority);
            }
            Console.ReadLine();
            while (!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach (Process process in unarrivedProcesses.ToList<Process>())
                {
                    if (process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                if (arrivedProcesses.Count == 1)
                {
                    highestPriorityPos = 0;
                }
                else
                {
                    highestPriorityPos = 0;
                    for (int i = 0; i < arrivedProcesses.Count - 1; i++)
                    {
                        Console.WriteLine("I is: " + i + " Pos Of Highest Priority: " + highestPriorityPos);

                        if (arrivedProcesses[highestPriorityPos].burstCountdown == 0)
                        {
                            highestPriorityPos++;
                        }
                        else if (arrivedProcesses[i + 1].priority > arrivedProcesses[highestPriorityPos].priority && arrivedProcesses[i + 1].burstCountdown != 0)
                        {
                            highestPriorityPos = i + 1;
                        }
                    }

                    if(currentlyWorkingPos != highestPriorityPos)
                    {
                        arrivedProcesses[highestPriorityPos].isWorking = true;
                        arrivedProcesses[currentlyWorkingPos].isWorking = false;
                        currentlyWorkingPos = highestPriorityPos;
                    }
                }

                //Checks Processes That have arrived
                foreach (Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    if (!processIsWorking && process.burstCountdown > 0)
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Aren't Finished
                    else if (processIsWorking && process.burstCountdown > 0)
                    {
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }

                if (currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    Console.WriteLine("Pos Of Current Working: " + currentlyWorkingPos);
                    doneProcesses.Add(arrivedProcesses[currentlyWorkingPos]);
                    arrivedProcesses.RemoveAt(currentlyWorkingPos);
                    arrivedProcesses.RemoveAll(item => item == null);
                }

                
                Console.WriteLine("Arrived Processes");
                for (int i = 0; i < arrivedProcesses.Count; i++)
                {
                    Console.WriteLine("P" + arrivedProcesses[i].name);
                }

                Console.WriteLine("Done Processes");
                for (int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                //Waits Untill All Processes Have Entered doneProcesses Before Saying That The Algorithm Is Done
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
                //Console.ReadLine();
            }
            for (int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with Priority is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with Priority is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with Priority is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes Priority FCFS is " + averageTurnaroundTime + " secs");
        }

        static void RoundRobin(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority, int quantumTime)
        {
            Console.WriteLine("Round Robin Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>(); //Holds Processes That Are Finished
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int quatumCountdown = quantumTime;


            for (int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown);
            }
            Console.ReadLine();

            while (!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach (Process process in unarrivedProcesses.ToList<Process>())
                {
                    if (process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                if(quatumCountdown == 0)
                {
                    Console.WriteLine("Got Inside Zero Quantum");
                    quatumCountdown = quantumTime;
                    if (processIsWorking)
                    {
                        if(arrivedProcesses.Count() > 1)
                        {
                            processIsWorking = false;
                            Console.WriteLine("Process Queue Before");
                            for (int i = 0; i < arrivedProcesses.Count; i++)
                            {
                                Console.WriteLine("P" + arrivedProcesses[i].name);
                            }
                            //Console.WriteLine("Process At Start: Process " + arrivedProcesses[0].name);
                            //Console.WriteLine("Next Process: Process " + arrivedProcesses[1].name);
                            arrivedProcesses[0].isWorking = false;
                            arrivedProcesses.Add(arrivedProcesses[0]);
                            arrivedProcesses.RemoveAt(0);
                            arrivedProcesses.RemoveAll(item => item == null);
                            Console.WriteLine("Process Queue After");
                            for (int i = 0; i < arrivedProcesses.Count; i++)
                            {
                                Console.WriteLine("P" + arrivedProcesses[i].name);
                            }
                            //Console.WriteLine("Old First Process New Loc: Process " + arrivedProcesses[arrivedProcesses.Count - 1].name + " at pos " + (arrivedProcesses.Count - 1));
                            //Console.WriteLine("New Process Working: Process " + arrivedProcesses[0].name);
                        }
                    }   
                    
                }
                else if(quantumTime > 0 && arrivedProcesses.Count > 0 && arrivedProcesses[0].burstCountdown == 0)
                {
                    quatumCountdown = quantumTime;
                    Console.WriteLine("Process Queue Before");
                    for(int i = 0; i < arrivedProcesses.Count; i++)
                    {
                        Console.WriteLine("P" + arrivedProcesses[i].name);
                    }
                    //Console.WriteLine("Process At Start: Process " + arrivedProcesses[0].name);
                    //Console.WriteLine("Next Process: Process " + arrivedProcesses[1].name);
                    arrivedProcesses[0].isWorking = false;
                    arrivedProcesses.Add(arrivedProcesses[0]);
                    arrivedProcesses.RemoveAt(0);
                    arrivedProcesses.RemoveAll(item => item == null);
                    Console.WriteLine("Process Queue After");
                    for (int i = 0; i < arrivedProcesses.Count; i++)
                    {
                        Console.WriteLine("P" + arrivedProcesses[i].name);
                    }
                    //Console.WriteLine("Old First Process New Loc: Process " + arrivedProcesses[arrivedProcesses.Count - 1].name + " at pos " + (arrivedProcesses.Count - 1));
                    //Console.WriteLine("New Process Working: Process " + arrivedProcesses[0].name);
                }

                
                //Checks Processes That have arrived
                foreach (Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    if (!processIsWorking && process.burstCountdown > 0)
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        quatumCountdown--;
                        Console.WriteLine("New Quantum Time: " + quatumCountdown);
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Aren't Finished
                    else if (processIsWorking && process.burstCountdown > 0)
                    {
                        isdone = false;
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            quatumCountdown--;
                            Console.WriteLine("New Quantum Time: " + quatumCountdown);
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }

                if (currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    doneProcesses.Add(arrivedProcesses[0]);
                    arrivedProcesses.RemoveAt(0);
                    arrivedProcesses.RemoveAll(item => item == null);
                }

                
                //Waits Untill All Processes Have Arrived Before Checking If All Of Them Are Done
                Console.WriteLine("Done Processes");
                for(int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
                //Console.ReadLine();
            }
            for (int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with RR is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with RR is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with RR is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes with RR is " + averageTurnaroundTime + " secs");
        }

        static void ShortestJobRemaining(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            Console.WriteLine("Shortest Job Remaining Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>();
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int shortestProcessPos = 0;
            int currentlyWorkingPos = 0;
            int totalTime = 0;


            for (int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown);
            }

            while (!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach (Process process in unarrivedProcesses.ToList<Process>())
                {
                    if (process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                //Finds the Position Of The Process With The Shortest Burst Time That Isn't Zero And If It Isn't The Currently Working Process It Is Made To Be 
                if(arrivedProcesses.Count() == 1)
                {
                    shortestProcessPos = 0;
                    currentlyWorkingPos = 0;
                }
                else
                {
                    shortestProcessPos = 0;
                    currentlyWorkingPos = 0;
                    for (int i = 0; i < arrivedProcesses.Count - 1; i++)
                    {
                        if (arrivedProcesses[shortestProcessPos].burstCountdown == 0)
                        {
                            shortestProcessPos++;
                        }
                        else if (arrivedProcesses[i + 1].burstCountdown < arrivedProcesses[shortestProcessPos].burstCountdown && arrivedProcesses[i + 1].burstCountdown != 0)
                        {
                            shortestProcessPos = i + 1;
                        }
                    }
                    Console.WriteLine("Pos Of Current Working: " + currentlyWorkingPos);
                    if (currentlyWorkingPos != shortestProcessPos)
                    {
                        arrivedProcesses[shortestProcessPos].isWorking = true;
                        arrivedProcesses[currentlyWorkingPos].isWorking = false;
                        currentlyWorkingPos = shortestProcessPos;
                    }
                }
                


                //Checks Processes That have arrived
                foreach (Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    //Modified To Check Whether The Current Porcess Being Looked Over Is The Same As The One Singled Out To Have The Shortest Remaining Burst Time
                    if (!processIsWorking && process.burstCountdown > 0 && shortestProcessPos == arrivedProcesses.IndexOf(process))
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Haven't Started
                    //Modified With (processIsWorking || !processIsWorking) To Detect Processes That Where Skipped Over Due To No Currently Having The Shortest Remaining Time While processIsWorking Is False
                    else if ((processIsWorking || !processIsWorking) && process.burstCountdown > 0)
                    {
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }

                if (currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    Console.WriteLine("Pos Of Current Working: " + currentlyWorkingPos);
                    doneProcesses.Add(arrivedProcesses[currentlyWorkingPos]);
                    arrivedProcesses.RemoveAt(currentlyWorkingPos);
                    arrivedProcesses.RemoveAll(item => item == null);
                }

                Console.WriteLine("Arrived Processes");
                for (int i = 0; i < arrivedProcesses.Count; i++)
                {
                    Console.WriteLine("P" + arrivedProcesses[i].name);
                }

                Console.WriteLine("Done Processes");
                for (int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
                totalTime++;
            }

            for (int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Time: " + totalTime);
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with SJR is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with SJR is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with SJR is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes with SJR is " + averageTurnaroundTime + " secs");
        }

        static void LongestJobRemaining(int numberOfProcesses, int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            Console.WriteLine("Longest Job Remaining Scheduling");
            List<Process> unarrivedProcesses = new List<Process>();
            List<Process> arrivedProcesses = new List<Process>();
            List<Process> doneProcesses = new List<Process>();
            bool isdone = false;
            bool processIsWorking = false;
            bool currentProcessIsDone = false;
            float totalWaitTime = 0;
            float totalTurnaroundTime = 0;
            int longestProcessPos = 0;
            int currentlyWorkingPos = 0;
            int totalTime = 0;


            for (int i = 0; i < numberOfProcesses; i++)
            {
                unarrivedProcesses.Add(new Process(minBurst, maxBurst, minArrival, maxArrival, minPriority, maxPriority));
                unarrivedProcesses[i].name = i;
                Console.WriteLine("Process " + i + ": Burst Time: " + unarrivedProcesses[i].burstCountdown + " Arrival Time: " + unarrivedProcesses[i].arrivalCountdown);
            }
            while (!isdone)
            {
                //Checks Processes That Haven't Arrived Yet
                foreach (Process process in unarrivedProcesses.ToList<Process>())
                {
                    if (process.arrivalCountdown == 0)
                    {
                        arrivedProcesses.Add(process);
                        unarrivedProcesses.Remove(process);
                        unarrivedProcesses.RemoveAll(item => item == null);
                        Console.WriteLine("Process " + process.name + " has arrived");
                    }
                    else
                    {
                        Console.WriteLine("Process " + process.name + " has yet to arrive");
                        process.arrivalCountdown -= 1;
                        Console.WriteLine("New Arrival: " + process.arrivalCountdown);

                    }
                }

                //Finds the Position Of The Process With The Shortest Burst Time That Isn't Zero And If It Isn't The Currently Working Process It Is Made To Be 
                if (arrivedProcesses.Count() == 1)
                {
                    longestProcessPos = 0;
                    currentlyWorkingPos = 0;
                }
                else
                {
                    longestProcessPos = 0;
                    for (int i = 0; i < arrivedProcesses.Count - 1; i++)
                    {
                        if (arrivedProcesses[longestProcessPos].burstCountdown == 0)
                        {
                            longestProcessPos++;
                        }
                        else if (arrivedProcesses[i + 1].burstCountdown > arrivedProcesses[longestProcessPos].burstCountdown && arrivedProcesses[i + 1].burstCountdown != 0)
                        {
                            longestProcessPos = i + 1;
                        }
                    }
                    if (currentlyWorkingPos != longestProcessPos)
                    {
                        arrivedProcesses[longestProcessPos].isWorking = true;
                        arrivedProcesses[currentlyWorkingPos].isWorking = false;
                        currentlyWorkingPos = longestProcessPos;
                        Console.WriteLine(" Pos of Current Working: " + currentlyWorkingPos);
                    }
                }



                //Checks Processes That have arrived
                foreach (Process process in arrivedProcesses)
                {
                    //Picks The Next Process To Start Working If None Are Currently Doing So
                    //Modified To Check Whether The Current Porcess Being Looked Over Is The Same As The One Singled Out To Have The Longest Remaining Burst Time
                    if (!processIsWorking && process.burstCountdown > 0 && longestProcessPos == arrivedProcesses.IndexOf(process))
                    {
                        Console.WriteLine("Process " + process.name + " has started working");
                        processIsWorking = true;
                        process.isWorking = true;
                        process.burstCountdown -= 1;
                        Console.WriteLine("New Burst: " + process.burstCountdown);
                        if (process.burstCountdown <= 0)
                        {
                            currentProcessIsDone = true;
                            process.isWorking = false;
                            Console.WriteLine("Process " + process.name + " is done");
                        }
                    }
                    //If A Process Is Working Decriments The Currently Working Process And Increments The Wait Time On All That Haven't Started
                    //Modified With (processIsWorking || !processIsWorking) To Detect Processes That Where Skipped Over Due To No Currently Having The Shortest Remaining Time While processIsWorking Is False
                    else if ((processIsWorking || !processIsWorking) && process.burstCountdown > 0)
                    {
                        if (process.isWorking)
                        {
                            Console.WriteLine("Process " + process.name + " burst has gone down by one");
                            process.burstCountdown -= 1;
                            Console.WriteLine("New Burst: " + process.burstCountdown);
                            if (process.burstCountdown <= 0)
                            {
                                currentProcessIsDone = true;
                                process.isWorking = false;
                                Console.WriteLine("Process " + process.name + " is done");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Process " + process.name + " is waiting");
                            process.waitTime += 1;
                            Console.WriteLine("New Wait: " + process.waitTime);
                        }
                    }
                }

                if (currentProcessIsDone)
                {
                    processIsWorking = false;
                    currentProcessIsDone = false;
                    Console.WriteLine("Pos Of Current Working: " + currentlyWorkingPos);
                    doneProcesses.Add(arrivedProcesses[currentlyWorkingPos]);
                    arrivedProcesses.RemoveAt(currentlyWorkingPos);
                    arrivedProcesses.RemoveAll(item => item == null);
                }

                Console.WriteLine("Arrived Processes");
                for (int i = 0; i < arrivedProcesses.Count; i++)
                {
                    Console.WriteLine("P" + arrivedProcesses[i].name);
                }

                Console.WriteLine("Done Processes");
                for (int i = 0; i < doneProcesses.Count; i++)
                {
                    Console.WriteLine("P" + doneProcesses[i].name);
                }
                if (doneProcesses.Count == numberOfProcesses)
                {
                    isdone = true;
                }
                //Console.ReadLine();
                totalTime++;
            }

            for (int i = 0; i < numberOfProcesses; i++)
            {
                Console.WriteLine("Process " + doneProcesses[i].name + " Wait Time: " + doneProcesses[i].waitTime);
                totalWaitTime += doneProcesses[i].waitTime;

                doneProcesses[i].turnaroundTime = doneProcesses[i].waitTime + doneProcesses[i].burstTime;
                totalTurnaroundTime += doneProcesses[i].turnaroundTime;
                Console.WriteLine("Process " + doneProcesses[i].name + " Turn Around Time: " + doneProcesses[i].turnaroundTime);
            }

            float averageWaitTime = totalWaitTime / numberOfProcesses;
            float averageTurnaroundTime = totalTurnaroundTime / numberOfProcesses;
            Console.WriteLine("Total Time: " + totalTime);
            Console.WriteLine("Total Wait Time For " + numberOfProcesses + " processes with LJR is " + totalWaitTime + " secs");
            Console.WriteLine("Average Wait Time For " + numberOfProcesses + " processes with LJR is " + averageWaitTime + " secs");
            Console.WriteLine("Total Turnabout Time For " + numberOfProcesses + " processes with LJR is " + totalTurnaroundTime + " secs");
            Console.WriteLine("Average Turnaround Time For " + numberOfProcesses + " processes with LJR is " + averageTurnaroundTime + " secs");
        }
    }

    


    public class Process
    {
        public int name;
        public int burstTime;
        public int burstCountdown;
        public int arrivalTime;
        public int arrivalCountdown;
        public int turnaroundTime;
        public int priority;
        public int waitTime = 0;
        public bool isWorking;
        Random r = new Random();

        public Process(int minBurst, int maxBurst, int minArrival, int maxArrival, int minPriority, int maxPriority)
        {
            burstTime = r.Next(minBurst, maxBurst);
            burstCountdown = burstTime;
            arrivalTime = r.Next(minArrival, maxArrival);
            arrivalCountdown = arrivalTime;
            priority = r.Next(minPriority, maxPriority);
        }
    }
}
