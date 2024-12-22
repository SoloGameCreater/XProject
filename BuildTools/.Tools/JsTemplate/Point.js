var Point = require("./../JsUtils/TypeTemplate").extend({
    Parse: function(str) {
        var strArr = str.split(",");
        return {x:Number(strArr[0]),y:Number(strArr[1])};
    }
});

module.exports = Point;