//Object
Random random = new Random();

//Constants
	//Chart
	int CHART_MAX_ROW = 5;
	int CHART_LINE = 0;
	int CHART_COLUMN = 1;
	int CHARTS_MAX_QUANTITY = 10;

	//Game
	int GAME_MAX_NUMBER = 100;
	int GAME_MIN_NUMBER = 1;
	int HIT_COEFICIENT = -1;

	//Player
	int HIT_COLUMN = 0;
	int POINT_COLUMN = 1;
	int PLAYERS_MAX_QUANTITY = 10;

//Integers
	//Player
	int playersQuantity;

	//Game
	int roundCount;
	int chartsQuantityByPlayer;
	int menuChoose;

//Booleans
	//Game
	bool hasWinner;

//Vector
	//Player
	string[] playersName;

	//Game
	int[] gameDrawnNumbers;
	int[] playerCanScore;

//Matrixes
	//Player
	int[,] playersScores;

//Matrix Vector
	//Player
	int[,][,] playerChartHits;

	//Game
	int[,][,] gameCharts;

void SystemTitle()
{
	Console.Clear();
    Console.WriteLine("\t\t\t  ______ _                     _____                      \r\n\t\t\t  | ___ (_)                   |  __ \\                     \r\n\t\t\t  | |_/ /_ _ __   __ _  ___   | |  \\/ __ _ _ __ ___   ___ \r\n\t\t\t  | ___ \\ | '_ \\ / _` |/ _ \\  | | __ / _` | '_ ` _ \\ / _ \\\r\n\t\t\t  | |_/ / | | | | (_| | (_) | | |_\\ \\ (_| | | | | | |  __/\r\n\t\t\t  \\____/|_|_| |_|\\__, |\\___/   \\____/\\__,_|_| |_| |_|\\___|\r\n\t\t\t                  __/ |                                   \r\n\t\t\t                 |___/                                    \n\n");
}

int TitledNumberScan(string message, int min, int max)
{
	int result;
	string input;

	do
	{
		do
		{
			SystemTitle();
            Console.Write(message);
            input = Console.ReadLine();

		} while (!int.TryParse(input, out result));

	} while (int.Parse(input) < min || int.Parse(input) > max);

	return result;
}

string TitledStringScan(String message)
{
    string input;
    do
    {
        SystemTitle();
        Console.Write(message);
        input = Console.ReadLine();
    } while (string.IsNullOrEmpty(input) || input.Trim().Length == 0);

    return input;
}

int GetPlayersQuantity()
{
	SystemTitle();
	return TitledNumberScan("\t\t\tHow many players will play?\n\t\t\tQuantity: ", 1, PLAYERS_MAX_QUANTITY);
}

void GetPlayersName(string[] playersName, int playersQuantity)
{
	SystemTitle();
	for (int player = 0; player < playersQuantity; player++)
		playersName[player] = TitledStringScan($"\t\t\tPlayer {player + 1}, type your name: ");
}

int GetChartsQuantity()
{
	SystemTitle();
	return TitledNumberScan("\t\t\tHow many charts by player: ", 1, CHARTS_MAX_QUANTITY);
}

void InitPlayersScores(int[,] playersScores, int playersQuantity)
{
    for (int player = 0; player < playersQuantity; player++)
        for (int j = 0; j < 2; j++)
            playersScores[player, j] = 0;
}

void InitPlayerChartHits(int[,][,] playerChartHits, int playersQuantity, int chartsQuantityByPlayer)
{
    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
            playerChartHits[player, chartCount] = new int[2, CHART_MAX_ROW];
}

void InitGameCharts(int[,][,] gameCharts, int playersQuantity, int chartsQuantityByPlayer)
{
    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
            gameCharts[player, chartCount] = CreateChart();
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

void SortTitle()
{
	SystemTitle();
    Console.Write("\t\t\t\t\t\t   ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine("SORTING\n");
    Console.ResetColor();
}

string checkChart(int[,] chart, int[,] playerChartHits, int[,] playersScores, int[] playerCanScore, int drawn, int player, int roundCount, string situation)
{
	int chartHitQuantity = 0;
	bool alreadyhit = false;

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
				chartHitQuantity++;

                chart[line, column] = drawn * HIT_COEFICIENT;

                ShowMatrices(gameCharts, playersQuantity, chartsQuantityByPlayer, drawn);
                System.Threading.Thread.Sleep(10);
			}
			
			if (playerCanScore[CHART_COLUMN] >= roundCount && playerChartHits[CHART_COLUMN, column] == CHART_MAX_ROW && !alreadyhit)
			{
				playersScores[player, POINT_COLUMN]++;
				playerCanScore[CHART_COLUMN] = roundCount;
				alreadyhit = true;

                situation += $"\n\t\t\t\t\t  Player {player + 1} hited a column!";
                Console.WriteLine(situation);
                Console.ReadKey();
            }

		}
		
		if (playerCanScore[CHART_LINE] >= roundCount && playerChartHits[CHART_LINE, line] == CHART_MAX_ROW)
		{
			playersScores[player, POINT_COLUMN]++;
			playerCanScore[CHART_LINE] = roundCount;

            situation += $"\n\t\t\t\t\t  Player {player + 1} hited a line!";
            Console.WriteLine(situation);
            Console.ReadKey();
        }
	}

    if (chartHitQuantity == CHART_MAX_ROW * CHART_MAX_ROW)
	{
        playersScores[player, POINT_COLUMN] += 5;
		
		situation += $"\n\t\t\t\t\t  Player {player + 1} hited a full chart!";
		Console.WriteLine(situation);
        Console.ReadKey();
    }

	return situation;
}

