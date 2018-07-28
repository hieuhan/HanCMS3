$(function () {
    salesManager.init();
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status === 403) {
            var response = $.parseJSON(xhr.responseText);
        }
    });
});
var salesManagerConfigs = {
    rootPath: '/'
};

var salesManager = {
    init: function () {
        this.events();
        this.startTime();
    },
    events: function () {
        $('.logout').off('click').on('click', function (e) {
            e.preventDefault();
            $().lawsDialog({
                messages: ['Bạn muốn đăng xuất khỏi hệ thống?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        click: function () {
                            $(this).dialog('close');
                            window.location.href = cms.virtualPath('/dang-xuat-tai-khoan');
                        }
                    }
                ]
            });
        });

        $('.media-select').off('click').on('click',
            function (event) { 
                event.preventDefault(); 
                var mediaId = $(this).data('id');
                $.lawsAjax({
                    url: salesManager.virtualPath('/Ajax/MediaSelect'),
                    type: 'Post',
                    traditional: true,
                    data: { mediaId: mediaId },
                    success: function (resp) {
                        if (resp.Completed) {
                            if (resp.Message !== void 0 && resp.Message.length > 0) {
                                var imageSelect = window.opener.document.getElementById('popup-media');
                                var filePath = window.opener.document.getElementById('ImagePath');
                                filePath.value = resp.Message;
                                imageSelect.src = salesManager.virtualPath('/' + resp.Message); 
                                window.close();
                            }
                        }
                    }
                });
            });

        $('#popup-action').off('click').on('click',
            function (event) {
                event.preventDefault();
                var popup = window.open("/actions/select", "ActionPopup", "width=1150,height=550,scrollbars=1");
                popup.focus();
                return false;
            });

        $('#popup-media').off('click').on('click',
            function (event) {
                event.preventDefault();
                salesManager.PopupCenter('/medias/select', 'MediaPopup', 850, 550);
                //var popup = window.open("/medias/select", "MediaPopup", "width=850,height=550,scrollbars=1");
                //popup.focus();
                return false;
            });

        $('#popupPriceList').off('click').on('click',
            function (event) {
                event.preventDefault();
                var url = $(this).data('url');
                salesManager.PopupCenter(url, 'popupPriceList', 1200, 800);
            });

        $('#selectCustomer').off('click').on('click',
            function (event) {
                event.preventDefault();
                var url = '/Customers/Select';
                salesManager.PopupCenter(url, 'popupPriceList', 1200, 800);
            });

        $('#removeSelectedCustomer').off('click').on('click',
            function (event) {
                event.preventDefault();
                $('#selectCustomer').val(''); $('#CustomerId').val(0);
                $(this).css('visibility', 'hidden');
            });

        $('#selectPriceList').off('click').on('click',
            function (event) {
                event.preventDefault();
                if ($("input:radio[name='PriceListId']").is(":checked")) {
                    var priceListId = $(this).data('id'),
                    priceListIdClone = $("input:radio[name='PriceListId']:checked").val();
                    $.ajaxPost({
                        qname: 'selectPriceList',
                        url: salesManager.virtualPath('/Ajax/SelectPriceList'),
                        data: { priceListId: priceListId, priceListIdClone: priceListIdClone },
                        success: function (resp) {
                            if (resp.Message != null && resp.Message.length > 0) {
                                alert(resp.Message);
                                window.close();
                                window.opener.location.reload(true);
                            }
                        }
                    });
                } else {
                    alert('Bạn vui lòng chọn bảng giá !');
                    return false;
                }
            });

        $('.inputBillDetails').off('click').on('click',
            function (event) {
                event.preventDefault();
                salesManager.PopupCenter($(this).data('url'), 'InputBillDetails', 1200, 800);
            });

        $('#addCustomer').off('click').on('click',
            function (event) {
                event.preventDefault();
                if ($("input:radio[name='CustomerId']").is(":checked")) {
                    var customer = $("input:radio[name='CustomerId']:checked"),
                        customerName = customer.data('name'),
                        customerId = customer.val(),
                        removeSelectedCustomer = window.opener.document.getElementById('removeSelectedCustomer'),
                        inuptCustomerName = window.opener.document.getElementById('selectCustomer'),
                    inputCustomerId = window.opener.document.getElementById('CustomerId');
                    inuptCustomerName.value = customerName;
                    inputCustomerId.value = customerId;
                    removeSelectedCustomer.style.visibility = 'visible';
                    window.close();
                } else {
                    alert('Bạn vui lòng chọn khách hàng !');
                    return false;
                }
            });

        $('#selectall').click(function (event) {
            if (this.checked) {
                $('.checkall').each(function () {
                    this.checked = true;
                });
            } else {
                $('.checkall').each(function () {
                    this.checked = false;
                });
            }
        });

        if ($('.datepicker').length) {
            $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' });
            $('.datepicker').datepicker("option", "monthNames", ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"]);
            $('.datepicker').datepicker("option", "monthNamesShort", ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"]);
            $('.datepicker').datepicker("option", "dayNamesMin", ["CN", "T2", "T3", "T4", "T5", "T6", "T7"]);
        }

        $('.action-select').off('click').on('click',
            function (event) {
                event.preventDefault();
                var actionId = $(this).data('id');
                $.lawsAjax({
                    url: salesManager.virtualPath('/Ajax/ActionSelect'),
                    type: 'Post',
                    traditional: true,
                    data: { actionId: actionId },
                    success: function (resp) {
                        if (resp.Completed) {
                            if (resp.Data !== null && resp.Data.ActionId > 0) {
                                var defaultActionId = window.opener.document.getElementById('DefaultActionId');
                                defaultActionId.value = resp.Data.ActionId;
                                var actionName = window.opener.document.getElementById('ActionName');
                                actionName.value = resp.Data.ActionName;
                                window.close();
                            }
                        }
                    }
                });
            });

        $('input.currency').on('input', function (e) {
            $(this).val(salesManager.formatCurrency(this.value.replace(/[.]/g, '')));
        }).on('keypress', function (e) {
            if (!$.isNumeric(String.fromCharCode(e.which))) e.preventDefault();
        }).on('paste', function (e) {
            var cb = e.originalEvent.clipboardData || window.clipboardData;
            if (!$.isNumeric(cb.getData('text'))) e.preventDefault();
            });
        var productMessages = $('#productMessages');
        if (productMessages.length && productMessages.html().length > 0) {
            productMessages.fadeIn('slow').delay(3000).fadeOut('slow');
        }
    },
    virtualPath: function (patch) {
        var host = window.location.protocol + '//' + window.location.host;
        return host + patch;
    },
    startTime: function () {
        var today = new Date();
        var h = today.getHours();
        var m = today.getMinutes();
        var s = today.getSeconds();
        var d = today.getDay();
        var month = today.getMonth();
        var date = today.getDate();
        var year = today.getFullYear();
        var daysArray = new Array('Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5',
            'Thứ 6', 'Thứ 7');
        var monthsArray = new Array('tháng một', 'tháng 2', 'tháng 3', 'tháng 4', 'tháng 5', 'tháng 6',
            'tháng 7', 'tháng 8', 'tháng 9', 'tháng 10', 'tháng 11', 'tháng 12')
        m = salesManager.checkTime(m);
        s = salesManager.checkTime(s);
        $('#divDateTime').html(daysArray[d] + ' ngày ' + date + ' ' + monthsArray[month] + ' năm ' + year + ' <span style=color:orange;>' + h + ':' + m + ':' + s + '</span>');
        var t = setTimeout(salesManager.startTime, 500);
    },
    checkTime: function (i) {
        if (i < 10) { i = '0' + i }; 
        return i;
    },
    PopupCenter: function (url, title, w, h) {
        var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : window.screenX;
        var dualScreenTop = window.screenTop != undefined ? window.screenTop : window.screenY;

        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        var left = ((width / 2) - (w / 2)) + dualScreenLeft;
        var top = ((height / 2) - (h / 2)) + dualScreenTop;
        var newWindow = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        if (window.focus) {
            newWindow.focus();
        }
    },
    MediaSelect: function (mediaPath) {
        var imgSelect = window.opener.document.getElementById('ImageSelect');
            var filePath = window.opener.document.getElementById('FilePath');
            filePath.value = mediaPath;
            imgSelect.src = mediaPath;
            window.close();
    },
    formatCurrency: function (number) {
        var n = number.split('').reverse().join("");
        var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
        return n2.split('').reverse().join('');
    }
}

