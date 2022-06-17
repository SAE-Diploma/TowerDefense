using System.Collections;
using System.Collections.Generic;

public class SaveObject
{
    private int points;
    public int Points => points;

    private PermanentUpgrades[] permanentUpgrades;
    public PermanentUpgrades[] PermanentUpgrades => permanentUpgrades;

    public SaveObject(int points, PermanentUpgrades[] permanentUpgrades)
    {
        this.points = points;
        this.permanentUpgrades = permanentUpgrades;
    }
}
