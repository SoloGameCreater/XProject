var ArrayFloat = require("./../JsUtils/TypeTemplate").extend({
    Parse: function(str) {
        str = str.trim();
        if(str == "")
        {
            return null;
        }
        var strArr = str.split(",");
        var resArr = new Array(strArr.length);
        for(var i = 0;i<strArr.length;++i) {
            resArr[i] = Number(strArr[i]);
        }

        return resArr;
    }
});

module.exports = ArrayFloat;