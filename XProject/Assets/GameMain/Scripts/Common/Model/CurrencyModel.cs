using Framework;
using SaveFile;

public enum CurrencyType
{
    None,
    Coin = 1, //金币
    TreasureChest = 100, //合成物宝箱
}

public class CurrencyModel : GlobalSystem<CurrencyModel>, IInitable
{
    private SaveFileCurrency _saveFileCurrency;

    private SaveFileDictionary<int, SaveFileSafeCount> UserCurrency
    {
        get { return _saveFileCurrency.UserAllCurrencyDic; }
    }

    public void Init()
    {
        _saveFileCurrency = SaveFileManager.Instance.GetSaveFile<SaveFileCurrency>();
    }

    public void Release()
    {
    }

    public void SetCurrency(CurrencyType typeId, int amount)
    {
        var intResId = (int)typeId;

        if (UserCurrency.ContainsKey(intResId))
        {
            UserCurrency[intResId].SetValue(amount);
        }
        else
        {
            var newSafeCount = new SaveFileSafeCount();
            newSafeCount.SetValue(amount);
            UserCurrency.Add(intResId, newSafeCount);
        }

        SaveFileManager.Instance.TryAutoSave("货币变更:SetCurrency");
    }

    public void AddCurrency(CurrencyType typeId, int amount)
    {
        if (amount <= 0) return;

        var intResId = (int)typeId;

        if (UserCurrency.ContainsKey(intResId))
        {
            var preAmount = UserCurrency[intResId].GetValue();
            UserCurrency[intResId].SetValue(preAmount + amount);
        }
        else
        {
            var newSafeCount = new SaveFileSafeCount();
            newSafeCount.SetValue(amount);
            UserCurrency.Add(intResId, newSafeCount);
        }

        SaveFileManager.Instance.TryAutoSave("货币变更:AddCurrency");
    }

    /// <summary>
    /// 消耗道具，默认数量为1
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="amount"></param>
    public void CostCurrency(CurrencyType typeId, int amount = 1)
    {
        if (amount < 1) return;

        var intResId = (int)typeId;
        if (!UserCurrency.ContainsKey(intResId)) return;
        if (UserCurrency[intResId].GetValue() < amount) return;

        var preAmount = UserCurrency[intResId].GetValue();
        UserCurrency[intResId].SetValue(preAmount - amount);
        SaveFileManager.Instance.TryAutoSave("货币变更:CostCurrency");
    }

    public int GetCurrencyAmount(CurrencyType typeId)
    {
        return UserCurrency.ContainsKey((int)typeId) ? UserCurrency[(int)typeId].GetValue() : 0;
    }

    public bool IsCurrencyEnough(CurrencyType typeId, int needNum)
    {
        return GetCurrencyAmount(typeId) >= needNum;
    }
}
