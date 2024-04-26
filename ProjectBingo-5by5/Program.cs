﻿//Objects
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;

Random random = new Random();

//Constants
int CHART_MAXSIZE = 25;
int CHART_MAX_ROW = 5;
int GAME_MAX_NUMBER = 40;
int NUMBER_MATCHER = -1;
int PLAYER_COLUMN = 0;
int MATCHED_COLUMN = 1;

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
int[] gameDrawnNumbers;

//Matrixes
int[,] playersScores;

//Matrix Vector
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

void checkChart(int[,] chart, int drawn, int player, int[,] playersScores, bool[] canScore)
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
                playersScores[player, MATCHED_COLUMN]++;
            }
            
            if (chart[column, line] < 0)
                lineMatchedNumbers[line]++;
        }

        if ((canScore[0] && columnMatchedNumbers[column] == CHART_MAX_ROW))
        {
            playersScores[player, PLAYER_COLUMN]++;
            canScore[0] = false;
        }

        if (canScore[1] && lineMatchedNumbers[column] == CHART_MAX_ROW)
        {
            playersScores[player, PLAYER_COLUMN]++;
            canScore[1] = false;
        }

        if (chartMatchedNumbers == CHART_MAXSIZE)
            playersScores[player, PLAYER_COLUMN] += 5;
    }
}

int[] sortScoreBoard(int[,] playersScores)
{
    int[,] ranking = new int[3, 2] { { 0, playersScores[0, 0] }, { 1, playersScores[1, 0] }, { 2, playersScores[2, 0] } };

    int i, j, auxElement, aux2;
    bool changed = true;

    for (i = 0; i < 3 && changed; i++)
    {
        changed = false;

        for (j = 0; j < 3 - (1 + i); j++)
        {
            if (ranking[j, 1] < ranking[j + 1, 1])
            {
                auxElement = ranking[j, 1];
                ranking[j, 0] = ranking[j + 1, 1];
                ranking[j + 1, 0] = auxElement;

                aux2 = ranking[j, 1];
                ranking[j, 1] = ranking[j + 1, 1];
                ranking[j + 1, 1] = auxElement;
                changed = true;
            }

            if (ranking[j, 1] == ranking[j + 1, 1])
            {
                if (playersScores[ranking[j, 0], 1] < playersScores[ranking[j + 1, 0], 1])
                {
                    auxElement = ranking[j, 0];
                    ranking[j, 0] = ranking[j + 1, 0];
                    ranking[j + 1, 0] = auxElement;

                    aux2 = ranking[j, 1];
                    ranking[j, 1] = ranking[j + 1, 1];
                    ranking[j + 1, 1] = aux2;
                    changed = true;
                }

            }
        }
    }

    return new int[3] { ranking[0, 0], ranking[1, 0], ranking[2, 0] };
}

void showScoreBoard(string[] playersName, int[,] playersScores, int playersQuantity)
{
    SystemTitle();

    Console.WriteLine("------------- GAME RANKING -------------\n\n");

    int[] ranking = sortScoreBoard(playersScores);

    for (int player = 0; player < playersQuantity; player++)
        Console.WriteLine($"{playersName[ranking[player]]} ended up with {playersScores[ranking[player], PLAYER_COLUMN]} points {playersScores[ranking[player], 0]} {playersScores[ranking[player], 1]} {ranking[player]}!");
}

void GameResultMessage(int roundsQuantity, int playersQuantity, string[] playersName, int[,] playersScores, int player){
    SystemTitle();

    Console.WriteLine($"\nThe game ended with {roundsQuantity}° rounds!");
    Console.WriteLine($"\n{playersName[player]} is the winner!\n");

    showScoreBoard(playersName, playersScores, playersQuantity);

    Console.WriteLine("Press any key to continue...");
    //Console.ReadKey();

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
    playersScores = new int[playersQuantity, 2];
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
                checkChart(gameCharts[chartCount], gameDrawnNumbers[roundsQuantity], player, playersScores, canScore);

                if (playersScores[player, PLAYER_COLUMN] >= 5 && winner == 0)
                {
                    winner = player;
                    hasWinner = true;
                }
            }

    } while (!hasWinner);

    winner = 0;
    roundsQuantity = 10;

    GameResultMessage(roundsQuantity, playersQuantity, playersName, playersScores, winner);

    //for (int player = 0, chartCount = 0; player < playersQuantity; player++)
    //    for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
    //        ShowMatrix(gameCharts[chartCount], playerChart + 1, playersName[player]);

    Console.Write("\n\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");