using System.Collections.Generic;
using System.Linq;

public class WorldState
{
    public HashSet<string> Facts = new HashSet<string>();

    public bool Has(string fact) => Facts.Contains(fact);

    public WorldState Clone()
    {
        return new WorldState { Facts = new HashSet<string>(Facts) };
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var f in Facts)
            hash = hash * 31 + f.GetHashCode();
        return hash;
    }

    public override bool Equals(object obj)
    {
        if (obj is not WorldState other) return false;
        return Facts.SetEquals(other.Facts);
    }
}
