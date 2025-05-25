using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Framework;
using Localizetion;
using UnityEngine;
using UnityEngine.UI;

[AssetAddress("UIDebug/DebugUI")]
public class DebugUI : UIPopup
{
    [ComponentBinder("Close")] private Button _closeButton;
    [ComponentBinder("ToggleItem")] private Transform _toggleItem; // 左侧选项按钮
    [ComponentBinder("DebugButton")] private Transform _debugButtonItem; // 右侧按钮

    [ComponentBinder("Root/ContentNode/ScrollView/Viewport/Content")]
    private Transform _debugContent; // 右侧内容


    private Dictionary<string, List<OptionDefinition>> _options;

    public GameObject selectObj;

    public override void OnViewOpen(UIViewParam param)
    {
        base.OnViewOpen(param);
        GetConfig();
        AddMenuBtn();
        _closeButton.onClick.AddListener(OnCloseClick);
    }

    private void GetConfig()
    {
        if (_options != null)
        {
            return;
        }

        _options = new Dictionary<string, List<OptionDefinition>>();

        var op = ScanForOptions(DebugOptions.Current);
        foreach (var option in op)
        {
            List<OptionDefinition> list;

            if (!_options.TryGetValue(option.Category, out list))
            {
                list = new List<OptionDefinition>();
                _options[option.Category] = list;
            }

            list.Add(option);
        }


        foreach (var kv in _options)
        {
            kv.Value.Sort((d1, d2) => d1.SortPriority.CompareTo(d2.SortPriority));
        }
    }

    /// <summary>
    /// 扫描指定对象中所有可用作调试选项的成员（属性和方法）
    /// 复制自 SRDebugger 工具并使用 DebugOptions 特性
    /// </summary>
    /// <param name="obj">要扫描的目标对象</param>
    /// <returns>包含所有找到的调试选项定义的集合</returns>
    public ICollection<OptionDefinition> ScanForOptions(object obj)
    {
        // 创建一个列表来存储所有找到的调试选项
        var options = new List<OptionDefinition>();

        // 通过反射获取目标对象类型的所有成员（属性和方法）
        // 只获取实例、公共、可读写属性、可调用方法
        var members =
            obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                     BindingFlags.SetProperty | BindingFlags.InvokeMethod);

        // 遍历每个成员，检查是否适合作为调试选项
        foreach (var memberInfo in members)
        {
            // 查找成员上的 Category 特性，用于确定调试选项的分类
            var categoryAttribute = DebugReflection.GetAttribute<CategoryAttribute>(memberInfo);
            // 如果没有指定分类，则使用默认分类 "Default"
            var category = categoryAttribute == null ? "Default" : categoryAttribute.Category;

            // 查找成员上的 Sort 特性，用于确定在调试界面中的排序优先级
            var sortAttribute = DebugReflection.GetAttribute<DebugOptions.SortAttribute>(memberInfo);
            // 如果没有指定排序优先级，则默认为 0
            var sortPriority = sortAttribute == null ? 0 : sortAttribute.SortPriority;

            // 查找成员上的 DisplayName 特性，用于确定在调试界面中显示的名称
            var nameAttribute = DebugReflection.GetAttribute<DebugOptions.DisplayNameAttribute>(memberInfo);
            // 如果没有指定显示名称，则使用成员的实际名称
            var name = nameAttribute == null ? memberInfo.Name : nameAttribute.Name;

            // 如果当前成员是属性
            if (memberInfo is PropertyInfo)
            {
                // 将成员信息转换为属性信息
                var propertyInfo = memberInfo as PropertyInfo;

                // 如果属性没有 getter 方法，跳过此属性
                if (propertyInfo.GetGetMethod() == null)
                {
                    continue;
                }

                // 忽略静态属性，只处理实例属性
                if ((propertyInfo.GetGetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }

                // 创建属性类型的调试选项定义并添加到列表中
                options.Add(new OptionDefinition(name, category, sortPriority,
                    new PropertyReference(obj, propertyInfo)));
            }
            // 如果当前成员是方法
            else if (memberInfo is MethodInfo)
            {
                // 将成员信息转换为方法信息
                var methodInfo = memberInfo as MethodInfo;

                // 忽略静态方法，只处理实例方法
                if (methodInfo.IsStatic)
                {
                    continue;
                }

                // 只处理无参数且返回类型为 void 的方法
                // 这样的方法适合作为调试按钮的回调
                if (methodInfo.ReturnType != typeof(void) || methodInfo.GetParameters().Length > 0)
                {
                    continue;
                }

                // 创建方法类型的调试选项定义并添加到列表中
                options.Add(new OptionDefinition(name, category, sortPriority,
                    new MethodReference(obj, methodInfo)));
            }
        }

        // 返回所有找到的调试选项定义
        return options;
    }

