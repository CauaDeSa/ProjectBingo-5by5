//Object
Random random = new Random();

//Constants
	//Chart
	int CHART_MAX_ROW = 5;
	//Game
	int GAME_MAX_NUMBER = 100;
	int GAME_MIN_NUMBER = 1;
	int HIT_COEFICIENT = -1;
	//Player
	int PLAYER_COLUMN = 0;
	int POINT_COLUMN = 1;
	int MATCHED_COLUMN = 1;

//Integers
	//Player
	int playersQuantity;
	//Game
	int gameChartsQuantity;
	int roundCount;
	int winner;

//Booleans
	//Game
	bool[] canScore;
	bool hasWinner;

//Vectors
	//Chart
	int[] ChartsQuantityByPlayer;
	//Player
	string[] playersName;
	//Game
	int[] gameDrawnNumbers;

//Matrixes
int[,] playersScores;

//Matrix Vector
int[][,] gameCharts;


//Temp
string[] nomes;

void SystemTitle()
{
	Console.Clear();
    Console.WriteLine("\t______ _                     _____                      \r\n\t| ___ (_)                   |  __ \\                     \r\n\t| |_/ /_ _ __   __ _  ___   | |  \\/ __ _ _ __ ___   ___ \r\n\t| ___ \\ | '_ \\ / _` |/ _ \\  | | __ / _` | '_ ` _ \\ / _ \\\r\n\t| |_/ / | | | | (_| | (_) | | |_\\ \\ (_| | | | | | |  __/\r\n\t\\____/|_|_| |_|\\__, |\\___/   \\____/\\__,_|_| |_| |_|\\___|\r\n\t                __/ |                                   \r\n\t               |___/                                    \n\n");
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
				number = random.Next(GAME_MIN_NUMBER, GAME_MAX_NUMBER);

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

bool isAlreadyDrawn(int[] gameDrawnNumbers, int roundCount)
{
	bool isRepeated = false;

	for (int i = 0; i < roundCount && !isRepeated; i++)
	{
		if (gameDrawnNumbers[i] == gameDrawnNumbers[roundCount])
			isRepeated = true;
	}

	return isRepeated;
}

void DrawNumber(int[] gameDrawnNumbers, int roundCount)
{
	do
	{
		gameDrawnNumbers[roundCount] = random.Next(GAME_MIN_NUMBER, GAME_MAX_NUMBER);

	} while (isAlreadyDrawn(gameDrawnNumbers, roundCount));
}

void checkChart(int[,] chart, int drawn, int player, int[,] playersScores, bool[] canScore)
{
	int chartMatchedNumbers = 0;
	int[] lineMatchedNumbers = new int[CHART_MAX_ROW];
	int[] columnMatchedNumbers = new int[CHART_MAX_ROW];

	for (int line = 0; line < CHART_MAX_ROW; line++)
	{
		for (int column = 0; column < CHART_MAX_ROW; column++)
		{
			if (chart[line, column] < 0) {
				lineMatchedNumbers[line]++;
				columnMatchedNumbers[column]++;
				chartMatchedNumbers++;
			}

			if (chart[line, column] == drawn)
			{
				chart[line, column] = drawn * HIT_COEFICIENT;
				playersScores[player, MATCHED_COLUMN]++;

			}
			
			if ((canScore[0] && columnMatchedNumbers[column] == CHART_MAX_ROW))
			{
				playersScores[player, PLAYER_COLUMN]++;
				canScore[0] = false;
			}

			if (canScore[1] && lineMatchedNumbers[line] == CHART_MAX_ROW)
			{
				playersScores[player, PLAYER_COLUMN]++;
				canScore[1] = false;
			}
			
			if (chartMatchedNumbers == CHART_MAX_ROW * CHART_MAX_ROW)
				playersScores[player, PLAYER_COLUMN] += 5;

		}
	}
}

int[] sortScoreBoard(int[,] playersScores)
{
	int[,] ranking = new int[playersScores.GetLength(0), 2];
	
	for (int player = 0; player < playersScores.Length / 2; player++)
	{
		ranking[player, PLAYER_COLUMN] = player;
		ranking[player, POINT_COLUMN] = playersScores[player, 0];
    }

	int i, j, auxElement;
	bool changed = true;

	for (i = 0; i < 3 && changed; i++)
	{
		changed = false;

		for (j = 0; j < 3 - (1 + i); j++)
		{
			if (ranking[j, POINT_COLUMN] < ranking[j + 1, POINT_COLUMN])
			{
				auxElement = ranking[j, PLAYER_COLUMN];
				ranking[j, PLAYER_COLUMN] = ranking[j + 1, PLAYER_COLUMN];
				ranking[j + 1, PLAYER_COLUMN] = auxElement;

				auxElement = ranking[j, POINT_COLUMN];
				ranking[j, POINT_COLUMN] = ranking[j + 1, POINT_COLUMN];
				ranking[j + 1, POINT_COLUMN] = auxElement;
				changed = true;
			}

			if (ranking[j, POINT_COLUMN] == ranking[j + 1, POINT_COLUMN])
			{
				if (playersScores[ranking[j, PLAYER_COLUMN], 1] < playersScores[ranking[j + 1, PLAYER_COLUMN], 1])
				{
					auxElement = ranking[j, PLAYER_COLUMN];
					ranking[j, PLAYER_COLUMN] = ranking[j + 1, PLAYER_COLUMN];
					ranking[j + 1, PLAYER_COLUMN] = auxElement;

                    auxElement = ranking[j, POINT_COLUMN];
					ranking[j, POINT_COLUMN] = ranking[j + 1, POINT_COLUMN];
					ranking[j + 1, POINT_COLUMN] = auxElement;
					changed = true;
				}

			}
		}
	}

	return new int[3] { ranking[0, PLAYER_COLUMN], ranking[1, PLAYER_COLUMN], ranking[2, PLAYER_COLUMN] };
}

void showScoreBoard(string[] playersName, int[,] playersScores, int playersQuantity)
{
	//SystemTitle();

	Console.WriteLine("------------- GAME RANKING -------------\n\n");

	int[] ranking = sortScoreBoard(playersScores);

	for (int player = 0; player < playersQuantity; player++)
		Console.WriteLine($"{playersName[ranking[player]]} ended up with {playersScores[ranking[player], PLAYER_COLUMN]} points {playersScores[ranking[player], 0]} {playersScores[ranking[player], 1]} {ranking[player]}!");
}

void GameResultMessage(int roundCount, int playersQuantity, string[] playersName, int[,] playersScores, int winner)
{
	SystemTitle();

	Console.WriteLine($"\nThe game ended with {roundCount}° rounds!");
	Console.WriteLine($"\n{playersName[winner]} is the winner!\n");
    //Console.ReadKey();

    showScoreBoard(playersName, playersScores, playersQuantity);

	//Console.WriteLine("Press any key to continue...");
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
				Console.Write($"[{matrix[line, column] * HIT_COEFICIENT:00}]");
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
	roundCount = -1;
	winner = 0;

	//playersQuantity = getPlayersQuantity();
	nomes = new string[3] { "caua", "edenilson", "augusto" };
	playersQuantity = 3;

	gameDrawnNumbers = new int[GAME_MAX_NUMBER];
	playersName = new string[playersQuantity];
	playersScores = new int[playersQuantity, 2];
	playersScores[0, 0] = playersScores[1, 0] = playersScores[2, 0] = playersScores[0, 1] = playersScores[1, 1] = playersScores[2, 1] = 0; ;
	ChartsQuantityByPlayer = new int[playersQuantity];

	for (int player = 0; player < playersQuantity; player++)
	{
		//playersName[player] = GetPlayerName();
		playersName[player] = nomes[player];
		//ChartsQuantityByPlayer[player] = GetChartsQuantity(playersName[player]);
		ChartsQuantityByPlayer[player] = 4;
		gameChartsQuantity += ChartsQuantityByPlayer[player];
	}

	gameCharts = new int[gameChartsQuantity][,];

	for (int player = 0, chartCount = 0; player < playersQuantity; player++)
		for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
			gameCharts[chartCount] = CreateChart();

	do
	{
		DrawNumber(gameDrawnNumbers, ++roundCount);

		for (int player = 0, chartCount = 0; player < playersQuantity; player++)
			for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
			{
				checkChart(gameCharts[chartCount], gameDrawnNumbers[roundCount], player, playersScores, canScore);

				if (playersScores[player, PLAYER_COLUMN] >= 5)
				{
					hasWinner = true;
					winner = player;
				}
			}

	} while (!hasWinner);

	GameResultMessage(roundCount, playersQuantity, playersName, playersScores, winner);

	//for (int player = 0, chartCount = 0; player < playersQuantity; player++)
	//    for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++, chartCount++)
	//        ShowMatrix(gameCharts[chartCount], playerChart + 1, playersName[player]);

	Console.Write("\n\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");