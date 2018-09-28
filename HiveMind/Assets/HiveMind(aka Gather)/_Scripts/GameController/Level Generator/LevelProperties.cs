using UnityEngine;

[System.Serializable]
class LevelProperties
{
    public string seed;
    public bool useRandomSeed;
    public int bots, sarlacs;
    //public Color[] teamColors;
    public PlayerSelection[] players;
    public int[,] map;
    public LevelProperties()
    {
        this.seed = "default";
        this.useRandomSeed = false;
        this.bots = 2;
        this.sarlacs = 1;
        this.players = new PlayerSelection[1];
        //this.teamColors = new Color[]{ Color.magenta, Color.red, Color.yellow};
        this.map = new int[64,64];
    }
    public LevelProperties(string dees, bool random, int b, int s, PlayerSelection[] activePlayers, int[,] levelMap)
    {
        this.seed = dees;
        this.useRandomSeed = random;
        this.bots = b;
        this.sarlacs = s;
        players = activePlayers;
        this.map = levelMap;
    }
}
