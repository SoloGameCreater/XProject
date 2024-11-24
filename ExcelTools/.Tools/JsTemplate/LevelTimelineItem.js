/**
 * Created by Simon on 18/7/5.
 */

var pickFromArray = function(arr){
    var index = Math.floor(Math.random() * arr.length);
    return arr[index];
};

var LevelTimelineItem = {
    Parse:function(str){
        if(str==null || str == ""){
            return null;
        }
        //var reg = /([\-0-9]+),\(([\w|\W]+)\)/g;
        var reg = /([\-0-9]+),([\-0-9]+),\(([\w|\W]+)\)/g;
        var strArr = reg.exec(str);
        if(strArr==null){
            ///customer template use default
            reg = /([\-0-9]+),\(([\w|\W]+)\)/g;
            strArr = reg.exec(str);
            if(strArr==null) {
                reg = /([\-0-9]+)/g;
                strArr = reg.exec(str);
            }
        }

        //var strArr = str.split(",");
        if(strArr !== null && strArr.length >= 2){
            var timeInterval = parseFloat(strArr[1]);
            if(timeInterval==NaN){
                console.error("LevelTimelineItem error 00:",str);
                return null;
            }
            var customerType = "takeout";
            var cTemplate = undefined;
            var foods = null;

            if(strArr.length == 3){
                foods = strArr[2].split(",");
            } else if(strArr.length >= 4){
                cTemplate = parseInt(strArr[2]);
                foods = strArr[3].split(",");
            }
            var foodsNum = foods.length;

            var data = {
                time:timeInterval/1000.0,
                // customerType:customerType,
                // foodsNum:foodsNum,
                // customerNum:1,
                foods:foods
            };
            // if(cTemplate !== undefined) {
            //     data.cTemplate = cTemplate;
            // }
            return data;
        }
        console.error("LevelTimelineItem error 01:",str);
        return null;
    }
};

module.exports = LevelTimelineItem;
//console.log(LevelTimelineItem.Parse("1000,1,(abc2,def)"));
//console.log(LevelTimelineItem.Parse("1000,(abc2,def)"));
//console.log(LevelTimelineItem.Parse("0,d4"));
//console.log(LevelTimelineItem.Parse("0,t"));