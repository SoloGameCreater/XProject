---
name: ugui-prefab-builder
description: 生成 Unity UGUI 纯结构 Prefab（仅 Hierarchy、RectTransform、UGUI 组件与布局参数，不含脚本与事件绑定）。当用户提出“搭建背包/商店/设置等 UI 结构”“先出 UI 骨架再做行为绑定”“按 DSL 批量稳定产出 UGUI Prefab”“要求固定命名给后续行为层抓取节点”等请求时使用。
---

# UGUI 结构 Prefab 搭建

## 目标

根据自然语言或 DSL，稳定产出可复用的 UGUI 结构 Prefab。
保证结构可被后续行为层可靠定位，且严格不写行为逻辑。

## 执行流程

### 1. 解析输入并标准化

- 优先读取 DSL；仅在 DSL 缺失字段时回落到自然语言推断。
- 使用如下字段作为标准输入模型（可扩展）：
  - `ui=inventory|shop|settings|quest|character|...`
  - `layout=left_tabs+grid+detail+bottom_bar`
  - `grid=8x6 cell=100 spacing=8 padding=12 scroll=vertical`
  - `tabs=All,Equip,Consumable,Material`
  - `detail=true|false`
  - `bottom_bar=Use,Drop,Close`
  - `text=TMP|UGUIText`
  - `with_canvas=true|false`
  - `canvas_scaler=ScaleWithScreenSize ref=1920x1080 match=0.5`

### 2. 定义产物与路径

- 始终生成主 Prefab：`<UIName>View.prefab`。
- 按需生成 Cell Prefab：`<UIName>ItemCell.prefab`（仅结构，不挂脚本）。
- 默认不生成 Canvas 与 EventSystem；仅当 `with_canvas=true` 时生成。

### 3. 生成 Hierarchy 骨架

- 根节点必须为 `RectTransform`，默认 Stretch 到父节点。
- 优先使用以下标准分区节点（后续行为层依赖这些锚点）：
  - `SafeArea`（可选）
  - `Header`
  - `Body`
  - `Footer`
  - `LeftTabs`
  - `GridScroll`
  - `Viewport`
  - `Content`
  - `DetailPanel`
  - `Mask`（仅使用 Mask 方案时）
- 命名前缀规范：
  - 容器：`Group_` / `Layout_`
  - 按钮：`Btn_`
  - Toggle：`Tgl_`
  - 输入框：`Inp_`
  - 滑条：`Sld_`
  - 文本：`Txt_`
  - 图片：`Img_` / `Icon_`

### 4. 应用组件与关键参数

- `with_canvas=true` 时：
  - 创建 `Canvas + CanvasScaler + GraphicRaycaster`。
  - 默认 `CanvasScaler = Scale With Screen Size`，参考分辨率 `1920x1080`，`Match=0.5`（可被 DSL 覆盖）。
- 使用网格滚动时，强制结构：
  - `GridScroll(ScrollRect)`
    - `Viewport(RectMask2D 或 Mask+Image)`
      - `Content(GridLayoutGroup)`
- Scroll/Grid 约束：
  - 明确 `cellSize`、`spacing`、`padding`、`startCorner`、`startAxis`、`constraint`。
  - 不在 `Content` 随意叠加会冲突的 `ContentSizeFitter + LayoutGroup` 组合。
  - 自动扩高优先交给后续行为层动态设置 `Content.sizeDelta`。

### 5. 套用默认模板（Inventory）

当 `ui=inventory` 且未提供冲突 DSL 时，优先输出以下骨架：

```text
InventoryView
├─ Header
│  ├─ Txt_Title
│  └─ Btn_Close
├─ Body
│  ├─ LeftTabs
│  │  ├─ Tgl_All
│  │  ├─ Tgl_Equip
│  │  ├─ Tgl_Consumable
│  │  └─ Tgl_Material
│  ├─ GridScroll
│  │  └─ Viewport
│  │     └─ Content
│  │        └─ ItemCell_Prototype
│  └─ DetailPanel
│     ├─ Img_Icon
│     ├─ Txt_Name
│     ├─ Txt_Desc
│     └─ Group_Stats
└─ Footer
   ├─ Btn_Use
   └─ Btn_Drop
```

`InventoryItemCell` 默认结构：

```text
InventoryItemCell
├─ Img_BG
├─ Icon_Item
├─ Txt_Count
├─ Img_Selected
└─ Img_RarityFrame
```

### 6. 执行硬性禁止项

- 不创建或修改 UI 框架代码（UIManager、窗口栈、动画系统等）。
- 不创建、挂载、修改任何行为脚本。
- 不绑定任何 UnityEvent（OnClick、OnValueChanged 等）。
- 不实现任何业务逻辑（排序、堆叠、同步、网络）。
- 不生成美术资源，仅使用占位 Image/Sprite。

### 7. 执行交付前自检

- 检查是否生成 `<UIName>View.prefab`，并可放入场景展示。
- 检查分区节点与前缀命名是否符合规范。
- 检查滚动网格结构是否为 `GridScroll/Viewport/Content`。
- 检查布局组件参数是否有明显冲突。
- 检查 Prefab 内是否存在脚本组件或事件绑定。
- 检查后续行为层是否可按节点名稳定定位控件。

## 输出格式

按以下格式回复执行结果，避免遗漏：

```text
【输入归一化】
- ui:
- layout:
- grid:
- tabs:
- detail:
- bottom_bar:
- text:
- with_canvas:

【产物】
- 主 Prefab:
- 子 Prefab(可选):

【结构摘要】
- 根节点:
- 分区节点:
- ScrollRect 关键链路:

【参数摘要】
- CanvasScaler:
- GridLayoutGroup:
- 其他 Layout:

【合规检查】
- 无脚本:
- 无事件绑定:
- 命名规范:
- 可绑定性:
```

## 失败与降级

- 输入歧义时，先输出“归一化 DSL 草案”再继续执行。
- 字段缺失时，使用安全默认值并在结果中明确标注。
- 若用户要求添加脚本或绑定事件，明确拒绝并提示应交由行为类 Skill 处理。
