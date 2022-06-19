using System.Collections;
using System.Collections.Generic;

public class SaveObject
{
    private int points;
    public int Points => points;

    private PermanentUpgrade[] permanentUpgrades;
    public PermanentUpgrade[] PermanentUpgrades => permanentUpgrades;

    public SaveObject(int points, PermanentUpgrade[] permanentUpgrades)
    {
        this.points = points;
        this.permanentUpgrades = permanentUpgrades;
    }
}
