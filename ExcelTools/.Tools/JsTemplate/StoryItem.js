/**
 * Created by Simon on 18/5/16.
 */
var StoryItem = {
    Parse:function(str){
        if(str==null){
            return [];
        }
        if(str.length>0){
            //顺序很重要，解析代码以来与顺序
            var cmds = this.ParseNextStep(str);
            if(cmds==null){
                cmds = this.ParseCommand(str);
            }
            if(cmds==null){
                cmds = this.ParseWalk(str);
            }
            if(cmds==null){
                cmds = this.ParseAction(str);
            }
            if(cmds==null){
                cmds = this.ParseDialog(str);
            }
            return cmds || [];
        }

        return [];
    },

    //character
    ParseWalk:function(str){
        //if(str[0]!="("){
        //    return null;
        //}
        var cmds = [];
        var reg = /(\w+),([\-0-9]+),([\-0-9]+)/g
        while(true){
            var a = reg.exec(str);
            if(a==null){
                break;
            }
            cmds.push({
                //type:"action",
                type:"walk",
                character:a[1],
                gridTile:{x:Number(a[2]),y:Number(a[3])}
            })
        }
        if(cmds.length==0){
            return null;
        }
        return cmds;
    },

    ParseAction:function(str){
        if(-1!=str.search("\\(")){
            return null;
        }
        var cmds = [];

        var reg = /(\w+)/g
        while(true){
            var a = reg.exec(str);
            if(a==null){
                break;
            }
            cmds.push({
                type:"action",
                character:a[1]
            });
        }

        //cmds.push({type:"action",character:str});
        return cmds;
    },

    //dialog
    ParseDialog:function(str){
        var cmds = [];
        var reg = /(\w+),\(([\w|\W]+)\)/g
        while(true){
            var a = reg.exec(str);
            if(a==null){
                break;
            }
            cmds.push({
                type:"dialog",
                character:a[1],
                word:a[2]
            })
        }

        return cmds;
    },
    ParseNextStep:function(str){
        if(str[0]!="#"){
            return null;
        }
        var cmds = [];
        cmds.push({type:"wait_next_step_event"});
        return cmds;
    },

    ParseCommand:function(str0){
        if(str0[0]!="@"){
            return null;
        }
        var cmds = [];
        var strs = str0.split("@")

        for(var i = 0;i<strs.length;++i){
            var str = strs[i];
            if(str==""){
                continue;
            }
            var di = str.indexOf(",");
            if(di==-1){
                di = str.length;
            }
            var avatarName = str.substr(0,di);
            var cmd = "";
            if(di==-1){
                cmd = "";
            }
            else{
                cmd = str.substr(di+1);
            }
            cmds.push({type:"command",avatarName:avatarName,cmd:cmd});
        }

        return cmds;
    }
};

module.exports = StoryItem;
//console.log(StoryItem.Parse("#"));
//console.log(StoryItem.Parse("Customer_Normal,-15,46"));
//console.log(StoryItem.Parse("(Customer_Normal,15,46)"));
//console.log(StoryItem.Parse("(Customer_Normal,15,46),(Customer_Normal,15,-46)"));
//console.log(StoryItem.Parse("Customer_Normal,Customer_Normal"));
//console.log(StoryItem.Parse("Customer_Normal,(Hello,Hi)"));
//console.log(StoryItem.Parse("@Anna,say:hello!"));
//console.log(StoryItem.Parse("@Anna,born:10,10,@Girl,born:10,10"));