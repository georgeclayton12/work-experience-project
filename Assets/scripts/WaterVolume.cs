using System.Collections.Generic;

public class WaterVolume
{
    // public int TotalVolume => SurfaceBlocks.Count + VolumeBlocls.Count;
    public int TotalVolume { get { return SurfaceLocations.Count + VolumeBlocks.Count; } }

    public List<int[]> SurfaceLocations = new List<int[]>();
    public List<int[]> VolumeBlocks = new List<int[]>();

    public List<int[]> GetVolume()
    {
        List<int[]> items = new List<int[]>();
        items.AddRange(VolumeBlocks);
        return items;
    }
}