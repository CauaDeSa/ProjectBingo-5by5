//Objects
Random random = new Random();

//Constants
int CHART_MAXSIZE = 25;
int CHART_MAX_ROW = 5;
int NAME_MAXSIZE = 10;
int GAME_MAX_NUMBER = 100;
int NUMBER_MATCHER = -1;

//Integers
int playersQuantity;
int gameChartsQuantity;
int chartCount;
int drawnQuantity;

//Booleans
bool lineCanScore;
bool columnCanScore;
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

bool isAlreadyDrawn(int[] gameDrawnNumbers, int drawnQuantity)
{
    bool isRepeated = false;
    
    for (int i = 0; i < drawnQuantity && !isRepeated; i++)
    {
        if (gameDrawnNumbers[i] == gameDrawnNumbers[drawnQuantity])
            isRepeated = true;
    }

    return isRepeated;
}

void DrawNumber(int[] gameDrawnNumbers, int drawnQuantity)
{
    do
    {
        gameDrawnNumbers[drawnQuantity] = random.Next(0, GAME_MAX_NUMBER);

    } while (isAlreadyDrawn(gameDrawnNumbers, drawnQuantity));
}

int checkChart(int[,] chart, int drawn, bool lineCanScore, bool columnCanScore)
{
    int points = 0;
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

        if ((columnCanScore && columnMatchedNumbers[column] == CHART_MAX_ROW) || (lineCanScore && lineMatchedNumbers[column] == CHART_MAX_ROW))
            points++;

        if (chartMatchedNumbers == CHART_MAXSIZE)
            points += 5;
    } 

    return points;
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
    lineCanScore = true;
    columnCanScore = true;

    gameChartsQuantity = 0;
    chartCount = 0;
    drawnQuantity = -1;

    playersQuantity = getPlayersQuantity();

    gameDrawnNumbers = new int[GAME_MAX_NUMBER];
    playersName = new string[NAME_MAXSIZE];
    playersScore = new int[playersQuantity];
    ChartsQuantityByPlayer = new int[playersQuantity];

    for (int player = 0; player < playersQuantity; player++)
    {
        playersName[player] = GetPlayerName();
        ChartsQuantityByPlayer[player] = GetChartsQuantity(playersName[player]);

        gameChartsQuantity += ChartsQuantityByPlayer[player];
    }

    gameCharts = new int[gameChartsQuantity][,];

    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++)
            gameCharts[chartCount++] = CreateChart();

    chartCount = 0;

    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++)
            ShowMatrix(gameCharts[chartCount++], playerChart + 1, playersName[player]);

    do
    {
        DrawNumber(gameDrawnNumbers, ++drawnQuantity);

        chartCount = 0;

        for (int player = 0; player < playersQuantity; player++)
            for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++)
            {
                playersScore[player] = checkChart(gameCharts[chartCount++], gameDrawnNumbers[drawnQuantity], lineCanScore, columnCanScore);

                if (playersScore[player] >= 5)
                    hasWinner = true;
            }

    } while (!hasWinner);

    chartCount = 0;

    for (int player = 0; player < playersQuantity; player++)
        for (int playerChart = 0; playerChart < ChartsQuantityByPlayer[player]; playerChart++)
            ShowMatrix(gameCharts[chartCount++], playerChart + 1, playersName[player]);

    Console.Write("\n\nType 'y' to play again: ");
} while (Console.ReadLine() == "y");