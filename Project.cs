struct Block
{
    public int x, y;

    public Block(int X, int Y){x = X; y = Y;}
}

class PlayerBlock
{
    static Block[][] Tetrominoes =
    [
        [new Block(0,0), new Block(-1,0), new Block(1,0), new Block(2,0)],   // I
        [new Block(0,0), new Block(0,1), new Block(1,0), new Block(1,1)],    // O
        [new Block(0,0), new Block(0,1), new Block(-1,0), new Block(1,0)],   // T
        [new Block(0,0), new Block(-1,0), new Block(1,0), new Block(-1,1)],  // S
        [new Block(0,0), new Block(-1,0), new Block(1,0), new Block(1,-1)],  // Z
        [new Block(0,0), new Block(-1,0), new Block(1,0), new Block(-1,-1)], // J
        [new Block(0,0), new Block(0,1), new Block(0,2), new Block(1,0)]     // L
    ];

    public int x, y;
    public Block[] currentShape;

    public void Move(int dx, int dy)
    {
        x += dx;
        y += dy;
    }

    public void Rotate()
    {
        for(int i = 0; i < currentShape.Length; i++)
        {
            int tempY = currentShape[i].y;
            currentShape[i].y = -currentShape[i].x;
            currentShape[i].x = tempY;
        }
    }
}

class World
{
    public const int width = 10;
    public const int length = 20;

    private int[,] worldBlock = new int[width, length];

    public bool isCanAdd(Block[] block, int x, int y)
    {
        foreach (Block part in block){
            if(x + part.x >= 0 && x + part.x < width && y + part.y < length && y + part.y >= 0 && worldBlock[x + part.x, y + part.y] == 0){
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void Add(Block[] block, int x, int y)
    {
        foreach (Block part in block){
            worldBlock[x + part.x, y + part.y] = 1;
        }
    }

    public void ClearLine()
    {
        for(int i = 0; i < length; i++)
        {
            bool flag = true;
            for(int j = 0; j < width; j++)
            {
                if(worldBlock[i, j] == 0)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                for(int j = 0; j < width; j++)
                {
                    worldBlock[i, j] = 0;
                }
                for(int k = i; k > 0; k--)
                {
                    for(int l = 0; l < width; l++)
                    {
                        worldBlock[k, l] = worldBlock[k-1, l];
                    }
                }
                for(int l = 0; l < width; l++)
                {
                    worldBlock[0, l] = 0;
                }
                i--;
            }
        }
    }
}

class Game
{
    PlayerBlock player = new PlayerBlock {x = 5, y = 1, currentShape = PlayerBlock.Tetrominoes[DateTime.Now.Ticks%7]};

    World world = new();
    DateTime lastTime = DateTime.Now;
    public void Update()
    {
        if(Console.KeyAvailable) {
            var key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.RightArrow) {
                player.Move(1, 0); // 오른쪽으로 이동
                if(!world.isCanAdd(player.x, player.y))
                {
                    player.Move(-1, 0);
                }
            } else if(key == ConsoleKey.LeftArrow) {
                player.Move(-1, 0); // 왼쪽으로 이동
                if(!world.isCanAdd(player.x, player.y))
                {
                    player.Move(1, 0);
                }
            } else if(key == ConsoleKey.R) {
                player.Rotate(); // 왼쪽으로 이동
                if(!world.isCanAdd(player.x, player.y))
                {
                    player.Rotate();
                    player.Rotate();
                    player.Rotate();
                }
            }
        }
        if((DateTime.Now - lastTime).TotalSeconds >= 1)
        {
            if(world.isCanAdd(player.currentShape, player.x, player.y + 1))
            {
                player.Move(0, 1);
            }
            else if(!world.isCanAdd(player.currentShape, 5, 1))
            {
                Console.WriteLine("game over");
            }
            else
            {
                world.Add(player.currentShape, player.x, player.y);
                world.ClearLine();
                player.currentShape = PlayerBlock.Tetrominoes[DateTime.Now.Ticks%7];
            }
            lastTime = DateTime.Now;
        }
    }
}