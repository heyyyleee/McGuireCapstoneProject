using FinchAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Finch_Starter
{

    class Program
    {
        // *************************************************************
        // Application:     Finch Guessing Games
        // Author:          Hailey McGuire
        // Description:     An app where you guess the finch readings
        // Application Type: Console
        // Date Created:    03/31/2021
        // Date Revised:    04/18/2021
        // *************************************************************

        static void Main(string[] args)
        {
            // 
            // Create new Finch
            //
            Finch myFinch;
            myFinch = new Finch();

            //
            // Set theme
            //
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            WelcomeScreen();
            MainMenu();

            // 
            // Disconnect Finch
            //
            myFinch.disConnect();
        }

        #region USER INTERFACE AND SETTINGS

        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t" + headerText);
            Console.WriteLine();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine("\tPress any key to continue: ");
            Console.ReadKey();
        }

        private static void WelcomeScreen()
        {

            DisplayScreenHeader("Welcome to Your Finch Guessing Game Series");

            Console.WriteLine();
            Console.WriteLine("\tGet ready to get guessing!");
            Console.WriteLine();

            DisplayContinuePrompt();

        }

        private static void ClosingScreen()
        {
            Console.WriteLine();
            DisplayScreenHeader("Thank you for guessing!");

            DisplayContinuePrompt();
        }

        static bool ConnectFinchRobot(Finch myFinch)
        {
            bool robotConnected = true;

            DisplayScreenHeader("Connect Finch Robot");

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tConnecting the Finch robot now. Please be sure the USB cable is connected to the robot and computer.");
            Console.WriteLine();
            DisplayContinuePrompt();

            robotConnected = myFinch.connect();

            myFinch.setLED(0, 0, 0);
            myFinch.noteOff();

            return robotConnected;
        }

        static void MainMenu()
        {

            bool quit = false;
            string userInput;

            Console.Clear();

            Finch myFinch = new Finch();


            do
            {
                DisplayScreenHeader("Main Menu");

                Console.CursorVisible = true;

                //
                // Get user input 
                //
                Console.WriteLine();
                Console.WriteLine("\tPlease connect your Finch Robot and then select your game: ");
                Console.WriteLine();
                Console.WriteLine("\ta) Temperature Guessing Game");
                Console.WriteLine("\tb) Timer Guessing Game");
                Console.WriteLine("\tc) Random Number Guessing Game");
                Console.WriteLine("\td) My Scores");
                Console.WriteLine("\te) Connect Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        GuessTemperatureMenu(myFinch);
                        break;

                    case "b":

                        GuessTimerMenu(myFinch);
                        break;

                    case "c":

                        GuessRandomNumber();
                        break;

                    case "d":

                        ViewMyScoresMenu();
                        break;

                    case "e":
                        ConnectFinchRobot(myFinch);
                        break;

                    case "q":
                        quit = true;
                        ClosingScreen();
                        break;


                    // 
                    // Validate user input
                    //
                    default:
                        Console.WriteLine();
                        Console.Beep();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);
        }

        #endregion

        #region SCORES

        private static void ViewMyScoresMenu()
        {
            bool quit = false;
            string userInput;

            int[] allTimerScores = null;
            int[] TempScores = null;
            int[] allRanNumScores = null;

            Console.CursorVisible = true;

            //
            // Get user input
            //
            do
            {
                DisplayScreenHeader("High Scores Menu");
                Console.WriteLine();
                Console.WriteLine("\tSelect an option: ");
                Console.WriteLine();
                Console.WriteLine("\ta) Enter Scores for Temperature Guessing Game");
                Console.WriteLine("\tb) Enter Scores for Timer Guessing Game");
                Console.WriteLine("\tc) Enter Scores for Random Number Guessing Game");
                Console.WriteLine("\td) View My Scores");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        TempScores = TempHighScores();
                        break;

                    case "b":

                        allTimerScores = TimerScores();
                        break;

                    case "c":

                        allRanNumScores = RandomNumberScores();
                        break;

                    case "d":

                        ViewMyScores(allTimerScores, TempScores, allRanNumScores);
                        break;

                    case "q":
                        quit = true;
                        ClosingScreen();
                        break;

                    // 
                    // Validate user input
                    //
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);


        }

        private static void ViewMyScores(int[] allTimerScores, int[] tempScores, int[] allRanNumScores)
        {
            // 
            // Display all of the users best scores back on one screen
            //
            DisplayScreenHeader("Your High Scores");

            Console.WriteLine();
            Console.WriteLine("\tTemperature Guessing Game:");
            Console.WriteLine("\t--------------------------");
            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", tempScores[i].ToString());
            }
            Console.WriteLine();
            Console.WriteLine("\tTimer Guessing Game:");
            Console.WriteLine("\t--------------------------");
            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", allTimerScores[i].ToString());
            }
            Console.WriteLine();
            Console.WriteLine("\tRandom Number Guessing Game:");
            Console.WriteLine("\t--------------------------");
            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", allRanNumScores[i].ToString());
            }

            DisplayContinuePrompt();

        }

        #endregion

        #region TIMER GUESSING GAME

        static void GuessTimerMenu(Finch myFinch)
        {
            bool quit = false;
            string userInput;
            int userTimerGuess = 0;
            int timeToPlay = 0;

            ConnectFinchRobot(myFinch);

            Console.CursorVisible = true;

            //
            // Get user input
            do
            {
                DisplayScreenHeader("Guess Timer Menu");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\tFirst, listen to the sound. Then, enter your guess and find out how close you are to the length of the sound.");
                Console.WriteLine();
                Console.WriteLine("\ta) Hear the sound");
                Console.WriteLine("\tb) Enter your guess");
                Console.WriteLine("\tc) Get the answer");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        timeToPlay = PlayTimerSound(myFinch);
                        break;

                    case "b":

                        userTimerGuess = EnterTimerGuess();
                        break;

                    case "c":

                        GetTimerAnswer(userTimerGuess, timeToPlay);
                        break;


                    case "q":
                        quit = true;
                        MainMenu();
                        break;


                    // 
                    // Validate user input
                    //
                    default:
                        Console.WriteLine();
                        Console.Beep();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);

        }

        static int GetTimerAnswer(int userTimerGuess, int timeToPlay)
        {
            int timeDifference;
            int absTimeDifference = 0;


            //
            // Display user input again and show them the correct answer
            //
            DisplayScreenHeader("Final Timer Results");
            Console.WriteLine();
            Console.WriteLine("\tYour answer: {0}", userTimerGuess);
            Console.WriteLine();
            Console.WriteLine("\tThe correct answer: {0}", timeToPlay / 1000);
            Console.WriteLine();

            //
            // Tell the user how far they were from the correct answer - this is their score
            //

            if (userTimerGuess > timeToPlay / 1000)
            {

                timeDifference = (userTimerGuess) - (timeToPlay / 1000);
                absTimeDifference = Math.Abs(timeDifference);
                Console.WriteLine("\tYou were only {0} away", absTimeDifference);
            }
            else if (timeToPlay / 1000 > userTimerGuess)
            {

                timeDifference = (timeToPlay / 1000) - (userTimerGuess);
                absTimeDifference = Math.Abs(timeDifference);
                Console.WriteLine("\tYou were only {0} away", absTimeDifference);
            }
            else if (timeToPlay / 1000 == userTimerGuess)
            {
                Console.WriteLine();
                Console.WriteLine("\tGreat job, you are correct!");
            }

            Console.WriteLine();
            DisplayContinuePrompt();

            return absTimeDifference;
        }

        static int EnterTimerGuess()
        {
            bool parseSuccess = false;
            int userTimerGuess;

            Console.CursorVisible = true;

            //
            // Get the user's guess
            //
            DisplayScreenHeader("Enter Your Guess");
            Console.WriteLine();
            Console.Write("\tYour guess of length [in seconds]: ");

            //
            // Validate the input
            //
            parseSuccess = int.TryParse(Console.ReadLine(), out userTimerGuess);

            if (parseSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("\tYour guess is {0}", userTimerGuess);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tPlease enter an integer.");
            }

            DisplayContinuePrompt();
            return userTimerGuess;
        }

        static int PlayTimerSound(Finch myFinch)
        {
            //
            // Get a random integer and convert it to seconds and play a tone for the specified length
            // Return that value as the correct answer
            //
            Random random = new Random();
            int timeToPlay = random.Next(1000, 10000);

            Console.WriteLine();
            Console.WriteLine("\tListen to the sound your Finch robot makes and try to guess how long it is.");
            myFinch.noteOn(400);
            myFinch.wait(timeToPlay);
            myFinch.noteOff();

            Console.WriteLine();
            DisplayContinuePrompt();

            return timeToPlay;
        }

        static int[] TimerScores()
        {
            DisplayScreenHeader("Timer Guessing Game Scores");

            //
            // Get user high scores and display them back in decending order
            // Store scores in an array and send to the high score menu
            //

            int[] allTimerScores = new int[3];

            Console.CursorVisible = true;

            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Console.Write("\tEnter your three best scores: ");
                allTimerScores[i] = int.Parse(Console.ReadLine());
            }

            Array.Sort(allTimerScores);
            Array.Reverse(allTimerScores);
            Console.WriteLine();
            Console.WriteLine("\tYour High Scores");
            Console.WriteLine("\t------------------");

            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", allTimerScores[i].ToString());
            }


            DisplayContinuePrompt();

            return allTimerScores;
        }



        #endregion

        #region NUMBER GUESSING GAME

        static int GuessRandomNumber()
        {
            //
            // Create new random number from 1-100
            //

            Random random = new Random();
            int winNumber = random.Next(0, 100);
            bool win = false;
            int count = 0;
            bool parseSuccess;

            DisplayScreenHeader("Number Guessing Game");

            //
            // Get user input - Parse against random number and give user feedback to guess the number
            //
            do
            {
                Console.CursorVisible = true;

                Console.WriteLine();
                Console.Write("\tGuess a number between 10-100: ");
                int answer;
                parseSuccess = int.TryParse(Console.ReadLine(), out answer);

                if (parseSuccess)
                {

                    if (answer > winNumber)
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tThat is too high! Guess a lower number.");
                    }
                    else if (answer < winNumber)
                    {
                        Console.WriteLine();
                        Console.WriteLine("\t That is too low. Guess a higher number.");
                    }

                    //
                    // Display the count of how many tries it took the user to get the correct number
                    //
                    else if (answer == winNumber)
                    {
                        ++count;
                        Console.WriteLine();
                        Console.WriteLine("\tYou guessed correctly in {0} tries!", count);
                        DisplayContinuePrompt();
                        win = true;
                    }
                    ++count;

                }
                // 
                // Validating input
                //
                else
                {
                    Console.WriteLine();
                    Console.Beep();
                    Console.WriteLine("\tPlease enter an integer.");
                }

            } while (win == false);

            return count;


        }

        static int[] RandomNumberScores()
        {
            DisplayScreenHeader("Random Number Guessing Game Scores");

            //
            // Create array for top 3 scores
            //

            int[] allRanNumScores = new int[3];

            Console.CursorVisible = true;

            Console.WriteLine();
            //
            // Get users best scores and store in an array in decending order

            for (int i = 0; i < 3; i++)
            {
                Console.Write("\tEnter your three best scores: ");
                allRanNumScores[i] = int.Parse(Console.ReadLine());
            }

            Array.Sort(allRanNumScores);
            Array.Reverse(allRanNumScores);
            Console.WriteLine();
            Console.WriteLine("\tYour High Scores");
            Console.WriteLine("\t------------------");


            //
            // Display scores using for loop
            //
            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", allRanNumScores[i].ToString());
            }


            DisplayContinuePrompt();

            return allRanNumScores;
        }

        #endregion  

        #region GUESS TEMPERATURE

        static void GuessTemperatureMenu(Finch myFinch)
        {
            bool quit = false;
            string userInput;
            int userTempGuess = 0;
            double currentTempInFarenheit = 0;
            int absDifference = 0;

            Console.CursorVisible = true;

            // 
            // Get user input
            //
            do
            {
                DisplayScreenHeader("Guess Temperature Menu");
                Console.WriteLine();
                Console.WriteLine("\tGuess what the ambient temperature of the Finch is currently");
                Console.WriteLine("\ta) Enter Your Guess");
                Console.WriteLine("\tb) Get the Answer");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        userTempGuess = EnterTempGuess();
                        break;

                    case "b":

                        currentTempInFarenheit = GetTempInFarenheit(myFinch);
                        absDifference = TempAnswer(myFinch, userTempGuess, currentTempInFarenheit);
                        break;

                    case "q":
                        quit = true;
                        MainMenu();
                        break;

                    //
                    // Validate user input
                    //
                    default:
                        Console.WriteLine();
                        Console.Beep();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);
        }

        static int TempAnswer(Finch myFinch, int userTempGuess, double currentTempInFarenheit)
        {
            int currentTemp = Convert.ToInt32(currentTempInFarenheit);
            int absDifference = 0;
            int difference;
            int[] TempScores = new int[3];

            //
            // Display the users guess back and display the correct current temp
            //
            DisplayScreenHeader("Final Temperature Results");
            Console.WriteLine();
            Console.WriteLine("\tYour answer: {0}", userTempGuess);
            Console.WriteLine();
            Console.WriteLine("\tThe current temperature: {0}", GetTempInFarenheit(myFinch).ToString("n0"));
            Console.WriteLine();

            //
            // Give the user feedback about how close they were to the correct answer
            //
            if (userTempGuess > currentTemp)
            {
                GetTempInFarenheit(myFinch);
                int currentTemperature = Convert.ToInt32(GetTempInFarenheit(myFinch));
                difference = (userTempGuess) - (currentTemperature);
                absDifference = Math.Abs(difference);
                Console.WriteLine("\tYou were only {0} away", absDifference);

            }
            else if (currentTemp > userTempGuess)
            {
                GetTempInFarenheit(myFinch);
                int currentTemperature = Convert.ToInt32(GetTempInFarenheit(myFinch));
                difference = (currentTemperature) - (userTempGuess);
                absDifference = Math.Abs(difference);
                Console.WriteLine("\tYou were only {0} away", absDifference);

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tYou got it, good job!");
                Console.WriteLine();

            }

            DisplayContinuePrompt();

            return absDifference;

        }

        static int EnterTempGuess()
        {
            bool parseSuccess = false;
            int userTempGuess;

            Console.CursorVisible = true;

            DisplayScreenHeader("Enter Your Guess");
            Console.WriteLine();
            Console.Write("\tYour guess of temperature [in Farenheit]: ");

            //
            // Get and validate user guess for temperature
            //
            parseSuccess = int.TryParse(Console.ReadLine(), out userTempGuess);

            if (parseSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("\tYour guess is {0}", userTempGuess);
            }
            else
            {
                Console.WriteLine();
                Console.Beep();
                Console.WriteLine("\tPlease enter an integer.");
            }

            DisplayContinuePrompt();
            return userTempGuess;

        }

        static double GetTempInFarenheit(Finch myFinch)
        {
            // 
            // Convert celcius to farenheit
            //
            double currentTempInFarenheit;
            double currentTemp;

            currentTemp = myFinch.getTemperature();

            currentTempInFarenheit = currentTemp * 1.8 + 32;

            return currentTempInFarenheit;
        }

        static int[] TempHighScores()
        {
            DisplayScreenHeader("Temperature Guessing Game Scores");

            //
            // Get user high score and store in array
            // Display back to user in decending order
            //
            int[] TempScores = new int[3];

            Console.CursorVisible = true;

            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Console.Write("\tEnter your three best scores: ");
                TempScores[i] = int.Parse(Console.ReadLine());
            }

            Array.Sort(TempScores);
            Array.Reverse(TempScores);
            Console.WriteLine();
            Console.WriteLine("\tYour High Scores");
            Console.WriteLine("\t------------------");

            for (int i = 2; i >= 0; i--)
            {
                Console.WriteLine("\t{0}", TempScores[i].ToString());
            }


            DisplayContinuePrompt();

            return TempScores;
        }



        #endregion



    }

}
