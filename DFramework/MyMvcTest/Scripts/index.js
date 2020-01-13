var DateTimeModule = (function() {

    var formatAddZero = function(data) {
        var d = parseInt(data);
        if (d >= 0 && d <= 9) {
            return "0" + d;
        }
        return d;
    };

    var strToDate = function (dateStr, separator) {
        if (!separator) {
            separator = "-";
        }
        var dateArr = dateStr.split(separator);
        var year = parseInt(dateArr[0]);
        var month = parseInt(dateArr[1]);
        if (dateArr[1].indexOf("0") === 0) {
            month = parseInt(dateArr[1].substring(1));
        }
        var day = parseInt(dateArr[2]);
        return new Date(year, month - 1, day);
    };

    var strToTime = function(dateTimeStr, separator) {
        if (!separator) {
            separator = "-";
        }

        var dateTimeArr = dateTimeStr.split(" ");

        var dateArr = dateTimeArr[0].split(separator);
        var year = parseInt(dateArr[0]);
        var month = parseInt(dateArr[1]);
        if (dateArr[1].indexOf("0") === 0) {
            month = parseInt(dateArr[1].substring(1));
        }
        var day = parseInt(dateArr[2]);

        if (dateTimeArr.length > 1) {
            var timeArr = dateTimeStr.split(" ")[1].split(":");
            console.log(timeArr);
            return new Date(year, month - 1, day, (timeArr.length >= 1 && timeArr[0] !== null ? timeArr[0] : 0), (timeArr.length >= 2 && timeArr[1] !== null ? timeArr[1] : 0), (timeArr.length >=3 &&timeArr[2] !== null ? timeArr[2] : 0));
        } else {
            return new Date(year, month - 1, day,0,0,0);
        }
    };
/*
* 2020-01-01
*/
    var getFormatDateStr = function (dateData, separate) {
        var date = dateData instanceof Date ? dateData : strToDate(dateData);
        separate = separate ? separate : "-";
        var year = date.getFullYear();
        var month = formatAddZero(date.getMonth() + 1);
        var strDate = formatAddZero(date.getDate());
        var currentDate = year + separate + month + separate + strDate;
        return currentDate;
    };

    /*
 * 2020-01-01 12:23:34
 */
    var getFormatTimeStr = function (dateData, separate) {
        var date = dateData instanceof Date ? dateData : strToTime(dateData);
        separate = separate ? separate : "-";
        var year = date.getFullYear();
        var month = formatAddZero(date.getMonth() + 1);
        var strDate = formatAddZero(date.getDate());
        var hour = formatAddZero(date.getHours());
        var mm = formatAddZero(date.getMinutes());
        var ss = formatAddZero(date.getSeconds());
        var currentTime = year + separate + month + separate + strDate + " " + hour + ":" + mm + ":" + ss;
        return currentTime;
    };

    var dateDiffDay = function(firstDate, secondDate) {
        var firstD = new Date(getFormatDate(firstDate));
        var secondD = new Date(getFormatDate(secondDate));
        var diff = Math.abs(firstD.getTime() - secondD.getTime());
        var result = parseInt(diff / (1000 * 60 * 60 * 24));
        return result;
    };

    var dateDiffGreater = function(firstDate, secondDate) {
        var firstD = new Date(getFormatDate(firstDate));
        var secondD = new Date(getFormatDate(secondDate));
        return firstD - secondD > 0;
    };

    return {
        strToDate: strToDate,
        strToTime: strToTime,
        getFormatDateStr: getFormatDateStr,
        getFormatTimeStr: getFormatTimeStr,
        dateDiffDay: dateDiffDay,
        dateDiffGreater: dateDiffGreater
    };
})();

