//Object
using System;
using System.Data.Common;

Random random = new Random();

//Constants
	//Chart
	int CHART_MAX_ROW = 5;
	int CHART_LINE = 0;
	int CHART_COLUMN = 1;
	//Game
	int GAME_MAX_NUMBER = 100;
	int GAME_MIN_NUMBER = 1;
	int HIT_COEFICIENT = -1;
	//Player
	int PLAYER_COLUMN = 0;
	int HIT_COLUMN = 0;
	int POINT_COLUMN = 1;

//Integers
	//Player
	int playersQuantity;
	//Game
	int gameChartsQuantity;
	int roundCount;
	int chartsQuantityByPlayer;

//Booleans
	//Game
	bool[] canScore;
	bool hasWinner;

//Vector
	//Player
	string[] playersName;
	//Game
	int[] gameDrawnNumbers;

//Matrixes
	//Player
	int[,] playersScores;

//Matrix Vector
	//Player
	int[,][,] playerChartHits;
	//Game
	int[,][,] gameCharts;

//Temp
string[] nomes;

void SystemTitle()
{
	Console.Clear();
    Console.WriteLine("\t\t\t\t______ _                     _____                      \r\n\t\t\t\t| ___ (_)                   |  __ \\                     \r\n\t\t\t\t| |_/ /_ _ __   __ _  ___   | |  \\/ __ _ _ __ ___   ___ \r\n\t\t\t\t| ___ \\ | '_ \\ / _` |/ _ \\  | | __ / _` | '_ ` _ \\ / _ \\\r\n\t\t\t\t| |_/ / | | | | (_| | (_) | | |_\\ \\ (_| | | | | | |  __/\r\n\t\t\t\t\\____/|_|_| |_|\\__, |\\___/   \\____/\\__,_|_| |_| |_|\\___|\r\n\t\t\t\t                __/ |                                   \r\n\t\t\t\t               |___/                                    \n\n");
}

int ScanNumber()
{
	int result;
	string input;
    do
    {
        input = Console.ReadLine();
    } while (!int.TryParse(input, out result));

	return result;
}

string ScanString()
{
    string input;
    do
    {
        input = Console.ReadLine();
    } while (string.IsNullOrEmpty(input) || input.Trim().Length == 0);

    return input;
}

int GetPlayersQuantity()
{
	SystemTitle();
	Console.Write("How many players will play?\nQuantity: ");

	return ScanNumber();
}

string GetPlayerName()
{
	SystemTitle();
	Console.Write("New Player, type your name: ");
	return ScanString();
}

int GetChartsQuantity(String playerName)
{
	SystemTitle();
	Console.Write($"Player {playerName}, type how many charts do you want: ");
	return ScanNumber();
}

bool IsAlreadyAt(int[,] chart, int chartFillCounter, int number)
{
    bool isRepeated = false;

	for (int column = 0, line = 0; chartFillCounter > 0 && !isRepeated; chartFillCounter--, column++)
    {
        if (column == CHART_MAX_ROW)
        {
            line++;
            column = 0;
        }

        if (number == chart[line, column])
            isRepeated = true;
    }

    return isRepeated;
}

int[,] CreateChart()
{
	int chartFillCounter = 0;

	int[,] chart = new int[CHART_MAX_ROW, CHART_MAX_ROW];

	for (int line = 0; line < CHART_MAX_ROW; line++)
	{
		for (int column = 0; column < CHART_MAX_ROW; column++, chartFillCounter++)
		{
			do
			{
				chart[line, column] = random.Next(GAME_MIN_NUMBER, GAME_MAX_NUMBER);

			} while (IsAlreadyAt(chart, chartFillCounter, chart[line, column]));
		}
	}

	return chart;
}

bool isAlreadyDrawn(int[] gameDrawnNumbers, int roundCount)
{
	bool isRepeated = false;

	for (int i = 0; i < roundCount && !isRepeated; i++)
		if (gameDrawnNumbers[i] == gameDrawnNumbers[roundCount])
			isRepeated = true;

	return isRepeated;
}

void DrawNumber(int[] gameDrawnNumbers, int roundCount)
{
	do
	{
		gameDrawnNumbers[roundCount] = random.Next(GAME_MIN_NUMBER, GAME_MAX_NUMBER);

	} while (isAlreadyDrawn(gameDrawnNumbers, roundCount));
}

void checkChart(int[,] chart, int[,] playerChartHits, int[,] playersScores, bool[] canScore, int drawn, int player)
{
	int chartHitQuantity = 0;

	for (int i = 0; i < 5; i++)
        chartHitQuantity += playerChartHits[CHART_LINE, i];

	for (int line = 0; line < CHART_MAX_ROW; line++)
	{
		for (int column = 0; column < CHART_MAX_ROW; column++)
		{
			if (chart[line, column] == drawn)
			{
				playerChartHits[CHART_LINE, line]++;
				playerChartHits[CHART_COLUMN, column]++;
				playersScores[player, HIT_COLUMN]++;

                ShowMatrices(gameCharts, playersQuantity, chartsQuantityByPlayer, drawn);
                //System.Threading.Thread.Sleep(10);


                chart[line, column] = drawn * HIT_COEFICIENT;
			}
			
			if (canScore[CHART_COLUMN] && playerChartHits[CHART_COLUMN, column] == CHART_MAX_ROW)
			{
				playersScores[player, POINT_COLUMN]++;
				canScore[CHART_COLUMN] = false;
			}

			if (canScore[CHART_LINE] && playerChartHits[CHART_LINE, line] == CHART_MAX_ROW)
			{
				playersScores[player, POINT_COLUMN]++;
				canScore[CHART_LINE] = false;
			}
		}
	}

    if (chartHitQuantity == CHART_MAX_ROW * CHART_MAX_ROW)
        playersScores[player, POINT_COLUMN] += 5;
}

