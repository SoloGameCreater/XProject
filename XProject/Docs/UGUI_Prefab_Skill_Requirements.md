# UGUI 纯 UI 结构拼接 Skill 需求说明（Prefab 生成器）

> 目标：根据用户描述快速生成 **Unity UGUI 的纯 UI 结构 Prefab**（仅结构与组件参数，不含任何行为脚本与事件绑定）。  
> 适用：你已有 UI 框架，希望用另一套技能处理 UI 行为/绑定。

---

## 1. Skill 目标

- 在 Unity 工程中生成 **UGUI 结构 Prefab**：Hierarchy、RectTransform、UGUI 组件、Layout/Scroll 参数。
- 输出可被后续“行为 Skill”稳定定位与绑定（靠统一命名与固定分区节点）。
- **不**生成 UI 行为：不写脚本、不挂脚本、不绑定 Button/Toggle 的事件、不调用 UI 框架接口。

---

## 2. 输入规范

Skill 必须支持两种输入方式：

### 2.1 自然语言（给人用）
示例：

- “我要一个背包 UI：左侧分类栏 + 右侧格子列表（8×6）+ 右侧详情面板 + 底部操作栏（使用/丢弃）”
- “做一个设置 UI：顶部标题栏 + 中间滚动选项列表 + 底部保存/取消按钮”

### 2.2 结构化 DSL（给机器用，强烈建议）
当自然语言容易歧义时，用 DSL 确保稳定输出。**DSL 优先级高于自然语言**。

建议字段（可扩展）：

- `ui=inventory|shop|settings|quest|character|...`
- `layout=left_tabs+grid+detail+bottom_bar`（按模板组合）
- `grid=8x6 cell=100 spacing=8 padding=12 scroll=vertical`
- `tabs=All,Equip,Consumable,Material`
- `detail=true|false`
- `bottom_bar=Use,Drop,Close`
- `text=TMP|UGUIText`
- `with_canvas=true|false`（默认 false）
- `canvas_scaler=ScaleWithScreenSize ref=1920x1080 match=0.5`（仅当 with_canvas=true）

---

## 3. 输出规范

### 3.1 输出物（必须）
- 主 Prefab：`<UIName>View.prefab`

### 3.2 输出物（可选）
- Cell 子 Prefab：`<UIName>ItemCell.prefab`  
  说明：
  - 仅 UI 结构，不挂脚本。
  - 供后续行为 Skill 绑定/池化/渲染。

### 3.3 Hierarchy 结构要求
- Prefab 根节点必须为 `RectTransform`（默认 Stretch 填满父节点）。
- 默认 **不生成** `Canvas` 与 `EventSystem`（除非 `with_canvas=true`）。

---

## 4. 命名规范（硬约束）

### 4.1 分区节点（建议固定）
以下节点名用于让后续“行为 Skill”可靠抓取结构（尽量保持一致）：

- `SafeArea`（可选）
- `Header`
- `Body`
- `Footer`
- `LeftTabs`
- `GridScroll`
- `Viewport`
- `Content`
- `DetailPanel`
- `Mask`（如用 Mask 方案）

### 4.2 统一前缀（建议硬约束）
- 容器：`Group_` / `Layout_`
- 按钮：`Btn_`
- Toggle：`Tgl_`
- 输入框：`Inp_`
- 滑条：`Sld_`
- 文本：`Txt_`
- 图片：`Img_` / `Icon_`

---

## 5. 组件与参数约定（UGUI 常见踩坑点）

### 5.1 Canvas 相关
- 默认不生成 `Canvas` / `EventSystem`。
- 若 `with_canvas=true`：
  - 生成 `Canvas` + `CanvasScaler` + `GraphicRaycaster`
  - CanvasScaler 建议默认：`Scale With Screen Size`，参考分辨率 `1920x1080`，Match `0.5`（可由 DSL 覆盖）

### 5.2 ScrollRect + Grid 的标准结构（强规则）
当使用网格滚动时，必须按以下结构生成：

- `GridScroll`（挂 `ScrollRect`）
  - `Viewport`（挂 `RectMask2D`，或 `Mask + Image`）
    - `Content`（挂 `GridLayoutGroup`）

#### 重要约束
- 避免在 `Content` 上随意叠加 `ContentSizeFitter` 与 `LayoutGroup` 引发冲突。
- 网格滚动默认策略：
  - `Content`：Anchors 设为 Top-Left（或 Top-Stretch，根据模板固定）
  - `GridLayoutGroup`：明确 `cellSize/spacing/padding/startCorner/startAxis/constraint`
  - 若需要自动扩高：交给后续行为 Skill 计算并设置 `Content.sizeDelta`

---

## 6. 背包 UI 模板（纯结构默认）

### 6.1 默认骨架（Inventory）
- `InventoryView (RectTransform Stretch)`
  - `Header`
    - `Txt_Title`
    - `Btn_Close`（仅 Button 组件，不绑定）
  - `Body (HorizontalLayoutGroup)`
    - `LeftTabs`
      - `Tgl_All`
      - `Tgl_Equip`
      - `Tgl_Consumable`
      - `Tgl_Material`
    - `GridScroll (ScrollRect)`
      - `Viewport (RectMask2D)`
        - `Content (GridLayoutGroup)`
          - `ItemCell_Prototype`（或引用 `InventoryItemCell.prefab`）
    - `DetailPanel`
      - `Img_Icon`
      - `Txt_Name`
      - `Txt_Desc`
      - `Group_Stats`
  - `Footer`
    - `Btn_Use`
    - `Btn_Drop`

### 6.2 ItemCell（结构建议）
- `InventoryItemCell`
  - `Img_BG`
  - `Icon_Item`
  - `Txt_Count`
  - `Img_Selected`（预留选中态）
  - `Img_RarityFrame`（预留稀有度）

---

## 7. 非目标（必须写清楚，防止“创作型跑偏”）

- 不生成/不修改 UI 框架代码（UIManager、窗口栈、动画系统等都不碰）。
- 不创建/绑定任何行为脚本。
- 不绑定任何事件（OnClick、OnValueChanged、UnityEvent 统统不做）。
- 不实现业务逻辑（背包堆叠、排序、服务器同步等）。
- 不生成美术资源，统一使用占位图（默认 Sprite 或纯色 Image）。

---

## 8. 示例输入

### 8.1 自然语言
> 我要一个背包 UI：左侧分类栏 + 右侧格子列表（8×6）+ 右侧详情面板 + 底部操作栏（使用/丢弃）

### 8.2 DSL
```text
ui=inventory
layout=left_tabs+grid+detail+bottom_bar
grid=8x6 cell=100 spacing=8 padding=12 scroll=vertical
tabs=All,Equip,Consumable,Material
detail=true
bottom_bar=Use,Drop,Close
text=TMP
with_canvas=false
```

---

## 9. 验收标准（输出是否合格）

- [ ] 生成 `<UIName>View.prefab`，可拖入场景正常显示结构（占位样式即可）。
- [ ] Hierarchy 分区节点齐全、命名符合规范。
- [ ] ScrollRect 结构正确：`GridScroll/Viewport/Content`。
- [ ] Layout 组件参数合理（无明显冲突配置）。
- [ ] Prefab 内不包含任何脚本与事件绑定。
- [ ] 可被后续“行为 Skill”通过节点命名稳定定位控件。
