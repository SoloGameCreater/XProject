
using Framework;
using SaveFile;

public class CurrencyModel : GlobalSystem<CurrencyModel>, IInitable
{
    private SaveFileCurrency _saveFileCurrency;
    private SaveFileDictionary<int, SaveFileSafeCount> UserCurrency { get { return _saveFileCurrency.UserAllCurrencyDic; } }
    public void Init()
    {
        _saveFileCurrency = SaveFileManager.Instance.GetSaveFile<SaveFileCurrency>();
    }

    public void Release() { }
    
    
}