//Objects
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;

Random random = new Random();

//Constants
int CHART_MAXSIZE = 25;
int CHART_MAX_ROW = 5;
int GAME_MAX_NUMBER = 100;
int NUMBER_MATCHER = -1;

//Integers
int playersQuantity;
int gameChartsQuantity;
int roundsQuantity;
int winner;

//Booleans
bool[] canScore;
bool hasWinner;

//Vectors
int[] ChartsQuantityByPlayer;
string[] playersName;
int[] playersScore;
int[] gameDrawnNumbers;

//Matrixes
int[][,] gameCharts;

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
    SystemTitle();
    Console.Write("New Player, type your name: ");
    return Console.ReadLine();
}

int GetChartsQuantity(String playerName)
{
    SystemTitle();
    Console.Write($"Player {playerName}, type how many charts do you want: ");
    return int.Parse(Console.ReadLine());
}

int[,] CreateChart()
{
    int chartFillCounter = 0;
    int number;

    int[,] chart = new int[CHART_MAX_ROW, CHART_MAX_ROW];

    for (int line = 0; line < CHART_MAX_ROW; line++)
    {
        for (int column = 0; column < CHART_MAX_ROW; column++)
        {
            do
            {
                number = random.Next(1, GAME_MAX_NUMBER);

            } while (IsAlreadyAt(chart, chartFillCounter, number));

            chart[line, column] = number;
            chartFillCounter++;
        }
    }

    return chart;
}

bool IsAlreadyAt(int[,] chart, int chartFillCounter, int number)
{
    bool isRepeated = false;

    for (int i = 0, column = 0, line = 0; i < chartFillCounter && !isRepeated; i++)
    {
        if (column == CHART_MAX_ROW)
        {
            line++;
            column = 0;
        }

        if (number == chart[line, column++])
            isRepeated = true;
    }

    return isRepeated;
}

bool isAlreadyDrawn(int[] gameDrawnNumbers, int roundsQuantity)
{
    bool isRepeated = false;
    
    for (int i = 0; i < roundsQuantity && !isRepeated; i++)
    {
        if (gameDrawnNumbers[i] == gameDrawnNumbers[roundsQuantity])
            isRepeated = true;
    }

    return isRepeated;
}

void DrawNumber(int[] gameDrawnNumbers, int roundsQuantity)
{
    do
    {
        gameDrawnNumbers[roundsQuantity] = random.Next(0, GAME_MAX_NUMBER);

    } while (isAlreadyDrawn(gameDrawnNumbers, roundsQuantity));
}

void checkChart(int[,] chart, int drawn, int player, int[] playersScore, bool[] canScore)
{
    int chartMatchedNumbers = 0;
    int[] lineMatchedNumbers = new int[CHART_MAX_ROW];
    int[] columnMatchedNumbers = new int[CHART_MAX_ROW];

    for (int line = 0; line < CHART_MAX_ROW; line++)
        for (int column = 0; column < CHART_MAX_ROW; column++)
            if (chart[line, column] == drawn)
                chart[line, column] = drawn * NUMBER_MATCHER;

    for (int column = 0; column < CHART_MAX_ROW; column++)
    {
        for (int line = 0; line < CHART_MAX_ROW; line++)
        {
            if (chart[line, column] < 0)
            {
                columnMatchedNumbers[line]++;
                chartMatchedNumbers++;
            }
            
            if (chart[column, line] < 0)
                lineMatchedNumbers[line]++;
        }

        if ((canScore[0] && columnMatchedNumbers[column] == CHART_MAX_ROW))
        {
            playersScore[player]++;
            canScore[0] = false;
        }

        if (canScore[1] && lineMatchedNumbers[column] == CHART_MAX_ROW)
        {
            playersScore[player]++;
            canScore[1] = false;
        }

        if (chartMatchedNumbers == CHART_MAXSIZE)
            playersScore[player] += 5;
    }
}

//void sortScoreboard(string[] playersName, int[] playersScore, int playersQuantity)
//{
//    int score;
//    string name;

//    for (int player = 1; player < playersQuantity; player++)
//    {
//        if (playersScore[player] > playersScore[player - 1])
//        {
//            score = playersScore[player];
//            name = playersName[player];

//            playersScore[player] = playersScore[player - 1];
//            playersName[player] = playersName[player - 1];

//            playersScore[player - 1] = score;
//            playersName[player - 1] = name;
//        }
//    }
//}

