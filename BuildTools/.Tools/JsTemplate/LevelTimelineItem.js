var LevelTimelineItem = require("./../JsUtils/TypeTemplate").extend({
    Parse: function(str) {
        if(str==null || str == "") {
            return null;
        }
        //var reg = /([\-0-9]+),\(([\w|\W]+)\)/g;
        var reg = /([\-0-9]+),([\-0-9]+),\(([\w|\W]+)\)/g;
        var strArr = reg.exec(str);
        if(strArr==null) {
            ///customer template use default
            reg = /([\-0-9]+),\(([\w|\W]+)\)/g;
            strArr = reg.exec(str);
            if(strArr==null) {
                reg = /([\-0-9]+)/g;
                strArr = reg.exec(str);
            }
        }

        //var strArr = str.split(",");
        if(strArr !== null && strArr.length >= 2) {
            var timeInterval = parseFloat(strArr[1]);
            if(timeInterval==NaN) {
                console.error("LevelTimelineItem error 00:",str);
                return null;
            }
            var customerType = 1;
            var foods = null;

            if(strArr.length == 3) {
                foods = strArr[2].split(",");
            } else if(strArr.length >= 4) {
                customerType = parseInt(strArr[2]);
                foods = strArr[3].split(",");
            }

            var data = {
                time:timeInterval/1000.0,
                customerType:customerType,
                foods:foods
            };
            return data;
        }
        console.error("LevelTimelineItem error 01:",str);
        return null;
    },

    needExport: function() {
        return true;
    },

    getFields: function() {
        var fields = [];
        fields.push(this._getField(0, "time", "float"));
        fields.push(this._getField(1, "customerType", "int"));
        fields.push(this._getField(2, "foods", "int[]"));

        return fields;
    }
});

module.exports = LevelTimelineItem;
//console.log(LevelTimelineItem.Parse("1000,1,(abc2,def)"));
//console.log(LevelTimelineItem.Parse("1000,(abc2,def)"));
//console.log(LevelTimelineItem.Parse("0,d4"));
//console.log(LevelTimelineItem.Parse("0,t"));