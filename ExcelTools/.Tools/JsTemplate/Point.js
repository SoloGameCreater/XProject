/**
 * Created by Simon on 18/07/25.
 */

var Point = {
    Parse:function(str){
        var strArr = str.split(",");
        return {x:Number(strArr[0]),y:Number(strArr[1])};
    }
}

module.exports = Point;