$.fn.lawsExists = function (callback) {
    var args = [].slice.call(arguments, 1);
    if (this.length) {
        callback.call(this, args);
    }
    return this;
}

$.extend({
    lawsVnAjax: function (url, type, dataGetter, onsuccess) {
        var execOnSuccess = $.isFunction(onsuccess) ? onsuccess : $.noop;
        var getData = $.isFunction(dataGetter) ? dataGetter : function () { return dataGetter; };
        $.ajax({
            url: url,
            type: type,
            traditional: true,
            data: getData(),
            beforeSend: function () {
                $('#loadmore').prop('disabled', true).css('cursor', 'wait').text('Đang tải dữ liệu...');
            },
            error: function (jqXhr, errorMessage) {
                if (jqXhr.status === 0) {
                    lawsVn.dialog({
                        messages: ['Không có kết nối mạng. Vui lòng kiểm tra lại.']
                        , showIcon: false
                    });
                } else if (jqXhr.status == 404) {
                    lawsVn.dialog({
                        messages: ['Không tìm thấy trang yêu cầu. [404]']
                        , showIcon: false
                    });
                } else if (jqXhr.status == 500) {
                    lawsVn.dialog({
                        messages: ['Lỗi máy chủ nội bộ. [500].']
                        , showIcon: false
                    });
                } else if (errorMessage === 'parsererror') {
                    lawsVn.dialog({
                        messages: ['Yêu cầu phân tích cú pháp JSON lỗi.']
                        , showIcon: false
                    });
                } else if (errorMessage === 'timeout') {
                    lawsVn.dialog({
                        messages: ['Hết thời gian yêu cầu.']
                        , showIcon: false
                    });
                } else if (errorMessage === 'abort') {
                    lawsVn.dialog({
                        messages: ['Yêu cầu xử lý bị hủy.']
                        , showIcon: false
                    });
                } else if (jqXhr.status != 403) {
                    lawsVn.dialog({
                        messages: ['Lỗi :.n' + jqXhr.responseText]
                        , showIcon: false
                    });
                }
                $('#loadmore').prop('disabled', true).css('cursor', 'default').text('Xem thêm');
            },
            success: function (data, status, xhr) {
                window.setTimeout(function () {
                    execOnSuccess(data);
                }, 10);
                $('#loadmore').prop('disabled', true).css('cursor', 'default').text('Xem thêm');
            }
        });
    }
});

