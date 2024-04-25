//Objects
using System.Numerics;

Random random = new Random();

//Constants
int CHART_MAXSIZE = 25;
int NAME_MAXSIZE = 10;

//Integers
int playersQuantity;
int chartsQuantity = 0;
int gameChart = 0;

//Booleans
bool repeated;

//Vectors
int[] playerChartsQuantity;
string[] playerChartsName;

//Matrixes
float[][,] gameCharts;

void SystemTitle()
{
    Console.Clear();
    Console.WriteLine("______ _                     _____                      \r\n| ___ (_)                   |  __ \\                     \r\n| |_/ /_ _ __   __ _  ___   | |  \\/ __ _ _ __ ___   ___ \r\n| ___ \\ | '_ \\ / _` |/ _ \\  | | __ / _` | '_ ` _ \\ / _ \\\r\n| |_/ / | | | | (_| | (_) | | |_\\ \\ (_| | | | | | |  __/\r\n\\____/|_|_| |_|\\__, |\\___/   \\____/\\__,_|_| |_| |_|\\___|\r\n                __/ |                                   \r\n               |___/                                    \n\n");
}

int getPlayersQuantity()
{
    SystemTitle();
    Console.Write("How many players will play?\nQuantity: ");
    return int.Parse(Console.ReadLine());
}
string GetPlayerName()
{
    Console.Write("New Player, type your name: ");
    return Console.ReadLine();
}

int GetPlayerChartsQuantity(String playerName)
{
    SystemTitle();
    Console.Write($"Player {playerName}, type how many charts do you want: ");
    return int.Parse(Console.ReadLine());
}
float[,] CreateChart()
{
    int chartFillCounter = 0;
    int number;

    float[,] chart = new float[CHART_MAXSIZE, CHART_MAXSIZE];

    for (int line = 0; line < CHART_MAXSIZE / 5; line++)
    {
        for (int column = 0; column < CHART_MAXSIZE / 5; column++)
        {
            do
            {
                number = random.Next(0, 100);

            } while (IsAlreadyAt(chart, chartFillCounter, number));

            chart[line, column] = number;
        }
    }

    return chart;
}

bool IsAlreadyAt(float[,] chart, int chartFillCounter, int number)
{
    bool isRepeated = false;

    for (int line = 0, column = 0; line < chartFillCounter && !isRepeated; line++)
    {
        if (line == CHART_MAXSIZE / 5)
            column++;

        if (number == chart[line, column])
            isRepeated = true;
    }

    return isRepeated;
}

void ShowMatrix(float[,] matrix, int playerChart, String playerName)
{
    Console.WriteLine($"\n\n{playerName} {playerChart}° chart");

    for (int line = 0; line < CHART_MAXSIZE / 5; line++)
    {
        Console.WriteLine();

        for (int column = 0; column < CHART_MAXSIZE / 5; column++)
            Console.Write($"{matrix[line, column]:00} ");
    }
}

do
{
    SystemTitle();

    playersQuantity = getPlayersQuantity();

    playerChartsName = new string[NAME_MAXSIZE];
    playerChartsQuantity = new int[playersQuantity];

    for (int player = 0; player < playersQuantity; player++)
    {
        playerChartsName[player] = GetPlayerName();
        playerChartsQuantity[player] = GetPlayerChartsQuantity(playerChartsName[player]);
        chartsQuantity += playerChartsQuantity[player];
    }

    gameCharts = new float[chartsQuantity][,];

    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < playerChartsQuantity[player]; playerChart++)
            gameCharts[gameChart++] = CreateChart();

    gameChart = 0;

    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < playerChartsQuantity[player]; playerChart++)
            ShowMatrix(gameCharts[gameChart++], playerChart + 1, playerChartsName[player]);

    Console.Write("\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");