using System;
using System.Threading;

class DriveGame
{
    static char car = '■'; // Changed car symbol to the extended ASCII character 254 (■)
    static int carPosition = 28; // Initial position of the car (0-59, centered on a 10-character road)
    static int speed = 100; // Speed of the game in milliseconds (1000 ms = 1 second)
    static Random random = new Random(); // Random number generator for road generation
    static string[] roadRows = new string[12]; // Array to hold the road rows
    static int previousRoadStart = 25; // Track the previous road start position

    static void Main()
    {
        Console.CursorVisible = false; // Hide the cursor for better visual experience
        InitializeRoad(); // Initialize the road with walls

        while (true) // Main game loop
        {
            DrawRoad(); // Draw the current road and car position
            HandleInput(); // Handle user input for car movement
            Thread.Sleep(speed); // Control the speed of the game loop
            ScrollRoad(); // Scroll the road down
        }
    }

    static void InitializeRoad()
    {
        // Initialize the road with walls and random spaces for the road
        for (int i = 0; i < roadRows.Length; i++)
        {
            roadRows[i] = GenerateRoadRow(); // Generate each row independently
        }
    }

    static string GenerateRoadRow()
    {
        // Create a new road row with walls and a random space for the road
        char[] roadRow = new char[62]; // 60 characters wide + 1 for left border + 1 for right border
        for (int i = 0; i < roadRow.Length; i++)
        {
            roadRow[i] = '#'; // Fill with walls
        }

        // Randomly create a space for the road, ensuring continuity
        int roadStart = previousRoadStart + random.Next(-1, 2); // Allow slight movement left or right (max 1 character)
        roadStart = Math.Max(1, Math.Min(roadStart, 50)); // Ensure space is within bounds
        previousRoadStart = roadStart; // Update previous road start for continuity

        // Create a road of fixed width (10 spaces)
        for (int i = roadStart; i < roadStart + 10; i++)
        {
            if (i < 60) roadRow[i] = ' '; // Create space for the road
        }

        return new string(roadRow); // Return the generated road row
    }

    static void DrawRoad()
    {
        Console.Clear(); // Clear the console for the new frame

        // Draw the top border of the box
        Console.WriteLine("════════════════════════════════════════════════════════════════");
        
        // Draw the game title centered
        string title = "DRIVE";
        int titlePosition = (64 - title.Length) / 2; // Center the title
        Console.WriteLine(new string(' ', titlePosition) + title); // Print the title

        // Draw the title border
        Console.WriteLine("════════════════════════════════════════════════════════════════");

        // Draw each row of the road
        for (int i = 0; i < roadRows.Length; i++)
        {
            Console.Write('║'); // Left border
            Console.Write(roadRows[i]); // Display the generated road row
            Console.Write('║'); // Right border
            Console.WriteLine(); // Move to the next line
        }

        // Set the cursor position to draw the car
        Console.SetCursorPosition(carPosition + 1, roadRows.Length + 2); // Position the car on the last row (account for borders and title)
        Console.WriteLine(car); // Draw the car at its current position

        // Draw the bottom border of the box
        Console.WriteLine("════════════════════════════════════════════════════════════════");

        // Draw exit prompt with borders
        string exitPrompt = "Press 'X' to exit the game.";
        int exitPosition = (64 - exitPrompt.Length) / 2; // Center the exit prompt
        int leftMostPosition = (((64 - exitPrompt.Length) / 2)-1);
        Console.Write('║'); // Reverted left border for exit prompt back to the original character (║)
        Console.Write(new string(' ', exitPosition) + exitPrompt); // Print the exit prompt
        Console.WriteLine(new string(' ', leftMostPosition) + '║'); // Right border for exit prompt
        Console.WriteLine("════════════════════════════════════════════════════════════════");
    }

    static void ScrollRoad()
    {
        // Shift all rows up by one
        for (int i = 0; i < roadRows.Length - 1; i++)
        {
            roadRows[i] = roadRows[i + 1]; // Move the row above down
        }
        roadRows[roadRows.Length - 1] = GenerateRoadRow(); // Generate a new row at the bottom
    }

    static void HandleInput()
    {
        if (Console.KeyAvailable) // Check if a key has been pressed
        {
            var keyInfo = Console.ReadKey(true); // Read the key without displaying it
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow: // Move car left
                    if (carPosition > 1) carPosition--; // Decrease position if not at the left edge
                    break;
                case ConsoleKey.RightArrow: // Move car right
                    if (carPosition < 58) carPosition++; // Increase position if not at the right edge
                    break;
                case ConsoleKey.UpArrow: // Accelerate
                    speed = Math.Max(1000, speed - 50); // Decrease sleep time (increase speed), but not less than 1000 ms
                    break;
                case ConsoleKey.DownArrow: // Deaccelerate
                    speed = Math.Min(1000, speed + 50); // Increase sleep time (decrease speed), but not more than 1000 ms
                    break;
                case ConsoleKey.X: // Exit the game
                    Environment.Exit(0); // Exit the application
                    break;
                case ConsoleKey.Oem5: // Exit the game (lowercase)
                    Environment.Exit(0); // Exit the application
                    break;
            }
        }
    }
}