int[] sortScoreBoard(int[,] playersScores)
{
	int[] ranking = new int[playersScores.GetLength(0)];
	int auxElement;
	bool changed = true;
	
	for (int player = 0; player < playersScores.GetLength(0); player++)
		ranking[player] = player;

	for (int i = 0; i < playersScores.GetLength(0) && changed; i++)
	{
		changed = false;

		for (int player = 0; player < playersScores.GetLength(0) - (1 + i); player++)
		{
			if (playersScores[ranking[player], POINT_COLUMN] < playersScores[ranking[player + 1], POINT_COLUMN])
			{
				auxElement = ranking[player];
				ranking[player] = ranking[player + 1];
				ranking[player + 1] = auxElement;
				changed = true;
			}

			if (playersScores[ranking[player], POINT_COLUMN] == playersScores[ranking[player + 1], POINT_COLUMN])
			{
				if (playersScores[ranking[player], HIT_COLUMN] < playersScores[ranking[player + 1], HIT_COLUMN])
				{
					auxElement = ranking[player];
					ranking[player] = ranking[player + 1];
					ranking[player + 1] = auxElement;
					changed = true;
				}
			}
		}
	}

    return ranking;
}

void showScoreBoard(string[] playersName, int[,] playersScores, int playersQuantity)
{
	SystemTitle();
	Console.WriteLine("\t\t\t\t\t------------- GAME RANKING -------------\n\n");

	int[] ranking = sortScoreBoard(playersScores);

	for (int player = 0; player < playersQuantity; player++)
		Console.WriteLine($"\t\t\t\t\t{playersName[ranking[player]]} ended up with {playersScores[ranking[player], POINT_COLUMN]} points and {playersScores[ranking[player], HIT_COLUMN]} hits!!\n");
}

void GameResultMessage(int roundCount, int playersQuantity, string[] playersName, int[,] playersScores)
{
	SystemTitle();

	Console.WriteLine($"\n\t\t\t\t    The game ended with {roundCount}° rounds!");
    Console.ReadKey();

    showScoreBoard(playersName, playersScores, playersQuantity);
	Console.ReadKey();
}

void ShowMatrices(int[,][,] gameCharts, int playerQuantity, int chartsQuantityByPlayer, int drawn)
{
	SystemTitle();
    for (int player = 0; player < playersQuantity; player++)
    {
        for (int line = 0; line < CHART_MAX_ROW; line++)
        {
            for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
            {
                for (int column = 0; column < CHART_MAX_ROW; column++)
                {
					if (column == 0 && playerChart == 0)
						Console.Write("\t");

                    if (gameCharts[player, chartCount][line, column] < 0)
					{
						if(gameCharts[player, chartCount][line, column] == drawn * HIT_COEFICIENT)
						{
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Red;
                        }

						else { 
													
						}

						Console.Write($"[{gameCharts[player, chartCount][line, column] * HIT_COEFICIENT:00}]");
                        Console.ResetColor();
                    }

					else
					{
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write($" {gameCharts[player, chartCount][line, column]:00} ");
                        Console.ResetColor();
                    }
					
                }
                Console.Write("\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine("\n");
    }
}

do
{
	SystemTitle();

	canScore = new bool[2];
	canScore[0] = canScore[1] = true;
	gameChartsQuantity = 0;
	roundCount = -1;
	hasWinner = false;

	//playersQuantity = GetPlayersQuantity();
	nomes = new string[3] { "caua", "edenilson", "augusto" };
	playersQuantity = 3;

	gameDrawnNumbers = new int[GAME_MAX_NUMBER];
	playersName = new string[playersQuantity];
	playersScores = new int[playersQuantity, 2];
	playersScores[0, 0] = playersScores[1, 0] = playersScores[2, 0] = playersScores[0, 1] = playersScores[1, 1] = playersScores[2, 1] = 0;
	chartsQuantityByPlayer = 4;

	for (int player = 0; player < playersQuantity; player++)
	{
		//playersName[player] = GetPlayerName();
		playersName[player] = nomes[player];
		//chartsQuantityByPlayer = GetChartsQuantity(playersName[player]);
		chartsQuantityByPlayer = 4;
		gameChartsQuantity += chartsQuantityByPlayer;
	}

	gameCharts = new int[gameChartsQuantity, chartsQuantityByPlayer][,];
	playerChartHits = new int[gameChartsQuantity, chartsQuantityByPlayer][,];

	for (int player = 0; player < playersQuantity; player++)
		for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
			gameCharts[player, chartCount] = CreateChart();

	for (int player = 0; player < playersQuantity; player++)
		for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
			playerChartHits[player, chartCount] = new int[2, 5];

	do
	{
		DrawNumber(gameDrawnNumbers, ++roundCount);

		for (int player = 0; player < playersQuantity; player++)
		{
			for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
			{
				checkChart(gameCharts[player, chartCount], playerChartHits[player, chartCount], playersScores, canScore, gameDrawnNumbers[roundCount], player);

                if (playersScores[player, POINT_COLUMN] >= 5)
					hasWinner = true;

			}
		}

	} while (!hasWinner);

	GameResultMessage(roundCount, playersQuantity, playersName, playersScores);

	ShowMatrices(gameCharts, playersQuantity, chartsQuantityByPlayer, 0);

	Console.Write("\n\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");