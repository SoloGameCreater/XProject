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
    }
}