(function ($) {
    $.fn.lawsDialog = function (options) {
        var defaultOptions = {
            title: 'Thông báo',
            width: 'auto',
            height: 'auto',
            minWidth: 'auto',
            minHeight: 'auto',
            resizable: false,
            autoOpen: true,
            modal: true,
            show: 'fade',
            hide: 'blind',
            closeText: "Đóng",
            position: { my: "center", at: "top+150", of: window.top },
            dialogClass: 'lawsVnDialog',
            buttons: null,
            onCreate: {},
            onOpen: {},
            onClose: {},
            hideClose: true,
            showIcon: false, //hiện icon chuông hay ko
            messages: []
        };
        if (typeof options == 'object') {
            options = $.extend(defaultOptions, options);
        } else {
            options = defaultOptions;
        }
        var self = this;
        var execOnClose = $.isFunction(options.onClose) ? options.onClose : $.noop;
        var execOnOpen = $.isFunction(options.onOpen) ? options.onOpen : $.noop;
        var execOnCreate = $.isFunction(options.onCreate) ? options.onCreate : $.noop;
        options.messages = $.isArray(options.messages) ? options.messages : [];

        var html = '<div class="content-thongbao">' +
            '<div class="rows-thongbao" style=" font-size: 15px;font-weight: bold; line-height: 24px;">';
        if (options.showIcon) {
            html += '<img alt="img-tb" class="img-tb" src="' + lawsVnConfig.rootPath + 'assets/images/icon-tb.png">';
        }
        html += options.messages[0] +
            '</div>';
        if (options.messages.length > 1) {
            html +=
                '<div class="rows-thongbao center" style="font-size: 13px; font-style: italic; line-height: 24px;">' +
                '<span>' + options.messages[1] + '</span> <br>';
        }
        if (options.messages.length > 2) {
            html += '<span style="color: #d81c22">' + options.messages[2] + '</span>';
        }
        html += '</div>';
        if (!self.length) {
            self = $(html);
        }
        self.dialog({
            title: options.title,
            width: options.width,
            height: options.height,
            minWidth: options.minWidth,
            minHeight: options.minHeight,
            resizable: options.resizable,
            autoOpen: options.autoOpen,
            modal: options.modal,
            closeText: options.closeText,
            position: options.position,
            dialogClass: options.dialogClass,
            show: options.show,
            hide: options.hide,
            buttons: options.buttons || [
                {
                    text: 'Đóng',
                    class: 'btn-thongbao1',
                    click: function () {
                        $(self).dialog('close');
                        window.setTimeout(function () {
                            execOnClose();
                        }, 10);
                    }
                }
            ],
            create: function () {
                window.setTimeout(function () {
                    execOnCreate();
                }, 10);
            },
            open: function (event, ui) {
                $(self).closest(".ui-dialog").find(":button").blur();
                $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                //ẩn nút đóng x
                if (options.hideClose) {
                    $(self).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
                if (options.title === '') {
                    //$(this).siblings('.ui-dialog-titlebar').remove();
                    $(this).dialog("widget").find(".ui-dialog-titlebar").remove();
                }
                window.setTimeout(function () {
                    execOnOpen(event, ui);
                }, 10);
            },
            close: function (event, ui) {
                $(self).dialog('close');
                $(self).dialog('destroy').remove();
                window.setTimeout(function () {
                    execOnClose(event, ui);
                }, 10);
            }
        });
    }
})(jQuery);

$.extend({
    lawsAjax: function (options) {
        var defaults =
            {
                url: '',
                type: 'Get',
                data: {},
                dataType: 'json',
                async: false,
                cache: false,
                traditional: false,
                timeout: 5000,
                beforeSend: function () {
                    $('#loading').fadeIn('normal');
                },
                success: function (data, status, xhr) {
                    if (data.Message !== void 0 && data.Message.length > 0) {
                        $().lawsDialog({
                            messages: [data.Message],
                            showIcon: false,
                            onClose: function () {
                                if (data.ReturnUrl !== void 0 && data.ReturnUrl.length > 0) {
                                    window.location.href = data.ReturnUrl;
                                }
                            }
                        });
                    }
                },
                error: function (jqXhr, errorMessage) {
                    $('#loading').fadeOut('normal');
                    if (jqXhr.status === 0) {
                        $().lawsDialog({
                            messages: ['Không có kết nối mạng. Vui lòng kiểm tra lại.']
                            , showIcon: false
                        });
                    } else if (jqXhr.status === 404) {
                        $().lawsDialog({
                            messages: ['Không tìm thấy trang yêu cầu. [404]']
                            , showIcon: false
                        });
                    } else if (jqXhr.status === 500) {
                        $().lawsDialog({
                            messages: ['Lỗi máy chủ nội bộ. [500].']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'parsererror') {
                        $().lawsDialog({
                            messages: ['Yêu cầu phân tích cú pháp JSON lỗi.']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'timeout') {
                        $().lawsDialog({
                            messages: ['Hết thời gian yêu cầu.']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'abort') {
                        $().lawsDialog({
                            messages: ['Yêu cầu xử lý bị hủy.']
                            , showIcon: false
                        });
                    } else if (jqXhr.status !== 403) {
                        $().lawsDialog({
                            messages: ['Lỗi :.n' + jqXhr.responseText]
                            , showIcon: false
                        });
                    }
                },
                always: function () {
                    $('#loading').fadeOut('normal');
                }
            }
        options = $.extend(defaults, options);
        if (options.url.length) {
            $.ajax({
                url: options.url,
                type: options.type,
                data: options.data,
                dataType: options.dataType,
                async: options.async,
                cache: options.cache,
                traditional: options.traditional,
                timeout: options.timeout,
                beforeSend: function () {
                    if ($.isFunction(options.beforeSend)) {
                        window.setTimeout(function () {
                            options.beforeSend();
                        }, 10);
                    }
                },
                success: function (data, status, xhr) {
                    if ($.isFunction(options.success)) {
                        window.setTimeout(function () {
                            options.success(data, status, xhr);
                        }, 10);
                    }
                },
                error: function (jqXhr, errorMessage) {
                    window.setTimeout(function () {
                        options.error(jqXhr, errorMessage);
                    }, 10);
                }
            }).always(function () {
                window.setTimeout(function () {
                    options.always();
                }, 10);
            });
        }
    }
});

(function ($) {
    var queues = {};
    var activeReqs = {};

    $.saleManagerAjax = function (qname, opts) {

        if (typeof opts === 'undefined') {
            throw ('saleManagerAjax: opts is underfined');
        }

        var deferred = $.Deferred(),
            promise = deferred.promise();

        promise.success = promise.done;
        promise.error = promise.fail;
        promise.complete = promise.always;

        var deferredOpts = typeof opts === 'function';
        var options = !deferredOpts ? $.extend(true, {}, opts) : null;
        enqueue(function () {
            var jqXHR = $.ajax.apply(window, [deferredOpts ? opts() : options]);

            jqXHR.done(function () {
                deferred.resolve.apply(this, arguments);
            });
            jqXHR.fail(function () {
                deferred.reject.apply(this, arguments);
            });

            jqXHR.always(dequeue);

            return jqXHR;
        });

        return promise;

        function enqueue(cb) {
            if (!queues[qname]) {
                queues[qname] = [];
                var xhr = cb();
                activeReqs[qname] = xhr;
            }
            else {
                queues[qname].push(cb);
            }
        }

        function dequeue() {
            if (!queues[qname]) {
                return;
            }
            var nextCallback = queues[qname].shift();
            if (nextCallback) {
                var xhr = nextCallback();
                activeReqs[qname] = xhr;
            }
            else {
                delete queues[qname];
                delete activeReqs[qname];
            }
        }
    };
    //Đăng ký phương thức icget và icget
    $.each(['ajaxGet', 'ajaxPost'], function (i, method) {
        $[method] = function (options) {
            var defaults = {
                qname: 'saleManagerQuereFirst',
                resultId: null,
                url: '',
                type: method,
                dataType: 'json',
                data: {},
                headers: {},
                statusCode: {},
                cache: false,
                timeout: 5000,
                removeLoadmore: true,
                //xhr: function () {
                //    var xhr = new window.XMLHttpRequest();
                //    if ($('.progress').length === 0) {
                //        $('body').append('<div class="progress"></div>');
                //        //$('#loadingbar').addClass('waiting').append($('<dt/><dd/>'));
                //        //$('#loadingbar').width((50 + Math.random() * 30) + "%");
                //    }
                //    xhr.upload.addEventListener('progress', function (evt) {
                //        if (evt.lengthComputable) {
                //            var percentComplete = evt.loaded / evt.total;
                //            $('.progress').css({
                //                width: percentComplete * 100 + '%'
                //            });
                //            if (percentComplete === 1) {
                //                $('.progress').addClass('hide');
                //            }
                //        }
                //    }, false);
                //    xhr.addEventListener('progress', function (evt) {
                //        if (evt.lengthComputable) {
                //            var percentComplete = evt.loaded / evt.total;
                //            $('.progress').css({
                //                width: percentComplete * 100 + '%'
                //            });
                //        }
                //    }, false);
                //    return xhr;
                //},
                beforeSend: function () {
                    if ($('#loadingbar').length === 0) {
                        $('body').append('<div id="loadingbar"></div>');
                        $('#loadingbar').addClass('waiting').append($('<dt/><dd/>'));
                        $('#loadingbar').width((50 + Math.random() * 30) + "%");
                    }
                },
                error: function (xhr, text, e) {
                    if (xhr.status === 0) {
                        alert('Không có kết nối mạng. Vui lòng kiểm tra lại.');
                    } else if (xhr.status === 404) {
                        alert('Không tìm thấy trang yêu cầu.')
                    } else if (xhr.status === 500) {
                        alert('Lỗi máy chủ nội bộ. [500].');
                    } else if (text === 'parsererror') {
                        alert('Yêu cầu phân tích cú pháp JSON lỗi.');
                    } else if (text === 'timeout') {
                        alert('Hết thời gian yêu cầu.');
                    } else if (text === 'abort') {
                        alert('Yêu cầu xử lý bị hủy.');
                    } else if (xhr.status !== 403) {
                        alert('Lỗi :.n' + xhr.responseText);
                    }
                },
                complete: function (xhr, text) { },
                success: function (data, text, xhr) {
                    if (options.resultId != null && options.resultId && options.dataType === 'html' && data != null && data.length > 0) {
                        $(options.resultId).html(data);
                    }
                },
                always: function () {
                    $('#loadingbar').width('101%').delay(200).fadeOut(400, function () {
                        $('#loadingbar').remove();
                    });
                }
            }
            options = $.extend(defaults, options);
            //if ($.isFunction(options.data)) {
            //    options.type = options.type || options.success;
            //    options.success = data;
            //    options.data = undefined;
            //}
            return $.saleManagerAjax(options.qname, {
                type: options.type === 'ajaxPost' ? 'post' : 'get',
                url: options.url,
                data: options.data,
                dataType: options.dataType,
                //xhr:options.xhr,
                beforeSend: function () {
                    if ($.isFunction(options.beforeSend)) {
                        window.setTimeout(function () {
                            options.beforeSend();
                        }, 10);
                    }
                },
                complete: function (data, status, xhr) {
                    if ($.isFunction(options.complete)) {
                        window.setTimeout(function () {
                            options.complete(data, status, xhr);
                        }, 10);
                    }
                },
                success: function (data, status, xhr) {
                    if ($.isFunction(options.success)) {
                        window.setTimeout(function () {
                            options.success(data, status, xhr);
                        }, 10);
                    }
                },
                error: function (jqXhr, errorMessage) {
                    window.setTimeout(function () {
                        options.error(jqXhr, errorMessage);
                    }, 10);
                }
            }).always(function () {
                window.setTimeout(function () {
                    options.always();
                }, 10);
            });
        };
    });

    var isQueueRunning = function (qname) {
        return (queues.hasOwnProperty(qname) && queues[qname].length > 0) || activeReqs.hasOwnProperty(qname);
    };

    var isAnyQueueRunning = function () {
        for (var i in queues) {
            if (isQueueRunning(i)) return true;
        }
        return false;
    };

    $.saleManagerAjax.isRunning = function (qname) {
        if (qname) return isQueueRunning(qname);
        else return isAnyQueueRunning();
    };

    $.saleManagerAjax.getActiveRequest = function (qname) {
        if (!qname) throw ('saleManagerAjax: queue name is required');

        return activeReqs[qname];
    };

    $.saleManagerAjax.abort = function (qname) {
        if (!qname) throw ('saleManagerAjax: queue name is required');

        var current = $.lawAjax.getActiveRequest(qname);
        delete queues[qname];
        delete activeReqs[qname];
        if (current) current.abort();
    };

    $.saleManagerAjax.clear = function (qname) {
        if (!qname) {
            for (var i in queues) {
                if (queues.hasOwnProperty(i)) {
                    queues[i] = [];
                }
            }
        }
        else {
            if (queues[qname]) {
                queues[qname] = [];
            }
        }
    };

})(jQuery);