int[] sortScoreBoard(int[,] playersScores)
{
	int[] ranking = new int[playersScores.GetLength(0)];
	int auxElement;
	bool changed = true;
	
	for (int position = 0; position < playersScores.GetLength(0); position++)
		ranking[position] = position;

	for (int i = 0; i < playersScores.GetLength(0) && changed; i++)
	{
		changed = false;

		for (int position = 0; position < playersScores.GetLength(0) - (1 + i); position++)
		{
			if (playersScores[ranking[position], POINT_COLUMN] < playersScores[ranking[position + 1], POINT_COLUMN])
			{
				auxElement = ranking[position];
				ranking[position] = ranking[position + 1];
				ranking[position + 1] = auxElement;
				changed = true;
			}

			if (playersScores[ranking[position], POINT_COLUMN] == playersScores[ranking[position + 1], POINT_COLUMN])
			{
				if (playersScores[ranking[position], HIT_COLUMN] < playersScores[ranking[position + 1], HIT_COLUMN])
				{
					auxElement = ranking[position];
					ranking[position] = ranking[position + 1];
					ranking[position + 1] = auxElement;
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
    Console.WriteLine("\t\t\t\t\t\tGame Ranking\n");

	int[] ranking = sortScoreBoard(playersScores);

	for (int player = 0; player < playersQuantity; player++)
        Console.WriteLine($"\t\t\t\t\t  {playersName[ranking[player]]} ended up with {playersScores[ranking[player], POINT_COLUMN]} points!");
}

void ShowGameResult(int roundCount, int playersQuantity, string[] playersName, int[,] playersScores, string situation)
{
	SystemTitle();

	Console.WriteLine($"\n\t\t\t\t    The game ended with {roundCount}° rounds!");

    showScoreBoard(playersName, playersScores, playersQuantity);
    Console.WriteLine(situation);

    Console.WriteLine("\n\t\t\t\t\t  Press any key to continue.");
    Console.ReadKey();
}

void ShowMatrices(int[,][,] gameCharts, int playerQuantity, int chartsQuantityByPlayer, int drawn)
{
    Console.SetCursorPosition(0, 12);
    for (int player = 0; player < playersQuantity; player++)
    {
		Console.WriteLine($"\n\t\t\t\t\t       PLAYER {player + 1} CHARTS\n");

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

	gameDrawnNumbers = new int[GAME_MAX_NUMBER];
	playerCanScore = new int[2] { GAME_MAX_NUMBER, GAME_MAX_NUMBER };

	roundCount = - 1;
	hasWinner = false;
	string situation = "\n\t\t\t\t\t\t   History\n";

	playersQuantity = GetPlayersQuantity();

	playersName = new string[playersQuantity];
	playersScores = new int[playersQuantity, 2];

	InitPlayersScores(playersScores, playersQuantity);

	GetPlayersName(playersName, playersQuantity);

    chartsQuantityByPlayer = GetChartsQuantity();

    gameCharts = new int[playersQuantity, chartsQuantityByPlayer][,];
	playerChartHits = new int[playersQuantity, chartsQuantityByPlayer][,];

	InitGameCharts(gameCharts, playersQuantity, chartsQuantityByPlayer);

	InitPlayerChartHits(playerChartHits, playersQuantity, chartsQuantityByPlayer);

	SortTitle();

    do
	{
		DrawNumber(gameDrawnNumbers, ++roundCount);

		for (int player = 0; player < playersQuantity; player++)
		{
			for (int playerChart = 0, chartCount = 0; playerChart < chartsQuantityByPlayer; playerChart++, chartCount++)
			{
				situation = checkChart(gameCharts[player, chartCount], playerChartHits[player, chartCount], playersScores, playerCanScore, gameDrawnNumbers[roundCount], player, roundCount, situation);

                if (playersScores[player, POINT_COLUMN] >= 5)
					hasWinner = true;
			}
		}

	} while (!hasWinner);

    ShowGameResult(roundCount, playersQuantity, playersName, playersScores, situation);

    do
	{
        menuChoose = TitledNumberScan("\t\t\t[0] Exit\n\t\t\t[1] Players matrices\n\t\t\t[2] Ranking\n\t\t\t[3] Play Again\n\t\t\tYour choose: ", 0, 3);

        switch (menuChoose)
		{
			case 1:
				SystemTitle();
				ShowMatrices(gameCharts, playersQuantity, chartsQuantityByPlayer, 0);
                Console.WriteLine("\n\t\t\t\t\tPress any key to continue.");
                Console.ReadKey();
				break;

			case 2:
				ShowGameResult(roundCount, playersQuantity, playersName, playersScores, situation); 
				break;
		}
	} while (menuChoose != 0 && menuChoose != 3);
} while (menuChoose == 3);