int[] sortScoreBoard(int[] playersScores)
{
    int minor, higher, aux, count, lastOccurrence;
    int[] ranking = new int[playersScores.Length];

    lastOccurrence = count = higher =  minor = aux = 0;

    for (int i = 0; i < playersScores.Length; i++)
        if (playersScores[higher] < playersScores[i])
            higher = i;

    ranking[count] = higher;

    for (int i = 0; i < playersScores.Length; i++)
        if (playersScores[i] < playersScores[minor])
            aux = minor = i;

    for (int last = 1; last < playersScores.Length; last++)
    {
        minor = aux;

        for (int position = 0; position < playersScores.Length; position++)
        {
            if (playersScores[position] > playersScores[minor])
                if (playersScores[position] == playersScores[ranking[last]])
                {
                    if (position > lastOccurrence)
                        minor = position;
                }
                
                else
                    minor = position;
        }

        lastOccurrence = minor;

        if (playersScore[ranking[last - 1]] != playersScores[minor])
            ranking[last] = minor;
    }

    return ranking;
}

void showScoreBoard(string[] playersName, int[] playersScore, int playersQuantity)
{
    SystemTitle();

    Console.WriteLine("------------- GAME RANKING -------------\n\n");

    //sortScoreboard(playersName, playersScore, playersQuantity);
    int[] ranking = sortScoreBoard(playersScore);

    for (int player = 0; player < playersQuantity; player++)
        Console.WriteLine($"{playersName[ranking[player]]} ended up with {playersScore[ranking[player]]} points!");
}

void GameResultMessage(int roundsQuantity, int playersQuantity, string[] playersName, int[] playersScore, int player){
    SystemTitle();

    Console.WriteLine($"\nThe game ended with {roundsQuantity}° rounds!");
    Console.WriteLine($"\n{playersName[player]} is the winner!\n");

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();

    showScoreBoard(playersName, playersScore, playersQuantity);
}

void ShowMatrix(int[,] matrix, int playerChart, String playerName)
{
    Console.WriteLine($"\n\n{playerName} {playerChart}° chart");

    for (int line = 0; line < CHART_MAX_ROW; line++)
    {
        Console.WriteLine();

        for (int column = 0; column < CHART_MAX_ROW; column++)
        {
            if (matrix[line, column] < 0)
                Console.Write($"[{matrix[line, column] * NUMBER_MATCHER:00}]");
            else 
                Console.Write($" {matrix[line, column]:00} ");
        }
    }
}

do
{
    SystemTitle();

    hasWinner = false;
    canScore = new bool[2];
    canScore[0] = canScore[1] = true;
    gameChartsQuantity = 0;
    winner = 0;
    roundsQuantity = -1;

    //playersQuantity = getPlayersQuantity();
    string[] nomes = new string[3] { "caua", "edenilson", "augusto" };
    playersQuantity = 3;

    gameDrawnNumbers = new int[GAME_MAX_NUMBER];
    playersName = new string[playersQuantity];
    playersScore = new int[playersQuantity];
    ChartsQuantityByPlayer = new int[playersQuantity];

    for (int player = 0; player < playersQuantity; player++)
    {
        //playersName[player] = GetPlayerName();
        playersName[player] = nomes[player] ;
        //ChartsQuantityByPlayer[player] = GetChartsQuantity(playersName[player]);
        ChartsQuantityByPlayer[player] = 4;
        gameChartsQuantity += ChartsQuantityByPlayer[player];
    }

    gameCharts = new int[gameChartsQuantity][,];

    for (int player = 0, chartCount = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
            gameCharts[chartCount] = CreateChart();

    for (int player = 0, chartCount = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
            ShowMatrix(gameCharts[chartCount], playerChart + 1, playersName[player]);

    do
    {
        DrawNumber(gameDrawnNumbers, ++roundsQuantity);

        for (int player = 0, chartCount = 0; player < playersQuantity; player++)
            for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
            {
                 checkChart(gameCharts[chartCount], gameDrawnNumbers[roundsQuantity], player, playersScore, canScore);

                if (playersScore[player] >= 5 && winner == 0)
                {
                    winner = player;
                    hasWinner = true;
                }
            }

    } while (!hasWinner);

    GameResultMessage(roundsQuantity, playersQuantity, playersName, playersScore, winner);

    for (int player = 0, chartCount = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
            ShowMatrix(gameCharts[chartCount], playerChart + 1, playersName[player]);

    Console.Write("\n\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");