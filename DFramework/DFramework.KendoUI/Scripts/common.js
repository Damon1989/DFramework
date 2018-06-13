!function ($) {
    var o = $({});
    $.subscribe = function () {
        var eventName = arguments[0];
        var arrEvent = eventName.split(',');
        for (var i = 0; i < arrEvent.length; i++) {
            arguments[0] = arrEvent[i];
            o.on.apply(o, arguments);
            console.log(arguments[0] + " subscribed");
        }
    };
    $.unsubscribe = function () {
        var eventName = arguments[0];
        var arrEvent = eventName.split(',');
        for (var i = 0; i < arrEvent.length; i++) {
            arguments[0] = arrEvent[i];
            o.off.apply(o, arguments);
            console.log(arguments[0] + " unsubscribed");
        }
    };
    $.publish = function () {
        o.trigger.apply(o, arguments);
        console.log(arguments[0] + " published");
    }
}(window.jQuery)

var initDataEvents = function () {
    $("body").on("click", "[data-event]", function () {
        var action = $(this).data("event");
        var target = $(this)[0];
        if (!action)
            console.error("data-event is null");
        var actionArr = action.split(',');
        for (var i = 0; i < actionArr.length; i++) {
            $.publish($.trim(actionArr[i]), [$.extend($(this).data(), { target: target })]);
        }
    });
}

//返回表单提交的数据(object)，若对象中包含数组，则返回array
//format='attr'，则数组返回多个属性   如：name[0]
$.fn.getFormData = function (format) {
    var tags = this.find('input[type!=radio][type!=checkbox][name],textarea[name],input[type=radio]:checked,input[type=checkbox]:checked');
    var formData = {};
    if (tags.length > 0) {
        tags.each(function (i, e) {
            var t = $(this);
            var name = t.attr("name");
            var value = t.val();
            if (formData[name] === undefined) {
                formData[name] = value;
            } else {
                //name已存在，转为数组
                if (formData[name] instanceof Array) {
                    formData[name].push(value);
                } else {
                    var arr = new Array();
                    arr.push(value);
                    arr.push(formData[name]);
                    formData[name] = arr;
                }
            }
        })
    }
    if (format == 'attr') {
        for (var i in formData) {
            if (formData[i] instanceof Array) {
                for (var j = 0; j < formData[i].length; j++) {
                    formData[i + '[' + j + ']'] = formData[i][j];
                }
                delete formData[i];
            }
        }
    }
    return formData;
}

//提交表单，form提交至action，其他标签提交至data-url
$.fn.submitForm = function (data, callback) {
    if (typeof data === 'function') {
        callback = data;
        data = {};
    }
    var formData = this.getFormData();
    var params = $.extend({}, formData, data);
    var loadBtn = this.find("[data-loading-text]");
    var url = this[0].nodeName.toLocaleLowerCase() == "form" ? this.attr("action") : (this.data("url") || null);
    $.ajax({
        url: url,
        data: params,
        type: "post",
        beforeSend: function () {
            loadBtn.button('loading');
        },
        success: function (d) {
            if (d.ErrorCode) {
                if (d.ErrorCode == 0) {
                    callback.call(this, d.Result);
                } else {
                    if (d.Message)
                        alert(d.Message);
                }
            } else {
                callback.call(this, d);
            }
        },
        complete: function () {
            loadBtn.button('reset');
        }
    });
}
$.fn.clearForm = function (clearHidden) {
    if (clearHidden == true) {
        this.find('input[name],textarea[name]').val(null);
    } else {
        this.find('input[name][type!=hidden],textarea[name]').val(null);
    }
}

//初始化预览图片，imgElementForJQuerySelector：对应显示图片<img>元素的jquery选择器
$.fn.initImagePreview = function (imgElementForJQuerySelector, callback) {
    var $inputImage = this;
    $inputImage.on('change', function (evt) {
        if (!window.FileReader) return;
        var files = evt.target.files;
        if (files.length > 0) {
            var file = files[0];
            if (file.type.match('image.*')) {
                var reader = new FileReader();
                reader.onload = function (e) { $(imgElementForJQuerySelector).attr('src', e.target.result); };
                reader.readAsDataURL(file);

                callback && callback.call(this);
            }
            else {
                alert('不支持的格式');
            }
        }
    });
}
$(function () {
    initDataEvents();
});

var FileExtension = {
    image: ["jpg", "jpeg", "png", "bmp", "gif", "3g2", "ogv"],
    word: ['doc', 'docx'],
    excel: ['xls', 'xlsx'],
    ppt: ['ppt,pptx'],
    text: ['txt'],
    zip: ['rar', 'zip'],
    pdf: ['pdf'],
    video: ['mp4', 'flv', 'wmv'],
    flash: ['swf'],
    audio: ['mp3']
}

var fileServerDomain = "res2.age06.com";
var fileServerIp = "180.153.239.18";

String.prototype.changeDomainToIp = function () {
    return this.replace(fileServerDomain, fileServerIp);
}

String.prototype.getExtension = function () {
    return this.substr(this.lastIndexOf(".") + 1).toLowerCase();
}

String.prototype.isImage = function () {
    return FileExtension.image.Contain(this.getExtension());
}
String.prototype.isText = function () {
    return FileExtension.text.Contain(this.getExtension());
}
String.prototype.isWord = function () {
    return FileExtension.word.Contain(this.getExtension());
}
String.prototype.isExcel = function () {
    return FileExtension.excel.Contain(this.getExtension());
}
String.prototype.isPPT = function () {
    return FileExtension.ppt.Contain(this.getExtension());
}
String.prototype.isPDF = function () {
    return FileExtension.pdf.Contain(this.getExtension());
}
String.prototype.isZip = function () {
    return FileExtension.zip.Contain(this.getExtension());
}
String.prototype.IsVideo = function () {
    return FileExtension.video.Contain(this.getExtension());
}
String.prototype.IsFlash = function () {
    return FileExtension.flash.Contain(this.getExtension());
}
String.prototype.IsAudio = function () {
    return FileExtension.audio.Contain(this.getExtension());
}
String.prototype.startWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substr(0, str.length) == str)
        return true;
    else
        return false;
    return true;
}
String.prototype.endWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substring(this.length - str.length) == str)
        return true;
    else
        return false;
    return true;
}

var getSubTitle = function (str, length) {
    var num = 0;
    var cut = 0;
    for (var i = 0; i < str.length; i++) {
        str[i].checkChinese() ? num += 2 : num += 1;
        if (num <= length)
            cut++;
    }
    if (num > length) {
        return str.substring(0, cut);
    } else {
        return str;
    }
}

String.prototype.checkChinese = function () {
    var reg = new RegExp("[\\u4E00-\\u9FFF]+", "g");
    return reg.test(this);
}

//根据QueryString参数名称获取值
var getQueryString = function (name) {
    var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
    return (result == null || result.length < 1) ? "" : unescape(decodeURI(result[1]));
}

Array.prototype.Contain = function (value) {
    for (var i in this) {
        if (this[i] === value) {
            return true;
        }
    }
    return false;
}

function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}