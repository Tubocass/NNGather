using UnityEngine;

[System.Serializable]
class LevelProperties
{
    public string seed;
    public int bots, sarlacs;
    public Color[] teamColors;
    public int[,] map;
    public LevelProperties()
    {
        this.seed = "dees";
        this.bots = 2;
        this.sarlacs = 1;
        this.teamColors = new Color[]{ Color.magenta, Color.red, Color.yellow};
        this.map = new int[64,64];
    }
    public LevelProperties(string dees, int b, int s, Color[] colors, int[,] levelMap)
    {
        this.seed = dees;
        this.bots = b;
        this.sarlacs = s;
        this.teamColors = colors;
        this.map = levelMap;
    }
}
