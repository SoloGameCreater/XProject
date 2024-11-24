var ArrayString = require("./../JsUtils/TypeTemplate").extend({
    Parse: function(str) {
        var strArr = str.split(",");
        return strArr;
    }
});

module.exports = ArrayString;