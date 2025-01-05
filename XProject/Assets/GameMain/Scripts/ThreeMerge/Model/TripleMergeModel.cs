using SaveFile.TripleMerge;

namespace TripleMerge
{
    public class TripleMergeModel
    {
        public SaveFileTripleMerge SaveFileTripleMerge;

        public void ClearData()
        {
            SaveFileTripleMerge.Clear();
        }

        public void AddRegionId(int regionId)
        {
            if(!SaveFileTripleMerge.OpenRegionIds.Contains(regionId))
                SaveFileTripleMerge.OpenRegionIds.Add(regionId);
        }
    }
}