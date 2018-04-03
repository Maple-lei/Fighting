/**
 * 因为ECMA3(老版本浏览器)中，全局undefined是可以被重写的，例如undefined = true。这种写法是为了得到真实的undefined

 */
; (function ($, window, document, undefined) {

    function parse() {
        $(function(){
            $(".combobox").each(function () {
                //绑定在标签上的属性
                var strOptions = $(this).attr("data-options");

                if(strOptions){
                    var objOption = $.parseJSON(strOptions);
                }            
            })
        })        
    }

    parse();

    $.fn.combobox = function (options, param) {

        //直接调用combobox的方法
        if (typeof (options) == "string") {
            var method = $.fn.combobox.methods[options];

            if (method) {
                return method(this, param);
            } else {
                return;
            }
        }

        options = options || {}; //如果options没有指定，则，赋给 {}
        return this.each(function () {
            //之前绑定在对象上的参数
            var previousOption = $.data(this, "combobox");

            var mergeOptions;
            if (previousOption) {
                mergeOptions = $.extend({}, previousOption, options);  //把传入的参数和默认参数进行合并
            } else {
                mergeOptions = $.extend({}, $.fn.combobox.defaults, options);

            }
            $.data(this, "combobox", mergeOptions);

            //创建基本元素
            create(this, mergeOptions);

            //如果是直接传进来的数据，则直接绑定
            if (mergeOptions.data) {
                loadData(this, mergeOptions.data);
            } else {
                //通过ajax去后台获取
                request(this, mergeOptions);
            }
        })
    }

    $.fn.combobox.defaults = {
        valueField: "value",
        textField: "text",
        width: 200,
        data: null,
        url: null,
        method: "post",
        queryParams: {},
        onLoadSuccess: function () {

        },
        onLoadError: function () {

        },
        onSelect: function () {

        },
        loader: function (options, success, error) {
            if (!options.url) {
                return false;
            }

            $.ajax({
                url: options.url,
                type: options.method,
                dataType: "json",
                data: options.queryParams,
                success: function (data) {
                    success(data);
                },
                error: function () {
                    error.apply(this, arguments);
                }
            })
        }
    }

    $.fn.combobox.methods = {
        getValue: function (target, param) {
            var selectedItem = $(target).find("option:selected");
            return selectedItem.val();
        },

        getText: function (target, param) {
            var selectedItem = $(target).find("option:selected");
            return selectedItem.text();
        },

        //返回选项（options）对象。
        options: function () {
            var result = $.extend({}, $.fn.combobox.defaults);

            return result;
        },

        //当用户选择一个列表项时触发。
        onSelect: function (record) {

        },

        //
        clear: function () {

        }
    }

    /**
     * 创建元素
     */
    function create(target, options) {
        if ($(target).is("select")) {
            $(target).removeClass("form-control").addClass("form-control");
            $(target).css("width", options.width);

        } else {
            //此方法有问题*****
            $(target).css("display", "none");

            var newElement = "<select class = 'form-control'></select>";
            $(target).after(newElement);
        }
    }

    /**
     * 加载数据，之前的下拉项将会被删除
     */
    function loadData(target, data, param) {

        var targetOptions = $(target).data("combobox");

        var valueField = targetOptions.valueField;
        var textField = targetOptions.textField;

        var arrOptionHtml = [];
        for (var i = 0; i < data.length; i++) {
            var value = data[i][valueField];
            var text = data[i][textField];

            arrOptionHtml.push(" <option value='" + value + "'>" + text + "</option>");
        }

        $(target).html(arrOptionHtml.join(""));

        $(target).find("option").each(function () {
            $(this).bind("click", function () {

            })
        })
    }

    //从服务器加载数据
    function request(target, options) {

        options.loader.call(target, options, function (data) {
            loadData(target, data);
        }, function () {
            options.onLoadError.apply(this, arguments);
        });
    }

})(jQuery, window, document)