public enum UIViewLayer
{
    // 必须连续，数值越大，层级越高, todo 精简层级
    None = 0,
    Lobby,              //大厅
    Normal,             //普通层级
    Guide,              //新手引导层
    BindReward,         //绑定奖励显示层级
    Tips,               //提示层级
    Notice,             //公告层级
    UnPaymentsNotice,   //补单层级
    ChooseProgress,     //选择存档
    Waiting,            //菊花层级
    NetNotice,          //断网提示
    Loading,            //转场
    TopNotice,          //高层级提示
    Max                 //最高层级
}