var CalculationModule = (function () {
    /*
     * 判断obj是否为一个整数
     */
    var isInteger = function(obj) {
        return Math.floor(obj) === obj;
    };

    /*
     * 将一个浮点数转成整数，返回整数和倍数。如3.14>>314,倍数是100
     * @param floatNum {number} 小数
     * @return {object}
     *  {times:100,num:314}
     */
    var toInteger = function(floatNum) {
        var ret = { times: 1, num: 0 };
        if (isInteger(floatNum)) {
            ret.num = floatNum;
            return ret;
        }
        var strfi = floatNum + "";
        var dotPos = strfi.indexOf(".");
        var len = strfi.substr(dotPos + 1).length;
        var times = Math.pow(10, len);
        var intNum = Number(floatNum.toString().replace(".", ""));
        ret.times = times;
        ret.num = intNum;
        return ret;
    };

    /*
     * 核心方法，实现加减乘除运算，确保不丢失精度
     * 思路：把小数放大为整数（乘），进行算术运算，再缩小为小数（除）
     * @param a {number} 运算数1
     * @param b {number} 运算数2
     * @param digits {number}精度，保留的小数点数，比如2，即保留为两位小数
     * @param op {string} 运算类型，有加减乘除（add/subtract/multiply/divide）
     */
    var operation = function(a, b,op) {
        var o1 = toInteger(a);
        var o2 = toInteger(b);
        var n1 = o1.num;
        var n2 = o2.num;
        var t1 = o1.times;
        var t2 = o2.times;
        var max = t1 > t2 ? t1 : t2;
        var result = null;
        switch (op) {
        case "add":
        {
            if (t1 === t2) { //两个小数位数相同
                result = n1 + n2;
            } else if (t1 > t2) { //o1小数位大于o2
                result = n1 + n2 * (t1 / t2);
            } else {
                result = n1 * (t1 / t2) + n2;
            }
            return result / max;
        }
        case "subtract":
        {
            if (t1 === t2) { //两个小数位数相同
                result = n1 - n2;
            } else if (t1 > t2) { //o1小数位大于o2
                result = n1 - n2 * (t1 / t2);
            } else {
                result = n1 * (t1 / t2) - n2;
            }
            return result / max;
        }
        case "multiply":
        {
            result = (n1 * n2) * (t1 * t2);
            return result;
        }
        case "divide":
        {
            result = (n1 * n2) / (t1 * t2);
            return result;
        }
        }

    };

    var getDefaultDigits = function(digits) {
        return digits = digits === undefined ? 2 : digits;
    };

    var subDecimal = function (num, digits) {
        return Math.floor(num * Math.pow(10, digits)) / (Math.pow(10, digits));
    };

    // 加减乘除的四个接口
    var add = function(a, b) {
        return operation(a, b, 'add');
    };
    var subtract = function (a, b) {
        return operation(a, b, 'subtract');
    };
    var multiply = function (a, b) {
        return operation(a, b, 'multiply');
    };
    var divide = function (a, b) {
        return operation(a, b, 'divide');
    };
    /*
     * 四舍五入
     */
    var addToFixed = function (a, b, digits) {
        digits= getDefaultDigits(digits);
        return add(a, b).toFixed(digits);
    };

    var subtractToFixed = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return subtract(a, b).toFixed(digits);
    };

    var multiplyToFixed = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return multiply(a, b).toFixed(digits);
    };

    var divideToFixed = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return divide(a, b).toFixed(digits);
    };

    /*
     * 小数位截取
     */
    var addSubDecimal = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return subDecimal(add(a, b), digits);
    };

    var subtractSubDecimal = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return subDecimal(subtract(a, b), digits);
    };

    var multiplySubDecimal = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return subDecimal(multiply(a, b), digits);
    };

    var divideSubDecimal = function (a, b, digits) {
        digits = getDefaultDigits(digits);
        return subDecimal(divide(a, b), digits);
    };

    return {
        add: add,
        subtract: subtract,
        multiply: multiply,
        divide: divide,
        addToFixed: addToFixed,
        subtractToFixed: subtractToFixed,
        multiplyToFixed: multiplyToFixed,
        divideToFixed: divideToFixed,
        addSubDecimal: addSubDecimal,
        subtractSubDecimal: subtractSubDecimal,
        multiplySubDecimal: multiplySubDecimal,
        divideSubDecimal: divideSubDecimal
    };
})();


/*
 * 对Date的扩展，将Date转化为指定格式的String
 * 月（M）、日（d）、小时（h）、分（m）、秒(s)、季度（q）可以用1-2个占位符
 * 年（y）可以用1-4个占位符，毫秒（S）只能用1个占位符（是1-3位的数字）
 * 例子：
 * （new Date()）.Format("yyyy-MM-dd hh:mm:ss.S")  ==》2020-01-09 22:27:07.116
 * (new Date()).Format(yyyy-M-d h:m:s.S) ==>2020-1-9 22:27:40.636
 */
Date.prototype.Format = function(fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份
        "d+": this.getDate(), //日
        "h+": this.getHours(), //小时
        "m+": this.getMinutes(), //分
        "s+": this.getSeconds(), //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}