    private void AddMenuBtn()
    {
        _toggleItem.gameObject.SetActive(false);
        _debugButtonItem.gameObject.SetActive(false);
        foreach (var kv in _options)
        {
            GameObject obj = GameObject.Instantiate(_toggleItem.gameObject, _toggleItem.transform.parent);
            obj.gameObject.SetActive(true);
            obj.transform.Find("Text").GetComponent<Text>().text = kv.Key;
            obj.transform.Find("Selected").gameObject.SetActive(false);
            obj.GetComponent<Button>().onClick.AddListener(() => { RefreshDebugButtonShow(kv.Value, obj); });

            RefreshDebugButtonShow(kv.Value, obj);
        }
    }

    private void RefreshDebugButtonShow(List<OptionDefinition> datas, GameObject newObj)
    {
        List<OptionDefinition> list = datas;
        var hasValue = list?.Count > 0;
        if (!hasValue) return;

        if (selectObj != null)
        {
            selectObj.transform.Find("Selected").gameObject.SetActive(false);
        }

        newObj.transform.Find("Selected").gameObject.SetActive(true);
        selectObj = newObj;
        for (var i = 0; i < _debugContent.transform.childCount; i++)
        {
            var child = _debugContent.transform.GetChild(i);
            var childName = child.gameObject.name;
            // 现在只有button,剩下两个后面添加
            if (childName == "DebugButton" || childName == "DebugInput" || childName == "DebugText")
            {
                continue;
            }

            GameObject.Destroy(child.gameObject);
        }

        foreach (var optionInfo in list)
        {
            var method = optionInfo.Method;

            if (method != null)
            {
                GameObject obj = GameObject.Instantiate(_debugButtonItem.gameObject, _debugContent);
                obj.gameObject.SetActive(true);
                obj.transform.Find("Text").GetComponent<Text>().text = optionInfo.Name;
                obj.GetComponent<Button>().onClick.AddListener(() => { method.Invoke(null); });
            }
            else
            {
                // todo 暂时没有功能
                DebugUtil.LogWarning($"DebugUI: {optionInfo.Name} 没有对应的debug功能");
                // var property = optionInfo.Property;
                // if (property != null && property.CanWrite)
                // {
                //     GameObject obj = GameObject.Instantiate(debugInput.gameObject, _debugContent);
                //     obj.gameObject.SetActive(true);
                //     obj.GetComponentInChildren<Text>().text = optionInfo.Name;
                //     InputField ipt = obj.GetComponentInChildren<InputField>();
                //     Type type = optionInfo.GetType();
                //     obj.GetComponentInChildren<Button>().onClick.AddListener(() => { property.SetValue(Convert.ChangeType(ipt.text, optionInfo.Property.PropertyType)); });
                //     ipt.text = optionInfo.Property.GetValue().ToString();
                // }
                // else
                // {
                //     GameObject obj = GameObject.Instantiate(debugText.gameObject, _debugContent);
                //     obj.gameObject.SetActive(true);
                //     obj.GetComponentInChildren<Text>().text = string.Format("{0}:{1}", optionInfo.Name, optionInfo.Property.GetValue());
                // }
            }
        }
    }

    public override async Task OnViewClose()
    {
        _closeButton.onClick.RemoveListener(OnCloseClick);
        await base.OnViewClose();
    }

    private void OnCloseClick()
    {
        DoViewClose();
    }
}