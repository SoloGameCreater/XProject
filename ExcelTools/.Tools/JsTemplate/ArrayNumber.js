/**
 * Created by Simon on 18/07/25.
 */
var ArrayNumber = {
    Parse:function(str){
        var strArr = str.split(",");
        var resArr = new Array(strArr.length);
        for(var i = 0;i<strArr.length;++i){
            resArr[i] = Number(strArr[i]);
        }

        return resArr;
    }
}

module.exports = ArrayNumber;

//console.log(ArrayNumber.Parse("1,2.3,56"));