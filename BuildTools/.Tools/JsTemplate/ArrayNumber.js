var ArrayNumber = require("./../JsUtils/TypeTemplate").extend({
    Parse: function(str) {
        str = str.trim();
        if(str == "")
        {
            return null;
        }
        var strArr = str.split(",");
        var resArr = new Array(strArr.length);
        for(var i = 0;i<strArr.length;++i) {
            resArr[i] = parseInt(strArr[i]);
        }

        return resArr;
    }
});

module.exports = ArrayNumber;