/**
 * Created by Simon on 18/07/25.
 */
var Range = {
    Parse:function(str){
        var strArr = str.split(",");
        console.log(strArr);
        return [Number(strArr[0]),Number(strArr[1])];
    }
}

module.exports = Range;