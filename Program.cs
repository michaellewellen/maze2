// Read Maze from the file and convert it into an array of char arrays
using System.Collections;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.Win32.SafeHandles;

const int NUMMONSTERS = 20;
string[] rawMaze = File.ReadAllLines("maze.txt");
char[][] maze = new char[rawMaze.Length][];
for (int i = 0; i<rawMaze.Length; i++)
{
    maze[i] = rawMaze[i].ToCharArray();
}

// Welcome to the Maze
Console.Clear();
Console.WriteLine(@"This is our maze program:
Using your arrow keys navigate the maze. '^' are coins. You must gather 10 to open the secret door
% are bad guys, run into one of them and you are dead.
# is the exit, make it here to win the game.

Press any key to continue");
Console.ReadKey(true);
Random rand = new Random();
// game is finished when we set completed to true;
bool completed = false;

// position of our dude
(int x, int y) position = (0,0);

// position of the monsters
(int x, int y)[] monsterPosition = getMonsterPosition(ref maze);

int score = 0;
Console.Clear();
dispayMaze(maze);
Console.SetCursorPosition(0,0);
Console.Write("@");

while (!completed)
{
    completed = false;
    // Gather a keystroke from the user, determine if you can move there
    ConsoleKey option = Console.ReadKey(true).Key;
    char square;

    // erase current location
    Console.SetCursorPosition(position.x,position.y);
    Console.Write(" ");

    // Rewrite/write new location
    if (canMove(option, ref position.x, ref position.y, maze, false))
    {
        Console.SetCursorPosition(position.x,position.y);
        Console.Write("@");
    }
    
    // React to what is found in that square
    square = maze[position.x][position.y];
    switch(square)
    {
        case '^':
            score += 100;
            maze[position.x][position.y] = ' ';
            break;
        case '$':
            score += 1000;
            maze[position.x][position.y] = ' ';
            break;
        case '%':
            Console.Clear();
            Console.WriteLine($"The monster got you. Score: {score}");
            completed = true;
            break;
        case '#':
            Console.Clear();
            Console.WriteLine($"You escaped the maze, score {score}");
            completed = true;
            break;
        default:
            break;
    }
    // Move bad guys
    ConsoleKey direction = ConsoleKey.None;
    for (int i = 0; i<NUMMONSTERS; i++)
    {
        maze[monsterPosition[i].x][monsterPosition[i].y] = ' ';
        int num = rand.Next(1,5);
        Console.SetCursorPosition(monsterPosition[i].x, monsterPosition[i].y);
        Console.Write(" ");
        switch (num)
        {
            case 1: 
                direction = ConsoleKey.UpArrow;
                break;
            case 2:
                direction = ConsoleKey.DownArrow;
                break;
            case 3: 
                direction = ConsoleKey.LeftArrow;
                break;
            case 4: 
                direction = ConsoleKey.RightArrow;
                break;
        }
        if(canMove(direction,ref monsterPosition[i].x, ref monsterPosition[i].y, maze, true))
        {
            maze[monsterPosition[i].x][monsterPosition[i].y] = '%';
            Console.SetCursorPosition(monsterPosition[i].x,monsterPosition[i].y);
            Console.Write("%");
        }

    }

    // Check to see if bad guys moved on top of our dude
    for (int i = 0; i<NUMMONSTERS; i++)
    {
        if (monsterPosition[i].x == position.x && monsterPosition[i].y == position.y)
        {
            Console.Clear();
            Console.WriteLine($"The monster got you. Score {score}");
            completed = true;
        }       
    }
}

// print score and time
// would you like to play again?


static bool canMove(ConsoleKey option, ref int x, ref int y, char[][] maze, bool isMonster)
{
    
    
    switch(option)
    {
        case ConsoleKey.UpArrow:
            if (x < 0 || y-1 < 0)
                return false;
            if(maze[x][y-1] != '*' && (!isMonster))
                return true;
            else if(maze[x][y-1] == ' ' && (isMonster))
                return true;
            else return false;
        case ConsoleKey.DownArrow:
            if (x < 0 || y+1 < 0)
                return false;
            if(maze[x][y+1] != '*' && (!isMonster))
                return true;
            else if(maze[x][y+1] == ' ' && (isMonster))
                return true;
            else return false;
        case ConsoleKey.RightArrow:
            if (x + 1 < 0 || y < 0)
                return false;
            if(maze[x+1][y] != '*' && (!isMonster))
                return true;
            else if(maze[x+1][y] == ' ' && (isMonster))
                return true;
            else return false;
        case ConsoleKey.LeftArrow:
            if (x - 1 < 0 ||y < 0)
                return false;
            if(maze[x-1][y] != '*' && (!isMonster))
                return true;
            else if(maze[x-1][y] == ' ' && (isMonster))
                return true;
            else return false;
    }
   return false;
}

static (int,int)[] getMonsterPosition(ref char[][] maze)
{
    (int,int)[] positions = new (int, int)[NUMMONSTERS];
    Random rand = new Random();
    int x = 0;
    int y = 0; 
    for (int i = 0; i<NUMMONSTERS; i++)
    {
        bool valid = false;
        while (!valid)
        {
            x = rand.Next(1,maze.Length);
            y = rand.Next(1,maze[0].Length);
            if(maze[x][y] == ' ')
            {
                maze[x][y] = '%';
                positions[i].Item1 = x;
                positions[i].Item2 = y;
                valid = true;
            }
        }
    }
    return positions;
}
static void dispayMaze(char[][] maze)
{
    foreach(char[] i in maze)
    {
        foreach(char j in i)
        {
            Console.Write(j);
        }
        Console.WriteLine();
    }
}